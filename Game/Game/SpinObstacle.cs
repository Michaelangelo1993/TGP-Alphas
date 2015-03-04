using System;

using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace Game
{
	public class SpinObstacle : Obstacle
	{
		
		//Private variables.
		private 	SpriteUV[] 	spinSprite;
		private 	SpriteUV[] 	pivSprite;
		private 	Bounds2		spinBounds;
		private 	TextureInfo	textureSpinObstacle;
		private 	TextureInfo	textureSpinPiv;
		
		private int 	 numberOfObstacles = 3;
		
		public Vector2 GetPosition1 { get { return spinSprite[0].Position; }}
		public Vector2 GetPosition2 { get { return spinSprite[1].Position; }}
		public Vector2 GetPosition3 { get { return spinSprite[2].Position; }}
		override public float GetEndPosition() { return (spinSprite[2].Position.X + spinBounds.Point10.X*2); }
		
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
				spinBounds				= spinSprite[i].Quad.Bounds2();
				spinSprite[i].CenterSprite();
				
				spinSprite[i].Position = pivSprite[i].Position;
				
				//scene.AddChild(pivSprite[i]);
				scene.AddChild(spinSprite[i]);
			}
			
			spinSprite[0].Angle = 2.12f;
			spinSprite[1].Angle = 1.02f;
			spinSprite[2].Angle = 2.12f;
		}
		
		override public void Dispose()
		{
			textureSpinObstacle.Dispose();
			textureSpinPiv.Dispose();
		}
		
		override public void Update(float t)
		{			
			var motionData = Motion.GetData(0);
			
			for (int i = 0; i < numberOfObstacles; i++)
			{
				pivSprite[i].Position += new Vector2(-t, 0.0f);
				spinSprite[i].Position = pivSprite[i].Position;
				
				if(motionData.Acceleration.X< 0)
					Left ();
				else if(motionData.Acceleration.X > 0)
					Right ();
				
				if (spinSprite[i].Angle > 2.12f)
					spinSprite[i].Angle 	= 2.12f;
				else if (spinSprite[i].Angle < 1.02f)
					spinSprite[i].Angle 	= 1.02f;
			}
		}

		public void Right()	
		{
			spinSprite[0].Rotate(-0.01f);
			spinSprite[1].Rotate(0.01f);
			spinSprite[2].Rotate(-0.01f);
		}
		
		public void Left()
		{
			spinSprite[0].Rotate(0.01f);
			spinSprite[1].Rotate(-0.01f);
			spinSprite[2].Rotate(0.01f);
		}
		
		override public void Reset(float x)
		{
			
			spinSprite[0].Angle = 2.12f;
			spinSprite[1].Angle = 1.02f;
			spinSprite[2].Angle = 2.12f;
			
			for (int i = 0; i < numberOfObstacles; i++)
			{
				pivSprite[i].Position = new Vector2(x +(i * 200.0f),Director.Instance.GL.Context.GetViewport().Height*0.45f);
				spinSprite[i].Position = pivSprite[i].Position;
			}			
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

