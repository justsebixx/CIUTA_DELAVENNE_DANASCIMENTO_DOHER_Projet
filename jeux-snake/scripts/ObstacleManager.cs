using Godot;
using System.Collections.Generic;

// Gère la génération, l'affichage et la détection des obstacles.
public partial class ObstacleManager : Node2D
{
	private List<Vector2> obstacles = new List<Vector2>(); 
	private List<ColorRect> obstacleRects = new List<ColorRect>(); 
	private const int ObstacleCount = 30; 

	// Point d'entrée - Réinitialise et génère de nouveaux obstacles
	public void Initialize()
	{
		ClearObstacles();
		GenerateObstacles();
		DrawObstacles();
	}

	// Supprime tous les obstacles existants (données + visuels).
	private void ClearObstacles()
	{
		obstacles.Clear(); // Vide la liste des positions
		
		// Supprime les ColorRect de la scène Godot
		foreach (var r in obstacleRects)
		{
			if (r.IsInsideTree()) // Vérifie que le nœud est bien dans l'arbre de scène
				r.QueueFree(); // Supprime proprement le nœud
		}
		obstacleRects.Clear();
	}

	// Génère aléatoirement les positions des obstacles 
	private void GenerateObstacles()
	{
		var random = new RandomNumberGenerator();
		random.Randomize(); // Initialise le générateur aléatoire

		HashSet<Vector2> usedPositions = new HashSet<Vector2>(); // Évite les doublons

		// Réserve la zone centrale (5x5) pour le spawn du serpent
		for (int x = 13; x <= 17; x++)
			for (int y = 8; y <= 12; y++)
				usedPositions.Add(new Vector2(x, y));

		// Génère ObstacleCount obstacles à des positions uniques
		while (obstacles.Count < ObstacleCount)
		{
			// Position aléatoire avec marge de 2 cases des bords
			int x = random.RandiRange(2, Main.GridWidth - 3);
			int y = random.RandiRange(2, Main.GridHeight - 3);
			Vector2 pos = new Vector2(x, y);

			// Ajoute seulement si la position n'est pas déjà prise
			if (!usedPositions.Contains(pos))
			{
				obstacles.Add(pos);
				usedPositions.Add(pos);
			}
		}
	}

	// Crée les représentations visuelles des obstacles (carrés blancs).
	// Appelé après GenerateObstacles() pour afficher les obstacles à l'écran.
	private void DrawObstacles()
	{
		foreach (var obs in obstacles)
		{
			ColorRect rect = new ColorRect();
			rect.Size = new Vector2(Main.CellSize - 2, Main.CellSize - 2); // 18x18 pixels (case 20 - marge 2)
			rect.Position = obs * Main.CellSize + Vector2.One; // Convertit pos grille → pixels, +1 pour centrer
			rect.Color = new Color(1f, 1f, 1f); // Blanc pur
			AddChild(rect);
			obstacleRects.Add(rect);
		}
	}

	// Vérifie si une position donnée contient un obstacle.
	// position : position à tester (ex: tête du serpent)
	// Retourne : true si collision avec un obstacle
	public bool CheckCollision(Vector2 position)
	{
		return obstacles.Contains(position);
	}

	// Retourne une copie de la liste des positions d'obstacles.
	// Utilisé par FoodManager pour éviter de placer la nourriture sur un obstacle.
	public List<Vector2> GetObstaclePositions()
	{
		return new List<Vector2>(obstacles); // Retourne une copie pour éviter les modifications externes
	}
}
