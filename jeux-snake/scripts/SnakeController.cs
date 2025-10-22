using Godot;
using System.Collections.Generic;

public partial class SnakeController : Node2D
{
	private List<Vector2> snake = new List<Vector2>();
	private List<ColorRect> snakeRects = new List<ColorRect>();
	private Vector2 direction = Vector2.Right;

	public void Initialize(Vector2 startPos)
	{
		// Serpent initial (3 segments)
		snake.Add(startPos);
		snake.Add(startPos + Vector2.Left);
		snake.Add(startPos + Vector2.Left * 2);
		
		foreach (var pos in snake)
		{
			ColorRect rect = CreateSnakeSegment(true);
			rect.Position = pos * Main.CellSize + Vector2.One;
			AddChild(rect);
			snakeRects.Add(rect);
		}
	}

	public void HandleInput()
	{
		if (Input.IsActionJustPressed("ui_up") && direction != Vector2.Down)
			direction = Vector2.Up;
		else if (Input.IsActionJustPressed("ui_down") && direction != Vector2.Up)
			direction = Vector2.Down;
		else if (Input.IsActionJustPressed("ui_left") && direction != Vector2.Right)
			direction = Vector2.Left;
		else if (Input.IsActionJustPressed("ui_right") && direction != Vector2.Left)
			direction = Vector2.Right;
	}

	public Vector2 Move()
	{
		Vector2 newHead = snake[0] + direction;
		snake.Insert(0, newHead);
		return newHead;
	}

	public bool CheckSelfCollision(Vector2 position)
	{
		return snake.GetRange(1, snake.Count - 1).Contains(position);
	}

	public void Grow()
	{
		ColorRect newRect = CreateSnakeSegment(false);
		AddChild(newRect);
		snakeRects.Insert(0, newRect);
	}

	public void RemoveTail()
	{
		snake.RemoveAt(snake.Count - 1);
		ColorRect last = snakeRects[snakeRects.Count - 1];
		snakeRects.RemoveAt(snakeRects.Count - 1);
		snakeRects.Insert(0, last);
	}

	public void UpdateVisuals()
	{
		for (int i = 0; i < snakeRects.Count; i++)
		{
			snakeRects[i].Position = snake[i] * Main.CellSize + Vector2.One;
			
			if (i == 0)
				snakeRects[i].Color = new Color(0.5f, 1.0f, 0.3f); // Tête
			else
				snakeRects[i].Color = new Color(0.2f, 0.8f, 0.2f); // Corps
		}
	}

	public List<Vector2> GetSnakePositions()
	{
		return new List<Vector2>(snake);
	}

	private ColorRect CreateSnakeSegment(bool isHead)
	{
		ColorRect rect = new ColorRect();
		rect.Size = new Vector2(18, 18);
		rect.Color = isHead ? new Color(0.5f, 1.0f, 0.3f) : new Color(0.2f, 0.8f, 0.2f);
		
		ColorRect innerGlow = new ColorRect();
		innerGlow.Size = new Vector2(14, 14);
		innerGlow.Position = new Vector2(2, 2);
		innerGlow.Color = new Color(0.7f, 1.0f, 0.5f, 0.3f);
		rect.AddChild(innerGlow);
		
		return rect;
	}
}
