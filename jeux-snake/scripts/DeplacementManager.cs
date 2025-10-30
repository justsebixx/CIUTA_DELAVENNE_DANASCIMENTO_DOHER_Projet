using Godot;
using System.Collections.Generic;

public partial class DeplacementManager : Node2D
{
	private Vector2 direction = Vector2.Right;
	private Vector2 nextDirection = Vector2.Right;
	
	public void HandleInput()
	{
		if (Input.IsActionPressed("ui_right") && direction != Vector2.Left)
			nextDirection = Vector2.Right;
		else if (Input.IsActionPressed("ui_left") && direction != Vector2.Right)
			nextDirection = Vector2.Left;
		else if (Input.IsActionPressed("ui_up") && direction != Vector2.Down)
			nextDirection = Vector2.Up;
		else if (Input.IsActionPressed("ui_down") && direction != Vector2.Up)
			nextDirection = Vector2.Down;
	}
	
	public Vector2 GetNextPosition(Vector2 currentHead)
	{
		direction = nextDirection;
		return currentHead + direction;
	}
	
	public Vector2 GetCurrentDirection()
	{
		return direction;
	}
}
