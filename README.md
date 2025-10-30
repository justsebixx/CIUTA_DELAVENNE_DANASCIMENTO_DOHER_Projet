# Snake Game - Godot C#

Implémentation moderne du jeu Snake développée avec **Godot 4.x** et **C#**, mettant en avant une architecture modulaire et une documentation complète du code.

![Godot](https://img.shields.io/badge/Godot-4.x-478CBF?logo=godot-engine&logoColor=white)
![C#](https://img.shields.io/badge/C%23-12.0-239120?logo=csharp&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-green)

## Table des matières

- [Aperçu](#aperçu)
- [Fonctionnalités](#fonctionnalités)
- [Architecture](#architecture)

---

## Aperçu

Ce projet est une implémentation complète du jeu Snake démontrant les bonnes pratiques de développement en C# avec Godot Engine. Le code source met l'accent sur :

- **Architecture modulaire** avec séparation des responsabilités via le pattern Manager
- **Documentation exhaustive** avec commentaires détaillés à visée pédagogique
- **Difficulté progressive** avec augmentation automatique de la vitesse selon le score
- **Principes de code propre** respectant les conventions C# et Godot

**Spécifications de la grille** : 30×20 cases (600×400 pixels)  
**Évolution de la difficulté** : Ajustement automatique de la vitesse tous les 5 points

---

## Fonctionnalités

### Gameplay principal
- Serpent initial de 3 segments au centre de la grille
- Mécanisme de croissance lors de la consommation de nourriture
- 30 obstacles générés aléatoirement par session de jeu
- Zone de spawn protégée (5×5 cases) garantissant un départ équitable
- Augmentation progressive de la vitesse (intervalle de 150ms à 50ms minimum)
- Détection complète des collisions (murs, obstacles, auto-collision)

### Interface utilisateur
- Affichage en temps réel du score et du meilleur score
- Écran de Game Over avec statistiques détaillées
- Redémarrage automatique après un compte à rebours de 3 secondes
- Grille semi-transparente pour le guidage visuel

### Implémentation technique
- Architecture basée sur des Managers pour une séparation claire des responsabilités
- Code commenté de manière exhaustive à des fins d'apprentissage
- Aucune dépendance externe
- Syntaxe et fonctionnalités modernes de C# 12

---

## Architecture

Le projet implémente le **Pattern Manager** pour séparer les responsabilités :
Main.cs (Orchestrateur du jeu)
- BackgroundManager.cs    → Rendu de la grille et arrière-plan visuel
- DeplacementManager.cs   → Gestion des entrées et directions
- ObstacleManager.cs      → Génération et détection des obstacles
- FoodManager.cs          → Apparition et détection de la nourriture
- UIManager.cs            → Affichage du score et interface Game Over
- SnakeController.cs      → Logique du serpent (mouvement, croissance, auto-collision)

### Flux de données
1. **Main** initialise toutes les instances des managers
2. **_Process()** capture les entrées via `DeplacementManager`
3. **Main** calcule la prochaine position et valide selon les règles de collision
4. **FoodManager** vérifie la consommation de nourriture
5. **Main** met à jour l'état du serpent et synchronise les visuels

---

