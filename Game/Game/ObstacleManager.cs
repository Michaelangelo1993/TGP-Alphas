using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace Game
{
	public class ObstacleManager
	{
		private static TntWall			tntWall;
		private static Seasaw 			seasaw;
		private static Spring 			spring;
		private static SpinObstacle     spinObstacle;
		private static Geiser			geiser;
		
		public ObstacleManager (Scene scene)
		{
			tntWall 		= new TntWall(scene, 1500.0f, 100.0f);
			seasaw 			= new Seasaw(scene, AppMain.GetBackground().GetFloorHeight(), 2300.0f);
			spring 			= new Spring(scene, new Vector2(3100.0f, 0.0f));
			spinObstacle 	= new SpinObstacle(scene, new Vector2(3900.0f, 0.0f));
			geiser			= new Geiser(scene, new Vector2(4700.0f, 0.0f));
		}
		
		public void Update(float moveSpeed)
		{
			seasaw.Update(0, moveSpeed);
			spring.Update(0, moveSpeed);
			spinObstacle.Update(0, moveSpeed);
			tntWall.Update(0, moveSpeed);
			geiser.Update(0, moveSpeed);
			
			if (tntWall.GetShake())
			{
				AppMain.SetShake(true);
				tntWall.SetShakeOff();
			}
		}
		
		public void UpdateTouchData(TouchData[] touches)
		{
			if(touches.Length <= 0) // Screen Not Touched
			{
				if(spring.BeingPushed)
					spring.ReleaseSpring(true);
				tntWall.ReleasePlunger();
			}
			else
			{
				Vector2 touchPos = AppMain.GetTouchPosition();
				
				if((touchPos.X-100 < spring.GetPosition.X) &&
				   (touchPos.X+125 > spring.GetPosition.X + spring.GetSpringWidth) &&
				   (touchPos.Y < spring.GetOriginalHeight+100)) // Touching spring
				{
					spring.PushSpring();
				}
			}	
		}
		
		public void CleanUp()
		{
			tntWall.Dispose();
			seasaw.Dispose();
			spring.Dispose();
			spinObstacle.Dispose();
			geiser.Dispose();
		}
	}
}

