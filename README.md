<div align="center">

# ImageEditor

**Un éditeur d’image Windows simple pour redimensionner, recadrer, convertir et compresser une image avant de la publier.**

![C#](https://img.shields.io/badge/C%23-WinForms-512BD4?logo=dotnet&logoColor=white)
![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.8-512BD4)
![Windows](https://img.shields.io/badge/Plateforme-Windows-0078D6?logo=windows&logoColor=white)
![État](https://img.shields.io/badge/%C3%89tat-En%20d%C3%A9veloppement-orange)

</div>

## Présentation

ImageEditor est une application de bureau développée en **C# avec WinForms**. Elle permet de préparer rapidement une image destinée à un avatar, une bannière, une miniature ou un site web.

L’application regroupe dans une seule interface :

- le redimensionnement ;
- plusieurs méthodes de recadrage ;
- la conversion de format ;
- la compression avec estimation du poids ;
- la recherche automatique d’une qualité adaptée à un poids cible ;
- un historique **Annuler / Rétablir**.

Le traitement des images est réalisé avec **SixLabors.ImageSharp**.

## Fonctionnalités

### Chargement

- Ouverture d’une image avec le bouton dédié.
- Glisser-déposer d’un fichier directement dans la fenêtre.
- Formats d’entrée courants pris en charge par ImageSharp, notamment PNG, JPEG, BMP, GIF, WebP et TIFF.

### Redimensionnement

- Choix libre de la largeur et de la hauteur en pixels.
- Conservation facultative du ratio de l’image originale.
- Mise à jour automatique de la hauteur ou de la largeur lorsque le ratio est conservé.

### Recadrage

| Mode | Fonctionnement |
|---|---|
| **Aucun** | Redimensionne l’image sans recadrage forcé. |
| **Cover** | Remplit entièrement la taille demandée puis rogne le débord au centre. |
| **Contain** | Conserve toute l’image dans la zone cible, avec des marges si nécessaire. |
| **Manuel** | Permet de dessiner et d’ajuster une zone de recadrage avec la souris. |

Le recadrage manuel comprend :

- huit poignées de redimensionnement ;
- le déplacement de la sélection ;
- l’affichage en direct de ses dimensions ;
- un assombrissement de la partie située en dehors de la sélection.

### Conversion et compression

Formats de sortie disponibles :

| Format | Utilisation conseillée |
|---|---|
| **JPEG** | Photographies et images sans transparence. |
| **PNG** | Images sans perte et images avec transparence. |
| **WebP** | Images web légères avec une bonne qualité. |
| **GIF** | Export au format GIF. |

Pour JPEG et WebP, l’application permet :

- de choisir manuellement la qualité ;
- de définir un poids maximum en Ko ;
- de rechercher automatiquement la meilleure qualité permettant de respecter la cible ;
- d’afficher une alerte lorsque le poids demandé ne peut pas être atteint.

### Aperçu et interface

- Aperçu mis à jour automatiquement.
- Affichage du poids estimé avant l’enregistrement.
- Affichage des dimensions finales.
- Fond quadrillé permettant de visualiser la transparence.
- Interface organisée en sections pour séparer les dimensions, le recadrage et l’export.

### Traitement asynchrone

Les rendus lourds sont réalisés en arrière-plan afin d’éviter de bloquer la fenêtre.

Le système comprend :

- un délai court avant le recalcul pour regrouper les changements rapides ;
- l’annulation logique des aperçus devenus inutiles ;
- une protection empêchant plusieurs traitements d’utiliser simultanément la même image ;
- l’enregistrement du fichier sans bloquer l’interface.

### Historique

Les réglages peuvent être annulés et rétablis sans conserver de copie complète de l’image en mémoire.

L’historique mémorise notamment :

- les dimensions ;
- le ratio ;
- le mode et la zone de recadrage ;
- le format de sortie ;
- la qualité ;
- le poids cible.

| Action | Raccourci |
|---|---|
| Annuler | `Ctrl + Z` |
| Rétablir | `Ctrl + Y` |
| Rétablir, raccourci alternatif | `Ctrl + Maj + Z` |

L’historique est limité à **100 étapes** et est réinitialisé lors du chargement d’une nouvelle image.

## Prérequis

Pour compiler le projet :

- Windows 10 ou Windows 11 ;
- Visual Studio 2019 ou plus récent ;
- la charge de travail **Développement Desktop en .NET** ;
- les outils de développement **.NET Framework 4.8** ;
- NuGet pour restaurer ImageSharp.

> Aucun exécutable précompilé n’est publié pour le moment. Le projet doit être compilé depuis les sources.

## Installation depuis les sources

### Avec Visual Studio

1. Cloner le dépôt :

```powershell
git clone https://github.com/Porygon31/ImageEditor.git
cd ImageEditor
```

2. Ouvrir :

```text
ImageEditor.sln
```

3. Attendre la restauration des packages NuGet.
4. Choisir la configuration `Debug` ou `Release`.
5. Lancer le projet avec `F5`.

### En ligne de commande

Depuis un terminal Visual Studio ou une machine disposant de MSBuild :

```powershell
msbuild ImageEditor.sln -t:Restore,Build -p:Configuration=Release
```

L’exécutable compilé est ensuite disponible dans :

```text
ImageEditor\bin\Release\
```

## Utilisation

1. Ouvrir ou glisser-déposer une image.
2. Choisir la largeur et la hauteur de sortie.
3. Sélectionner le mode de recadrage.
4. Choisir le format d’export.
5. Régler la qualité ou activer le poids cible.
6. Vérifier l’aperçu et le poids estimé.
7. Cliquer sur **Enregistrer**.

## Structure du projet

```text
ImageEditor/
├── ImageEditor.sln
├── README.md
└── ImageEditor/
    ├── ImageEditor.csproj
    ├── ImageProcessor.cs
    ├── MainForm.cs
    ├── MainForm.Designer.cs
    ├── MainForm.History.cs
    ├── MainForm.Timer.cs
    ├── ModernControls.cs
    ├── Program.cs
    └── Properties/
```

### Rôle des fichiers principaux

| Fichier | Rôle |
|---|---|
| `ImageProcessor.cs` | Chargement, redimensionnement, recadrage, encodage et recherche du poids cible avec ImageSharp. |
| `MainForm.cs` | Logique principale de l’interface, aperçu asynchrone, enregistrement et recadrage manuel. |
| `MainForm.Designer.cs` | Définition des contrôles et de la disposition de la fenêtre. |
| `MainForm.History.cs` | Gestion de l’historique Annuler / Rétablir. |
| `MainForm.Timer.cs` | Timer utilisé pour regrouper les changements rapides avant recalcul. |
| `ModernControls.cs` | Contrôles personnalisés utilisés par la nouvelle interface. |
| `Program.cs` | Point d’entrée de l’application. |

## Fonctionnement du poids cible

Pour JPEG et WebP, ImageEditor réalise plusieurs encodages avec différentes valeurs de qualité.

Une recherche dichotomique permet de trouver rapidement la qualité la plus élevée qui reste sous le poids demandé. Lorsque même la qualité minimale est trop lourde, l’application conserve le meilleur résultat disponible et prévient l’utilisateur.

## Limites actuelles

- Le traitement par lot n’est pas encore disponible.
- Il n’existe pas encore de comparaison interactive avant/après.
- Le poids cible ajuste la qualité, mais ne réduit pas automatiquement les dimensions.
- Aucune version installable ou portable n’est encore publiée dans les Releases.
- Le projet référence actuellement ImageSharp `2.1.9` ; NuGet peut signaler des avertissements de sécurité `NU1902` et `NU1903` tant que la dépendance n’a pas été mise à jour.

## Idées d’évolution

- comparaison avant/après ;
- préréglages pour avatars, bannières et réseaux sociaux ;
- traitement de plusieurs images en lot ;
- réduction automatique des dimensions pour atteindre un poids cible ;
- création d’une version portable ;
- ajout de tests automatisés pour le traitement des images.

## Contribution

Les signalements de bugs et les propositions d’amélioration peuvent être ajoutés dans les issues GitHub.

Pour proposer une modification :

1. créer une branche ;
2. effectuer et tester les changements ;
3. ouvrir une pull request avec une description claire.

---

Développé par [Porygon31](https://github.com/Porygon31).
