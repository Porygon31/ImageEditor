namespace ImageEditor
{
    /// <summary>
    /// Timer utilisé par l'interface WinForms.
    ///
    /// Pourquoi cette petite classe existe ?
    /// MainForm.cs utilise à la fois :
    /// - System.Threading, pour l'annulation des traitements asynchrones ;
    /// - System.Windows.Forms, pour l'interface graphique.
    ///
    /// Ces deux bibliothèques contiennent une classe appelée "Timer".
    /// Sans cette précision, le compilateur ne sait pas laquelle choisir.
    ///
    /// En héritant explicitement de System.Windows.Forms.Timer, tous les
    /// "Timer" utilisés dans le namespace ImageEditor correspondent maintenant
    /// au timer prévu pour l'interface graphique.
    /// </summary>
    internal sealed class Timer : System.Windows.Forms.Timer
    {
        // Aucun code supplémentaire n'est nécessaire.
        // Toute la logique est déjà fournie par System.Windows.Forms.Timer.
    }
}
