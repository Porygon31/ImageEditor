# ImageEditor

Application Windows (WinForms, .NET Framework 4.8) pour préparer une photo avant de
l'uploader comme **avatar** ou **bannière** sur un site web : redimensionner, recadrer,
convertir de format et compresser jusqu'à un poids cible.

Traitement d'image assuré par [SixLabors.ImageSharp](https://github.com/SixLabors/ImageSharp) 2.1.

## Fonctionnalités

- **Ouvrir** une image (bouton ou glisser-déposer).
- **Redimensionner** (largeur/hauteur, option « garder le ratio »).
- **Recadrer** :
  - *Cover* — remplit la zone cible puis rogne le débord (centré).
  - *Contain* — l'image entière tient dans la zone, avec marges.
  - *Manuel* — dessin de la zone à la souris, puis ajustement des bords/coins
    (8 poignées) ; dimensions affichées en direct.
- **Convertir** : PNG, JPEG, WebP, GIF.
- **Compresser** : slider de qualité, ou **poids cible en Ko** (la qualité JPEG/WebP est
  baissée automatiquement par recherche dichotomique pour passer sous la cible).
- Aperçu live et poids estimé.

## Build

Ouvrir `ImageEditor.sln` dans Visual Studio (2019+) et lancer (F5).

En ligne de commande :

```
msbuild ImageEditor.sln -t:Restore,Build -p:Configuration=Release
```

Nécessite les assemblies de référence .NET Framework 4.8.

## Structure

- `ImageEditor/ImageProcessor.cs` — cœur du traitement ImageSharp (sans dépendance UI).
- `ImageEditor/MainForm.cs` / `MainForm.Designer.cs` — interface WinForms.
- `ImageEditor/Program.cs` — point d'entrée.
