using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace Game
{
	public class TntWall
	{
		private SpriteUV 	boxSprite;
		private TextureInfo	boxTextureInfo;
		
		private SpriteUV 	pluSprite;
		private TextureInfo	pluTextureInfo;
		
		private SpriteUV 	rockSprite;
		private TextureInfo	rockTextureInfo;
		
		private SpriteUV 	exploSprite;
		private TextureInfo	exploTextureInfo;
		
		private SpriteUV 	dynaSprite;
		private TextureInfo	dynaTextureInfo;
		
		private bool		blown = false;
		private bool		ready, shake, beingPushed;
		private int 		counter = 0, noOnSpritesheetWidth 	= 5, noOnSpritesheetHeight 	= 5, heightCount = 0, widthCount = 0;
		private float		scale = 5.0f;
		
		public void SetShakeOff() { shake = false; }
		public bool GetShake() { return shake; }
		public bool IsReady { get { return ready; }}
		public bool SetReady { set { ready = value; }}
		
		public TntWall (Scene scene, float x, float y)
		{
			boxTextureInfo 			= new TextureInfo("/Application/textures/box2.png");
			pluTextureInfo			= new TextureInfo("/Application/textures/tntplun2.png");
			rockTextureInfo 		= new TextureInfo("/Application/textures/rock.png");
			exploTextureInfo 		= new TextureInfo("/Application/textures/explosion.png");
			dynaTextureInfo	 		= new TextureInfo("/Application/textures/dyna2.png");
						
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
			
			
			exploSprite.UV.S 		= new Vector2(1.0f/noOnSpritesheetWidth,1.0f/noOnSpritesheetHeight);		
			exploSprite.Quad.S 		= new Vector2(130.0f, 130.0f);
			exploSprite.Position	= new Vector2(rockSprite.Position.X - 250.0f, rockSprite.Position.Y - 100.0f);
			exploSprite.Scale		= new Vector2(scale, scale);
			exploSprite.Visible 	= false;
			
			scene.AddChild(rockSprite);
			scene.AddChild(pluSprite);
			scene.AddChild(boxSprite);
			scene.AddChild(dynaSprite);
			scene.AddChild(exploSprite);
			
			ready 	= true;
			shake 	= false;
			counter = 0;
		}
				
		public void Dispose()
		{
			boxTextureInfo.Dispose();
			pluTextureInfo.Dispose();
			rockTextureInfo.Dispose ();
			exploTextureInfo.Dispose ();
		}
		
		public void Update(float deltaTime, float t)
		{	
			if(ready && beingPushed)
			{
				pluSprite.Position = new Vector2(pluSprite.Position.X, pluSprite.Position.Y-1.0f);
				
				if(pluSprite.Position.Y <= boxSprite.Position.Y + 10.0f)
				{
					blowUpRock ();
					ready = false;
				}
			}
			
			if(blown)
			{
				if(counter>20)
				{
					exploSprite.Visible = false;
					counter=0;
				}
				else
				{	
					counter++;
					
					if (widthCount == noOnSpritesheetWidth)
					{
						heightCount++;
						widthCount = 0;
					}
				
					if (heightCount == noOnSpritesheetHeight)
					{
						heightCount = 0;
					}
					widthCount++;
					exploSprite.UV.T = new Vector2((1.0f/noOnSpritesheetWidth)*widthCount,(1.0f/noOnSpritesheetHeight)*heightCount);
				}
			}
			
			boxSprite.Position 	 += new Vector2(-t, 0);
			dynaSprite.Position  = new Vector2(boxSprite.Position.X + 120.0f, boxSprite.Position.Y);
			pluSprite.Position	 = new Vector2(boxSprite.Position.X, pluSprite.Position.Y);
			rockSprite.Position  = new Vector2(boxSprite.Position.X + 200.0f, boxSprite.Position.Y); 
			exploSprite.Position = new Vector2(rockSprite.Position.X - 250.0f, rockSprite.Position.Y- 100.0f);
		}
		
		public void blowUpRock()
		{
			rockSprite.Visible = false;
			//boxSprite.Visible = false;
			//pluSprite.Visible = false;
			dynaSprite.Visible = false;
			
			if(!blown)
			{
				exploSprite.Position = new Vector2(rockSprite.Position.X-200.0f,rockSprite.Position.Y-80.0f);
				exploSprite.Visible = true;
				
				blown 	= true;
				counter = 0;
				shake	= true;
			}
		}
		
		public void Tapped()
		{
			beingPushed = true;
		}
		
		public void ReleasePlunger()
		{
			beingPushed = false;
		}
		
		public Vector2 GetPosition()
		{
			return pluSprite.Position;
		}
		
		public void Reset(Scene scene, float x)
		{
			rockSprite.Visible  = true;
			//boxSprite.Visible = true;
			//pluSprite.Visible = true;
			dynaSprite.Visible  = true;
			
			boxSprite.Position  += new Vector2(x, 0);
			pluSprite.Position  = new Vector2(boxSprite.Position.X, boxSprite.Position.Y +44);
			counter = 20;
			ready = true;
			blown = false;
			beingPushed = false;
		}
	}
}

