using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace Game
{
	public class Meteor : Obstacle
	{
		//Marker
		private SpriteUV lineSprite;
		private TextureInfo lineTextureInfo;
		public float linePos;
		
		//Meteor
		private bool meteorBroken;
		private SpriteUV meteorSprite;
		private TextureInfo meteorTextureInfo;
		public float meteorPos;
		private static float sizeX, sizeY, defaultXPos;
		private static int 	noOnMeteorSheetWidth, noOnMeteorSheetHeight;
		
		private static int 			frameTime, animationDelay,
									noOnSpritesheetWidth,
									widthCount;
		
		
		private bool active = true;
				
		override public float GetEndPosition() { return (meteorSprite.Position.X + 160); }
		
		public Meteor (Scene scene, Vector2 position)
		{
			meteorBroken 	= false;
			sizeX 			= 160.0f;
			sizeY			= 333.0f;
			frameTime 		= 0;
			animationDelay 	= 3;
			widthCount 		= 0;
			
			//Line sprite initialise
			lineTextureInfo			= new TextureInfo("/Application/textures/meteorLine.png");
			lineSprite 				= new SpriteUV(lineTextureInfo);
			lineSprite.Position		= new Vector2(position.X, position.Y - 544.0f);
			
			//Meteor sprite initialise 
			meteorTextureInfo  		= new TextureInfo("/Application/textures/meteorSprite.png");
			noOnSpritesheetWidth 	= 4;
			meteorSprite	 		= new SpriteUV(meteorTextureInfo);
			meteorSprite.UV.S 		= new Vector2(1.0f/noOnSpritesheetWidth,1.0f);
			meteorSprite.Position 	= position;
			meteorSprite.Quad.S 	= new Vector2(160, 333);
			Bounds2 meteorBounds 	= meteorSprite.Quad.Bounds2();
			
			
			// Add sprites to scene
			scene.AddChild(meteorSprite);
			scene.AddChild(lineSprite);
		}
		
		override public void Dispose(Scene scene)
		{
			scene.RemoveChild(meteorSprite, true);
			scene.RemoveChild(lineSprite, true);
			lineTextureInfo.Dispose();
			meteorTextureInfo.Dispose();			
		}
		
		public void BreakMeteor()
		{
			meteorBroken = true;
		}
				
		override public void Update(float speed)
		{
			float xPos = AppMain.GetPlayer().GetPos().X;
			
			Vector2 touchPos = AppMain.GetTouchPosition();
			
			lineSprite.Position = new Vector2(lineSprite.Position.X - speed, lineSprite.Position.Y);
			meteorSprite.Position = new Vector2(meteorSprite.Position.X - speed, meteorSprite.Position.Y);
			
			if (active == true)
			{
				AnimateMeteor();
								
				if(Touch.GetData(0).ToArray().Length> 0 && 
				touchPos.Y <= meteorSprite.Position.Y + 333.0f && 
				touchPos.Y >= meteorSprite.Position.Y - 47.0f && 
				touchPos.X <= meteorSprite.Position.X + 114.0f 
				&& touchPos.X >= meteorSprite.Position.X - 47.0f)
				{
					BreakMeteor();
				}
				
				if(meteorSprite.Position.X < xPos + 700)
				{
					meteorSprite.Position  		= new Vector2(meteorSprite.Position.X, meteorSprite.Position.Y-speed *1.667f);
					//meteorSprite.Position.X -= 1;
					
					
					//Check to see whether meteor has reached the ground
					if(meteorSprite.Position.Y <= 100)
					{
						meteorBroken= true;
						if(widthCount == noOnSpritesheetWidth)
						{
						AppMain.GetPlayer().KillByFire();
						
						}
					}
				}
			}
				
		}
		
		private void AnimateMeteor()
		{
			if(meteorBroken == true)
			{
				if(frameTime == animationDelay)
				{
					if (widthCount == noOnSpritesheetWidth)
						widthCount = 0;
					meteorSprite.UV.T = new Vector2((1.0f/noOnSpritesheetWidth)*widthCount, 0.0f);
					widthCount++;
					if (widthCount == 4)
					{
						meteorSprite.Visible = false;
						lineSprite.Visible = false;
						AppMain.SetShake(true);
						active = false;
					}
					frameTime = 0;
				}
			
				frameTime++;
			}
		}
		
		override public void Reset(float x)
		{
			meteorSprite.Position = new Vector2(x, 544);
			//lineSprite.Position = new Vector2(meteorSprite.Position.X, );
			meteorBroken 	= false;
			frameTime 		= 0;
			animationDelay 	= 3;
			widthCount 		= 0;
		}
	}
}

