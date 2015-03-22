using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.HighLevel.UI;
	
namespace Game
{
	public class AppMain
	{
		private static Sce.PlayStation.HighLevel.GameEngine2D.Scene 	gameScene;
		private static Sce.PlayStation.HighLevel.UI.Scene 				uiScene;
		private static Sce.PlayStation.HighLevel.UI.Scene 				highscoresScene;
		
		// Member Variables go here
		private static ScreenManager	screenManager;
		private static TutorialManager  tutorialManager;
		private static ObstacleManager  obstacleManager;
		private static HighScoreManager highscoresManager;
		private static SoundManager		soundManager;
		private static Background		background;
		private static Player 			player;
				
		private static int 				frameTime = 0, currentFrameTime = 0, scoreFrameTime = 0;
		private static float			moveSpeed = 3.0f, maxSpeed = 9.0f;
		private static bool				shakeCamera = false;
		
		private static Vector2 oldTouchPos = new Vector2( 0.0f, 0.0f ); // Position of first touch on screen
		private static Vector2 newTouchPos = new Vector2( 0.0f, 0.0f ); // Position of last touch on screen
		public static void SetShake(bool shake) { shakeCamera = shake; }
		public static Player GetPlayer() { return player; }
		public static SoundManager GetSoundManager() { return soundManager; }
		public static Background GetBackground() { return background; }
		
		// score
		private static float score = 0;
		private static Sce.PlayStation.HighLevel.UI.Label gameSpeedLabel;
		private static Sce.PlayStation.HighLevel.UI.Label scoreLabel;
		private static Sce.PlayStation.HighLevel.UI.Label highscoresLabel;
		
		public static void Main (string[] args)
		{
			Initialize();
			
			//Game loop
			bool quitGame = false;
			while (!quitGame) 
			{
				Update ();
				
				Director.Instance.Update();
				Director.Instance.Render();
				UISystem.Render();
				
				Director.Instance.GL.Context.SwapBuffers();
				Director.Instance.PostSwap();
			}
			
			//Clean up after ourselves.
			//Dispose code goes h
			
			Director.Terminate ();
		}

		public static void Initialize ()
		{			
			//Set up director and UISystem.
			Director.Initialize ();
			UISystem.Initialize(Director.Instance.GL.Context);
			
			//Set game scene
			gameScene = new Sce.PlayStation.HighLevel.GameEngine2D.Scene();
			gameScene.Camera.SetViewFromViewport();
			
			//Set the ui scene.
			uiScene		 = new Sce.PlayStation.HighLevel.UI.Scene();
			Panel panel  = new Panel();
			panel.Width  = Director.Instance.GL.Context.GetViewport().Width;
			panel.Height = Director.Instance.GL.Context.GetViewport().Height;
			uiScene.RootWidget.AddChildLast(panel);
			
			//Set the highscores scene.
			highscoresManager		= new HighScoreManager(gameScene);
			highscoresScene			= new Sce.PlayStation.HighLevel.UI.Scene();
			Panel highscoresPanel	= new Panel();
			highscoresPanel.Width  	= Director.Instance.GL.Context.GetViewport().Width;
			highscoresPanel.Height 	= Director.Instance.GL.Context.GetViewport().Height;
			highscoresScene.RootWidget.AddChildLast(highscoresPanel);
			
			// Setup highscores label
			highscoresLabel = new Sce.PlayStation.HighLevel.UI.Label();
			highscoresLabel.Height = 200.0f;
			highscoresLabel.Text = "Retrieving Data";
			highscoresPanel.AddChildLast(highscoresLabel);
			highscoresScene.RootWidget.AddChildLast(highscoresPanel);
			
			// Setup ui scene labels		
			scoreLabel = new Sce.PlayStation.HighLevel.UI.Label();
			scoreLabel.SetPosition(10,8);
			int roundedScore = (int)FMath.Floor(score/100)*100;
			scoreLabel.Text = "Score: " + roundedScore.ToString("N0");
			panel.AddChildLast(scoreLabel);
			
			gameSpeedLabel = new Sce.PlayStation.HighLevel.UI.Label();
			gameSpeedLabel.SetPosition(770,8);
			float speed = FMath.Round(moveSpeed * 10) / 10; // round to 1dp
			gameSpeedLabel.Text = "Game Speed: " + moveSpeed.ToString("N1");	
			panel.AddChildLast(gameSpeedLabel);
			
			soundManager = new SoundManager();
			
			//Run the scene.
			Director.Instance.RunWithScene(gameScene, true);
			screenManager = new ScreenManager(gameScene);
		}
		
		public static void Update()
		{		
			if(screenManager.IsTransitioning())
			{
				screenManager.Update(gameScene);
				
				// Transition Finished, load relevant data
				if(!screenManager.IsTransitioning())
					if(screenManager.GetScreen() == Screens.Game)
				{
						SetupGame();
						soundManager.PlayBGM();
				}
					else if(screenManager.GetScreen() == Screens.GameOver)
						gameScene.Camera2D.SetViewY(new Vector2(0.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f),
			                           		new Vector2((Director.Instance.GL.Context.GetViewport().Width*0.5f), Director.Instance.GL.Context.GetViewport().Height*0.5f));
			}
			else
			{
				UpdateTouchData();
				
				switch(screenManager.GetScreen())
				{
					case Screens.Splash:
						screenManager.ChangeScreenTo(Screens.Menu);
					break;
					
					case Screens.Game:
						if(!tutorialManager.HasPopUp())
							GameUpdate ();
						else
							UpdateTouchData();
					break;
				}
			}			
		}
		
		public static void GameUpdate()
		{			
			// Update code here
			player.Update();
			if (!player.HasBeenKilled())
			{
				UpdateTouchData();
				obstacleManager.Update(moveSpeed);
				background.Update(moveSpeed);
				UpdateCamera();
				UpdateScore ();
			}
			
			// Sets the volcano background to follow the sprite
			background.SetVolcanoPosition((player.GetPos().X + 400)-(Director.Instance.GL.Context.GetViewport().Width/2), 0.0f);
			
			if(player.IsDead())
			{
				screenManager.ChangeScreenTo(Screens.GameOver);
				DestroyGame();
				highscoresManager.SaveScore((int)score);
			}	
		}
		
		public static void UpdateCamera()
		{			
			if (shakeCamera)
			{
				if(currentFrameTime == 1)
					gameScene.Camera2D.SetViewY(new Vector2(0.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f),
			                            new Vector2(player.GetPos().X + 410, Director.Instance.GL.Context.GetViewport().Height*0.5f));
				else if(currentFrameTime == 2)
					gameScene.Camera2D.SetViewY(new Vector2(0.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f),
			                            new Vector2(player.GetPos().X + 400, Director.Instance.GL.Context.GetViewport().Height*0.52f));
				else if(currentFrameTime == 3)
					gameScene.Camera2D.SetViewY(new Vector2(0.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f),
			                            new Vector2(player.GetPos().X + 390, Director.Instance.GL.Context.GetViewport().Height*0.5f));
				else if(currentFrameTime == 4)
				{
					gameScene.Camera2D.SetViewY(new Vector2(0.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f),
			                            new Vector2(player.GetPos().X + 400, Director.Instance.GL.Context.GetViewport().Height*0.48f));
					currentFrameTime = 0;
				}
				if (frameTime >= 40)
				{
					shakeCamera = false;
					frameTime = 0;
				}
				
				currentFrameTime++;
				frameTime++;
			}
			else //Update the camera to follow the player
				gameScene.Camera2D.SetViewY(new Vector2(0.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f),
			                           		new Vector2(player.GetPos().X + 400, Director.Instance.GL.Context.GetViewport().Height*0.5f));
		}
		
		public static Vector2 GetTouchPosition()
		{
			// Translate touch(-1 to 1) to screen pixels
			Vector2 touchPos = Input2.Touch00.Pos;
			if(touchPos.X >=0)
				touchPos = new Vector2((touchPos.X * 450) + 450, (touchPos.Y * 272)+272);	
			else
				touchPos = new Vector2(((touchPos.X+1) * 450), ((touchPos.Y+1) * 272));
			
			// Get world position from Player, add local touch position
			Vector2 playerPos = player.GetPos();
			playerPos = new Vector2(playerPos.X + 115/2 -150,0.0f);
			playerPos += touchPos;
			return playerPos;
		}
		
		public static void UpdateTouchData()
		{
			Vector2 touchPos = Input2.Touch00.Pos;
			var touches = Touch.GetData(0).ToArray();
			
			switch(screenManager.GetScreen())
			{
				// If currently on high scores screen, check if tapped back to menu
				case Screens.HighScores:
					// Tapped bottom left quadrant :: Clear High Scores
					if(Touch.GetData (0).Count > 0 && touchPos.Y < 0 && touchPos.X < 0)
					{
						highscoresManager.ClearHighScores();
						highscoresLabel.Text = highscoresManager.GetHighScores();
					}
					// Tapped bottom right quadrant :: Back to Menu
					else if(Touch.GetData (0).Count > 0 && touchPos.Y < 0 && touchPos.X > 0)
						screenManager.ChangeScreenTo(Screens.Menu);
				break;
				
				// If currently on menu screen, check if tapped play or highscores
				case Screens.Menu:
					if(touches.Length > 0 && touchPos.Y < 0 && touchPos.X < 0)
						screenManager.ChangeScreenTo(Screens.Game);	
					else if(touches.Length > 0 && touchPos.Y < 0 && touchPos.X > 0)
						screenManager.ChangeScreenTo(Screens.HighScores);	
					break;
				
				// If currently on game screen, check if popup active
				case Screens.Game:
					if(tutorialManager.HasPopUp())
					{
						// If tutorial manager is not ready but screen isnt touched, set ready, else if ready + tapped, close popup
						if((!tutorialManager.IsReady()) && touches.Length <= 0)
							tutorialManager.SetReady();
						else if(tutorialManager.IsReady() && tutorialManager.HasPopUp() && touches.Length > 0)
						{
							if(touchPos.Y < 0)
								tutorialManager.DisableTutorials(gameScene);
							else
								tutorialManager.ClosePopUp(gameScene);
						}
					}
				break;
			}
		}		
		
		public static void SetupGame()
		{
			background 		= new Background(gameScene);
			obstacleManager = new ObstacleManager(gameScene);
			player 			= new Player(gameScene, background.GetFloorHeight());
			background.addUnderFloor(gameScene);
			tutorialManager = new TutorialManager(gameScene);
		}
		
		public static void DestroyGame()
		{
			background.Dispose(gameScene);
			obstacleManager.CleanUp(gameScene);
			player.Dispose(gameScene);
			tutorialManager.Dispose(gameScene);
		}
		
		public static void UpdateScore()
		{
			if(moveSpeed < maxSpeed)
				moveSpeed += 0.001f;
			
			if(scoreFrameTime == 50)
				score += 1;
			else if (scoreFrameTime > 50)
				scoreFrameTime = 0;
			
			scoreFrameTime++;
			
			int roundedScore = (int)score;
			scoreLabel.Text = "Score: " + roundedScore.ToString("N0");
			
			gameSpeedLabel.Text = "Game Speed: " + moveSpeed.ToString("N1");	
		}
		
		public static void SetUISystem(string scene)
		{
			if(scene == "game")
				UISystem.SetScene(uiScene);
			else if(scene == "highscores")
			{
				UISystem.SetScene(highscoresScene);
				highscoresLabel.Text = highscoresManager.GetHighScores();
				highscoresLabel.SetPosition(Director.Instance.GL.Context.GetViewport().Width/2 - 55,
											Director.Instance.GL.Context.GetViewport().Height/2 - highscoresLabel.Height /2 + 100);
			}
			else
				UISystem.SetScene(null);
		}
	}
}
