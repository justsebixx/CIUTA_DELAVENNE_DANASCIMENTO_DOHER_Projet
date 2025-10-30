using Microsoft.VisualStudio.TestTools.UnitTesting;
using Godot;
using System.Collections.Generic;

namespace JeuxSnake.Tests
{
	/// <summary>
	/// Tests unitaires
	/// Suit le principe TDD (Test-Driven Development)
	/// </summary>
	[TestClass]
	public class LogiqueJeuTests
	{
		private LogiqueJeu logiqueJeu;

		// Méthode exécutée avant chaque test
		// Initialise une nouvelle instance de LogiqueJeu
		[TestInitialize]
		public void Initialisation()
		{
			logiqueJeu = new LogiqueJeu();
		}

		// Test 1 : Vérifie que le score augmente de 1 après avoir mangé une nourriture
		[TestMethod]
		public void CalculerNouveauScore_ApresAvoirMange_AugmenteDe1()
		{
			int scoreInitial = 5;
			int scoreObtenu = logiqueJeu.CalculerNouveauScore(scoreInitial);
			int scoreAttendu = 6;
			Assert.AreEqual(scoreAttendu, scoreObtenu, "Le score devrait augmenter de 1");
		}

		// Test 2 : Vérifie qu'un score de 0 devient 1
		[TestMethod]
		public void CalculerNouveauScore_DepuisZero_RetourneUn()
		{
			int scoreInitial = 0;
			int scoreObtenu = logiqueJeu.CalculerNouveauScore(scoreInitial);
			Assert.AreEqual(1, scoreObtenu, "Le premier point devrait donner un score de 1");
		}

		// Test 3 : Vérifie la détection d'un nouveau record
		[TestMethod]
		public void EstNouveauRecord_ScoreSuperieurAuRecord_RetourneVrai()
		{
			int scoreActuel = 15;
			int ancienRecord = 10;
			bool estRecord = logiqueJeu.EstNouveauRecord(scoreActuel, ancienRecord);
			Assert.IsTrue(estRecord, "Un score de 15 devrait battre un record de 10");
		}

		// Test 4 : Vérifie qu'un score égal au record ne le bat pas
		[TestMethod]
		public void EstNouveauRecord_ScoreEgalAuRecord_RetourneFaux()
		{
			int scoreActuel = 10;
			int ancienRecord = 10;
			bool estRecord = logiqueJeu.EstNouveauRecord(scoreActuel, ancienRecord);
			Assert.IsFalse(estRecord, "Un score égal au record ne devrait pas le battre");
		}

		// Test 5 : Vérifie la détection de collision avec le mur gauche
		[TestMethod]
		public void EstCollisionMur_PositionNegativeX_RetourneVrai()
		{
			Vector2 positionHorsMur = new Vector2(-1, 10);
			bool estCollision = logiqueJeu.EstCollisionMur(positionHorsMur);
			Assert.IsTrue(estCollision, "Une position X négative devrait être une collision avec le mur");
		}

		// Test 6 : Vérifie qu'une position valide ne cause pas de collision avec un mur
		[TestMethod]
		public void EstCollisionMur_PositionValide_RetourneFaux()
		{
			Vector2 positionValide = new Vector2(15, 10);
			bool estCollision = logiqueJeu.EstCollisionMur(positionValide);
			Assert.IsFalse(estCollision, "Une position au centre de la grille ne devrait pas être une collision");
		}

		// Test 7 : Vérifie la détection de collision avec un obstacle
		[TestMethod]
		public void EstCollisionObstacle_PositionSurObstacle_RetourneVrai()
		{
			Vector2 positionSerpent = new Vector2(5, 5);
			List<Vector2> listeObstacles = new List<Vector2>
			{
				new Vector2(5, 5),
				new Vector2(10, 10),
				new Vector2(15, 15)
			};
			bool estCollision = logiqueJeu.EstCollisionObstacle(positionSerpent, listeObstacles);
			Assert.IsTrue(estCollision, "Le serpent devrait entrer en collision avec l'obstacle en (5,5)");
		}

		// Test 8 : Vérifie qu'une position libre ne cause pas de collision avec un obstacle
		[TestMethod]
		public void EstCollisionObstacle_PositionLibre_RetourneFaux()
		{
			Vector2 positionSerpent = new Vector2(7, 7);
			List<Vector2> listeObstacles = new List<Vector2>
			{
				new Vector2(5, 5),
				new Vector2(10, 10)
			};
			bool estCollision = logiqueJeu.EstCollisionObstacle(positionSerpent, listeObstacles);
			Assert.IsFalse(estCollision, "La position (7,7) devrait être libre d'obstacles");
		}

		// Test 9 : Vérifie le calcul de la prochaine position vers la droite
		[TestMethod]
		public void CalculerProchainePosition_DirectionDroite_AugmenteX()
		{
			Vector2 positionActuelle = new Vector2(10, 10);
			Vector2 directionDroite = Vector2.Right;
			Vector2 nouvellePosition = logiqueJeu.CalculerProchainePosition(positionActuelle, directionDroite);
			Vector2 positionAttendue = new Vector2(11, 10);
			Assert.AreEqual(positionAttendue, nouvellePosition, "Se déplacer à droite devrait augmenter X de 1");
		}

		// Test 10 : Vérifie qu'un demi-tour (droite vers gauche) est interdit
		[TestMethod]
		public void EstChangementDirectionValide_DemiTour_RetourneFaux()
		{
			Vector2 directionActuelle = Vector2.Right;
			Vector2 nouvelleDirection = Vector2.Left;
			bool estValide = logiqueJeu.EstChangementDirectionValide(directionActuelle, nouvelleDirection);
			Assert.IsFalse(estValide, "Un demi-tour immédiat (droite -> gauche) devrait être interdit");
		}

		// Méthode exécutée après chaque test
		[TestCleanup]
		public void Nettoyage()
		{
			logiqueJeu = null;
		}
	}
}
