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
		
		private static SpinObstacle[]	spinObstacles;
				
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
			
			//Create spin obstacles.
			spinObstacles = new SpinObstacle[2];
			spinObstacles[0] = new SpinObstacle(gameScene, new Vector2(100.0f, 150.0f));	
			
			//Run the scene.
			Director.Instance.RunWithScene(gameScene, true);
		}
		
		public static void Update()
		{
			//Determine whether the player tapped the screen
			var touches = Touch.GetData(0);
			
			var motion = Motion.GetData(0);
			
			Vector3 acc = motion.Acceleration;
			
			Vector3 vel = motion.AngularVelocity;
			
			
			if(vel.Y > 0.10000f)
				
				spinObstacles[0].Left();
			
			if(vel.Y < -0.10000f)
				
				spinObstacles[0].Right();
			
			if(vel.Y < 0.10000f && vel.Y > -0.10000f )
				
				spinObstacles[0].Stop();
		
			
			//If tapped, do something
			if(touches.Count > 0)
			{
				
			}
				
			// Move update code here
			
			
			
			//if (spinObstacles[0].HasCollidedWith (player.Sprite) == true)
				//{
					//quitGame = true; 
				//}
			
		}		
	}
}
