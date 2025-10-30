using Godot;
using System.Collections.Generic;

public partial class FoodManager : Node2D
{
	private Vector2 foodPos;
	private ColorRect foodRect;

	public void Initialize(List<Vector2> snakePositions, List<Vector2> obstaclePositions)
	{
		SpawnFood(snakePositions, obstaclePositions);
	}

	public void SpawnFood(List<Vector2> snakePositions, List<Vector2> obstaclePositions)
	{
		if (foodRect != null)
			foodRect.QueueFree();
		
		int attempts = 0;
		do {
			foodPos = new Vector2(
				GD.RandRange(0, Main.GridWidth - 1),
				GD.RandRange(0, Main.GridHeight - 1)
			);
			attempts++;
			
			if (attempts > 1000)
			{
				GD.Print("⚠️ Impossible de placer la nourriture !");
				return;
			}
		} while (snakePositions.Contains(foodPos) || obstaclePositions.Contains(foodPos));
		
		CreateFoodVisual();
		GD.Print($"🍎 Nourriture placée en ({foodPos.X}, {foodPos.Y})");
	}

	public bool CheckFoodEaten(Vector2 position)
	{
		return position == foodPos;
	}

	private void CreateFoodVisual()
	{
		foodRect = new ColorRect();
		foodRect.Size = new Vector2(18, 18);
		foodRect.Color = new Color(1.0f, 0.2f, 0.2f);
		foodRect.Position = foodPos * Main.CellSize + Vector2.One;
		AddChild(foodRect);
		
		// Aura lumineuse
		ColorRect aura = new ColorRect();
		aura.Size = new Vector2(16, 16);
		aura.Position = new Vector2(1, 1);
		aura.Color = new Color(1.0f, 0.5f, 0.3f, 0.4f);
		foodRect.AddChild(aura);
		
		// Point lumineux
		ColorRect shine = new ColorRect();
		shine.Size = new Vector2(4, 4);
		shine.Position = new Vector2(3, 3);
		shine.Color = new Color(1.0f, 1.0f, 1.0f, 0.8f);
		foodRect.AddChild(shine);
	}
}
