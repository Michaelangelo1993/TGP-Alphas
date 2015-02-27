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
		private static Background		background;
		private static TntWall			tntWall;
		private static Player 			player;
		private static Seasaw 			seasaw;
		private static Spring 			spring;
		private static SpinObstacle     spinObstacle;
		private static Geiser			geiser;
		
		private static int 				frameTime = 0, currentFrameTime = 0;
		private static float			moveSpeed = 3.0f;
		private static bool				shakeCamera = false;
		
		private static Vector2 oldTouchPos = new Vector2( 0.0f, 0.0f ); // Position of first touch on screen
		private static Vector2 newTouchPos = new Vector2( 0.0f, 0.0f ); // Position of last touch on screen
		
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
			tntWall.Dispose();
			seasaw.Dispose();
			spring.Dispose();
			geiser.Dispose();
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
			player.Update(0.0f);
			UpdateTouchData();
			UpdateSeasaw();
			UpdateSpring();
			UpdateSpin();
			UpdateTnt();
			UpdateGeiser();
			background.Update(0.0f, moveSpeed);
			UpdateCamera();
			
			// Sets the volcano background to follow the sprite
			background.SetVolcanoPosition((player.GetPos().X + 400)-(Director.Instance.GL.Context.GetViewport().Width/2), 0.0f);
			
			if(player.IsDead())
			{
				screenManager.ChangeScreenTo(Screens.GameOver);	
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
		
		public static bool IsColliding(Seasaw s)
		{
			return (player.GetBox().Overlaps(s.GetBox()));
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
					else // No popup :: Add else if for pause etc?
					{
						if(touches.Length <= 0) // Screen Not Touched
						{
							if(spring.BeingPushed)
								spring.ReleaseSpring(true);
							tntWall.ReleasePlunger();
						}
						else
						{
							Vector2 touchPos = GetTouchPosition();
							
							if((touchPos.X-100 < spring.GetPosition.X) &&
							   (touchPos.X+125 > spring.GetPosition.X + spring.GetSpringWidth) &&
							   (touchPos.Y < spring.GetOriginalHeight+100)) // Touching spring
							{
								spring.PushSpring();
							}
						}
					break;
					}
				break;
			}
		}
		
		public static void UpdateSeasaw()
		{
			seasaw.Update(0, moveSpeed);
			
			// Check player seasaw collision
			if(IsColliding(seasaw))
				seasaw.SetIsOn();
			
			if(seasaw.IsOn())
			{
				player.SetAngle(seasaw.GetAngle());
				player.SetYPos(seasaw.GetNewPlayerYPos(player.GetPos()));
			}
			
			// If left of screen, reset 
			if(seasaw.GetPos().X+700 < player.GetPos().X)
			{
				seasaw.SetXPos(tntWall.GetPosition().X + 1000);
			}
			
		}
						
		public static void UpdateSpring()
		{
			spring.Update(0, moveSpeed);
			
			// If left of screen, reset 
			if(spring.GetPosition.X+700 < player.GetPos().X)
				spring.Reset(seasaw.GetPos().X+200);	
			
			// On/In Spring
			if(player.GetBottomBox().Overlaps(spring.GetBox()))
			{
				if(spring.MissedSpring)
				{
					// Die in lava	
					
				}	
				else if(spring.IsReleased)
					player.DoJump();
			}
			
		}
		
		public static void UpdateSpin()
		{
			spinObstacle.Update(0, moveSpeed);
			var motion = Motion.GetData(0);
			Vector3 acc = motion.Acceleration;
			
			Vector3 vel = motion.AngularVelocity;
			
			
			if(vel.Y > 0.10000f)
					
			spinObstacle.Left();
			
			if(vel.Y < -0.10000f)
				
				spinObstacle.Right();
			
			if(vel.Y < 0.10000f && vel.Y > -0.10000f )
					
			spinObstacle.Stop ();
			
			if(spinObstacle.GetPosition1.X+700 < player.GetPos().X)
				spinObstacle.Reset(spring.GetPosition.X+200);
			
		}
		
		public static void UpdateTnt()
		{
			tntWall.Update(0, moveSpeed);
			Vector2 touchPos = GetTouchPosition();
			Vector2 pluPos = tntWall.GetPosition();
			
			if(touchPos.Y <= pluPos.Y + 114.0f && touchPos.Y >= pluPos.Y - 50.0f
			   && touchPos.X <= pluPos.X + 114.0f && touchPos.X >= pluPos.X - 50.0f)				
			{
				tntWall.Tapped();
			}
			
			if (tntWall.GetShake())
			{
				shakeCamera = true;
				tntWall.SetShakeOff();
			}
			
			if(tntWall.GetPosition().X+700 < player.GetPos().X)
				tntWall.Reset(gameScene, geiser.GetPosition.X+200);
		}
		
		public static void UpdateGeiser()
		{
			geiser.Update(0, moveSpeed);
			Vector2 touchPos = GetTouchPosition();
			Vector2 geiserPos = geiser.GetPosition;
			if(touchPos.Y <= geiserPos.Y + 114.0f && touchPos.Y >= geiserPos.Y - 50.0f
			   && touchPos.X <= geiserPos.X + 114.0f && touchPos.X >= geiserPos.X - 50.0f)				
			{
				geiser.BreakSpike();
			}
			
			
			
			if(geiser.GetPosition.X+700 < player.GetPos().X)
				geiser.Reset(spinObstacle.GetPosition1.X + 200);
			
		}
		
		public static void SetupGame()
		{
			background 		= new Background(gameScene);
			tntWall 		= new TntWall(gameScene, 1500.0f, 100.0f);
			seasaw 			= new Seasaw(gameScene, background.GetFloorHeight(), 2300.0f);
			spring 			= new Spring(gameScene, new Vector2(3100.0f, 0.0f));
			spinObstacle 	= new SpinObstacle(gameScene, new Vector2(3900.0f, 0.0f));
			player 			= new Player(gameScene, background.GetFloorHeight());
			geiser			= new Geiser(gameScene, new Vector2(4700.0f, 0.0f));
			tutorialManager = new TutorialManager(gameScene);
		}
	}
}
