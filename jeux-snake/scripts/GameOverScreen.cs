using Godot;
using System.Threading.Tasks;

// Gère tous les éléments d'interface : scores, écran de game over, panneau d'infos.
// Responsabilité : Affichage des informations et messages à l'utilisateur.
public partial class UIManager : Node2D
{
	private Label scoreLabel; // Affiche le score actuel en temps réel
	private Label highScoreLabel; // Affiche le meilleur score de la session

	// Point d'entrée - Crée le panneau de score en bas de l'écran
	public void Initialize()
	{
		CreateScorePanel();
	}

	// Met à jour le score affiché et le record si battu.
	// ref highScore : permet de modifier directement la variable du record dans Main
	public void UpdateScore(int score, ref int highScore)
	{
		scoreLabel.Text = $"Score: {score}";
		
		// Si nouveau record, met à jour et change la couleur en jaune doré
		if (score > highScore)
		{
			highScore = score;
			highScoreLabel.Text = $"Record: {highScore}";
			highScoreLabel.AddThemeColorOverride("font_color", new Color(1.0f, 0.9f, 0.0f));
		}
	}

	// Affiche l'écran de fin avec statistiques et redémarre après 3 secondes.
	// async Task : permet d'attendre sans bloquer le jeu
	public async Task ShowGameOver(string reason, int score, int highScore, int snakeLength, int obstacleCount)
	{
		// Overlay noir semi-transparent sur tout l'écran
		ColorRect overlay = new ColorRect();
		overlay.Size = new Vector2(600, 450);
		overlay.Color = new Color(0, 0, 0, 0.85f); // 85% d'opacité
		AddChild(overlay);
		
		// Panneau central bleu foncé pour les infos
		ColorRect panel = new ColorRect();
		panel.Size = new Vector2(400, 300);
		panel.Position = new Vector2(100, 75); // Centré horizontalement
		panel.Color = new Color(0.1f, 0.1f, 0.15f);
		AddChild(panel);
		
		// Bordure cyan autour du panneau
		CreatePanelBorder(new Vector2(100, 75), new Vector2(400, 300));
		
		// Titre "Fin du jeu" en rouge
		Label gameOverLabel = new Label();
		gameOverLabel.Text = "Fin du jeu";
		gameOverLabel.Position = new Vector2(200, 100);
		gameOverLabel.AddThemeFontSizeOverride("font_size", 48);
		gameOverLabel.AddThemeColorOverride("font_color", new Color(1.0f, 0.3f, 0.3f));
		AddChild(gameOverLabel);
		
		// Raison du game over (ex: "Collision avec un mur")
		Label reasonLabel = new Label();
		reasonLabel.Text = reason;
		reasonLabel.Position = new Vector2(190, 160);
		reasonLabel.AddThemeFontSizeOverride("font_size", 18);
		reasonLabel.AddThemeColorOverride("font_color", new Color(1.0f, 0.7f, 0.3f)); // Orange
		AddChild(reasonLabel);
		
		// Score final et record
		Label finalScoreLabel = new Label();
		finalScoreLabel.Text = $"Score Final: {score}\nLe record: {highScore}";
		finalScoreLabel.Position = new Vector2(230, 200);
		finalScoreLabel.AddThemeFontSizeOverride("font_size", 22);
		finalScoreLabel.AddThemeColorOverride("font_color", new Color(0.5f, 0.9f, 1.0f)); // Cyan
		AddChild(finalScoreLabel);
		
		// Statistiques de la partie (longueur du serpent et nombre d'obstacles)
		Label statsLabel = new Label();
		statsLabel.Text = $"Longueur: {snakeLength} | Obstacles: {obstacleCount}";
		statsLabel.Position = new Vector2(180, 260);
		statsLabel.AddThemeFontSizeOverride("font_size", 16);
		statsLabel.AddThemeColorOverride("font_color", new Color(0.7f, 0.7f, 0.8f)); // Gris clair
		AddChild(statsLabel);
		
		// Message de redémarrage automatique
		Label restartLabel = new Label();
		restartLabel.Text = "Redémarrage dans 3 secondes...";
		restartLabel.Position = new Vector2(170, 310);
		restartLabel.AddThemeFontSizeOverride("font_size", 18);
		restartLabel.AddThemeColorOverride("font_color", new Color(0.5f, 1.0f, 0.5f)); // Vert clair
		AddChild(restartLabel);
		
		// Attend 3 secondes avant de retourner le contrôle à Main
		await ToSignal(GetTree().CreateTimer(3.0), SceneTreeTimer.SignalName.Timeout);
	}

	// Crée le panneau de score en bas de l'écran (zone 400-445px).
	// Affiche : Score actuel | Instructions | Record
	private void CreateScorePanel()
	{
		// Fond du panneau bleu très foncé
		ColorRect scorePanel = new ColorRect();
		scorePanel.Size = new Vector2(600, 45);
		scorePanel.Position = new Vector2(0, 400);
		scorePanel.Color = new Color(0.08f, 0.08f, 0.12f);
		AddChild(scorePanel);
		
		// Ligne de séparation cyan en haut du panneau
		ColorRect separator = new ColorRect();
		separator.Size = new Vector2(600, 2);
		separator.Position = new Vector2(0, 400);
		separator.Color = new Color(0.3f, 0.5f, 0.8f, 0.8f);
		AddChild(separator);
		
		// Label du score actuel (gauche)
		scoreLabel = new Label();
		scoreLabel.Position = new Vector2(20, 410);
		scoreLabel.AddThemeFontSizeOverride("font_size", 20);
		scoreLabel.AddThemeColorOverride("font_color", new Color(0.5f, 0.9f, 1.0f));
		scoreLabel.Text = "Score: 0";
		AddChild(scoreLabel);
		
		// Label du record (droite)
		highScoreLabel = new Label();
		highScoreLabel.Position = new Vector2(430, 410);
		highScoreLabel.AddThemeFontSizeOverride("font_size", 20);
		highScoreLabel.AddThemeColorOverride("font_color", new Color(1.0f, 0.84f, 0.0f)); // Jaune doré
		highScoreLabel.Text = "Record: 0";
		AddChild(highScoreLabel);
		
		// Instructions de contrôle (centre)
		Label instructions = new Label();
		instructions.Position = new Vector2(220, 412);
		instructions.AddThemeFontSizeOverride("font_size", 14);
		instructions.AddThemeColorOverride("font_color", new Color(0.6f, 0.7f, 0.8f));
		instructions.Text = "Flèches";
		AddChild(instructions);
	}

	// Dessine une bordure rectangulaire (4 lignes cyan).
	// Utilisé pour encadrer le panneau de game over.
	private void CreatePanelBorder(Vector2 position, Vector2 size)
	{
		int thickness = 2;
		Color borderColor = new Color(0.5f, 0.8f, 1.0f);
		
		// Bordure du haut
		ColorRect top = new ColorRect();
		top.Size = new Vector2(size.X, thickness);
		top.Position = position;
		top.Color = borderColor;
		AddChild(top);
	
		// Bordure du bas
		ColorRect bottom = new ColorRect();
		bottom.Size = new Vector2(size.X, thickness);
		bottom.Position = position + new Vector2(0, size.Y - thickness);
		bottom.Color = borderColor;
		AddChild(bottom);
		
		// Bordure de gauche
		ColorRect left = new ColorRect();
		left.Size = new Vector2(thickness, size.Y);
		left.Position = position;
		left.Color = borderColor;
		AddChild(left);
		
		// Bordure de droite
		ColorRect right = new ColorRect();
		right.Size = new Vector2(thickness, size.Y);
		right.Position = position + new Vector2(size.X - thickness, 0);
		right.Color = borderColor;
		AddChild(right);
	}
}
