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
	private SnakeController snakeController;
	private FoodManager foodManager;
	private Label scoreLabel;
	private float timer = 0;
	private int score = 0;

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
		// Score label
		scoreLabel = new Label();
		scoreLabel.Position = new Vector2(10, GridHeight * CellSize + 5);
		scoreLabel.AddThemeFontSizeOverride("font_size", 20);
		scoreLabel.AddThemeColorOverride("font_color", Colors.White);
		AddChild(scoreLabel);
		UpdateScore();
		
		// Initialiser le serpent
		snakeController = new SnakeController();
		AddChild(snakeController);
		snakeController.Initialize(new Vector2(15, 10));
		
		// Initialiser la nourriture
		foodManager = new FoodManager();
		AddChild(foodManager);
		foodManager.Initialize(snakeController.GetSnakePositions(), new System.Collections.Generic.List<Vector2>());
		
		GD.Print("🐍 Snake et 🍎 Food initialisés !");
		GD.Print("🐍 Snake initialisé !");
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class Main : Node2D
{
	private const int CellSize = 20;
	private const int GridWidth = 30;
	private const int GridHeight = 20;

	private readonly List<Vector2> snake = new List<Vector2>();
	private readonly List<ColorRect> snakeRects = new List<ColorRect>();
	private Vector2 direction = Vector2.Right;

	private Vector2 foodPos;
	private ColorRect foodRect;

	private int score = 0;
	private int highScore = 0;
	private float timer = 0f;

	private UIManager ui;

	public override void _Ready()
	{
		ColorRect bg = new ColorRect
		{
			Size = new Vector2(GridWidth * CellSize, GridHeight * CellSize),
			Color = new Color(0.1f, 0.1f, 0.15f),
			Position = Vector2.Zero
		};
		AddChild(bg);

		ui = new UIManager();
		AddChild(ui);
		ui.Initialize();

		Vector2 start = new Vector2(GridWidth / 2, GridHeight / 2);
		snake.Clear();
		snake.Add(start);
		snake.Add(start - Vector2.Right);
		snake.Add(start - Vector2.Right * 2);

		foreach (var _ in snake)
		{
			var r = new ColorRect { Size = new Vector2(18, 18), Color = Colors.Green };
			AddChild(r);
			snakeRects.Add(r);
		}

		foodRect = new ColorRect { Size = new Vector2(18, 18), Color = Colors.Red };
		AddChild(foodRect);

		SpawnFood();
		UpdateVisuals();
		ui.UpdateScore(score, ref highScore);
	}

	public override void _Process(double delta)
	{
		// Input
		deplacementManager.HandleInput();
		
		// Timer (vitesse)
		snakeController.HandleInput();
		
		timer += (float)delta;
		if (timer < 0.15f) return;
		timer = 0;
		
		// Nouvelle position
		Vector2 currentHead = snake[0];
		Vector2 newHead = deplacementManager.GetNextPosition(currentHead);
		Vector2 newHead = snakeController.Move();
		
		// Collision mur
		if (newHead.X < 0 || newHead.X >= GridWidth || 
			newHead.Y < 0 || newHead.Y >= GridHeight)
		{
			GD.Print("💀 Game Over - Mur");
			GD.Print($"💀 Game Over - Score final: {score}");
			GetTree().ReloadCurrentScene();
			return;
		}
		
		// Collision obstacle
		if (obstacleManager.CheckCollision(newHead))
		{
			GD.Print("💀 Game Over - Obstacle");
		// Collision soi-même
		if (snakeController.CheckSelfCollision(newHead))
		{
			GD.Print($"💀 Game Over - Score final: {score}");
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
		// ✅ Utiliser CheckFoodEaten au lieu de CheckFoodCollision
		if (foodManager.CheckFoodEaten(newHead))
		{
			score++;
			UpdateScore();
			snakeController.Grow();  // Faire grandir le serpent
			foodManager.SpawnFood(snakeController.GetSnakePositions(), new System.Collections.Generic.List<Vector2>());
		}
		else
		{
			snakeController.RemoveTail();
		}
		
		snakeController.UpdateVisuals();
		if (Input.IsActionJustPressed("ui_up") && direction != Vector2.Down) direction = Vector2.Up;
		else if (Input.IsActionJustPressed("ui_down") && direction != Vector2.Up) direction = Vector2.Down;
		else if (Input.IsActionJustPressed("ui_left") && direction != Vector2.Right) direction = Vector2.Left;
		else if (Input.IsActionJustPressed("ui_right") && direction != Vector2.Left) direction = Vector2.Right;

		timer += (float)delta;
		if (timer < 0.15f) return;
		timer = 0f;

		Vector2 newHead = snake[0] + direction;

		if (newHead.X < 0 || newHead.X >= GridWidth || newHead.Y < 0 || newHead.Y >= GridHeight)
		{
			TriggerGameOver("Tu as heurté le mur");
			return;
		}

		if (snake.Contains(newHead))
		{
			TriggerGameOver("Auto-collision");
			return;
		}

		snake.Insert(0, newHead);

		if (newHead == foodPos)
		{
			score += 10;
			ui.UpdateScore(score, ref highScore);
			var newRect = new ColorRect { Size = new Vector2(18, 18), Color = Colors.Green };
			AddChild(newRect);
			snakeRects.Insert(0, newRect);
			SpawnFood();
		}
		else
		{
			snake.RemoveAt(snake.Count - 1);
			var last = snakeRects[snakeRects.Count - 1];
			snakeRects.RemoveAt(snakeRects.Count - 1);
			snakeRects.Insert(0, last);
		}

		UpdateVisuals();
	}

	private void UpdateVisuals()
	{
		for (int i = 0; i < snakeRects.Count; i++)
		{
			snakeRects[i].Position = snake[i] * CellSize + Vector2.One;
			snakeRects[i].Color = i == 0 ? Colors.LimeGreen : Colors.Green;
		}
		foodRect.Position = foodPos * CellSize + Vector2.One;
	}

	private void SpawnFood()
	{
		var rng = new RandomNumberGenerator();
		rng.Randomize();
		do
		{
			foodPos = new Vector2(rng.RandiRange(0, GridWidth - 1), rng.RandiRange(0, GridHeight - 1));
		} while (snake.Contains(foodPos));
		foodRect.Position = foodPos * CellSize + Vector2.One;
	}

	private async void TriggerGameOver(string reason)
	{
		SetProcess(false);
		await ui.ShowGameOver(reason, score, highScore, snake.Count, 0);
		GetTree().ReloadCurrentScene();
	}

	private void UpdateScore()
	{
		scoreLabel.Text = $"🍎 Score: {score}";
	}
}
	
