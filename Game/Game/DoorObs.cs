using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace Game
{
	public class DoorObs
	{
		private SpriteUV 	doorSprite;
		private SpriteUV 	doorSprite2;
		private TextureInfo	doorTextureInfo;
		
		private float gap = 300.0f;
		
		private Boolean beingPushed = false;
		
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
				
		public void Dispose()
		{
			doorTextureInfo.Dispose();
		}
		
		public void Update(float deltaTime, float t)
		{	
			doorSprite.Position 	 += new Vector2(-t, 0);
			doorSprite2.Position 	 += new Vector2(-t, 0);
			
			if(doorSprite.Position.Y > 100 && !beingPushed)
			{
				doorSprite.Position = new Vector2(doorSprite.Position.X, doorSprite.Position.Y-8.0f);
			}
			
			if(doorSprite2.Position.Y > 100 && !beingPushed)
			{
				doorSprite2.Position = new Vector2(doorSprite2.Position.X, doorSprite2.Position.Y-8.0f);
			}
		}
		
		
		public void Tapped(float y, int door)
		{
			beingPushed = true;
			if(y >= 150)
			{
				if(door == 1)
					doorSprite.Position = new Vector2(doorSprite.Position.X, y - 50.0f);
				if(door == 2)
					doorSprite2.Position = new Vector2(doorSprite2.Position.X, y - 50.0f);
			}
				
			else
			{
				doorSprite.Position = new Vector2(doorSprite.Position.X, 100.0f);
				doorSprite2.Position = new Vector2(doorSprite2.Position.X, 100.0f);
			}
			//doorSprite.Position = new Vector2(doorSprite.Position.X, doorSprite.Position.Y+8.0f);
		}
		
		public void ReleaseDoor()
		{
			beingPushed = false;
		}
		
		public Vector2 GetPosition()
		{
			return doorSprite.Position;
		}
		
		public Vector2 GetPosition2()
		{
			return doorSprite2.Position;
		}
		
		public void Reset(Scene scene, float x)
		{			
			doorSprite.Position  += new Vector2(x, 0);
			doorSprite.Position  += new Vector2(x, 0);
		}
	}
}

