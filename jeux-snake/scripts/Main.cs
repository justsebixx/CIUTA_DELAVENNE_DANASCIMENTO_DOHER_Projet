using Godot;

public partial class Main : Node2D
{
	public const int CellSize = 20;
	public const int GridWidth = 30;
	public const int GridHeight = 20;
	
	private SnakeController snakeController;
	private float timer = 0;  // ⚠️ Le timer doit être une variable de classe !

	public override void _Ready()
	{
		// Fond simple
		ColorRect bg = new ColorRect();
		bg.Size = new Vector2(GridWidth * CellSize, GridHeight * CellSize);
		bg.Color = new Color(0.1f, 0.1f, 0.15f);
		AddChild(bg);
		
		// Initialiser le serpent
		snakeController = new SnakeController();
		AddChild(snakeController);
		snakeController.Initialize(new Vector2(15, 10));
		
		GD.Print("🐍 Snake initialisé !");
	}

	public override void _Process(double delta)
	{
		// Gestion des inputs
		snakeController.HandleInput();
		
		// Timer de mouvement
		timer += (float)delta;
		if (timer < 0.15f) return;  // Vitesse: bouge toutes les 0.15 secondes
		timer = 0;  // Réinitialiser après le mouvement
		
		// Déplacer le serpent
		Vector2 newHead = snakeController.Move();
		
		// Vérifier collisions murs
		if (newHead.X < 0 || newHead.X >= GridWidth || 
			newHead.Y < 0 || newHead.Y >= GridHeight)
		{
			GD.Print("💀 Game Over - Collision mur!");
			GetTree().ReloadCurrentScene();
			return;
		}
		
		// Vérifier collision avec soi-même
		if (snakeController.CheckSelfCollision(newHead))
		{
			GD.Print("💀 Game Over - Auto-collision!");
			GetTree().ReloadCurrentScene();
			return;
		}
		
		// Retirer la queue (pas de nourriture pour l'instant)
		snakeController.RemoveTail();
		
		// Mettre à jour les visuels
		snakeController.UpdateVisuals();
	}
}
