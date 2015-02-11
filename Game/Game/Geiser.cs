using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace Game
{
	public class Geiser
	{
		//Geiser
		private bool geiserOn;
		private SpriteUV geiserSprite;
		private TextureInfo geiserTextureInfo;
		public float geiserPos, sizeX, sizeY;
		
		private static int 			frameTime, animationDelay,
									noOnSpritesheetWidth,
									widthCount;
		
		//Spike
		private bool spikeBroken;
		private SpriteUV spikeSprite;
		private TextureInfo spikeTextureInfo;
		public float spikeCurrentHeight;
		
		public Vector2 GetPosition { get { return spikeSprite.Position; }}
		
		
		public Geiser (Scene scene, Vector2 position)
		{
			spikeBroken 	= false;
			sizeX 			= 116.6f;
			sizeY			= 240.0f;
			frameTime 		= 0;
			animationDelay 	= 3;
			widthCount 		= 0;

			//Geiser sprite initialise /width of each geiser is 116.6px
			geiserTextureInfo = new TextureInfo("/Application/textures/geiserSpriteSheet.png");
			noOnSpritesheetWidth 	= 8;
			
			//defaultXPos				= ((textureInfo.TextureSizef.X/noOnGeiserSheetWidth)*1.00f)*0.5f;
			geiserSprite = new SpriteUV(geiserTextureInfo);
			geiserSprite.UV.S 			= new Vector2(1.0f/noOnSpritesheetWidth,1.0f);
			geiserSprite.Position = position;
			geiserSprite.Quad.S = new Vector2(116, 240);
			geiserSprite.Scale = new Vector2(1.0f,1.0f);
			Bounds2 geiserBounds = geiserSprite.Quad.Bounds2();
			
			//Spike sprite initialise
			spikeTextureInfo = new TextureInfo("/Application/textures/stalagmite.png");
			spikeSprite = new SpriteUV(spikeTextureInfo);
			spikeSprite.Position = position;
			spikeSprite.Position = new Vector2(position.X+70, position.Y+475);
			spikeSprite.Quad.S = spikeTextureInfo.TextureSizef;
			Bounds2 spikeBounds = spikeSprite.Quad.Bounds2();
								
			// Add sprites to scene
			scene.AddChild(geiserSprite);
			scene.AddChild(spikeSprite);
		}
		
		public void Dispose()
		{
			
		}
		
		public void BreakSpike()
		{
			spikeBroken = true;
		}
				
		public void Update(float deltaTime, float speed)
		{
			AnimateGeiser();
			
			
			if(spikeBroken == true)
			{
				geiserSprite.Position = new Vector2(geiserSprite.Position.X-speed, geiserSprite.Position.Y);
				spikeSprite.Position = new Vector2((geiserSprite.Position.X + (geiserTextureInfo.TextureSizef.X/2) -spikeTextureInfo.TextureSizef.X/2)-speed, spikeSprite.Position.Y);
				
				//Check to see whether spike has reached the ground
				if(spikeSprite.Position.Y > geiserSprite.Position.Y)
				{
					spikeSprite.Position = new Vector2(geiserSprite.Position.X, spikeSprite.Position.Y-10);
					//Remove geiser from players path
					//spikeSprite.Position = new Vector2(400,0);
					//spikeBroken = false;
				}
				if(spikeSprite.Position.Y < geiserSprite.Position.Y+100)
				{
					//Remove geiser from players path
					geiserSprite.Visible = false;
					//spikeSprite.Position = new Vector2(400,0);
					//spikeBroken = false;
				}
			}
			else
				geiserSprite.Position = new Vector2(geiserSprite.Position.X-speed, geiserSprite.Position.Y);
				spikeSprite.Position = new Vector2((geiserSprite.Position.X + (sizeX/2) -spikeTextureInfo.TextureSizef.X/2)-speed, spikeSprite.Position.Y);
		}
		
		private void AnimateGeiser()
		{
			if(frameTime == animationDelay)
			{
				if (widthCount == noOnSpritesheetWidth)
					widthCount = 0;
				
				geiserSprite.UV.T = new Vector2((1.0f/noOnSpritesheetWidth)*widthCount, 0.0f);
				widthCount++;
				frameTime = 0;
			}
			
			frameTime++;
		}
		
		public void Reset(float x)
		{
			spikeBroken = false;
			geiserSprite.Visible = true;
			geiserSprite.Position += new Vector2(x, 0);
			spikeSprite.Position = new Vector2(geiserSprite.Position.X, geiserSprite.Position.Y+475);
			geiserSprite.Visible = true;
		}
	}
}
