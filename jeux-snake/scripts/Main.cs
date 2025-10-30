using Godot;
using System.Collections.Generic;

public partial class Main : Node2D
{
	public const int CellSize = 20;
	public const int GridWidth = 30;
	public const int GridHeight = 20;
	
	// Managers
	private BackgroundManager backgroundManager;
	private DeplacementManager deplacementManager;
	private ObstacleManager obstacleManager;
	private FoodManager foodManager;
	private UIManager uiManager;
	
	// Snake
	private List<Vector2> snake = new List<Vector2>();
	private List<ColorRect> snakeRects = new List<ColorRect>();
	
	// Game State
	private float timer = 0;
	private float moveInterval = 0.15f;
	private int score = 0;
	private int highScore = 0;
	private bool isGameOver = false;

	public override void _Ready()
	{
		InitializeGame();
	}

	private void InitializeGame()
	{
		// Background
		backgroundManager = new BackgroundManager();
		AddChild(backgroundManager);
		backgroundManager.Initialize();
		
		// UI
		uiManager = new UIManager();
		AddChild(uiManager);
		uiManager.Initialize();
		
		// Obstacles
		obstacleManager = new ObstacleManager();
		AddChild(obstacleManager);
		obstacleManager.Initialize();
		
		// Snake initial (3 segments)
		snake.Clear();
		snake.Add(new Vector2(15, 10));
		snake.Add(new Vector2(14, 10));
		snake.Add(new Vector2(13, 10));
		
		DrawSnake();
		
		// Food
		foodManager = new FoodManager();
		AddChild(foodManager);
		foodManager.Initialize(snake, obstacleManager.GetObstaclePositions());
		
		// Déplacement
		deplacementManager = new DeplacementManager();
		AddChild(deplacementManager);
		
		// Reset
		score = 0;
		isGameOver = false;
		uiManager.UpdateScore(score, ref highScore);
		
		GD.Print("🐍 Jeu Snake lancé !");
	}

	public override void _Process(double delta)
	{
		if (isGameOver) return;
		
		// Input
		deplacementManager.HandleInput();
		
		// Timer
		timer += (float)delta;
		if (timer < moveInterval) return;
		timer = 0;
		
		// Calculer nouvelle position
		Vector2 currentHead = snake[0];
		Vector2 newHead = deplacementManager.GetNextPosition(currentHead);
		
		// Vérifier collisions
		if (CheckCollisions(newHead))
			return;
		
		// Vérifier nourriture
		bool hasEaten = foodManager.CheckFoodEaten(newHead);
		
		// Déplacer serpent
		snake.Insert(0, newHead);
		
		if (hasEaten)
		{
			score++;
			uiManager.UpdateScore(score, ref highScore);
			foodManager.SpawnFood(snake, obstacleManager.GetObstaclePositions());
			GD.Print($"🍎 Miam ! Score: {score}");
			
			// Accélérer progressivement
			if (score % 5 == 0 && moveInterval > 0.05f)
			{
				moveInterval -= 0.01f;
				GD.Print($"⚡ Vitesse augmentée ! Intervalle: {moveInterval:F2}s");
			}
		}
		else
		{
			snake.RemoveAt(snake.Count - 1);
		}
		
		UpdateSnake();
	}

	private bool CheckCollisions(Vector2 newHead)
	{
		// Collision mur
		if (newHead.X < 0 || newHead.X >= GridWidth || 
			newHead.Y < 0 || newHead.Y >= GridHeight)
		{
			GameOver("💥 Collision avec le mur !");
			return true;
		}
		
		// Collision obstacle
		if (obstacleManager.CheckCollision(newHead))
		{
			GameOver("🧱 Collision avec un obstacle !");
			return true;
		}
		
		// Auto-collision
		if (snake.Contains(newHead))
		{
			GameOver("🐍 Collision avec soi-même !");
			return true;
		}
		
		return false;
	}

	private async void GameOver(string reason)
{
	isGameOver = true;
	GD.Print($"💀 {reason} | Score: {score}");
	
	// MASQUER TOUT LE JEU
	backgroundManager.Visible = true;
	obstacleManager.Visible = false;
	foodManager.Visible = false;
	
	// Masquer le serpent
	foreach (var rect in snakeRects)
	{
		rect.Visible = false;
	}
	
	// Afficher Game Over proprement
	await uiManager.ShowGameOver(reason, score, highScore, snake.Count, obstacleManager.GetObstaclePositions().Count);
	
	// Nettoyer et redémarrer
	ClearSnake();
	GetTree().ReloadCurrentScene();
}

	private void DrawSnake()
	{
		for (int i = 0; i < snake.Count; i++)
		{
			ColorRect rect = new ColorRect();
			rect.Size = new Vector2(CellSize - 2, CellSize - 2);
			rect.Position = snake[i] * CellSize + Vector2.One;
			
			// Tête plus claire
			if (i == 0)
				rect.Color = new Color(0.3f, 1.0f, 0.3f);
			else
				rect.Color = new Color(0.2f, 0.8f, 0.2f);
			
			AddChild(rect);
			snakeRects.Add(rect);
		}
	}

	private void UpdateSnake()
	{
		// Ajouter segments si nécessaire
		while (snakeRects.Count < snake.Count)
		{
			ColorRect rect = new ColorRect();
			rect.Size = new Vector2(CellSize - 2, CellSize - 2);
			rect.Color = new Color(0.2f, 0.8f, 0.2f);
			AddChild(rect);
			snakeRects.Add(rect);
		}
		
		// Mettre à jour positions
		for (int i = 0; i < snake.Count; i++)
		{
			snakeRects[i].Position = snake[i] * CellSize + Vector2.One;
			
			// Tête en vert clair
			if (i == 0)
				snakeRects[i].Color = new Color(0.3f, 1.0f, 0.3f);
			else
				snakeRects[i].Color = new Color(0.2f, 0.8f, 0.2f);
		}
	}

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
