using Godot;

public partial class BackgroundManager : Node2D
{
	public void Initialize()
	{
		CreateBackground();
		CreateGameBorder();
		DrawGrid();
	}

	private void CreateBackground()
	{
		// Fond principal
		ColorRect bg = new ColorRect();
		bg.Size = new Vector2(600, 450);
		bg.Color = new Color(0.05f, 0.05f, 0.1f);
		AddChild(bg);
		
		// Dégradé
		for (int i = 0; i < 10; i++)
		{
			ColorRect gradient = new ColorRect();
			gradient.Size = new Vector2(600, 40);
			gradient.Position = new Vector2(0, i * 40);
			float alpha = 1.0f - (i * 0.1f);
			gradient.Color = new Color(0.1f, 0.15f, 0.25f, alpha * 0.3f);
			AddChild(gradient);
		}
		
		// Points décoratifs
		for (int i = 0; i < 50; i++)
		{
			ColorRect dot = new ColorRect();
			dot.Size = new Vector2(2, 2);
			dot.Position = new Vector2(GD.RandRange(0, 600), GD.RandRange(0, 400));
			dot.Color = new Color(0.3f, 0.4f, 0.6f, GD.Randf() * 0.5f);
			AddChild(dot);
		}
	}

	private void CreateGameBorder()
	{
		int borderThickness = 3;
		Color borderColor = new Color(0.3f, 0.5f, 0.8f);
		
		// Haut
		ColorRect topBorder = new ColorRect();
		topBorder.Size = new Vector2(Main.GridWidth * Main.CellSize + borderThickness * 2, borderThickness);
		topBorder.Position = new Vector2(-borderThickness, -borderThickness);
		topBorder.Color = borderColor;
		AddChild(topBorder);
		
		// Bas
		ColorRect bottomBorder = new ColorRect();
		bottomBorder.Size = new Vector2(Main.GridWidth * Main.CellSize + borderThickness * 2, borderThickness);
		bottomBorder.Position = new Vector2(-borderThickness, Main.GridHeight * Main.CellSize);
		bottomBorder.Color = borderColor;
		AddChild(bottomBorder);
		
		// Gauche
		ColorRect leftBorder = new ColorRect();
		leftBorder.Size = new Vector2(borderThickness, Main.GridHeight * Main.CellSize);
		leftBorder.Position = new Vector2(-borderThickness, 0);
		leftBorder.Color = borderColor;
		AddChild(leftBorder);
		
		// Droite
		ColorRect rightBorder = new ColorRect();
		rightBorder.Size = new Vector2(borderThickness, Main.GridHeight * Main.CellSize);
		rightBorder.Position = new Vector2(Main.GridWidth * Main.CellSize, 0);
		rightBorder.Color = borderColor;
		AddChild(rightBorder);
		
		// Coins lumineux
		CreateCornerGlow(new Vector2(0, 0));
		CreateCornerGlow(new Vector2(Main.GridWidth * Main.CellSize - 10, 0));
		CreateCornerGlow(new Vector2(0, Main.GridHeight * Main.CellSize - 10));
		CreateCornerGlow(new Vector2(Main.GridWidth * Main.CellSize - 10, Main.GridHeight * Main.CellSize - 10));
	}

	private void CreateCornerGlow(Vector2 position)
	{
		ColorRect glow = new ColorRect();
		glow.Size = new Vector2(10, 10);
		glow.Position = position;
		glow.Color = new Color(0.5f, 0.8f, 1.0f, 0.6f);
		AddChild(glow);
	}

	private void DrawGrid()
	{
		for (int x = 0; x < Main.GridWidth; x++)
		{
			for (int y = 0; y < Main.GridHeight; y++)
			{
				ColorRect cell = new ColorRect();
				cell.Size = new Vector2(Main.CellSize - 1, Main.CellSize - 1);
				cell.Position = new Vector2(x * Main.CellSize, y * Main.CellSize);
				
				if ((x + y) % 2 == 0)
					cell.Color = new Color(0.12f, 0.12f, 0.18f);
				else
					cell.Color = new Color(0.10f, 0.10f, 0.15f);
				
				AddChild(cell);
			}
		}
	}
}
