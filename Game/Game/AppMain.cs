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
		
		private static float			moveSpeed = 3.0f;
				
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
			seasaw 		= new Seasaw(gameScene, new Vector2(3000.0f, 150.0f));
			spring 		= new Spring(gameScene, new Vector2(2000.0f, 0.0f));
			player 		= new Player(gameScene);
			
			
			//Run the scene.
			Director.Instance.RunWithScene(gameScene, true);
		}
		
		public static void Update()
		{
			player.SetAngle(0.0f);
			if(player.GetPos().Y > (100+115/2))
			{
				player.SetYPos((int)(player.GetPos().Y-2));
				// TODO : add player bounce fall
			}
			
			Vector2 touchPos = GetTouchPosition();
			
			// Update code here
			UpdateTouchData();
			UpdateSeasaw();
			UpdateSpring();
			player.Update(0);
			background.Update(0.0f, moveSpeed);
			tntWall.Update (0.0f, GetTouchPosition().X, GetTouchPosition().Y);
				
			// Move update code here
			//Update the camera to follow the player
			gameScene.Camera2D.SetViewY(new Vector2(0.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f),
			                            new Vector2(player.GetPos().X + 400, Director.Instance.GL.Context.GetViewport().Height*0.5f));
			// Sets the volcano background to follow the sprite
			background.SetVolcanoPosition((player.GetPos().X + 400)-(Director.Instance.GL.Context.GetViewport().Width/2), 0.0f);
			
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
			{
				touchPos = new Vector2((touchPos.X * 450) + 450, (touchPos.Y * 272)+272);	
			}
			else
			{
				touchPos = new Vector2(((touchPos.X+1) * 450), ((touchPos.Y+1) * 272));
			}
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
				System.Diagnostics.Debug.WriteLine(touchPos.Y);
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
			{
				// TODO: If player.IsOnSeasaw() == true, adjust angle and yoffset
				// TODO: Else, reduce y position and die in lava
				seasaw.SetIsOn();
				if(seasaw.IsOn())
				{
					player.SetAngle(seasaw.GetAngle());
					player.SetYPos(seasaw.GetNewPlayerYPos(player.GetPos()));
				}
			}
		}
						
		public static void UpdateSpring()
		{
			spring.Update(0, moveSpeed);
			
			// If left of screen, reset 
			if(spring.GetPosition.X+500 < player.GetPos().X)
			{
				spring.Reset();	
			}
			
			// On/In Spring
			if((player.GetPos().X+115/2 > spring.GetPosition.X-50) &&
			   (player.GetPos().X-115/2 < spring.GetPosition.X + spring.GetSpringWidth + 50))
			{
				if(spring.MissedSpring)
				{
					// Die in lava	
					
				}
				else
				{
					
					if(player.GetPos().Y-115/2 < spring.GetTop)
					{
						spring.MissSpring();	
					}
					else
					{
						System.Diagnostics.Debug.WriteLine(spring.GetTop);
						
						if(spring.IsReleased)
						{
							player.SetYPos(85+spring.GetTop);
							// TODO: Add player bounce
						}
					}
				}
			}
		}
	}
}
