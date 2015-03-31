using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace Game
{
	public class DoorObs : Obstacle
	{
		private SpriteUV 	doorSprite;
		private SpriteUV 	doorSprite2;
		private TextureInfo	doorTextureInfo;
		
		//Gap between doors
		private float gap = 300.0f;
		float door1Count = 60.0f;
		float door2Count = 60.0f;
		
		private Boolean beingPushed1 = false;
		private Boolean beingPushed2 = false;
		
		override public float GetEndPosition() { return (doorSprite2.Position.X + 64); }
		
		public DoorObs (Scene scene, float x, float y)
		{
			doorTextureInfo 		= new TextureInfo("/Application/textures/door.png");
						
			doorSprite	 			= new SpriteUV(doorTextureInfo);
			doorSprite2				= new SpriteUV(doorTextureInfo);
						
			doorSprite.Quad.S 		= doorTextureInfo.TextureSizef;
			doorSprite.Position		= new Vector2(x, y);
			
			doorSprite2.Quad.S 		= doorTextureInfo.TextureSizef;
			doorSprite2.Position	= new Vector2(x + gap, y);
			
			scene.AddChild(doorSprite);
			scene.AddChild(doorSprite2);
		}
				
		override public void Dispose(Scene scene)
		{
			scene.RemoveChild(doorSprite, true);
			scene.RemoveChild(doorSprite2, true);
			doorTextureInfo.Dispose();
		}
		
		override public void Update(float gameSpeed)
		{	
			Vector2 touchPos = AppMain.GetTouchPosition();
						
			if(Touch.GetData(0).ToArray().Length > 0 &&
				touchPos.Y <= doorSprite.Position.Y + 306.0f && touchPos.Y >= doorSprite.Position.Y - 50.0f
			   && touchPos.X <= doorSprite.Position.X + 114.0f && touchPos.X >= doorSprite.Position.X - 50.0f)				
			{
				beingPushed1 = true;
			}
			else if(Touch.GetData(0).ToArray().Length > 0 &&
				touchPos.Y <= doorSprite2.Position.Y + 306.0f &&
			    touchPos.Y >= doorSprite2.Position.Y - 50.0f &&
			    touchPos.X <= doorSprite2.Position.X + 114.0f &&
			    touchPos.X >= doorSprite2.Position.X - 50.0f)				
			{
				beingPushed2 = true;
			}
			else if (Touch.GetData(0).ToArray().Length <= 0)
				ReleaseDoor();
			
			if(beingPushed1 || beingPushed2)
				Tapped(touchPos.Y);
			
			//Move the doors
			doorSprite.Position 	 += new Vector2(-gameSpeed, 0);
			doorSprite2.Position 	 += new Vector2(-gameSpeed, 0);
			
			//Lower the doors if not being touched
			if(doorSprite.Position.Y > 60 && !beingPushed1)
				if(door1Count <= 0)
					doorSprite.Position = new Vector2(doorSprite.Position.X, doorSprite.Position.Y-gameSpeed*3);
				else
					door1Count-=gameSpeed/3;
			
			if(doorSprite2.Position.Y > 60 && !beingPushed2)
				if(door2Count <= 0)
					doorSprite2.Position = new Vector2(doorSprite2.Position.X, doorSprite2.Position.Y-gameSpeed*3);
				else
					door2Count-=gameSpeed/3;
			
			// Deaths
			if(doorSprite.Position.Y < AppMain.GetPlayer().GetPos().Y+50 &&
			   doorSprite.Position.X < AppMain.GetPlayer().GetPos().X+50 &&
			   doorSprite.Position.X > AppMain.GetPlayer().GetPos().X-50)
			{
				AppMain.GetPlayer().KillByFire();
			}
			else if(doorSprite2.Position.Y < AppMain.GetPlayer().GetPos().Y+50 &&
			   doorSprite2.Position.X < AppMain.GetPlayer().GetPos().X+50 &&
			   doorSprite2.Position.X > AppMain.GetPlayer().GetPos().X-50)
			{
				AppMain.GetPlayer().KillByFire();
			}
		}
		
		
		public void Tapped(float y)
		{
			//Ensure the door doesn't go below the floor
			if(y >= 150)
			{
				if(beingPushed1)
				{
					door1Count = 60;
					doorSprite.Position = new Vector2(doorSprite.Position.X, y - 50.0f);
				}
				else if(beingPushed2)
				{
					door2Count = 60;
					doorSprite2.Position = new Vector2(doorSprite2.Position.X, y - 50.0f);
				}
			}
				
			else
			{
				doorSprite.Position = new Vector2(doorSprite.Position.X, 60.0f);
				doorSprite2.Position = new Vector2(doorSprite2.Position.X, 60.0f);
			}
		}
		
		public void ReleaseDoor()
		{
			beingPushed1 = false;
			beingPushed2 = false;
		}
		
		override public void Reset(float x)
		{	
			//Reset position
			doorSprite.Position  = new Vector2(x, doorSprite.Position.Y);
			doorSprite2.Position  = new Vector2(x + gap, doorSprite.Position.Y);
		}
	}
}

