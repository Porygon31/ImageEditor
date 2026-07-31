using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ImageEditor
{
    public enum OutputFormat { Png, Jpeg, Webp, Gif }

    public enum CropMode { None, Cover, Contain, Manual }

    public sealed class ProcessSettings
    {
        public int Width;
        public int Height;
        public bool KeepRatio = true;
        public CropMode Crop = CropMode.None;
        // Manual crop region in ORIGINAL image pixel coordinates.
        public System.Drawing.Rectangle ManualRect;
        public OutputFormat Format = OutputFormat.Jpeg;
        public int Quality = 85;
        public bool UseTargetSize;
        public long TargetSizeBytes;
    }

    public sealed class RenderResult
    {
        public byte[] Bytes;
        public int UsedQuality;
        public bool TargetMet;
        public int Width;
        public int Height;

        public long SizeBytes => Bytes?.LongLength ?? 0;
    }

    public sealed class ImageProcessor : IDisposable
    {
        private Image<Rgba32> _original;

        public bool HasImage => _original != null;
        public int OriginalWidth => _original?.Width ?? 0;
        public int OriginalHeight => _original?.Height ?? 0;

        public void Load(string path)
        {
            var loaded = Image.Load<Rgba32>(path);
            _original?.Dispose();
            _original = loaded;
        }

        // Applies crop + resize. Caller owns the returned image and must dispose it.
        private Image<Rgba32> Process(ProcessSettings s)
        {
            int tw = s.Width > 0 ? s.Width : _original.Width;
            int th = s.Height > 0 ? s.Height : _original.Height;

            var img = _original.Clone();
            switch (s.Crop)
            {
                case CropMode.Manual:
                    var r = ClampRect(s.ManualRect, img.Width, img.Height);
                    if (r.Width > 0 && r.Height > 0)
                        img.Mutate(x => x.Crop(new Rectangle(r.X, r.Y, r.Width, r.Height)));
                    img.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(tw, th),
                        Mode = ResizeMode.Stretch
                    }));
                    break;

                case CropMode.Cover:
                    img.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(tw, th),
                        Mode = ResizeMode.Crop,
                        Position = AnchorPositionMode.Center
                    }));
                    break;

                case CropMode.Contain:
                    img.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(tw, th),
                        Mode = ResizeMode.Pad,
                        PadColor = s.Format == OutputFormat.Jpeg ? Color.White : Color.Transparent
                    }));
                    break;

                default: // None
                    img.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(tw, th),
                        Mode = s.KeepRatio ? ResizeMode.Max : ResizeMode.Stretch
                    }));
                    break;
            }
            return img;
        }

        // Full pipeline: crop + resize + encode (honoring target size if requested).
        public RenderResult Render(ProcessSettings s)
        {
            using (var img = Process(s))
            {
                // JPEG has no alpha: blend transparency over white so it does not turn black.
                if (s.Format == OutputFormat.Jpeg)
                    img.Mutate(x => x.BackgroundColor(Color.White));

                var result = new RenderResult { Width = img.Width, Height = img.Height };

                bool lossy = s.Format == OutputFormat.Jpeg || s.Format == OutputFormat.Webp;
                if (s.UseTargetSize && s.TargetSizeBytes > 0 && lossy)
                {
                    result.Bytes = EncodeToTargetSize(img, s.Format, s.TargetSizeBytes,
                        out int usedQ, out bool met);
                    result.UsedQuality = usedQ;
                    result.TargetMet = met;
                }
                else
                {
                    result.Bytes = Encode(img, s.Format, s.Quality);
                    result.UsedQuality = s.Quality;
                    result.TargetMet = !s.UseTargetSize || result.SizeBytes <= s.TargetSizeBytes;
                }
                return result;
            }
        }

        // Preview reflecting crop/resize (encoded as PNG so WinForms can display any format's result).
        public System.Drawing.Bitmap RenderPreview(ProcessSettings s)
        {
            using (var img = Process(s))
            {
                if (s.Format == OutputFormat.Jpeg)
                    img.Mutate(x => x.BackgroundColor(Color.White));

                byte[] png = Encode(img, OutputFormat.Png, 100);
                using (var ms = new MemoryStream(png))
                using (var tmp = new System.Drawing.Bitmap(ms))
                    return new System.Drawing.Bitmap(tmp); // independent copy; ms can be freed
            }
        }

        private static byte[] Encode(Image<Rgba32> img, OutputFormat fmt, int quality)
        {
            IImageEncoder enc;
            switch (fmt)
            {
                case OutputFormat.Jpeg: enc = new JpegEncoder { Quality = Clamp(quality, 1, 100) }; break;
                case OutputFormat.Webp: enc = new WebpEncoder { Quality = Clamp(quality, 1, 100) }; break;
                case OutputFormat.Gif: enc = new GifEncoder(); break;
                default: enc = new PngEncoder(); break;
            }
            using (var ms = new MemoryStream())
            {
                img.Save(ms, enc);
                return ms.ToArray();
            }
        }

        // Highest quality whose encoded size stays under the target (binary search).
        private static byte[] EncodeToTargetSize(Image<Rgba32> img, OutputFormat fmt,
            long targetBytes, out int usedQuality, out bool targetMet)
        {
            int low = 1, high = 100;
            byte[] best = null;
            int bestQ = 1;

            while (low <= high)
            {
                int mid = (low + high) / 2;
                byte[] candidate = Encode(img, fmt, mid);
                if (candidate.LongLength <= targetBytes)
                {
                    best = candidate;
                    bestQ = mid;
                    low = mid + 1; // try higher quality
                }
                else
                {
                    high = mid - 1;
                }
            }

            if (best != null)
            {
                usedQuality = bestQ;
                targetMet = true;
                return best;
            }

            // Even lowest quality exceeds the target: return the smallest we can make.
            usedQuality = 1;
            targetMet = false;
            return Encode(img, fmt, 1);
        }

        private static System.Drawing.Rectangle ClampRect(System.Drawing.Rectangle r, int w, int h)
        {
            int x = Math.Max(0, Math.Min(r.X, w));
            int y = Math.Max(0, Math.Min(r.Y, h));
            int rw = Math.Max(0, Math.Min(r.Width, w - x));
            int rh = Math.Max(0, Math.Min(r.Height, h - y));
            return new System.Drawing.Rectangle(x, y, rw, rh);
        }

        private static int Clamp(int v, int lo, int hi) => v < lo ? lo : (v > hi ? hi : v);

        public void Dispose()
        {
            _original?.Dispose();
            _original = null;
        }
    }
}
