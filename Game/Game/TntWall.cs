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
		private static SpriteUV 	boxSprite;
		private static TextureInfo	boxTextureInfo;
		private static SpriteUV 	pluSprite;
		private static TextureInfo	pluTextureInfo;
		private static SpriteUV 	rockSprite;
		private static TextureInfo	rockTextureInfo;
		private static SpriteUV 	exploSprite;
		private static TextureInfo	exploTextureInfo;
		private static SpriteUV 	dynoSprite;
		private static TextureInfo	dynoTextureInfo;
		private static Scene tScene;
		private static bool			blown = false;
		private static int 			counter = 20;
		private static float		startX;
		private static float		startY;
		private static bool			beingPushed;
		private static Vector2		min, max;
		private static Bounds2		box;
		
		public TntWall (Scene scene, float x, float y)
		{
			boxTextureInfo = new TextureInfo("/Application/textures/box2.png");
			pluTextureInfo = new TextureInfo("/Application/textures/tntplun.png");
			rockTextureInfo = new TextureInfo("/Application/textures/rock.png");
			exploTextureInfo = new TextureInfo("/Application/textures/explo.png");
			dynoTextureInfo = new TextureInfo("/Application/textures/dyno2.png");
			
			tScene = scene;
			startX = x;
			startY = y;
			
			boxSprite	 	= new SpriteUV(boxTextureInfo);
			pluSprite 		= new SpriteUV(pluTextureInfo);	
			rockSprite 		= new SpriteUV(rockTextureInfo);	
			exploSprite 	= new SpriteUV(exploTextureInfo);
			dynoSprite 	= new SpriteUV(dynoTextureInfo);
			
			boxSprite.Quad.S 	= boxTextureInfo.TextureSizef;
			boxSprite.Position = new Vector2(x, y);
			
			dynoSprite.Quad.S 	= boxTextureInfo.TextureSizef;
			dynoSprite.Position = new Vector2(x + 120.0f, y - 5.0f);
			
			pluSprite.Quad.S 	= pluTextureInfo.TextureSizef;
			pluSprite.Position = new Vector2(x, y + 44.0f);
			
			rockSprite.Quad.S 	= rockTextureInfo.TextureSizef;
			rockSprite.Position = new Vector2(x+200.0f, y);
			
			exploSprite.Quad.S 	= exploTextureInfo.TextureSizef;
			exploSprite.Position = new Vector2(rockSprite.Position.X - 150.0f, rockSprite.Position.Y);
			
			scene.AddChild(rockSprite);
			scene.AddChild(pluSprite);
			scene.AddChild(boxSprite);
			scene.AddChild(dynoSprite);
			
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
			if(beingPushed)
			{
				pluSprite.Position = new Vector2(pluSprite.Position.X, pluSprite.Position.Y-1.0f);
			}

			if(pluSprite.Position.Y <= boxSprite.Position.Y + 10.0f)
			{
				blowUpRock ();
				pluSprite.Position = new Vector2(boxSprite.Position.X, boxSprite.Position.Y + 44.0f);
			}
			
			counter--;
			
			if(counter<0)
			{
				tScene.RemoveChild(exploSprite, false);
				counter=20;
			}
			
			
			boxSprite.Position += new Vector2(-t, 0);
			dynoSprite.Position = new Vector2(boxSprite.Position.X + 120.0f, boxSprite.Position.Y  - 5.0f);
			pluSprite.Position = new Vector2(boxSprite.Position.X, pluSprite.Position.Y);
			rockSprite.Position = new Vector2(boxSprite.Position.X + 200.0f, boxSprite.Position.Y); 
			exploSprite.Position = new Vector2(rockSprite.Position.X - 150.0f, rockSprite.Position.Y);
			
			//Storing Bounds2 box data for collisions
			min.X			= boxSprite.Position.X;
			min.Y			= boxSprite.Position.Y;
			max.X			= boxSprite.Position.X + boxTextureInfo.TextureSizef.X;
			max.Y			= boxSprite.Position.Y + 200;
			box.Min 		= min;			
			box.Max 		= max;
		}
		
		public void blowUpRock()
		{
			tScene.RemoveChild(rockSprite, false);
			tScene.RemoveChild(boxSprite, false);
			tScene.RemoveChild(pluSprite, false);
			tScene.RemoveChild(dynoSprite,false);
			
			if(!blown)
			{
				exploSprite.Position = new Vector2(rockSprite.Position.X-200.0f,rockSprite.Position.Y-80.0f);
				tScene.AddChild(exploSprite);
				
				blown = true;
				counter = 20;
			}			
		}
		
		public void CheckCollision()
		{
			
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
		
		public void Reset(Scene scene)
		{
			tScene.RemoveChild(rockSprite, false);
			tScene.RemoveChild(boxSprite, false);
			tScene.RemoveChild(pluSprite, false);
			tScene.RemoveChild(dynoSprite,false);
			tScene.RemoveChild(exploSprite,false);
			scene.AddChild(rockSprite);
			scene.AddChild(pluSprite);
			scene.AddChild(boxSprite);
			scene.AddChild(dynoSprite);
			
			boxSprite.Position += new Vector2(2500, 0);
			pluSprite.Position = new Vector2(boxSprite.Position.X, boxSprite.Position.Y +44);
			counter = 20;
		}
		
		public void SetBlown() { blown = false; }
		public bool GetBlown() { return blown; }
		
		public Bounds2 GetBox() { return box; }
		
	}
}

