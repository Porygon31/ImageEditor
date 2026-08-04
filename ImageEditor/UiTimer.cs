using System;

namespace ImageEditor
{
    /// <summary>
    /// Petit wrapper autour du Timer WinForms utilisé par l'interface.
    ///
    /// Pourquoi ne pas hériter directement de System.Windows.Forms.Timer ?
    /// Un Timer WinForms est aussi un "Component". Visual Studio modifiait donc
    /// automatiquement le fichier .csproj pour ajouter :
    ///
    ///     <SubType>Component</SubType>
    ///
    /// Cette modification revenait après chaque annulation Git.
    ///
    /// Ici, la classe contient le vrai Timer au lieu d'en hériter. Pour le reste
    /// du programme, son utilisation reste identique : Interval, Tick, Start,
    /// Stop et Dispose sont simplement transmis au Timer WinForms interne.
    /// </summary>
    internal sealed class Timer : IDisposable
    {
        // Le vrai Timer WinForms reste privé : seul ce petit wrapper l'utilise.
        private readonly System.Windows.Forms.Timer _innerTimer =
            new System.Windows.Forms.Timer();

        /// <summary>
        /// Temps d'attente entre deux déclenchements, en millisecondes.
        /// </summary>
        public int Interval
        {
            get { return _innerTimer.Interval; }
            set { _innerTimer.Interval = value; }
        }

        /// <summary>
        /// Indique si le Timer est actuellement actif.
        /// </summary>
        public bool Enabled
        {
            get { return _innerTimer.Enabled; }
            set { _innerTimer.Enabled = value; }
        }

        /// <summary>
        /// Événement déclenché lorsque le délai du Timer est écoulé.
        /// </summary>
        public event EventHandler Tick
        {
            add { _innerTimer.Tick += value; }
            remove { _innerTimer.Tick -= value; }
        }

        /// <summary>
        /// Démarre ou redémarre le Timer.
        /// </summary>
        public void Start()
        {
            _innerTimer.Start();
        }

        /// <summary>
        /// Arrête le Timer sans le supprimer.
        /// </summary>
        public void Stop()
        {
            _innerTimer.Stop();
        }

        /// <summary>
        /// Libère le Timer WinForms interne quand il n'est plus utilisé.
        /// </summary>
        public void Dispose()
        {
            _innerTimer.Dispose();
        }
    }
}
