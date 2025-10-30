using Godot;
using System.Collections.Generic;

public partial class ObstacleManager : Node2D
{
	private List<Vector2> obstacles = new List<Vector2>();
	private List<ColorRect> obstacleRects = new List<ColorRect>();
	private const int ObstacleCount = 30;

	public void Initialize()
	{
		ClearObstacles();
		GenerateObstacles();
		DrawObstacles();
	}

	private void ClearObstacles()
	{
		obstacles.Clear();
		foreach (var r in obstacleRects)
		{
			if (r.IsInsideTree())
				r.QueueFree();
		}
		obstacleRects.Clear();
	}

	private void GenerateObstacles()
	{
		var random = new RandomNumberGenerator();
		random.Randomize();

		HashSet<Vector2> usedPositions = new HashSet<Vector2>();

		// Zone centrale de spawn du serpent à éviter
		for (int x = 13; x <= 17; x++)
			for (int y = 8; y <= 12; y++)
				usedPositions.Add(new Vector2(x, y));

		while (obstacles.Count < ObstacleCount)
		{
			// Remplacé l'appel à Next() (System.Random) par RandiRange() de Godot RNG
			int x = random.RandiRange(2, Main.GridWidth - 3);
			int y = random.RandiRange(2, Main.GridHeight - 3);
			Vector2 pos = new Vector2(x, y);

			if (!usedPositions.Contains(pos))
			{
				obstacles.Add(pos);
				usedPositions.Add(pos);
			}
		}
	}

	private void DrawObstacles()
	{
		foreach (var obs in obstacles)
		{
			ColorRect rect = new ColorRect();
			rect.Size = new Vector2(Main.CellSize - 2, Main.CellSize - 2);
			rect.Position = obs * Main.CellSize + Vector2.One;
			rect.Color = new Color(1f, 1f, 1f);
			AddChild(rect);
			obstacleRects.Add(rect);
		}
	}

	public bool CheckCollision(Vector2 position)
	{
		return obstacles.Contains(position);
	}

	public List<Vector2> GetObstaclePositions()
	{
		return new List<Vector2>(obstacles);
	}
}
