using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace Game
{
	public class TntWall : Obstacle
	{
		//Sprites
		private SpriteUV 	boxSprite;
		private TextureInfo	boxTextureInfo;
		private SpriteUV 	pluSprite;
		private TextureInfo	pluTextureInfo;
		private SpriteUV 	rockSprite;
		private TextureInfo	rockTextureInfo;
		private Bounds2		rockBounds;
		private SpriteUV 	exploSprite;
		private TextureInfo	exploTextureInfo;
		private SpriteUV 	dynaSprite;
		private TextureInfo	dynaTextureInfo;
		
		private bool		blown = false;
		private bool		ready, beingPushed, enlarged;
		private int 		counter = 0, noOnSpritesheetWidth = 5, noOnSpritesheetHeight 	= 5, 
							heightCount, widthCount = 0;
		private Random 		rand;
		
		private float		scale = 5.0f;
		
		public bool IsReady { get { return ready; }}
		public bool SetReady { set { ready = value; }}
		
		override public float GetEndPosition() { return (rockSprite.Position.X + 128); }
		public void Tapped() { beingPushed = true; }
		override public void ReleasePlunger() { beingPushed = false; }
		
		public TntWall (Scene scene, float x, float y)
		{
			heightCount = noOnSpritesheetHeight - 1;
			
			//Textures
			boxTextureInfo 			= new TextureInfo("/Application/textures/box2.png");
			pluTextureInfo			= new TextureInfo("/Application/textures/tntplun2.png");
			rockTextureInfo 		= new TextureInfo("/Application/textures/rock.png");
			exploTextureInfo 		= new TextureInfo("/Application/textures/explosion.png");
			dynaTextureInfo	 		= new TextureInfo("/Application/textures/dyno2.png");
			
			//Sprites			
			boxSprite	 			= new SpriteUV(boxTextureInfo);
			pluSprite 				= new SpriteUV(pluTextureInfo);	
			rockSprite 				= new SpriteUV(rockTextureInfo);	
			exploSprite 			= new SpriteUV(exploTextureInfo);
			dynaSprite 				= new SpriteUV(dynaTextureInfo);
			
			boxSprite.Quad.S 		= boxTextureInfo.TextureSizef;
			boxSprite.Position		= new Vector2(x, y);
			
			dynaSprite.Quad.S 		= boxTextureInfo.TextureSizef;
			dynaSprite.Position 	= new Vector2(x + 120.0f, y);
			
			pluSprite.Quad.S 		= pluTextureInfo.TextureSizef;
			pluSprite.Position 		= new Vector2(x, y + 44.0f);
			
			rockSprite.Quad.S 		= rockTextureInfo.TextureSizef;
			rockSprite.Position 	= new Vector2(x+200.0f, y);
			rockBounds				= rockSprite.Quad.Bounds2();
			
			//Set up the explosion spritesheet&scale
			exploSprite.UV.S 		= new Vector2(1.0f/noOnSpritesheetWidth,1.0f/noOnSpritesheetHeight);		
			exploSprite.Quad.S 		= new Vector2(130.0f, 130.0f);
			exploSprite.Position	= new Vector2(dynaSprite.Position.X, dynaSprite.Position.Y);
			exploSprite.Scale		= new Vector2(scale, scale);
			exploSprite.Visible 	= false;
			
			scene.AddChild(rockSprite);
			scene.AddChild(pluSprite);
			scene.AddChild(boxSprite);
			scene.AddChild(dynaSprite);
			scene.AddChild(exploSprite);
			
			//Ready and counter initialisation
			ready 	= true;
			counter = 20;
			enlarged = false;
			
			// Random TNT Boss Mode
			rand = new Random();
		}
				
		override public void Dispose(Scene scene)
		{
			scene.RemoveChild(rockSprite, true);
			scene.RemoveChild(pluSprite, true);
			scene.RemoveChild(boxSprite, true);
			scene.RemoveChild(dynaSprite, true);
			scene.RemoveChild(exploSprite, true);
			boxTextureInfo.Dispose();
			pluTextureInfo.Dispose();
			rockTextureInfo.Dispose();
			exploTextureInfo.Dispose();
			dynaTextureInfo.Dispose();
		}
		
		override public void Update(float t)
		{	
			//If the trap is ready and being pushed, blow up the rock
			if(ready && beingPushed)
			{
				// If enlarged, scale sprite back whilst moving sprite down 
				if(enlarged)
				{
					pluSprite.Position = new Vector2(pluSprite.Position.X, pluSprite.Position.Y-(t/3));
					if(pluSprite.Scale.Y > 1)
						pluSprite.Scale = new Vector2(1, pluSprite.Scale.Y-t/60);
					
					if(pluSprite.Position.Y <= boxSprite.Position.Y + 10.0f)
					{
						blowUpRock ();
						ready = false;
					}
				}
				else
				{
					pluSprite.Position = new Vector2(pluSprite.Position.X, pluSprite.Position.Y-(t/3));
					
					if(pluSprite.Position.Y <= boxSprite.Position.Y + 10.0f)
					{
						blowUpRock ();
						ready = false;
					}
				}
			}
			
			if(blown)
			{
				if(counter<0)
				{
					exploSprite.Visible = false;
					counter=20;
				}
				else
				{
					counter--;
					
					//Spritesheet scrolling
					if (widthCount == noOnSpritesheetWidth)
					{
						heightCount--;
						widthCount = 0;
					}
				
					if (heightCount < 0)
					{
						heightCount = noOnSpritesheetHeight - 1;
					}
					widthCount++;
					exploSprite.UV.T = new Vector2((1.0f/noOnSpritesheetWidth)*widthCount,(1.0f/noOnSpritesheetHeight)*heightCount);
				}
			}
			//Move the sprites
			boxSprite.Position 	 += new Vector2(-t, 0);
			dynaSprite.Position  = new Vector2(boxSprite.Position.X + 120.0f, boxSprite.Position.Y);
			pluSprite.Position	 = new Vector2(pluSprite.Position.X - t, pluSprite.Position.Y);
			rockSprite.Position  = new Vector2(boxSprite.Position.X + 200.0f, boxSprite.Position.Y); 
			exploSprite.Position = new Vector2(dynaSprite.Position.X - 230.0f, dynaSprite.Position.Y - 150.0f);
			
			//Get touch position
			Vector2 touchPos = AppMain.GetTouchPosition();
			
			if(enlarged)
			{
				if(touchPos.Y <= pluSprite.Position.Y + 242.0f && touchPos.Y >= pluSprite.Position.Y - 50.0f
				   && touchPos.X <= pluSprite.Position.X + 114.0f && touchPos.X >= pluSprite.Position.X - 50.0f)				
				{
					beingPushed = true;
				}
			}
			else
			{
				if(touchPos.Y <= pluSprite.Position.Y + 114.0f && touchPos.Y >= pluSprite.Position.Y - 50.0f
				   && touchPos.X <= pluSprite.Position.X + 114.0f && touchPos.X >= pluSprite.Position.X - 50.0f)				
				{
					beingPushed = true;
				}
			}
			
			if(Touch.GetData(0).ToArray().Length <= 0)
				ReleasePlunger();
		}
		
		public void blowUpRock()
		{
			rockSprite.Visible = false;
			dynaSprite.Visible = false;
			
			if(!blown)
			{
				//Start the explosion
				exploSprite.Position = new Vector2(dynaSprite.Position.X,dynaSprite.Position.Y);
				exploSprite.Visible = true;
				
				blown 	= true;
				counter = 20;
				AppMain.SetShake(true);
			}
		}
		
		override public void Reset(float x)
		{
			int randomNum = (rand.Next(0, 10));
			
			//Reset functions
			rockSprite.Visible  = true;
			dynaSprite.Visible  = true;
			
			boxSprite.Position  = new Vector2(x, boxSprite.Position.Y);
			pluSprite.Position  = new Vector2(boxSprite.Position.X, boxSprite.Position.Y +44);
			dynaSprite.Position  = new Vector2(boxSprite.Position.X + 120.0f, boxSprite.Position.Y);
			rockSprite.Position  = new Vector2(boxSprite.Position.X + 200.0f, boxSprite.Position.Y); 
			exploSprite.Position = new Vector2(dynaSprite.Position.X, dynaSprite.Position.Y);
			
			// 30% chance to have triple size plunger
			if(randomNum > 2)
			{
				enlarged = false;
				pluSprite.Scale = new Vector2(1,1);
			}
			else
			{
				enlarged = true;
				pluSprite.Scale = new Vector2(1,3);
			}
			
			counter = 20;
			ready = true;
			blown = false;
			beingPushed = false;
		}
	}
}

