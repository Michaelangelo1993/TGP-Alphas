using System;

using Sce.PlayStation.Core.Input;
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
		
		private int 	 numberOfObstacles = 3;
		
		public Vector2 GetPosition1 { get { return spinSprite[0].Position; }}
		public Vector2 GetPosition2 { get { return spinSprite[1].Position; }}
		public Vector2 GetPosition3 { get { return spinSprite[2].Position; }}
		
		
		//Public functions.
		public SpinObstacle (Scene scene, Vector2 position)
		{
			textureSpinObstacle     = new TextureInfo("/Application/textures/firebeam.png");
			textureSpinPiv     		= new TextureInfo("/Application/textures/piv.png");
		
			pivSprite	= new SpriteUV[numberOfObstacles];
			spinSprite	= new SpriteUV[numberOfObstacles];
			
			for (int i = 0; i < numberOfObstacles; i++)
			{
				pivSprite[i]			= new SpriteUV(textureSpinPiv);	
				pivSprite[i].Quad.S 	= textureSpinPiv.TextureSizef;
				pivSprite[i].CenterSprite();
				pivSprite[i].Position = new Vector2(position.X +(i *200.0f),Director.Instance.GL.Context.GetViewport().Height*0.45f);
				
				spinSprite[i]			= new SpriteUV(textureSpinObstacle);	
				spinSprite[i].Quad.S 	= textureSpinObstacle.TextureSizef;
				spinSprite[i].CenterSprite();
				
				spinSprite[i].Position = pivSprite[i].Position;
				
				//scene.AddChild(pivSprite[i]);
				scene.AddChild(spinSprite[i]);
			}
			spinSprite[0].Rotate(-0.9f);
			spinSprite[1].Rotate(0.9f);
			spinSprite[2].Rotate(-0.9f);
		}
		
		public void Dispose()
		{
			textureSpinObstacle.Dispose();
			textureSpinPiv.Dispose();
		}
		
		public void Update(float deltaTime, float t)
		{			
			for (int i = 0; i < numberOfObstacles; i++)
			{
				pivSprite[i].Position += new Vector2(-t, 0.0f);
				spinSprite[i].Position = pivSprite[i].Position;
			}
			
			var motion = Motion.GetData(0);
			Vector3 acc = motion.Acceleration;
			Vector3 vel = motion.AngularVelocity;
			
			
			if(vel.Y > 0.10000f)					
				Left();
			
			if(vel.Y < -0.10000f)
				Right();
			
			if(vel.Y < 0.10000f && vel.Y > -0.10000f )
				Stop ();
			
			//if(spinObstacle.GetPosition1.X+700 < player.GetPos().X)
				// Reset spinObstacle.Reset(spring.GetPosition.X+200);
		}

		public void Right()	
		{
			spinSprite[0].Rotate(0.060f);
			spinSprite[1].Rotate(0.060f);
			spinSprite[2].Rotate(0.060f);
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
			spinSprite[1].Rotate(-0.060f);
			spinSprite[2].Rotate(-0.060f);
		}
		
		public void Reset(float x)
		{
			for (int i = 0; i < numberOfObstacles; i++)
				pivSprite[i].Position = new Vector2(spinSprite[i].Position.X+x,spinSprite[i].Position.Y);
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

