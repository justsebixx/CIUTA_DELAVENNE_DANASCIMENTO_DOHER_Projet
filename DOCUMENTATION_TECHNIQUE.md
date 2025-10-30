# Documentation Technique – Jeu Snake

## I. Introduction

### A. Présentation du jeu
Développement d'une version moderne du jeu **Snake** en utilisant Godot Engine 4.5 et C#. Le joueur contrôle un serpent qui grandit en mangeant de la nourriture tout en évitant les murs, 30 obstacles fixes et son propre corps. Le jeu propose une accélération progressive tous les 5 points.

### B. Objectifs
Documenter l'architecture technique, le gameplay et les bonnes pratiques de développement.

---

## II. Architecture du Jeu

### A. Technologies
- **Godot Engine 4.5** - Moteur de jeu open-source
- **C# / .NET 8.0** - Langage de programmation
- **Résolution** : 600×450 pixels
- **Grille** : 30×20 cellules (20px par cellule)
- **Framerate** : 60 FPS

### B. Organisation des fichiers
```
/scripts
  ├── Main.cs                    # Contrôleur principal
  ├── BackgroundManager.cs       # Fond et grille
  ├── DeplacementManager.cs      # Mouvements et inputs
  ├── FoodManager.cs             # Nourriture
  ├── ObstacleManager.cs         # Obstacles
  └── GameOverScreen.cs          # Interface utilisateur
/scenes
  ├── Main.tscn                  # Scène principale
  └── game_over_screen.tscn      # Scène fin de jeu
```

---

## III. Gameplay

### A. Règles
- Départ : 3 segments, position centrale (15,10)
- Vitesse initiale : 0.15s par case
- **+1 point** et **+1 segment** par nourriture
- **Accélération** : -10ms tous les 5 points (min: 50ms)
- **30 obstacles** blancs générés aléatoirement
- **Game Over** : collision mur, obstacle ou auto-collision

### B. Contrôles
- **Flèches directionnelles** pour diriger le serpent
- Buffer de direction (empêche les demi-tours)

### C. Interface
- **Panneau score** (bas) : Score actuel (cyan), Record (or)
- **Écran Game Over** : Raison, statistiques, redémarrage auto 3s

---

## IV. Architecture Logicielle

### A. Classes principales

**Main.cs - Controlleur**
- Boucle de jeu
- Détection collisions (murs, obstacles, auto-collision)
- Gestion score et accélération
- Coordination des 5 managers

**BackgroundManager.cs**
- Création fond avec grille en damier
- Bordures et effets visuels

**DeplacementManager.cs**
- Capture inputs clavier
- Calcul prochaine position
- Validation anti-demi-tour

**FoodManager.cs**
- Génération position aléatoire (évite serpent/obstacles)
- Détection consommation d'une pomme

**ObstacleManager.cs**
- Génération 30 obstacles
- Protection zone centrale (spawn serpent)
- Détection collision

**UIManager.cs**
- Affichage scores temps réel
- Écran Game Over avec statistiques

### B. Flux d'exécution

**Initialisation :**
1. BackgroundManager → Fond et grille
2. UIManager → Panneau score
3. ObstacleManager → 30 obstacles
4. Serpent → 3 segments
5. FoodManager → Première nourriture
6. DeplacementManager → Prêt

**Boucle (60 FPS) :**
1. HandleInput() → Capture touches
2. Timer += delta → Accumuler temps
3. Si timer < moveInterval → Retour
4. GetNextPosition() → Calculer nouvelle tête
5. CheckCollisions() → Vérifier mur/obstacles/soi-même
6. CheckFoodEaten() → Vérifier nourriture
7. Déplacer serpent (ajouter tête, retirer queue si pas mangé)
8. UpdateScore() et accélération si nécessaire

---

## V. Optimisation

### A. Performances
- Framerate stable 60 FPS

### B. Bonnes pratiques
- **Architecture par composants** : Un Manager par fonctionnalité
- **Conventions** : PascalCase (classes/méthodes), camelCase (variables)

---

## VI. Installation

### A. Prérequis
- Godot Engine 4.5.1+ (.NET version)
- .NET SDK 8.0+
- 2 Go RAM minimum

### B. Étapes
1. Installer Godot (.NET) et .NET SDK 8.0
2. Cloner le dépôt GitHub
3. Ouvrir `project.godot` dans Godot
4. Appuyer sur F5 pour compiler et lancer

### C. Git - Format commits
- `feat:` Nouvelle fonctionnalité
- `fix:` Correction de bug
- `docs:` Documentation
- `refactor:` Refactorisation

---

## VII. Tests

### A. Tests unitaires (MSTest)
- Projet `JeuxSnake.Tests`
- Tests : scoring, collisions, etc...

### B. Tests fonctionnels
- Mouvements 4 directions
- Collisions (murs, obstacles, auto-collision)
- Score et high score
- Interface et Game Over

---

## VIII. Graphismes

### A. Palette de couleurs
- **Fond** : Bleu foncé avec grille damier
- **Serpent** : Tête vert citron, corps vert foncé
- **Nourriture** : Rouge vif + aura orange + point blanc
- **Obstacles** : Blanc pur
- **Interface** : Cyan (score), Or (record)

---

## IX. Améliorations Futures

- Menu principal et pause
- Effets sonores et musique

---

## X. Informations Projet

### Équipe
- **Membres** : CIUTA, DELAVENNE, DANASCIMENTO, DOHER
- **Formation** : BUT Informatique 2025-2026
- **Encadrant** : TMareIUT

### Dépôt GitHub
[CIUTA_DELAVENNE_DANASCIMENTO_DOHER_Projet](https://github.com/justsebixx/CIUTA_DELAVENNE_DANASCIMENTO_DOHER_Projet)

**Dernière mise à jour** : 30 octobre 2025  
**Version du document** : 1.0