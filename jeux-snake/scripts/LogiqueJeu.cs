using Godot;
using System.Collections.Generic;

namespace JeuxSnake
{
	/// <summary>
	/// Classe contenant la logique pure du jeu
	/// Permet de tester facilement les règles métier du jeu
	/// </summary>
	public class LogiqueJeu
	{
		private const int LARGEUR_GRILLE = 30;
		private const int HAUTEUR_GRILLE = 20;

		/// <summary>
		/// Calcule le nouveau score après avoir mangé une pomme
		/// </summary>
		public int CalculerNouveauScore(int scoreActuel)
		{
			return scoreActuel + 1;
		}

		/// <summary>
		/// Vérifie si le score actuel bat l'ancien record
		/// </summary>
		public bool EstNouveauRecord(int scoreActuel, int ancienRecord)
		{
			return scoreActuel > ancienRecord;
		}

		/// <summary>
		/// Vérifie si la position donnée est en collision avec un mur
		/// </summary>
		public bool EstCollisionMur(Vector2 position)
		{
			return position.X < 0 || position.X >= LARGEUR_GRILLE ||
				   position.Y < 0 || position.Y >= HAUTEUR_GRILLE;
		}

		/// <summary>
		/// Vérifie si la position donnée est en collision avec un obstacle
		/// </summary>
		public bool EstCollisionObstacle(Vector2 position, List<Vector2> obstacles)
		{
			foreach (var obstacle in obstacles)
			{
				if (position == obstacle)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Calcule la prochaine position en fonction de la position actuelle et de la direction
		/// </summary>
		public Vector2 CalculerProchainePosition(Vector2 positionActuelle, Vector2 direction)
		{
			return positionActuelle + direction;
		}

		/// <summary>
		/// Vérifie si le changement de direction est valide
		/// </summary>
		public bool EstChangementDirectionValide(Vector2 directionActuelle, Vector2 nouvelleDirection)
		{
			return directionActuelle + nouvelleDirection != Vector2.Zero;
		}
	}
}
