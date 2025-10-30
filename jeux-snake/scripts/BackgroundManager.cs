using Godot;

// Gère l'affichage du fond de jeu, des bordures et de la grille.
// Responsabilité : Tous les éléments visuels statiques.
public partial class BackgroundManager : Node2D
{
	// Point d'entrée principal - Crée tous les éléments visuels.
	// Appelé une seule fois au démarrage du jeu depuis Main.
	public void Initialize()
	{
		CreateBackground();
		CreateGameBorder();
		DrawGrid();
	}

	// Crée le fond avec dégradé et points décoratifs.
	private void CreateBackground()
	{
		// Fond principal bleu très foncé
		ColorRect bg = new ColorRect();
		bg.Size = new Vector2(600, 450);
		bg.Color = new Color(0.05f, 0.05f, 0.1f);
		AddChild(bg);
		
		// Dégradé vertical (10 bandes avec alpha décroissant)
		for (int i = 0; i < 10; i++)
		{
			ColorRect gradient = new ColorRect();
			gradient.Size = new Vector2(600, 40);
			gradient.Position = new Vector2(0, i * 40);
			float alpha = 1.0f - (i * 0.1f); // Transparence progressive
			gradient.Color = new Color(0.1f, 0.15f, 0.25f, alpha * 0.3f);
			AddChild(gradient);
		}
		
		// 50 points aléatoires pour effet "étoiles"
		for (int i = 0; i < 50; i++)
		{
			ColorRect dot = new ColorRect();
			dot.Size = new Vector2(2, 2);
			dot.Position = new Vector2(GD.RandRange(0, 600), GD.RandRange(0, 400));
			dot.Color = new Color(0.3f, 0.4f, 0.6f, GD.Randf() * 0.5f);
			AddChild(dot);
		}
	}

	// Crée les bordures autour de la zone de jeu (4 côtés + coins lumineux).
	// Les bordures servent aussi de référence visuelle pour les collisions.
	private void CreateGameBorder()
	{
		int borderThickness = 3;
		Color borderColor = new Color(0.3f, 0.5f, 0.8f);
		
		// Bordure du haut (légèrement débordante pour englober la grille)
		ColorRect topBorder = new ColorRect();
		topBorder.Size = new Vector2(Main.GridWidth * Main.CellSize + borderThickness * 2, borderThickness);
		topBorder.Position = new Vector2(-borderThickness, -borderThickness);
		topBorder.Color = borderColor;
		AddChild(topBorder);
		
		// Bordure du bas
		ColorRect bottomBorder = new ColorRect();
		bottomBorder.Size = new Vector2(Main.GridWidth * Main.CellSize + borderThickness * 2, borderThickness);
		bottomBorder.Position = new Vector2(-borderThickness, Main.GridHeight * Main.CellSize);
		bottomBorder.Color = borderColor;
		AddChild(bottomBorder);
		
		// Bordure gauche
		ColorRect leftBorder = new ColorRect();
		leftBorder.Size = new Vector2(borderThickness, Main.GridHeight * Main.CellSize);
		leftBorder.Position = new Vector2(-borderThickness, 0);
		leftBorder.Color = borderColor;
		AddChild(leftBorder);
		
		// Bordure droite
		ColorRect rightBorder = new ColorRect();
		rightBorder.Size = new Vector2(borderThickness, Main.GridHeight * Main.CellSize);
		rightBorder.Position = new Vector2(Main.GridWidth * Main.CellSize, 0);
		rightBorder.Color = borderColor;
		AddChild(rightBorder);
		
		// Coins lumineux pour effet visuel aux 4 angles
		CreateCornerGlow(new Vector2(0, 0)); // Haut-gauche
		CreateCornerGlow(new Vector2(Main.GridWidth * Main.CellSize - 10, 0)); // Haut-droite
		CreateCornerGlow(new Vector2(0, Main.GridHeight * Main.CellSize - 10)); // Bas-gauche
		CreateCornerGlow(new Vector2(Main.GridWidth * Main.CellSize - 10, Main.GridHeight * Main.CellSize - 10)); // Bas-droite
	}

	// Crée un petit carré lumineux pour accentuer les coins.
	private void CreateCornerGlow(Vector2 position)
	{
		ColorRect glow = new ColorRect();
		glow.Size = new Vector2(10, 10);
		glow.Position = position;
		glow.Color = new Color(0.5f, 0.8f, 1.0f, 0.6f); // Bleu clair semi-transparent
		AddChild(glow);
	}

	// Dessine la grille de jeu en damier (effet checkerboard).
	// Chaque cellule fait CellSize pixels (20x20).
	// Motif alterné selon la parité de (x + y).
	private void DrawGrid()
	{
		for (int x = 0; x < Main.GridWidth; x++)
		{
			for (int y = 0; y < Main.GridHeight; y++)
			{
				ColorRect cell = new ColorRect();
				cell.Size = new Vector2(Main.CellSize - 1, Main.CellSize - 1); // -1 pour l'espacement
				cell.Position = new Vector2(x * Main.CellSize, y * Main.CellSize);
				
				// Damier : cases alternées sombres/très sombres
				if ((x + y) % 2 == 0)
					cell.Color = new Color(0.12f, 0.12f, 0.18f);
				else
					cell.Color = new Color(0.10f, 0.10f, 0.15f);
				
				AddChild(cell);
			}
		}
	}
}
