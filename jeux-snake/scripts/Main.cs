using Godot;
using System.Collections.Generic;

public partial class Main : Node2D
{
	public const int CellSize = 20;
	public const int GridWidth = 30; 
	public const int GridHeight = 20; 
	
	private BackgroundManager backgroundManager;
	private DeplacementManager deplacementManager; 
	private ObstacleManager obstacleManager; 
	private FoodManager foodManager; 
	private UIManager uiManager; 
	
	private List<Vector2> snake = new List<Vector2>();
	private List<ColorRect> snakeRects = new List<ColorRect>(); 
	
	
	private float timer = 0; 
	private float moveInterval = 0.15f; 
	private int score = 0; 
	private int highScore = 0; 
	private bool isGameOver = false; 

	// Méthode Godot appelée au lancement de la scène
	public override void _Ready()
	{
		InitializeGame();
	}

	// Initialise tous les managers et l'état de départ du jeu.
	private void InitializeGame()
	{
		// Background Dessine la grille de fond
		backgroundManager = new BackgroundManager();
		AddChild(backgroundManager); 
		backgroundManager.Initialize();
		
		// UI Crée le panneau de score en bas
		uiManager = new UIManager();
		AddChild(uiManager);
		uiManager.Initialize();
		
		// Obstacles Génère 30 obstacles aléatoires
		obstacleManager = new ObstacleManager();
		AddChild(obstacleManager);
		obstacleManager.Initialize();
		
		// Snake initial 3 segments alignés horizontalement
		snake.Clear();
		snake.Add(new Vector2(15, 10)); 
		snake.Add(new Vector2(14, 10)); 
		snake.Add(new Vector2(13, 10)); 
		
		DrawSnake(); 
		
		foodManager = new FoodManager();
		AddChild(foodManager);
		foodManager.Initialize(snake, obstacleManager.GetObstaclePositions());
		
		// Déplacement - Gère les inputs clavier
		deplacementManager = new DeplacementManager();
		AddChild(deplacementManager);
		
		score = 0;
		isGameOver = false;
		uiManager.UpdateScore(score, ref highScore); 
		
		GD.Print(" Jeu Snake lancé !");
	}

	// Boucle principale - Appelée à chaque frame (60 FPS).
	public override void _Process(double delta)
	{
		if (isGameOver) return; 
		
		// Input - Détecte les touches directionnelles
		deplacementManager.HandleInput();
		
		timer += (float)delta;
		if (timer < moveInterval) return; 
		timer = 0; 
		
		// Calculer nouvelle position de la tête
		Vector2 currentHead = snake[0]; 
		Vector2 newHead = deplacementManager.GetNextPosition(currentHead);
		
		if (CheckCollisions(newHead))
			return; 
		
		bool hasEaten = foodManager.CheckFoodEaten(newHead);
		
		snake.Insert(0, newHead);
		
		if (hasEaten)
		{
			score++;
			uiManager.UpdateScore(score, ref highScore);
			foodManager.SpawnFood(snake, obstacleManager.GetObstaclePositions());
			GD.Print($" Miam ! Score: {score}");
			
			if (score % 5 == 0 && moveInterval > 0.05f) 
			{
				moveInterval -= 0.01f; 
				GD.Print($" Vitesse augmentée ! Intervalle: {moveInterval:F2}s");
			}
		}
		else
		{
			
			snake.RemoveAt(snake.Count - 1);
		}
		
		UpdateSnake();
	}

	// Vérifie toutes les collisions possibles pour la nouvelle position.
	private bool CheckCollisions(Vector2 newHead)
	{
		// Collision mur - Vérifie les limites de la grille
		if (newHead.X < 0 || newHead.X >= GridWidth || 
			newHead.Y < 0 || newHead.Y >= GridHeight)
		{
			GameOver(" Collision avec le mur !");
			return true;
		}
		
		if (obstacleManager.CheckCollision(newHead))
		{
			GameOver(" Collision avec un obstacle !");
			return true;
		}
		
		if (snake.Contains(newHead))
		{
			GameOver(" Collision avec soi-même !");
			return true;
		}
		
		return false; 
	}

	// Déclenche la séquence de game over : affichage, attente, redémarrage.
	private async void GameOver(string reason)
	{
		isGameOver = true; // Stoppe la boucle _Process
		GD.Print($" {reason} | Score: {score}");
		
		// Masquer le jeu (garde seulement le fond)
		backgroundManager.Visible = true;
		obstacleManager.Visible = false;
		foodManager.Visible = false;
		
		foreach (var rect in snakeRects)
		{
			rect.Visible = false;
		}
		
		await uiManager.ShowGameOver(reason, score, highScore, snake.Count, obstacleManager.GetObstaclePositions().Count);
		
		ClearSnake();
		GetTree().ReloadCurrentScene(); 
	}

	// Crée les visuels initiaux du serpent (3 segments au départ).
	private void DrawSnake()
	{
		for (int i = 0; i < snake.Count; i++)
		{
			ColorRect rect = new ColorRect();
			rect.Size = new Vector2(CellSize - 2, CellSize - 2); 
			rect.Position = snake[i] * CellSize + Vector2.One; 
			
			if (i == 0)
				rect.Color = new Color(0.3f, 1.0f, 0.3f); 
			else
				rect.Color = new Color(0.2f, 0.8f, 0.2f); 
			
			AddChild(rect);
			snakeRects.Add(rect);
		}
	}

	// Met à jour les visuels après chaque déplacement ou croissance.
	private void UpdateSnake()
	{
		while (snakeRects.Count < snake.Count)
		{
			ColorRect rect = new ColorRect();
			rect.Size = new Vector2(CellSize - 2, CellSize - 2);
			rect.Color = new Color(0.2f, 0.8f, 0.2f); 
			AddChild(rect);
			snakeRects.Add(rect);
		}
		
		for (int i = 0; i < snake.Count; i++)
		{
			snakeRects[i].Position = snake[i] * CellSize + Vector2.One;
			
			if (i == 0)
				snakeRects[i].Color = new Color(0.3f, 1.0f, 0.3f); 
			else
				snakeRects[i].Color = new Color(0.2f, 0.8f, 0.2f);
		}
	}

	// Supprime tous les segments du serpent (données + visuels).
	private void ClearSnake()
	{
		foreach (var rect in snakeRects)
		{
			if (rect.IsInsideTree()) 
				rect.QueueFree(); 
		}
		snakeRects.Clear();
		snake.Clear();
	}
}
