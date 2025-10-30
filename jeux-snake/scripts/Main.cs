using Godot;
using System.Collections.Generic;

public partial class Main : Node2D
{
	public const int CellSize = 20;
	public const int GridWidth = 30;
	public const int GridHeight = 20;
	
	private List<Vector2> snake = new List<Vector2>();
	private List<ColorRect> snakeRects = new List<ColorRect>();
	private DeplacementManager deplacementManager;
	private ObstacleManager obstacleManager;
	private float timer = 0;

	public override void _Ready()
	{
		// Fond noir
		ColorRect bg = new ColorRect();
		bg.Size = new Vector2(GridWidth * CellSize, GridHeight * CellSize);
		bg.Color = Colors.Black;
		AddChild(bg);
		
		// Obstacles
		obstacleManager = new ObstacleManager();
		AddChild(obstacleManager);
		obstacleManager.Initialize();
		
		// Serpent initial
		snake.Add(new Vector2(15, 10));
		snake.Add(new Vector2(14, 10));
		snake.Add(new Vector2(13, 10));
		
		DrawSnake();
		
		// Déplacement
		deplacementManager = new DeplacementManager();
		AddChild(deplacementManager);
		
		GD.Print("🐍 Jeu lancé !");
	}

	public override void _Process(double delta)
	{
		// Input
		deplacementManager.HandleInput();
		
		// Timer (vitesse)
		timer += (float)delta;
		if (timer < 0.15f) return;
		timer = 0;
		
		// Nouvelle position
		Vector2 currentHead = snake[0];
		Vector2 newHead = deplacementManager.GetNextPosition(currentHead);
		
		// Collision mur
		if (newHead.X < 0 || newHead.X >= GridWidth || 
			newHead.Y < 0 || newHead.Y >= GridHeight)
		{
			GD.Print("💀 Game Over - Mur");
			GetTree().ReloadCurrentScene();
			return;
		}
		
		// Collision obstacle
		if (obstacleManager.CheckCollision(newHead))
		{
			GD.Print("💀 Game Over - Obstacle");
			GetTree().ReloadCurrentScene();
			return;
		}
		
		// Collision soi-même
		if (snake.Contains(newHead))
		{
			GD.Print("💀 Game Over - Auto-collision");
			GetTree().ReloadCurrentScene();
			return;
		}
		
		// Déplacer
		snake.Insert(0, newHead);
		snake.RemoveAt(snake.Count - 1);
		
		UpdateSnake();
	}

	private void DrawSnake()
	{
		foreach (var pos in snake)
		{
			ColorRect rect = new ColorRect();
			rect.Size = new Vector2(CellSize - 2, CellSize - 2);
			rect.Position = pos * CellSize + Vector2.One;
			rect.Color = Colors.Green;
			AddChild(rect);
			snakeRects.Add(rect);
		}
	}

	private void UpdateSnake()
	{
		for (int i = 0; i < snake.Count; i++)
		{
			snakeRects[i].Position = snake[i] * CellSize + Vector2.One;
		}
	}
}
