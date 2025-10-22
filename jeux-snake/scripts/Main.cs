using Godot;
using System.Collections.Generic;

public partial class Main : Node2D
{
	private const int CellSize = 20;
	private const int GridWidth = 30;
	private const int GridHeight = 20;
	
	private List<Vector2> snake = new List<Vector2>();
	private List<ColorRect> snakeRects = new List<ColorRect>();
	private Vector2 direction = Vector2.Right;
	private Vector2 foodPos;
	private ColorRect foodRect;
	private Label scoreLabel;
	private int score = 0;
	private float timer = 0;

	public override void _Ready()
	{
		// Fond
		ColorRect bg = new ColorRect();
		bg.Size = new Vector2(600, 400);
		bg.Color = new Color(0.1f, 0.1f, 0.15f);
		AddChild(bg);
		
		// Score
		scoreLabel = new Label();
		scoreLabel.Position = new Vector2(10, 410);
		scoreLabel.Text = "Score: 0 - Utilisez les flèches";
		AddChild(scoreLabel);
		
		// Serpent initial (3 segments)
		snake.Add(new Vector2(15, 10));
		snake.Add(new Vector2(14, 10));
		snake.Add(new Vector2(13, 10));
		
		foreach (var pos in snake)
		{
			ColorRect rect = new ColorRect();
			rect.Size = new Vector2(18, 18);
			rect.Color = Colors.LimeGreen;
			rect.Position = pos * CellSize + Vector2.One;
			AddChild(rect);
			snakeRects.Add(rect);
		}
		
		// Première nourriture
		SpawnFood();
		
		GD.Print("Jeu initialisé - utilisez les flèches du clavier");
	}

	private void SpawnFood()
	{
		// Supprimer l'ancienne nourriture
		if (foodRect != null)
			foodRect.QueueFree();
		
		// Trouver une position libre
		do {
			foodPos = new Vector2(
				GD.RandRange(0, GridWidth - 1),
				GD.RandRange(0, GridHeight - 1)
			);
		} while (snake.Contains(foodPos));
		
		// Créer la nourriture
		foodRect = new ColorRect();
		foodRect.Size = new Vector2(18, 18);
		foodRect.Color = Colors.Red;
		foodRect.Position = foodPos * CellSize + Vector2.One;
		AddChild(foodRect);
	}

	public override void _Process(double delta)
	{
		// Contrôles
		if (Input.IsActionJustPressed("ui_up") && direction != Vector2.Down)
			direction = Vector2.Up;
		else if (Input.IsActionJustPressed("ui_down") && direction != Vector2.Up)
			direction = Vector2.Down;
		else if (Input.IsActionJustPressed("ui_left") && direction != Vector2.Right)
			direction = Vector2.Left;
		else if (Input.IsActionJustPressed("ui_right") && direction != Vector2.Left)
			direction = Vector2.Right;
		
		// Timer de mouvement
		timer += (float)delta;
		if (timer < 0.15f) return;
		timer = 0;
		
		// Nouvelle position de la tête
		Vector2 newHead = snake[0] + direction;
		
		// Vérifier collisions murs
		if (newHead.X < 0 || newHead.X >= GridWidth || 
			newHead.Y < 0 || newHead.Y >= GridHeight)
		{
			GameOver();
			return;
		}
		
		// Vérifier collision avec soi-même
		if (snake.Contains(newHead))
		{
			GameOver();
			return;
		}
		
		// Ajouter nouvelle tête
		snake.Insert(0, newHead);
		
		// Vérifier si on mange la nourriture
		if (newHead == foodPos)
		{
			score += 10;
			scoreLabel.Text = $"Score: {score}";
			
			// Ajouter un segment
			ColorRect newRect = new ColorRect();
			newRect.Size = new Vector2(18, 18);
			newRect.Color = Colors.Green;
			AddChild(newRect);
			snakeRects.Insert(0, newRect);
			
			SpawnFood();
		}
		else
		{
			// Retirer la queue
			snake.RemoveAt(snake.Count - 1);
			ColorRect last = snakeRects[snakeRects.Count - 1];
			snakeRects.RemoveAt(snakeRects.Count - 1);
			snakeRects.Insert(0, last);
		}
		
		// Mettre à jour les positions visuelles
		for (int i = 0; i < snakeRects.Count; i++)
		{
			snakeRects[i].Position = snake[i] * CellSize + Vector2.One;
			snakeRects[i].Color = i == 0 ? Colors.LimeGreen : Colors.Green;
		}
	}

	private void GameOver()
	{
		GD.Print($"Game Over! Score final: {score}");
		GetTree().ReloadCurrentScene();
	}
}
