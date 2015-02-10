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
		private static Background		background;
		private static TntWall			tntWall;
		private static Player 			player;
		private static Seasaw 			seasaw;
		private static Spring 			spring;
		
		private static int 				frameTime = 0, currentFrameTime = 0;
		private static float			moveSpeed = 3.0f;
		private static bool				shakeCamera = false;
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
			
			background 	= new Background(gameScene);
			tntWall 	= new TntWall(gameScene, 1000.0f, 100.0f);
			seasaw 		= new Seasaw(gameScene, background.GetFloorHeight(), 3000.0f);
			spring 		= new Spring(gameScene, new Vector2(2000.0f, 0.0f));
			player 		= new Player(gameScene, background.GetFloorHeight());
			
			//Run the scene.
			Director.Instance.RunWithScene(gameScene, true);
		}
		
		public static void Update()
		{		
			Vector2 touchPos = GetTouchPosition();
			
			// Update code here
			player.Update(0.0f);
			UpdateTouchData();
			UpdateSeasaw();
			UpdateSpring();
			background.Update(0.0f, moveSpeed);
			tntWall.Update (0.0f, GetTouchPosition().X, GetTouchPosition().Y);
			UpdateCamera();
			
			// Sets the volcano background to follow the sprite
			background.SetVolcanoPosition((player.GetPos().X + 400)-(Director.Instance.GL.Context.GetViewport().Width/2), 0.0f);
			
		}
		
		public static void UpdateCamera()
		{
			if (shakeCamera)
			{
				if(currentFrameTime == 1)
				{
					gameScene.Camera2D.SetViewY(new Vector2(0.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f),
			                            new Vector2(player.GetPos().X + 405, Director.Instance.GL.Context.GetViewport().Height*0.5f));
				}
				if(currentFrameTime == 2)
				{
					gameScene.Camera2D.SetViewY(new Vector2(0.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f),
			                            new Vector2(player.GetPos().X + 400, Director.Instance.GL.Context.GetViewport().Height*0.51f));
				}
				if(currentFrameTime == 3)
				{
					gameScene.Camera2D.SetViewY(new Vector2(0.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f),
			                            new Vector2(player.GetPos().X + 395, Director.Instance.GL.Context.GetViewport().Height*0.5f));
				}
				if(currentFrameTime == 4)
				{
					gameScene.Camera2D.SetViewY(new Vector2(0.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f),
			                            new Vector2(player.GetPos().X + 400, Director.Instance.GL.Context.GetViewport().Height*0.49f));
					currentFrameTime = 0;
				}
				if (frameTime == 60)
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
			var touches = Touch.GetData(0).ToArray();
			if(touches.Length <= 0)
			{
				// Screen Not Touched
				if(spring.BeingPushed)
				{
					spring.ReleaseSpring(true);
				}
			}
			else
			{
				// Screen Touched
				Vector2 touchPos = GetTouchPosition();
				// Touching spring
				if((touchPos.X-100 < spring.GetPosition.X) &&
				   (touchPos.X+125 > spring.GetPosition.X + spring.GetSpringWidth) &&
				   (touchPos.Y < spring.GetOriginalHeight+100))
				{
					//spring.ReleaseSpring(false);
					//spring.WindSpring();
					spring.PushSpring();
				}
				
				// TODO Touching TNT - Push down handle
				// TODO Touching Guiser Stalagmite - Release it
				// TODO Touching Spinning - Rotate them (unless Gyro?)
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
		}
						
		public static void UpdateSpring()
		{
			spring.Update(0, moveSpeed);
			
			// If left of screen, reset 
			if(spring.GetPosition.X+500 < player.GetPos().X)
				spring.Reset();	
			
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
	}
}
