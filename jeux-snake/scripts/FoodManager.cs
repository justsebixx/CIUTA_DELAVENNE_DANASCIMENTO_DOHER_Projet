using Godot;
using System.Collections.Generic;

// Gère l'apparition, l'affichage et la détection de la nourriture.
public partial class FoodManager : Node2D
{
	private Vector2 foodPos; 
	private ColorRect foodRect; 

	public void Initialize(List<Vector2> snakePositions, List<Vector2> obstaclePositions)
	{
		SpawnFood(snakePositions, obstaclePositions);
	}

	// Place aléatoirement la nourriture en évitant le serpent et les obstacles.
	public void SpawnFood(List<Vector2> snakePositions, List<Vector2> obstaclePositions)
	{
		// Supprime l'ancienne nourriture de la scène si elle existe
		if (foodRect != null)
			foodRect.QueueFree();
		
		int attempts = 0;
		do {
			// Génère une position aléatoire dans la grille (0 à GridWidth-1, 0 à GridHeight-1)
			foodPos = new Vector2(
				GD.RandRange(0, Main.GridWidth - 1),
				GD.RandRange(0, Main.GridHeight - 1)
			);
			attempts++;
			
			if (attempts > 1000)
			{
				GD.Print(" Impossible de placer la nourriture !");
				return;
			}
		} while (snakePositions.Contains(foodPos) || obstaclePositions.Contains(foodPos));
		
		CreateFoodVisual();
		GD.Print($" Nourriture placée en ({foodPos.X}, {foodPos.Y})");
	}

	// Vérifie si le serpent a mangé la nourriture.
	// position : position de la tête du serpent
	// Retourne : true si collision avec la nourriture
	public bool CheckFoodEaten(Vector2 position)
	{
		return position == foodPos;
	}

	// Crée le visuel de la nourriture style pomme brillante.
	private void CreateFoodVisual()
	{
		foodRect = new ColorRect();
		foodRect.Size = new Vector2(18, 18);
		foodRect.Color = new Color(1.0f, 0.2f, 0.2f); // Rouge vif
		foodRect.Position = foodPos * Main.CellSize + Vector2.One; // +1 pour centrer dans la case
		AddChild(foodRect);
		
		ColorRect aura = new ColorRect();
		aura.Size = new Vector2(16, 16);
		aura.Position = new Vector2(1, 1); // Décalage pour centrer l'aura
		aura.Color = new Color(1.0f, 0.5f, 0.3f, 0.4f);
		foodRect.AddChild(aura);
		
		ColorRect shine = new ColorRect();
		shine.Size = new Vector2(4, 4);
		shine.Position = new Vector2(3, 3);
		shine.Color = new Color(1.0f, 1.0f, 1.0f, 0.8f);
		foodRect.AddChild(shine);
	}
}
