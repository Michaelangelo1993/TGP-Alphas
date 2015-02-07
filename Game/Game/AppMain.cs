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
			uiScene = new Sce.PlayStation.HighLevel.UI.Scene();
			Panel panel  = new Panel();
			panel.Width  = Director.Instance.GL.Context.GetViewport().Width;
			panel.Height = Director.Instance.GL.Context.GetViewport().Height;
			
			uiScene.RootWidget.AddChildLast(panel);
			UISystem.SetScene(uiScene);
			
			background 		= new Background(gameScene);
			tntWall = new TntWall(gameScene, 600.0f, 100.0f);
			seasaw = new Seasaw(gameScene, new Vector2(2000.0f, 150.0f));
			spring = new Spring(gameScene, new Vector2(1500.0f, 0.0f));
			player = new Player(gameScene);
			
			//Run the scene.
			Director.Instance.RunWithScene(gameScene, true);
		}
		
		public static void Update()
		{
			//Determine whether the player tapped the screen
			var touches = Touch.GetData(0);
			var y = Input2.Touch00.Pos.Y;
			var x = Input2.Touch00.Pos.X;
			
			// Update code here
			seasaw.Update(0, moveSpeed);
			spring.Update(0, moveSpeed);
			player.Update(0);
			background.Update(0.0f, moveSpeed);
			tntWall.Update (0.0f);
			
			isColliding();
			
			//If tapped, do something
			if(touches.Count > 0)
			{
				tntWall.Tapped(x, y);
			}
				
			// Move update code here
			//Update the camera to follow the player
//			gameScene.Camera2D.SetViewY(new Vector2(0.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f),
//			                            new Vector2(player.GetPos().X + 400, Director.Instance.GL.Context.GetViewport().Height*0.5f));
			background.SetVolcanoPosition((player.GetPos().X + 400)-(Director.Instance.GL.Context.GetViewport().Width/2), 0.0f);
			
		}
		
		public static void isColliding()
		{
			if(player.GetBox().Overlaps(seasaw.GetBox()))
			{
				player.SetAngle(seasaw.GetAngle());
			}
			else
				player.SetAngle(0.0f);
		}
	}
}
