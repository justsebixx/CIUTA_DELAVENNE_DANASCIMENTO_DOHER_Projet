# Cahier des charges - Projet Snake

## 1. Présentation générale

### Titre du projet
**Jeu Snake**

### Membres du projet

Sebastian-Cristian Ciuta

Théo Da Nascimento

Baptiste Delavenne

Eyma Doher

### Contexte / Origine de la demande
Cours de qualité de développement

### Objectifs généraux
Développer un jeu Snake dans le langage de programmation que l'on souhaite (C# pour notre projet), en respectant les normes de codage, rédiger un cahier des charges, un plan de test et une documentation technique complète, effectuer des tests unitaires avec un framework de test, pousser le code sur GitHub et effectuer une revue de code sur les pull requests.

### Portée du projet

**Inclus :**
- Code et commentaires
- Cahier des charges
- Plan de test
- Documentation technique
- Tests unitaires
- GitHub
- Revue de code

**Exclus :**
- Code repris sur Internet
- Avoir le même code qu'un autre groupe
- Un jeu trop compliqué à développer

---

## 2. Acteurs du projet

- **Commanditaire** : Tony Maré
- **Chef de projet** : Sebastian-Cristian Ciuta
- **Utilisateurs finaux** : Les joueurs
- **Parties prenantes / intervenants externes** : Équipe de développement et Tony Maré

---

## 3. Besoins fonctionnels

### Fonctionnalités attendues

1. Serpent qui se déplace vers la gauche, la droite et tout droit, sans pouvoir faire demi-tour sur lui-même.

2. Une pomme qui apparaît aléatoirement sur la carte quand le serpent la mange, sans se retrouver sur le corps du serpent.

3. Les obstacles apparaissent à une position aléatoirement à chaque fois qu'on relance le jeu.

4. Une page de Game Over qui se déclenche au bon moment, quand le serpent mange une bombe ou quand le serpent rentre dans le mur ou dans son corps.

### Cas d'usage / scénarios d'utilisation
Un joueur qui a envie de jouer à un mini-jeu simple et rapide pour se détendre.

---

## 4. Contraintes techniques

### Technologies imposées
Choix libre pour le choix des technologies, dans notre cas on utilise **C#** sous le moteur de jeu **Godot Engine**.

### Compatibilité
- **Plateforme** : Windows

### Normes et standards à respecter
Non applicable

### Sécurité et confidentialité
Non applicable

---

## 5. Contraintes organisationnelles

### Planning prévisionnel
**Dernier délai** : 31 octobre 2025 avant 23h59

### Budget estimé
Pas de budget

### Ressources disponibles
4 ordinateurs portables personnels pour l'équipe de développement

---

## 6. Livrables attendus

### Documents
- Le code source zippé
- Le cahier des charges
- La documentation technique
- Le plan de test
- Un exécutable de votre application
- Un fichier texte avec le lien du repo Git

### Produits
Un jeu Snake

### Tests et validations
- Tests unitaires
- Pull requests Git
- Revue de code
- Code commenté

---

## 7. Critères de réussite

### Indicateurs de qualité
- Le code respecte les bonnes pratiques de développement et est commenté
- Un GitHub a été mis en place et bien structuré
- La logique de collisions est fiable, sans faux positifs ni faux négatifs observés

### Critères d'acceptation

#### Fonctionnalités
- Le serpent ne peut jamais effectuer de demi-tour instantané
- La pomme apparaît après chaque consommation à une position libre, jamais sur le serpent
- L'obstacle apparaît à une position aléatoirement à chaque fois qu'on relance le jeu
- L'écran Game Over s'affiche lorsque le serpent heurte un mur, son corps ou une bombe, avec possibilité de relancer une partie

#### Jouabilité
- Déplacements du serpent continus, sans à-coups visibles
- Les entrées clavier sont prises en compte dans la frame en cours ou la suivante

#### Qualité logicielle
- Le dépôt GitHub contient : README complet, plan de test, documentation technique, conventions de branches et modèle de PR
- Le code respecte les conventions de nommage définies

#### Livraison
- Un exécutable Windows fourni (ou export Godot) et testable sans installer Godot

#### Support/Compatibilité
- Le jeu démarre et fonctionne sur la machine cible de démo (Windows) sans dépendances supplémentaires non documentées

### Métriques de performance
- **Fluidité** : 60 FPS stables en 1080p sur la machine de démo
- **Réactivité** : changement de direction pris en compte en ≤ 1 frame

---

**Date de rédaction** : [Date]  
**Version** : 1.0  
**Statut** : Validé
