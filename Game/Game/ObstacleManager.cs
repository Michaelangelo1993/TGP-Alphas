using System;
using System.Collections.Generic;
using System.Linq;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace Game
{
	public class ObstacleManager
	{
		private static List<Obstacle> activeObstacles = new List<Obstacle>();
		private static List<Obstacle> deactiveObstacles = new List<Obstacle>();
		private static List<Obstacle> offScreenObjs = new List<Obstacle>();
		
		private int obstaclesDefeated;
		private float newXPos;
		private Random rand;
		
		public int GetObstaclesDefeated() { return obstaclesDefeated; }
		
		public ObstacleManager (Scene scene)
		{
			rand = new Random();
			newXPos = 1500;
			obstaclesDefeated = 0;
			
			deactiveObstacles.Add(new TntWall(scene, -1000.0f, 60.0f));
			deactiveObstacles.Add(new Seasaw(scene, -1000.0f, 100.0f));
			deactiveObstacles.Add(new Spring(scene, new Vector2(-1000.0f, 60.0f)));
			deactiveObstacles.Add(new SpinObstacle(scene, new Vector2(-1000.0f, 0.0f)));
			deactiveObstacles.Add(new Geiser(scene, new Vector2(-1000.0f, 60.0f)));
			deactiveObstacles.Add(new TntWall(scene, -1000.0f, 60.0f));
			deactiveObstacles.Add(new Seasaw(scene, -1000.0f, 100.0f));
			deactiveObstacles.Add(new Spring(scene, new Vector2(-1000.0f, 60.0f)));
			deactiveObstacles.Add(new SpinObstacle(scene, new Vector2(-1000.0f, 0.0f)));
			deactiveObstacles.Add(new Geiser(scene, new Vector2(-1000.0f, 60.0f)));
			deactiveObstacles.Add(new DoorObs(scene, -1000.0f, 100.0f));
			deactiveObstacles.Add(new DoorObs(scene, -1000.0f, 100.0f));
			
			float prevXPos = newXPos;
			while(activeObstacles.Count <= 3)
			{// Reset position of selected obstacle and move it to active
				prevXPos = newXPos;
				int randomPosition = (rand.Next(0, deactiveObstacles.Count));
				deactiveObstacles.ElementAt(randomPosition).Reset(newXPos);
				newXPos = 700 + deactiveObstacles.ElementAt(randomPosition).GetEndPosition();
				activeObstacles.Add(deactiveObstacles.ElementAt(randomPosition));
				deactiveObstacles.RemoveAt(randomPosition);
			}	
			newXPos = prevXPos;
		}
		
		public void Update(float moveSpeed)
		{
			if(activeObstacles.Count > 0)
			{	
				// check for offscreen obstacles and move them to deactive list
				var selected = activeObstacles.Where (Obstacle => Obstacle.GetEndPosition() + 80 < AppMain.GetPlayer().GetPos ().X).ToList();
				if(selected.Count > 0)
				{
					selected.ForEach(Obstacle => activeObstacles.Remove(Obstacle));
					selected.ForEach(Obstacle => deactiveObstacles.Add(Obstacle));
					obstaclesDefeated++;
				}
			}
			
			while(activeObstacles.Count <= 3)
			{// Reset position of selected obstacle and move it to active
				int randomPosition = (rand.Next(0, deactiveObstacles.Count));
				deactiveObstacles.ElementAt(randomPosition).Reset(newXPos);
				newXPos = 100 + deactiveObstacles.ElementAt(randomPosition).GetEndPosition();
				activeObstacles.Add(deactiveObstacles.ElementAt(randomPosition));
				deactiveObstacles.RemoveAt(randomPosition);
			}			
			foreach(Obstacle obj in activeObstacles)
			{
				obj.Update(moveSpeed);
			}
		}
		
		public void CleanUp()
		{
			foreach(Obstacle obj in deactiveObstacles)
			{
				obj.Dispose();
			}
		}
	}
}

