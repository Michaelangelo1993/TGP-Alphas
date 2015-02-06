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
		
		//Spike
		private bool spikeBroken;
		private SpriteUV spikeSprite;
		private TextureInfo spikeTextureInfo;
		public float spikeCurrentHeight;
				
		public Geiser (Scene scene, Vector2 position)
		{
			spikeBroken = false;
			
			//Geiser sprite initialise
			geiserTextureInfo = new TextureInfo("/Application/textures/Geiser.png");
			geiserSprite = new SpriteUV(geiserTextureInfo);
			geiserSprite.Position = position;
			geiserSprite.Quad.S = geiserTextureInfo.TextureSizef;
			Bounds2 geiserBounds = geiserSprite.Quad.Bounds2();
			
			//Spike sprite initialise
			spikeTextureInfo = new TextureInfo("/Application/textures/Spike.png");
			spikeSprite = new SpriteUV(spikeTextureInfo);
			spikeSprite.Position = position;
			spikeSprite.Position = new Vector2(position.X+70, position.Y+450);
			spikeSprite.Quad.S = spikeTextureInfo.TextureSizef;
			Bounds2 spikeBounds = spikeSprite.Quad.Bounds2();
								
			// Add sprites to scene
			scene.AddChild(geiserSprite);
			scene.AddChild(spikeSprite);
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
	}
}
