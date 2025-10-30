using Godot;
using System.Collections.Generic;

// Gère le mouvement, la croissance et l'affichage du serpent.
// Responsabilité : Logique complète du serpent (input, déplacement, collision avec soi-même).
public partial class SnakeController : Node2D
{
	private List<Vector2> snake = new List<Vector2>(); // Positions de chaque segment sur la grille
	private List<ColorRect> snakeRects = new List<ColorRect>(); // Représentations visuelles des segments
	private Vector2 direction = Vector2.Right; // Direction actuelle (Right, Left, Up, Down)

	// Point d'entrée - Crée un serpent de 3 segments à la position de départ.
	// startPos : position de la tête du serpent (ex: (15, 10))
	public void Initialize(Vector2 startPos)
	{
		// Serpent initial : tête + 2 segments vers la gauche
		// Ex: si startPos = (15,10) → [(15,10), (14,10), (13,10)]
		snake.Add(startPos);
		snake.Add(startPos + Vector2.Left); // 1 case à gauche
		snake.Add(startPos + Vector2.Left * 2); // 2 cases à gauche
		
		// Crée le visuel pour chaque segment
		foreach (var pos in snake)
		{
			ColorRect rect = CreateSnakeSegment(true); // true = segment initial (devient tête ou corps)
			rect.Position = pos * Main.CellSize + Vector2.One; // Convertit grille → pixels
			AddChild(rect);
			snakeRects.Add(rect);
		}
	}

	// Gère les entrées clavier et change la direction (empêche les demi-tours).
	// Appelé dans _Process() de Main pour détecter les pressions de touches en temps réel.
	public void HandleInput()
	{
		// Empêche de faire demi-tour (ex: si direction = Down, ne peut pas aller Up)
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
		Vector2 newHead = snake[0] + direction; // Calcule nouvelle position tête
		snake.Insert(0, newHead); // Ajoute la nouvelle tête en position 0
		return newHead;
	}

	// Vérifie si la tête du serpent touche son propre corps.
	// position : position de la tête à tester
	// Retourne : true si collision avec le corps (index 1 à fin)
	public bool CheckSelfCollision(Vector2 position)
	{
		// GetRange(1, Count-1) = tous les segments sauf la tête
		return snake.GetRange(1, snake.Count - 1).Contains(position);
	}

	// Ajoute un nouveau segment visuel à la tête du serpent.
	// Appelé quand le serpent mange de la nourriture.
	public void Grow()
	{
		ColorRect newRect = CreateSnakeSegment(false); // false = segment de corps
		AddChild(newRect);
		snakeRects.Insert(0, newRect); // Insère en position 0 (nouvelle tête visuelle)
	}

	// Retire la queue du serpent (données + recycle le visuel).
	// Le dernier ColorRect est déplacé en position 0 pour devenir la nouvelle tête.
	public void RemoveTail()
	{
		snake.RemoveAt(snake.Count - 1); // Supprime dernière position
		
		// Recycle le dernier ColorRect au lieu de le détruire/recréer
		ColorRect last = snakeRects[snakeRects.Count - 1];
		snakeRects.RemoveAt(snakeRects.Count - 1);
		snakeRects.Insert(0, last); // Le place en position 0 (nouvelle tête)
	}

	// Met à jour les positions visuelles et couleurs de tous les segments.
	// Appelé après chaque déplacement pour synchroniser affichage et données.
	public void UpdateVisuals()
	{
		for (int i = 0; i < snakeRects.Count; i++)
		{
			// Synchronise position visuelle avec position dans la grille
			snakeRects[i].Position = snake[i] * Main.CellSize + Vector2.One;
			
			// Colore différemment la tête (vert clair) et le corps (vert foncé)
			if (i == 0)
				snakeRects[i].Color = new Color(0.5f, 1.0f, 0.3f); // Tête : vert brillant
			else
				snakeRects[i].Color = new Color(0.2f, 0.8f, 0.2f); // Corps : vert sombre
		}
	}

	// Retourne une copie de la liste des positions du serpent.
	// Utilisé par FoodManager et ObstacleManager pour éviter les chevauchements.
	public List<Vector2> GetSnakePositions()
	{
		return new List<Vector2>(snake); // Copie pour éviter modifications externes
	}

	// Crée un segment visuel avec effet lumineux.
	// isHead : détermine la couleur initiale (mais UpdateVisuals() change les couleurs ensuite)
	private ColorRect CreateSnakeSegment(bool isHead)
	{
		// Carré principal (18x18 pixels)
		ColorRect rect = new ColorRect();
		rect.Size = new Vector2(18, 18);
		rect.Color = isHead ? new Color(0.5f, 1.0f, 0.3f) : new Color(0.2f, 0.8f, 0.2f);
		
		// Effet de lueur intérieure (carré vert clair semi-transparent)
		ColorRect innerGlow = new ColorRect();
		innerGlow.Size = new Vector2(14, 14);
		innerGlow.Position = new Vector2(2, 2); // Centré avec marge de 2px
		innerGlow.Color = new Color(0.7f, 1.0f, 0.5f, 0.3f); // Vert lumineux, 30% opacité
		rect.AddChild(innerGlow);
		
		return rect;
	}
}
