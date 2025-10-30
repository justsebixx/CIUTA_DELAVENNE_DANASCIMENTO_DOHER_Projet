using Godot;

public partial class Main : Node2D
{
	public const int CellSize = 20;
	public const int GridWidth = 30;
	public const int GridHeight = 20;
	
	private BackgroundManager backgroundManager;
	private SnakeController snakeController;
	private FoodManager foodManager;
	private Label scoreLabel;
	private float timer = 0;
	private int score = 0;

	public override void _Ready()
	{
		// Initialiser le fond
		backgroundManager = new BackgroundManager();
		AddChild(backgroundManager);
		backgroundManager.Initialize();
		
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
		
		GD.Print("🎮 Jeu initialisé !");
	}

	public override void _Process(double delta)
	{
		snakeController.HandleInput();
		
		timer += (float)delta;
		if (timer < 0.15f) return;
		timer = 0;
		
		Vector2 newHead = snakeController.Move();
		
		// Collision mur
		if (newHead.X < 0 || newHead.X >= GridWidth || 
			newHead.Y < 0 || newHead.Y >= GridHeight)
		{
			GD.Print($"💀 Game Over - Score final: {score}");
			GetTree().ReloadCurrentScene();
			return;
		}
		
		// Collision soi-même
		if (snakeController.CheckSelfCollision(newHead))
		{
			GD.Print($"💀 Game Over - Score final: {score}");
			GetTree().ReloadCurrentScene();
			return;
		}
		
		// Manger nourriture
		if (foodManager.CheckFoodEaten(newHead))
		{
			score++;
			UpdateScore();
			snakeController.Grow();
			foodManager.SpawnFood(snakeController.GetSnakePositions(), new System.Collections.Generic.List<Vector2>());
		}
		else
		{
			snakeController.RemoveTail();
		}
		
		snakeController.UpdateVisuals();
	}

	private void UpdateScore()
	{
		scoreLabel.Text = $"🍎 Score: {score}";
	}
}
