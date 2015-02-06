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
		private 	SpriteUV 	leverOnSprite;
		private 	SpriteUV 	leverOffSprite;
		private 	SpriteUV[] 	pivSprite;
		private 	TextureInfo	textureSpinObstacle;
		private 	TextureInfo	textureSpinPiv;
		private 	TextureInfo	textureSpinLeverOn;
		private 	TextureInfo	textureSpinLeverOff;
		private bool			stop;
		private bool			on;
		
		//Public functions.
		public SpinObstacle (float startX, Scene scene)
		{
			textureSpinObstacle     = new TextureInfo("/Application/textures/metal.png");
			textureSpinPiv     		= new TextureInfo("/Application/textures/piv.png");
			textureSpinLeverOn      = new TextureInfo ("/Application/textures/Lever2.png");
			textureSpinLeverOff     = new TextureInfo ("/Application/textures/Lever1.png");
			
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
			
			
			// Lever	
			leverOffSprite			= new SpriteUV(textureSpinLeverOff);	
			leverOffSprite.Quad.S 	= textureSpinLeverOff.TextureSizef;
			
			leverOnSprite			= new SpriteUV(textureSpinLeverOn);	
			leverOnSprite.Quad.S 	= textureSpinLeverOn.TextureSizef;
			
			//Add to the current scene.
			scene.AddChild(leverOffSprite);
			scene.AddChild(leverOnSprite);
			
			
			
			//Position Obstacle
			spinSprite[0].Position = new Vector2(450.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f);
			                              
			spinSprite[1].Position = new Vector2(650.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f);
			
			spinSprite[2].Position = new Vector2(850.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f);
			
			//Position pivot
			pivSprite[0].Position = new Vector2(440.0f,260.0f);
			                              
			pivSprite[1].Position = new Vector2(640.0f,260.0f);
			
			pivSprite[2].Position = new Vector2(840.0f,260.0f);
			
			//Position lever
			leverOffSprite.Position = new Vector2 (40.0f,80.0f);
			
			//Position lever
			leverOnSprite.Position = new Vector2 (40.0f,80.0f);
		}
		
		public void Dispose()
		{
			textureSpinObstacle.Dispose();
			textureSpinPiv.Dispose();
			textureSpinLeverOn.Dispose();
			textureSpinLeverOff.Dispose();
			
		}
		
		public void Update(float deltaTime)
		{			
			
			if(stop)
			{
				spinSprite[0].Rotate(0.001f);
				spinSprite[1].Rotate(0.001f);
				spinSprite[2].Rotate(0.001f);
				
				stop = false;
			}
			
			else
			{
				spinSprite[0].Rotate(0.060f);
				spinSprite[1].Rotate(0.090f);
				spinSprite[2].Rotate(0.030f);
				
			}  
			
			if(on)
			{
				leverOffSprite.Visible = false;
				leverOnSprite.Visible = true; 
				on = false;
			}
			
			else 
			{
				leverOffSprite.Visible = true; 
				leverOnSprite.Visible = false; 
			}
			
			
		}
		
		public void Tapped()
		{
			if(!stop)
			{
				stop = true;
			}
			
			if(!on)
			{
				on = true;
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
			Bounds2 bird = sprite.GetlContentLocalBounds();
			sprite.GetContentWorldBounds(ref bird);
			
			if (bird.Overlaps(beam1))
			{
				return true; 
			}
			
			if (bird.Overlaps(beam2))
			{
				return true; 
			}
			
			if (bird.Overlaps(beam3))
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


