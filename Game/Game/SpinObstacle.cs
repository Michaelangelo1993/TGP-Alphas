using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace Game
{
	public class SpinObstacle
	{
		
		//Private variables.
		private 	SpriteUV[] 	spinSprite;
		private 	SpriteUV[] 	pivSprite;
		private 	TextureInfo	textureSpinObstacle;
		private 	TextureInfo	textureSpinPiv;
		private 	TextureInfo	textureSpinLeverOn;
		private 	TextureInfo	textureSpinLeverOff;
		private bool			stop;
		private bool			on;
		
		public float Width;
		public float Height;
		
		public Vector2 GetPosition1 { get { return spinSprite[0].Position; }}
		public Vector2 GetPosition2 { get { return spinSprite[1].Position; }}
		public Vector2 GetPosition3 { get { return spinSprite[2].Position; }}
		public float GetWidth { get { return Width; }}
		
	
		
		
		
		//Public functions.
		public SpinObstacle (Scene scene, Vector2 position)
		{
			textureSpinObstacle     = new TextureInfo("/Application/textures/firebeam.png");
			textureSpinPiv     		= new TextureInfo("/Application/textures/piv.png");
			
			stop  = false;
			on = 	false; 
		
			pivSprite	= new SpriteUV[3];
			
			//1st pivot
			pivSprite[0]			= new SpriteUV(textureSpinPiv);	
			pivSprite[0].Quad.S 	= textureSpinPiv.TextureSizef;
			
			//Add to the current scene.
			scene.AddChild(pivSprite[0]);
			
			//2nd pivot
			pivSprite[1]			= new SpriteUV(textureSpinPiv);	
			pivSprite[1].Quad.S 	= textureSpinPiv.TextureSizef;
			
			//Add to the current scene.
			scene.AddChild(pivSprite[1]);
			
			//3rd pivot
			pivSprite[2]			= new SpriteUV(textureSpinPiv);	
			pivSprite[2].Quad.S 	= textureSpinPiv.TextureSizef;
			
			//Add to the current scene.
			scene.AddChild(pivSprite[2]);
			
			
			
			
			spinSprite	= new SpriteUV[3];
			
			//1st Obstacle
			spinSprite[0]			= new SpriteUV(textureSpinObstacle);	
			spinSprite[0].Quad.S 	= textureSpinObstacle.TextureSizef;
			spinSprite[0].CenterSprite(new Vector2(0.5f,0.5f));
			//Add to the current scene.
			scene.AddChild(spinSprite[0]);
			
			//2nd Obstacle
			spinSprite[1]			= new SpriteUV(textureSpinObstacle);	
			spinSprite[1].Quad.S 	= textureSpinObstacle.TextureSizef;
			spinSprite[1].CenterSprite(new Vector2(0.5f,0.5f));
			//Add to the current scene.
			scene.AddChild(spinSprite[1]);
			
			//3rd Obstacle
			spinSprite[2]			= new SpriteUV(textureSpinObstacle);	
			spinSprite[2].Quad.S 	= textureSpinObstacle.TextureSizef;
			spinSprite[2].CenterSprite(new Vector2(0.5f,0.5f));
			//Add to the current scene.
			scene.AddChild(spinSprite[2]);
			
			
			//Position Obstacle
			spinSprite[0].Position = new Vector2(450.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f);
			                              
			spinSprite[1].Position = new Vector2(650.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f);
			
			spinSprite[2].Position = new Vector2(850.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f);
			
			//Position pivot
			pivSprite[0].Position = new Vector2(440.0f,260.0f);
			                              
			pivSprite[1].Position = new Vector2(640.0f,260.0f);
			
			pivSprite[2].Position = new Vector2(840.0f,260.0f);
			
	
		}
		
		public void Dispose()
		{
			textureSpinObstacle.Dispose();
			textureSpinPiv.Dispose();
			
		}
		
		public void Update(float deltaTime)
		{			
				
		}
		
	
		
		
		public void Right()
			
		{
			
			spinSprite[0].Rotate(0.060f);
			spinSprite[1].Rotate(0.090f);
			spinSprite[2].Rotate(0.030f);
			
		}
		
		public void Stop()
			
		{
			
			spinSprite[0].Rotate(0.00f);
			spinSprite[1].Rotate(0.00f);
			spinSprite[2].Rotate(0.00f);
			
		}
		
		public void Left()
			
		{
			
			spinSprite[0].Rotate(-0.060f);
			spinSprite[1].Rotate(-0.090f);
			spinSprite[2].Rotate(-0.030f);
			
		}
		
		
		public bool HasCollidedWith(SpriteUV sprite)
		{
			//beam1 bounds
			Bounds2 beam1 = spinSprite[0].GetlContentLocalBounds();
			spinSprite[0].GetContentWorldBounds(ref beam1);
			
			//beam2 bounds
			Bounds2 beam2 = spinSprite[1].GetlContentLocalBounds();
			spinSprite[1].GetContentWorldBounds(ref beam2);
			
			//beam3 bounds
			Bounds2 beam3 = spinSprite[2].GetlContentLocalBounds();
			spinSprite[2].GetContentWorldBounds(ref beam3);
			
			//player bounds
			Bounds2 player = sprite.GetlContentLocalBounds();
			sprite.GetContentWorldBounds(ref player);
			
			if (player.Overlaps(beam1))
			{
				return true; 
			}
			
			if (player.Overlaps(beam2))
			{
				return true; 
			}
			
			if (player.Overlaps(beam3))
			{
				return true; 
			}
			
			else 
			{
				return false;
			}
		}
	}
}

