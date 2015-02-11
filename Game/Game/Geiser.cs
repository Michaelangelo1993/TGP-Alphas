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
		private bool geiserOnOff;
		private SpriteUV geiserSprite;
		private TextureInfo geiserTextureInfo;
		public float geiserPos;
		private static float sizeX, sizeY, defaultXPos;
		
		private static int 			noOnGeiserSheetWidth, noOnGeiserSheetHeight;

		
		//Spike
		private bool spikeBroken;
		private SpriteUV spikeSprite;
		private TextureInfo spikeTextureInfo;
		public float spikeCurrentHeight;
		
		
		
		public Geiser (Scene scene, Vector2 position)
		{
			spikeBroken = false;
			sizeX 		= 116.6f;
			sizeY		= 240.0f;

			//Geiser sprite initialise /width of each geiser is 116.6px
			geiserTextureInfo = new TextureInfo("/Application/textures/geiserSpriteSheet.png");
			noOnGeiserSheetWidth 	= 5;
			noOnGeiserSheetHeight 	= 1;
			//defaultXPos				= ((textureInfo.TextureSizef.X/noOnGeiserSheetWidth)*1.00f)*0.5f;
			geiserSprite = new SpriteUV(geiserTextureInfo);
			//geiser.UV.S 			= new Vector2(1.0f/noOnGeiserSheetWidth,1.0f/noOnGeiserSheetHeight);
			geiserSprite.Position = position;
			geiserSprite.Quad.S = new Vector2(sizeX, sizeY);
			Bounds2 geiserBounds = geiserSprite.Quad.Bounds2();
			
			//Spike sprite initialise
			spikeTextureInfo = new TextureInfo("/Application/textures/stalagmite.png");
			spikeSprite = new SpriteUV(spikeTextureInfo);
			spikeSprite.Position = position;
			spikeSprite.Position = new Vector2(position.X+70, position.Y+450);
			spikeSprite.Quad.S = spikeTextureInfo.TextureSizef;
			Bounds2 spikeBounds = spikeSprite.Quad.Bounds2();
								
			// Add sprites to scene
			scene.AddChild(geiserSprite);
			scene.AddChild(spikeSprite);
		}
		
		public void Dispose()
		{
			
		}
		
		public void BrokenSpike()
		{
			spikeBroken = false;
		}
		
		public void BreakSpike()
		{
			if(spikeBroken == false)
			{
				//if(spikeBounds tapped 3 times)
				{
					spikeSprite.Position = new Vector2(spikeSprite.Position.X, spikeSprite.Position.Y-1);
					spikeCurrentHeight--;
				}
			}
		}
		
		public void Update()
		{
			//AnimatePlayer();
			
			if(spikeBroken == true)
			{
				//Check to see whether spike has reached the ground
				if(spikeSprite.Position.Y < geiserSprite.Position.Y)
				{
					//Remove geiser from players path
					geiserSprite.Visible = false;
					spikeSprite.Position = new Vector2(400,0);
				}
				
			}
		}
		
		private void AnimateGeiser()
		{
			if(spikeBroken = false)
			{
				//don't know what i'm doing
			}
		}
		
		public void Reset()
		{
			spikeBroken = false;
			geiserSprite.Visible = true;
			spikeSprite.Position = new Vector2((spikeSprite.Position.X + 2500), (spikeSprite.Position.Y));
			geiserSprite.Position += new Vector2(2500, 0);
		}
	}
}
