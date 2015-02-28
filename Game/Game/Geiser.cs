using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace Game
{
	public class Geiser : Obstacle
	{
		//Geiser
		private bool geiserOn;
		private SpriteUV geiserSpriteSheet, geiserSprite;
		private TextureInfo geiserSheetTextureInfo, geiserTextureInfo;
		public float geiserPos, sizeX, sizeY;
		
		private static int 			frameTime, animationDelay,
									noOnSpritesheetWidth,
									widthCount;
		
		//Spike
		private bool spikeBroken;
		private SpriteUV spikeSprite;
		private TextureInfo spikeTextureInfo;
		public float spikeCurrentHeight;
		Bounds2 spikeBounds;
		
		public Vector2 GetPosition { get { return spikeSprite.Position; }}
		override public float GetEndPosition() { return (spikeSprite.Position.X + spikeBounds.Point10.X*2); }
		
		
		public Geiser (Scene scene, Vector2 position)
		{
			spikeBroken 	= false;
			sizeX 			= 116.6f;
			sizeY			= 240.0f;
			frameTime 		= 0;
			animationDelay 	= 3;
			widthCount 		= 0;
			geiserOn		= true;

			//Geiser sprite initialise /width of each geiser is 116.6px
			geiserSheetTextureInfo  = new TextureInfo("/Application/textures/geiserSpriteSheet.png");
			noOnSpritesheetWidth 	= 8;
			geiserTextureInfo		= new TextureInfo("/Application/textures/geiser.png");
			
			//defaultXPos				= ((textureInfo.TextureSizef.X/noOnGeiserSheetWidth)*1.00f)*0.5f;
			geiserSpriteSheet = new SpriteUV(geiserSheetTextureInfo);
			geiserSpriteSheet.UV.S 			= new Vector2(1.0f/noOnSpritesheetWidth,1.0f);
			geiserSprite	  = new SpriteUV(geiserTextureInfo);
			geiserSpriteSheet.Position = position;
			geiserSpriteSheet.Quad.S = new Vector2(116, 240);
			geiserSpriteSheet.Scale = new Vector2(1.0f,1.0f);
			Bounds2 geiserBounds = geiserSpriteSheet.Quad.Bounds2();
			geiserSprite.Position = new Vector2(geiserSpriteSheet.Position.X,geiserSpriteSheet.Position.Y) ;
			geiserSprite.Scale = new Vector2(117.0f,240.0f);
			
			//Spike sprite initialise
			spikeTextureInfo = new TextureInfo("/Application/textures/stalagmite.png");
			spikeSprite = new SpriteUV(spikeTextureInfo);
			spikeSprite.Quad.S = spikeTextureInfo.TextureSizef;
			spikeBounds = spikeSprite.Quad.Bounds2();
			spikeSprite.Position = new Vector2(position.X + 6 + spikeBounds.Point10.X/2, geiserSpriteSheet.Position.Y+475);
			
			// Add sprites to scene
			scene.AddChild(geiserSpriteSheet);
			scene.AddChild(spikeSprite);
			scene.AddChild(geiserSprite);
		}
		
		override public void Dispose()
		{
			
		}
		
		public void BreakSpike()
		{
			spikeBroken = true;
		}
				
		override public void Update(float deltaTime, float speed)
		{
			AnimateGeiser();
			
			if(spikeBroken == true)
			{
				geiserSpriteSheet.Position = new Vector2(geiserSpriteSheet.Position.X-speed, geiserSpriteSheet.Position.Y);
				spikeSprite.Position = new Vector2(spikeSprite.Position.X-speed, spikeSprite.Position.Y);
				geiserSprite.Position = geiserSpriteSheet.Position;
			
				//Check to see whether spike has reached the ground
				if(spikeSprite.Position.Y > geiserSpriteSheet.Position.Y)
				{
					spikeSprite.Position = new Vector2(spikeSprite.Position.X, spikeSprite.Position.Y-10);
				}
				if(spikeSprite.Position.Y < geiserSpriteSheet.Position.Y+100)
				{
					//Remove geiser from players path
					geiserSpriteSheet.Visible = false;
					geiserOn = false;
				}
			}
			else
			{
				geiserSpriteSheet.Position = new Vector2(geiserSpriteSheet.Position.X-speed, geiserSpriteSheet.Position.Y);
				spikeSprite.Position = new Vector2(spikeSprite.Position.X-speed, spikeSprite.Position.Y);
				geiserSprite.Position = geiserSpriteSheet.Position;
			}
			
			Vector2 touchPos = AppMain.GetTouchPosition();
			
			if(touchPos.Y <= spikeSprite.Position.Y + 114.0f && touchPos.Y >= spikeSprite.Position.Y - 50.0f
			   && touchPos.X <= spikeSprite.Position.X + 114.0f && touchPos.X >= spikeSprite.Position.X - 50.0f)				
			{
				BreakSpike();
			}
			
			//if(spikeSprite.Position.X+700 < player.GetPos().X)
				// Reset geiser.Reset(spinObstacle.GetPosition1.X + 200);
		}
		
		private void AnimateGeiser()
		{
			if(geiserOn)
			{
				if(frameTime == animationDelay)
				{
					if (widthCount == noOnSpritesheetWidth)
						widthCount = 0;
					
					geiserSpriteSheet.UV.T = new Vector2((1.0f/noOnSpritesheetWidth)*widthCount, 0.0f);
					widthCount++;
					frameTime = 0;
				}
			
				frameTime++;
			}
			else if(widthCount > 0)
			{
				if(frameTime == animationDelay)
				{
					if (widthCount == noOnSpritesheetWidth)
						widthCount = 0;
					
					geiserSpriteSheet.UV.T = new Vector2((1.0f/noOnSpritesheetWidth)*widthCount, 0.0f);
					widthCount++;
					frameTime = 0;
				}
			
				frameTime++;
			}
		}
		
		override public void Reset(float x)
		{
			spikeBroken = false;
			geiserSpriteSheet.Position = new Vector2(x, geiserSpriteSheet.Position.Y);
			spikeSprite.Position = new Vector2(geiserSpriteSheet.Position.X + 6 + spikeBounds.Point10.X/2, 475);
			geiserSprite.Position = geiserSpriteSheet.Position;
			geiserSpriteSheet.Visible = true;
			geiserOn = true;
		}
	}
}
