using Godot;
using System.Collections.Generic;

// Classe principale - Orchestre tous les managers et gère la boucle de jeu.
// Responsabilité : Initialisation, game loop, détection collisions, game over, score.
public partial class Main : Node2D
{
	// Constantes de la grille
	public const int CellSize = 20; // Taille d'une case en pixels
	public const int GridWidth = 30; // Largeur en cases (600px)
	public const int GridHeight = 20; // Hauteur en cases (400px)
	
	// Managers - Séparation des responsabilités
	private BackgroundManager backgroundManager; // Gère le fond de grille
	private DeplacementManager deplacementManager; // Gère les inputs directionnels
	private ObstacleManager obstacleManager; // Génère et détecte les obstacles
	private FoodManager foodManager; // Spawne et détecte la nourriture
	private UIManager uiManager; // Affiche scores et écran de game over
	
	// Snake - Données et visuels
	private List<Vector2> snake = new List<Vector2>(); // Positions des segments sur la grille
	private List<ColorRect> snakeRects = new List<ColorRect>(); // Représentations visuelles
	
	// Game State - Variables d'état du jeu
	private float timer = 0; // Compteur pour le déplacement automatique
	private float moveInterval = 0.15f; // Intervalle entre chaque déplacement (150ms au départ)
	private int score = 0; // Score actuel
	private int highScore = 0; // Meilleur score de la session
	private bool isGameOver = false; // Bloque le jeu si true

	// Méthode Godot appelée au lancement de la scène
	public override void _Ready()
	{
		InitializeGame();
	}

	// Initialise tous les managers et l'état de départ du jeu.
	// Ordre important : Background → UI → Obstacles → Snake → Food
	private void InitializeGame()
	{
		// Background - Dessine la grille de fond
		backgroundManager = new BackgroundManager();
		AddChild(backgroundManager); // Ajoute à l'arbre de scène
		backgroundManager.Initialize();
		
		// UI - Crée le panneau de score en bas
		uiManager = new UIManager();
		AddChild(uiManager);
		uiManager.Initialize();
		
		// Obstacles - Génère 30 obstacles aléatoires
		obstacleManager = new ObstacleManager();
		AddChild(obstacleManager);
		obstacleManager.Initialize();
		
		// Snake initial - 3 segments alignés horizontalement
		// Position de départ : (15,10) = centre de la grille
		snake.Clear();
		snake.Add(new Vector2(15, 10)); // Tête
		snake.Add(new Vector2(14, 10)); // Corps 1
		snake.Add(new Vector2(13, 10)); // Corps 2
		
		DrawSnake(); // Crée les ColorRect pour chaque segment
		
		// Food - Spawne la première nourriture (évite snake et obstacles)
		foodManager = new FoodManager();
		AddChild(foodManager);
		foodManager.Initialize(snake, obstacleManager.GetObstaclePositions());
		
		// Déplacement - Gère les inputs clavier
		deplacementManager = new DeplacementManager();
		AddChild(deplacementManager);
		
		// Reset des variables d'état
		score = 0;
		isGameOver = false;
		uiManager.UpdateScore(score, ref highScore); // Affiche "Score: 0"
		
		GD.Print(" Jeu Snake lancé !");
	}

	// Boucle principale - Appelée à chaque frame (60 FPS).
	// delta : temps écoulé depuis la dernière frame (≈0.016s à 60 FPS)
	public override void _Process(double delta)
	{
		if (isGameOver) return; // Bloque la boucle si game over
		
		// Input - Détecte les touches directionnelles
		deplacementManager.HandleInput();
		
		// Timer - Accumule le temps pour déclencher le mouvement
		timer += (float)delta;
		if (timer < moveInterval) return; // Pas encore le moment de bouger
		timer = 0; // Reset du timer
		
		// Calculer nouvelle position de la tête
		Vector2 currentHead = snake[0]; // Position actuelle de la tête
		Vector2 newHead = deplacementManager.GetNextPosition(currentHead);
		
		// Vérifier collisions (murs, obstacles, auto-collision)
		if (CheckCollisions(newHead))
			return; // Game over détecté, arrête le traitement
		
		// Vérifier si la nourriture est mangée
		bool hasEaten = foodManager.CheckFoodEaten(newHead);
		
		// Déplacer le serpent - Ajoute nouvelle tête
		snake.Insert(0, newHead);
		
		if (hasEaten)
		{
			// Croissance : garde la queue (ne la supprime pas)
			score++;
			uiManager.UpdateScore(score, ref highScore);
			foodManager.SpawnFood(snake, obstacleManager.GetObstaclePositions());
			GD.Print($" Miam ! Score: {score}");
			
			// Accélération progressive - Tous les 5 points, réduit l'intervalle
			if (score % 5 == 0 && moveInterval > 0.05f) // Limite minimale : 50ms
			{
				moveInterval -= 0.01f; // Réduit de 10ms
				GD.Print($" Vitesse augmentée ! Intervalle: {moveInterval:F2}s");
			}
		}
		else
		{
			// Pas de nourriture : retire la queue pour simuler le mouvement
			snake.RemoveAt(snake.Count - 1);
		}
		
		UpdateSnake(); // Synchronise les visuels avec les données
	}

	// Vérifie toutes les collisions possibles pour la nouvelle position.
	// newHead : position calculée de la prochaine tête
	// Retourne : true si collision détectée (déclenche game over)
	private bool CheckCollisions(Vector2 newHead)
	{
		// Collision mur - Vérifie les limites de la grille
		if (newHead.X < 0 || newHead.X >= GridWidth || 
			newHead.Y < 0 || newHead.Y >= GridHeight)
		{
			GameOver(" Collision avec le mur !");
			return true;
		}
		
		// Collision obstacle - Délègue au ObstacleManager
		if (obstacleManager.CheckCollision(newHead))
		{
			GameOver(" Collision avec un obstacle !");
			return true;
		}
		
		// Auto-collision - La tête touche le corps
		if (snake.Contains(newHead))
		{
			GameOver(" Collision avec soi-même !");
			return true;
		}
		
		return false; // Aucune collision
	}

	// Déclenche la séquence de game over : affichage, attente, redémarrage.
	// reason : message affiché à l'écran (ex: "💥 Collision avec le mur !")
	// async void : permet d'attendre 3 secondes sans bloquer Godot
	private async void GameOver(string reason)
	{
		isGameOver = true; // Stoppe la boucle _Process
		GD.Print($" {reason} | Score: {score}");
		
		// Masquer le jeu (garde seulement le fond)
		backgroundManager.Visible = true; // Fond reste visible
		obstacleManager.Visible = false;
		foodManager.Visible = false;
		
		// Masquer tous les segments du serpent
		foreach (var rect in snakeRects)
		{
			rect.Visible = false;
		}
		
		// Afficher l'écran de game over (attend 3 secondes)
		await uiManager.ShowGameOver(reason, score, highScore, snake.Count, obstacleManager.GetObstaclePositions().Count);
		
		// Nettoyer et redémarrer
		ClearSnake();
		GetTree().ReloadCurrentScene(); // Recharge la scène Main.tscn
	}

	// Crée les visuels initiaux du serpent (3 segments au départ).
	// Appelé une seule fois dans InitializeGame().
	private void DrawSnake()
	{
		for (int i = 0; i < snake.Count; i++)
		{
			ColorRect rect = new ColorRect();
			rect.Size = new Vector2(CellSize - 2, CellSize - 2); // 18x18 pixels
			rect.Position = snake[i] * CellSize + Vector2.One; // Convertit grille → pixels
			
			// Tête en vert clair, corps en vert foncé
			if (i == 0)
				rect.Color = new Color(0.3f, 1.0f, 0.3f); // Tête
			else
				rect.Color = new Color(0.2f, 0.8f, 0.2f); // Corps
			
			AddChild(rect);
			snakeRects.Add(rect);
		}
	}

	// Met à jour les visuels après chaque déplacement ou croissance.
	// Crée de nouveaux segments si le serpent a grandi.
	private void UpdateSnake()
	{
		// Ajouter des ColorRect si le serpent a grandi (après manger)
		while (snakeRects.Count < snake.Count)
		{
			ColorRect rect = new ColorRect();
			rect.Size = new Vector2(CellSize - 2, CellSize - 2);
			rect.Color = new Color(0.2f, 0.8f, 0.2f); // Couleur de corps
			AddChild(rect);
			snakeRects.Add(rect);
		}
		
		// Synchroniser positions et couleurs de tous les segments
		for (int i = 0; i < snake.Count; i++)
		{
			snakeRects[i].Position = snake[i] * CellSize + Vector2.One;
			
			// Colorer la tête différemment
			if (i == 0)
				snakeRects[i].Color = new Color(0.3f, 1.0f, 0.3f); // Tête
			else
				snakeRects[i].Color = new Color(0.2f, 0.8f, 0.2f); // Corps
		}
	}

	// Supprime tous les segments du serpent (données + visuels).
	// Appelé avant de redémarrer la scène.
	private void ClearSnake()
	{
		foreach (var rect in snakeRects)
		{
			if (rect.IsInsideTree()) // Vérifie que le nœud existe encore
				rect.QueueFree(); // Suppression propre Godot
		}
		snakeRects.Clear();
		snake.Clear();
	}
}
