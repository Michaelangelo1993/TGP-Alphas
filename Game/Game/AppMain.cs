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
		
		// Member Variables go here
		private static ScreenManager	screenManager;
		private static TutorialManager  tutorialManager;
		private static ObstacleManager  obstacleManager;
		private static Background		background;
		private static Player 			player;
				
		private static int 				frameTime = 0, currentFrameTime = 0;
		private static float			moveSpeed = 3.0f, maxSpeed = 9.0f;
		private static bool				shakeCamera = false;
		
		private static Vector2 oldTouchPos = new Vector2( 0.0f, 0.0f ); // Position of first touch on screen
		private static Vector2 newTouchPos = new Vector2( 0.0f, 0.0f ); // Position of last touch on screen
		public static void SetShake(bool shake) { shakeCamera = shake; }
		public static Player GetPlayer() { return player; }
		public static Background GetBackground() { return background; }
		
		// score
		private static float score = 0;
		private static Sce.PlayStation.HighLevel.UI.Label gameSpeedLabel;
		private static Sce.PlayStation.HighLevel.UI.Label scoreLabel;
		
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
			//Dispose code goes here
			background.Dispose();
			obstacleManager.CleanUp();
			player.Dispose();
			
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
			UISystem.SetScene(uiScene);
			uiScene.Visible = false;
			
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
			
			//Run the scene.
			Director.Instance.RunWithScene(gameScene, true);
			screenManager = new ScreenManager(gameScene);
		}
		
		public static void Update()
		{		
			if(screenManager.IsTransitioning())
			{
				screenManager.Update(gameScene);
				
				if(!screenManager.IsTransitioning())	// Transition Finished, load stuff
					if(screenManager.GetScreen() == Screens.Game)
						SetupGame();
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
			UpdateTouchData();
			obstacleManager.Update(moveSpeed);
			background.Update(moveSpeed);
			UpdateCamera();
			UpdateScore ();
			
			// Sets the volcano background to follow the sprite
			background.SetVolcanoPosition((player.GetPos().X + 400)-(Director.Instance.GL.Context.GetViewport().Width/2), 0.0f);
			
			if(player.IsDead())
				screenManager.ChangeScreenTo(Screens.GameOver);	
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
			switch(screenManager.GetScreen())
			{
				case Screens.Menu:
					if(Touch.GetData (0).Count > 0)
						screenManager.ChangeScreenTo(Screens.Game);				
					break;
				case Screens.Game:
					var touches = Touch.GetData(0).ToArray();
				
					if(tutorialManager.HasPopUp())
					{
						// If tutorial manager is not ready but screen isnt touched, set ready, else if ready + tapped, close popup
						if((!tutorialManager.IsReady()) && touches.Length <= 0)
							tutorialManager.SetReady();
						else if(tutorialManager.IsReady() && tutorialManager.HasPopUp() && touches.Length > 0)
						{
							Vector2 touchPos = Input2.Touch00.Pos;
						
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
			tutorialManager = new TutorialManager(gameScene);
			uiScene.Visible = true;
		}
		
		public static void UpdateScore()
		{
			if(moveSpeed < maxSpeed && obstacleManager.GetObstaclesDefeated() > 5)
				moveSpeed += 0.001f;
			score += 1 * moveSpeed;
			
			int roundedScore = (int)FMath.Floor(score/100)*100;
			scoreLabel.Text = "Score: " + roundedScore.ToString("N0");
			
			gameSpeedLabel.Text = "Game Speed: " + moveSpeed.ToString("N1");	
		}
	}
}
