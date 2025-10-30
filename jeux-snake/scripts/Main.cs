using Godot;

public partial class Main : Node2D
{
	public const int CellSize = 20;
	public const int GridWidth = 30;
	public const int GridHeight = 20;
	
	private ObstacleManager obstacleManager;
	private DeplacementManager deplacementManager;
	private SnakeController snakeController;
	private float timer = 0;

	public override void _Ready()
	{		
		// Obstacles
		obstacleManager = new ObstacleManager();
		AddChild(obstacleManager);
		obstacleManager.Initialize();
		
		// Serpent
		snakeController = new SnakeController();
		AddChild(snakeController);
		snakeController.Initialize(new Vector2(15, 10));
		
		// Déplacement
		deplacementManager = new DeplacementManager();
		AddChild(deplacementManager);
		
		GD.Print("🐍 Serpent lancé !");
	}

	public override void _Process(double delta)
	{
		// Input
		deplacementManager.HandleInput();
		
		// Timer (vitesse du serpent)
		timer += (float)delta;
		if (timer < 0.15f) return;
		timer = 0;
		
		// Calculer nouvelle position
		Vector2 currentHead = snakeController.GetSnakePositions()[0];
		Vector2 newHead = deplacementManager.GetNextPosition(currentHead);
		
		// Collision mur = téléportation côté opposé
		if (newHead.X < 0) newHead.X = GridWidth - 1;
		if (newHead.X >= GridWidth) newHead.X = 0;
		if (newHead.Y < 0) newHead.Y = GridHeight - 1;
		if (newHead.Y >= GridHeight) newHead.Y = 0;
		
		// Collision obstacle = traverser
		if (obstacleManager.CheckCollision(newHead))
		{
			GD.Print("🧱 Traversé un obstacle !");
		}
		
		// Déplacer le serpent
		snakeController.GetSnakePositions().Insert(0, newHead);
		snakeController.RemoveTail();
		snakeController.UpdateVisuals();
	}
}
