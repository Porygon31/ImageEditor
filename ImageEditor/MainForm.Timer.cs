using System;

namespace ImageEditor
{
    // Cette partie de MainForm contient uniquement le petit wrapper du Timer.
    // Le type est placé directement dans MainForm pour que MainForm.cs et
    // MainForm.History.cs puissent continuer à écrire simplement "Timer".
    public partial class MainForm
    {
        /// <summary>
        /// Petit wrapper autour du Timer WinForms.
        ///
        /// Le vrai Timer reste privé. Le reste du programme utilise seulement
        /// Interval, Tick, Start(), Stop() et Dispose().
        ///
        /// Comme cette classe est imbriquée dans MainForm et n'hérite pas de
        /// Component, Visual Studio n'a aucune raison de modifier le .csproj.
        /// </summary>
        private sealed class Timer : IDisposable
        {
            private readonly System.Windows.Forms.Timer _innerTimer =
                new System.Windows.Forms.Timer();

            /// <summary>
            /// Délai du Timer, en millisecondes.
            /// </summary>
            public int Interval
            {
                get { return _innerTimer.Interval; }
                set { _innerTimer.Interval = value; }
            }

            /// <summary>
            /// Événement déclenché lorsque le délai est écoulé.
            /// </summary>
            public event EventHandler Tick
            {
                add { _innerTimer.Tick += value; }
                remove { _innerTimer.Tick -= value; }
            }

            public void Start()
            {
                _innerTimer.Start();
            }

            public void Stop()
            {
                _innerTimer.Stop();
            }

            public void Dispose()
            {
                _innerTimer.Dispose();
            }
        }
    }
}
