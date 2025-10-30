using Godot;
using System.Threading.Tasks;

public partial class UIManager : Node2D
{
	private Label scoreLabel;
	private Label highScoreLabel;

	public void Initialize()
	{
		CreateScorePanel();
	}

	public void UpdateScore(int score, ref int highScore)
	{
		scoreLabel.Text = $"Score: {score}";
		
		if (score > highScore)
		{
			highScore = score;
			highScoreLabel.Text = $"Record: {highScore}";
			highScoreLabel.AddThemeColorOverride("font_color", new Color(1.0f, 0.9f, 0.0f));
		}
	}

	public async Task ShowGameOver(string reason, int score, int highScore, int snakeLength, int obstacleCount)
	{
		ColorRect overlay = new ColorRect();
		overlay.Size = new Vector2(600, 450);
		overlay.Color = new Color(0, 0, 0, 0.85f);
		AddChild(overlay);
		
		ColorRect panel = new ColorRect();
		panel.Size = new Vector2(400, 300);
		panel.Position = new Vector2(100, 75);
		panel.Color = new Color(0.1f, 0.1f, 0.15f);
		AddChild(panel);
		
		CreatePanelBorder(new Vector2(100, 75), new Vector2(400, 300));
		
		Label gameOverLabel = new Label();
		gameOverLabel.Text = "Fin du jeu";
		gameOverLabel.Position = new Vector2(200, 100);
		gameOverLabel.AddThemeFontSizeOverride("font_size", 48);
		gameOverLabel.AddThemeColorOverride("font_color", new Color(1.0f, 0.3f, 0.3f));
		AddChild(gameOverLabel);
		
	 
		Label reasonLabel = new Label();
		reasonLabel.Text = reason;
		reasonLabel.Position = new Vector2(190, 160);
		reasonLabel.AddThemeFontSizeOverride("font_size", 18);
		reasonLabel.AddThemeColorOverride("font_color", new Color(1.0f, 0.7f, 0.3f));
		AddChild(reasonLabel);
		
		Label finalScoreLabel = new Label();
		finalScoreLabel.Text = $"Score Final: {score}\nLe record: {highScore}";
		finalScoreLabel.Position = new Vector2(230, 200);
		finalScoreLabel.AddThemeFontSizeOverride("font_size", 22);
		finalScoreLabel.AddThemeColorOverride("font_color", new Color(0.5f, 0.9f, 1.0f));
		AddChild(finalScoreLabel);
		
		Label statsLabel = new Label();
		statsLabel.Text = $"Longueur: {snakeLength} | Obstacles: {obstacleCount}";
		statsLabel.Position = new Vector2(180, 260);
		statsLabel.AddThemeFontSizeOverride("font_size", 16);
		statsLabel.AddThemeColorOverride("font_color", new Color(0.7f, 0.7f, 0.8f));
		AddChild(statsLabel);
		
		Label restartLabel = new Label();
		restartLabel.Text = "Redémarrage dans 3 secondes...";
		restartLabel.Position = new Vector2(170, 310);
		restartLabel.AddThemeFontSizeOverride("font_size", 18);
		restartLabel.AddThemeColorOverride("font_color", new Color(0.5f, 1.0f, 0.5f));
		AddChild(restartLabel);
		
		await ToSignal(GetTree().CreateTimer(3.0), SceneTreeTimer.SignalName.Timeout);
	}

	private void CreateScorePanel()
	{
		ColorRect scorePanel = new ColorRect();
		scorePanel.Size = new Vector2(600, 45);
		scorePanel.Position = new Vector2(0, 400);
		scorePanel.Color = new Color(0.08f, 0.08f, 0.12f);
		AddChild(scorePanel);
		
		ColorRect separator = new ColorRect();
		separator.Size = new Vector2(600, 2);
		separator.Position = new Vector2(0, 400);
		separator.Color = new Color(0.3f, 0.5f, 0.8f, 0.8f);
		AddChild(separator);
		
		scoreLabel = new Label();
		scoreLabel.Position = new Vector2(20, 410);
		scoreLabel.AddThemeFontSizeOverride("font_size", 20);
		scoreLabel.AddThemeColorOverride("font_color", new Color(0.5f, 0.9f, 1.0f));
		scoreLabel.Text = "Score: 0";
		AddChild(scoreLabel);
		
		highScoreLabel = new Label();
		highScoreLabel.Position = new Vector2(430, 410);
		highScoreLabel.AddThemeFontSizeOverride("font_size", 20);
		highScoreLabel.AddThemeColorOverride("font_color", new Color(1.0f, 0.84f, 0.0f));
		highScoreLabel.Text = "Record: 0";
		AddChild(highScoreLabel);
		Label instructions = new Label();
		instructions.Position = new Vector2(220, 412);
		instructions.AddThemeFontSizeOverride("font_size", 14);
		instructions.AddThemeColorOverride("font_color", new Color(0.6f, 0.7f, 0.8f));
		instructions.Text = "Flèches";
		AddChild(instructions);
	}

	private void CreatePanelBorder(Vector2 position, Vector2 size)
	{
		int thickness = 2;
		Color borderColor = new Color(0.5f, 0.8f, 1.0f);
		
		ColorRect top = new ColorRect();
		top.Size = new Vector2(size.X, thickness);
		top.Position = position;
		top.Color = borderColor;
		AddChild(top);
	
		ColorRect bottom = new ColorRect();
		bottom.Size = new Vector2(size.X, thickness);
		bottom.Position = position + new Vector2(0, size.Y - thickness);
		bottom.Color = borderColor;
		AddChild(bottom);
		
		ColorRect left = new ColorRect();
		left.Size = new Vector2(thickness, size.Y);
		left.Position = position;
		left.Color = borderColor;
		AddChild(left);
		
		ColorRect right = new ColorRect();
		right.Size = new Vector2(thickness, size.Y);
		right.Position = position + new Vector2(size.X - thickness, 0);
		right.Color = borderColor;
		AddChild(right);
	}
}
