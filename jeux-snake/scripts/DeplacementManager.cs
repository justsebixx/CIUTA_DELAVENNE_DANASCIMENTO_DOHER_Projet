using Godot;
using System.Collections.Generic;

// Gère les inputs clavier et calcule la prochaine position du serpent.
// Responsabilité : Déplacement et direction du serpent.
public partial class DeplacementManager : Node2D
{
	// Direction actuelle du serpent (appliquée au prochain mouvement)
	private Vector2 direction = Vector2.Right;
	
	// Direction demandée par le joueur (mise en attente jusqu'au prochain tick)
	// Évite les changements de direction instantanés qui causeraient des bugs
	private Vector2 nextDirection = Vector2.Right;
	
	// Capture les inputs clavier et stocke la prochaine direction valide.
	// Empêche les demi-tours (ex: droite -> gauche directement).
	public void HandleInput()
	{
		// Interdit un demi-tour : si on va à droite, on ne peut pas aller à gauche
		if (Input.IsActionPressed("ui_right") && direction != Vector2.Left)
			nextDirection = Vector2.Right;
		else if (Input.IsActionPressed("ui_left") && direction != Vector2.Right)
			nextDirection = Vector2.Left;
		else if (Input.IsActionPressed("ui_up") && direction != Vector2.Down)
			nextDirection = Vector2.Up;
		else if (Input.IsActionPressed("ui_down") && direction != Vector2.Up)
			nextDirection = Vector2.Down;
	}
	
	// Applique la direction en attente et calcule la nouvelle position de la tête.
	// currentHead : position actuelle de la tête du serpent (ex: (15, 10))
	// Retourne : nouvelle position (ex: (16, 10) si direction = Right)
	public Vector2 GetNextPosition(Vector2 currentHead)
	{
		direction = nextDirection; // On valide la direction demandée
		return currentHead + direction; // Vector2.Right = (1, 0), donc déplacement d'une case
	}
	
	// Retourne la direction actuelle (utile pour l'affichage ou la logique externe)
	public Vector2 GetCurrentDirection()
	{
		return direction;
	}
}
