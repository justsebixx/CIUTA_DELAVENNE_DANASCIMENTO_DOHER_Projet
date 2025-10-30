using Godot;
using System.Collections.Generic;

// Gère le mouvement, la croissance et l'affichage du serpent.
public partial class SnakeController : Node2D
{
	private List<Vector2> snake = new List<Vector2>(); 
	private List<ColorRect> snakeRects = new List<ColorRect>(); 
	private Vector2 direction = Vector2.Right; 

	// Point d'entrée - Crée un serpent de 3 segments à la position de départ.
	public void Initialize(Vector2 startPos)
	{
		// Serpent initial : tête + 2 segments vers la gauche
		
		snake.Add(startPos);
		snake.Add(startPos + Vector2.Left); 
		snake.Add(startPos + Vector2.Left * 2); 
		
		// Crée le visuel pour chaque segment
		foreach (var pos in snake)
		{
			ColorRect rect = CreateSnakeSegment(true); 
			rect.Position = pos * Main.CellSize + Vector2.One; 
			AddChild(rect);
			snakeRects.Add(rect);
		}
	}

	// Gère les entrées clavier et change la direction (empêche les demi-tours).
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

	// Déplace le serpent d'une case dans la direction actuelle.
	// Ajoute une nouvelle tête, mais ne retire PAS la queue (géré par Main).
	// Retourne : la nouvelle position de la tête
	public Vector2 Move()
	{
		Vector2 newHead = snake[0] + direction;
		snake.Insert(0, newHead); 
		return newHead;
	}

	// Vérifie si la tête du serpent touche son propre corps.
	// position : position de la tête à tester
	// Retourne : true si collision avec le corps (index 1 à fin)
	public bool CheckSelfCollision(Vector2 position)
	{
		return snake.GetRange(1, snake.Count - 1).Contains(position);
	}

	// Ajoute un nouveau segment visuel à la tête du serpent.
	public void Grow()
	{
		ColorRect newRect = CreateSnakeSegment(false);
		AddChild(newRect);
		snakeRects.Insert(0, newRect); 
	}

	// Retire la queue du serpent
	public void RemoveTail()
	{
		snake.RemoveAt(snake.Count - 1); 
		
		ColorRect last = snakeRects[snakeRects.Count - 1];
		snakeRects.RemoveAt(snakeRects.Count - 1);
		snakeRects.Insert(0, last); 
	}

	// Met à jour les positions visuelles et couleurs de tous les segments.
	public void UpdateVisuals()
	{
		for (int i = 0; i < snakeRects.Count; i++)
		{
			// Synchronise position visuelle avec position dans la grille
			snakeRects[i].Position = snake[i] * Main.CellSize + Vector2.One;
			
			// Colore différemment la tête (vert clair) et le corps (vert foncé)
			if (i == 0)
				snakeRects[i].Color = new Color(0.5f, 1.0f, 0.3f);
			else
				snakeRects[i].Color = new Color(0.2f, 0.8f, 0.2f); 
		}
	}

	// Retourne une copie de la liste des positions du serpent.
	public List<Vector2> GetSnakePositions()
	{
		return new List<Vector2>(snake);
	}

	// Crée un segment visuel avec effet lumineux.
	private ColorRect CreateSnakeSegment(bool isHead)
	{
		// Carré principal (18x18 pixels)
		ColorRect rect = new ColorRect();
		rect.Size = new Vector2(18, 18);
		rect.Color = isHead ? new Color(0.5f, 1.0f, 0.3f) : new Color(0.2f, 0.8f, 0.2f);
		
		// Effet de lueur intérieure (carré vert clair semi-transparent)
		ColorRect innerGlow = new ColorRect();
		innerGlow.Size = new Vector2(14, 14);
		innerGlow.Position = new Vector2(2, 2);
		innerGlow.Color = new Color(0.7f, 1.0f, 0.5f, 0.3f); 
		rect.AddChild(innerGlow);
		
		return rect;
	}
}
