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
		private SpriteUV 	boxSprite;
		private TextureInfo	boxTextureInfo;
		private SpriteUV 	pluSprite;
		private TextureInfo	pluTextureInfo;
		private SpriteUV 	rockSprite;
		private TextureInfo	rockTextureInfo;
		private Bounds2		rockBounds;
		private SpriteUV 	exploSprite;
		private TextureInfo	exploTextureInfo;
		private SpriteUV 	dynoSprite;
		private TextureInfo	dynoTextureInfo;
		private bool		blown = false;
		private bool		ready;
		private int 		counter = 20;
		private bool		beingPushed;
		public bool IsReady { get { return ready; }}
		public bool SetReady { set { ready = value; }}
		
		override public float GetEndPosition() { return (rockSprite.Position.X + 128); }
		
		public TntWall (Scene scene, float x, float y)
		{
			boxTextureInfo 			= new TextureInfo("/Application/textures/box2.png");
			pluTextureInfo			= new TextureInfo("/Application/textures/tntplun2.png");
			rockTextureInfo 		= new TextureInfo("/Application/textures/rock.png");
			exploTextureInfo 		= new TextureInfo("/Application/textures/explo.png");
			dynoTextureInfo	 		= new TextureInfo("/Application/textures/dyno2.png");
						
			boxSprite	 			= new SpriteUV(boxTextureInfo);
			pluSprite 				= new SpriteUV(pluTextureInfo);	
			rockSprite 				= new SpriteUV(rockTextureInfo);	
			exploSprite 			= new SpriteUV(exploTextureInfo);
			dynoSprite 				= new SpriteUV(dynoTextureInfo);
			
			boxSprite.Quad.S 		= boxTextureInfo.TextureSizef;
			boxSprite.Position		= new Vector2(x, y);
			
			dynoSprite.Quad.S 		= boxTextureInfo.TextureSizef;
			dynoSprite.Position 	= new Vector2(x + 120.0f, y);
			
			pluSprite.Quad.S 		= pluTextureInfo.TextureSizef;
			pluSprite.Position 		= new Vector2(x, y + 44.0f);
			
			rockSprite.Quad.S 		= rockTextureInfo.TextureSizef;
			rockSprite.Position 	= new Vector2(x+200.0f, y);
			rockBounds				= rockSprite.Quad.Bounds2();
			
			exploSprite.Quad.S 		= exploTextureInfo.TextureSizef;
			exploSprite.Position	= new Vector2(rockSprite.Position.X - 150.0f, rockSprite.Position.Y);
			exploSprite.Visible 	= false;
			
			scene.AddChild(rockSprite);
			scene.AddChild(pluSprite);
			scene.AddChild(boxSprite);
			scene.AddChild(dynoSprite);
			scene.AddChild(exploSprite);
			
			ready 	= true;
			counter = 20;
		}
				
		override public void Dispose()
		{
			boxTextureInfo.Dispose();
			pluTextureInfo.Dispose();
			rockTextureInfo.Dispose ();
			exploTextureInfo.Dispose ();
		}
		
		override public void Update(float deltaTime, float t)
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
				if(counter<0)
				{
					exploSprite.Visible = false;
					counter=20;
				}
				else
				{
					counter--;
				}
			}
			
			boxSprite.Position 	 += new Vector2(-t, 0);
			dynoSprite.Position  = new Vector2(boxSprite.Position.X + 120.0f, boxSprite.Position.Y);
			pluSprite.Position	 = new Vector2(boxSprite.Position.X, pluSprite.Position.Y);
			rockSprite.Position  = new Vector2(boxSprite.Position.X + 200.0f, boxSprite.Position.Y); 
			exploSprite.Position = new Vector2(rockSprite.Position.X - 150.0f, rockSprite.Position.Y);
			
			Vector2 touchPos = AppMain.GetTouchPosition();
			
			if(touchPos.Y <= pluSprite.Position.Y + 114.0f && touchPos.Y >= pluSprite.Position.Y - 50.0f
			   && touchPos.X <= pluSprite.Position.X + 114.0f && touchPos.X >= pluSprite.Position.X - 50.0f)				
			{
				beingPushed = true;
			}
			
			if(Touch.GetData(0).ToArray().Length <= 0)
				ReleasePlunger();
		}
		
		public void blowUpRock()
		{
			rockSprite.Visible = false;
			dynoSprite.Visible = false;
			
			if(!blown)
			{
				exploSprite.Position = new Vector2(rockSprite.Position.X-200.0f,rockSprite.Position.Y-80.0f);
				exploSprite.Visible = true;
				
				blown 	= true;
				counter = 20;
				AppMain.SetShake(true);
			}
		}
		
		public void Tapped()
		{
			beingPushed = true;
		}
		
		override public void ReleasePlunger()
		{
			beingPushed = false;
		}
		
		public Vector2 GetPosition()
		{
			return pluSprite.Position;
		}
		
		override public void Reset(float x)
		{
			rockSprite.Visible  = true;
			dynoSprite.Visible  = true;
			
			boxSprite.Position  = new Vector2(x, boxSprite.Position.Y);
			pluSprite.Position  = new Vector2(boxSprite.Position.X, boxSprite.Position.Y +44);
			dynoSprite.Position  = new Vector2(boxSprite.Position.X + 120.0f, boxSprite.Position.Y);
			rockSprite.Position  = new Vector2(boxSprite.Position.X + 200.0f, boxSprite.Position.Y); 
			exploSprite.Position = new Vector2(rockSprite.Position.X - 150.0f, rockSprite.Position.Y);
			
			counter = 20;
			ready = true;
			blown = false;
			beingPushed = false;
		}
	}
}

