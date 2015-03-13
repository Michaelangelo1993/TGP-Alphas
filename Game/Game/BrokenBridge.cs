using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace Game
{
	public class BrokenBridge : Obstacle
	{
		
		//Private variables.
		private 			Trap 		trap;
		private 			SpriteUV 	ropeSprite1, ropeSprite2, ropeSprite3, ropeSprite4, plankSprite;
		private 			TextureInfo	textureRope1, textureRope2, textureRope3, textureRope4, texturePlank;
		private 			Bounds2		plankBounds;
		private				bool		touching, missedBridge;
		
		private 			Vector2		ropeBLOffset, ropeBROffset, ropeTLOffset, ropeTROffset;
		// Bottom Left = Rope 1, Top Left = Rope 2, Top Right = Rope 3, Bottom Right = Rope 4
						
		override public float GetEndPosition() { return (plankSprite.Position.X + ropeBROffset.X); }
		
		//Public functions.
		public BrokenBridge (Scene scene, Vector2 position)
		{	
			touching						=	false;
			missedBridge					=	true;
			
			trap = new Trap(scene, position);
			position = new Vector2(position.X, 470);
			
			texturePlank     				=	new TextureInfo("/Application/textures/plank5.png");
			plankSprite						= 	new SpriteUV(texturePlank);
			plankSprite.Quad.S 				= 	texturePlank.TextureSizef;
			plankBounds 					= 	plankSprite.Quad.Bounds2();
			plankSprite.Position 			=   position;
			
			// Offset rope positions for each corner by moving them in a little
			// Update Bounds 5=paddding, 50 = 3D offset. Right side moved in extra
			ropeBLOffset					= 	plankBounds.Point00 + new Vector2(5,  5);
			ropeBROffset					= 	plankBounds.Point10 + new Vector2(-65,  5);
			ropeTLOffset					= 	plankBounds.Point01 + new Vector2(55, -5);
			ropeTROffset					= 	plankBounds.Point11 + new Vector2(-25, -5);			
			
			textureRope1     				=	new TextureInfo("/Application/textures/ropedone.png");
			ropeSprite1						= 	new SpriteUV(textureRope1);
			ropeSprite1.Quad.S 				= 	textureRope1.TextureSizef;
			ropeSprite1.Position 			=   position + ropeBLOffset;
			
			textureRope2     				=	new TextureInfo("/Application/textures/ropedone.png");
			ropeSprite2						= 	new SpriteUV(textureRope2);
			ropeSprite2.Quad.S 				= 	textureRope2.TextureSizef;
			ropeSprite2.Position 			=   position + ropeTLOffset;
			
			textureRope3     				=	new TextureInfo("/Application/textures/ropedone.png");
			ropeSprite3						= 	new SpriteUV(textureRope3);
			ropeSprite3.Quad.S 				= 	textureRope3.TextureSizef;
			ropeSprite3.Position 			=   position + ropeTROffset;
			
			textureRope4     				=	new TextureInfo("/Application/textures/ropedone.png");
			ropeSprite4						= 	new SpriteUV(textureRope4);
			ropeSprite4.Quad.S 				= 	textureRope4.TextureSizef;
			ropeSprite4.Position 			=   position + ropeBROffset;
			
			scene.AddChild(plankSprite);
			scene.AddChild(ropeSprite1);
			scene.AddChild(ropeSprite2);
			scene.AddChild(ropeSprite3);
			scene.AddChild(ropeSprite4);
		}
		
		override public void Dispose()
		{
			textureRope1.Dispose();
			textureRope2.Dispose();
			textureRope3.Dispose();
			textureRope4.Dispose();
			texturePlank.Dispose();
		}
		
		override public void Update(float gameSpeed)
		{				
			Vector2 touch = AppMain.GetTouchPosition();
			
			trap.Update(gameSpeed);	
			CheckTouchData();
			CollisionCheck();
			MoveObjectsLeft(gameSpeed);

			// Not on bridge
			if(missedBridge)
				if(touching)
					MoveObjectsDown(touch.Y);
				// Uncomment the else to auto retract
				/*else 
					MoveObjectsUp(gameSpeed);*/			
		}
		
		private void CheckTouchData()
		{
			Vector2 touch = AppMain.GetTouchPosition();
						
			// Check if Touching Screen			
			if(Touch.GetData(0).ToArray().Length <= 0)
				touching = false;
			// Check if touching bridge/plank
			else if((touch.X > plankSprite.Position.X-50) &&
				    (touch.X < plankSprite.Position.X + 50 + plankBounds.Point11.X) &&
				    (touch.Y > plankSprite.Position.Y))
			{
				touching  = true;
			}
		}
		
		private void CollisionCheck()
		{
			// Check for collision with player
			if(AppMain.GetPlayer().GetPos().Y-115/2 > plankSprite.Position.Y &&
		   	   AppMain.GetPlayer().GetPos().X > plankSprite.Position.X + ropeBLOffset.X - 50 &&
		   	   AppMain.GetPlayer().GetPos().X < plankSprite.Position.X + ropeBROffset.X + 50)
			{
				missedBridge = false;
			}
		}
		
		private void MoveObjectsLeft(float gameSpeed)
		{
			ropeSprite1.Position -= new Vector2(gameSpeed, 0);
			ropeSprite2.Position -= new Vector2(gameSpeed, 0);
			ropeSprite3.Position -= new Vector2(gameSpeed, 0);
			ropeSprite4.Position -= new Vector2(gameSpeed, 0);
			plankSprite.Position -= new Vector2(gameSpeed, 0);
		}
		
		private void MoveObjectsDown(float yPos)
		{
			if(plankSprite.Position.Y > 70)
			{
				ropeSprite1.Position = new Vector2(ropeSprite1.Position.X, yPos + ropeBLOffset.Y);
				ropeSprite2.Position = new Vector2(ropeSprite2.Position.X, yPos + ropeTLOffset.Y);
				ropeSprite3.Position = new Vector2(ropeSprite3.Position.X, yPos + ropeTROffset.Y);
				ropeSprite4.Position = new Vector2(ropeSprite4.Position.X, yPos + ropeBROffset.Y);
				plankSprite.Position = new Vector2(plankSprite.Position.X, yPos);	
			}
			
			// Sometimes position can bug too low
			if(plankSprite.Position.Y < 70)
			{
				ropeSprite1.Position = new Vector2(ropeSprite1.Position.X, 70 + ropeBLOffset.Y);
				ropeSprite2.Position = new Vector2(ropeSprite2.Position.X, 70 + ropeTLOffset.Y);
				ropeSprite3.Position = new Vector2(ropeSprite3.Position.X, 70 + ropeTROffset.Y);
				ropeSprite4.Position = new Vector2(ropeSprite4.Position.X, 70 + ropeBROffset.Y);
				plankSprite.Position = new Vector2(plankSprite.Position.X, 70);	
			}
		}
					
		private void MoveObjectsUp(float gameSpeed)
		{
			if(plankSprite.Position.Y < 470)
			{
				ropeSprite1.Position += new Vector2(0, gameSpeed*2);
				ropeSprite2.Position += new Vector2(0, gameSpeed*2);
				ropeSprite3.Position += new Vector2(0, gameSpeed*2);
				ropeSprite4.Position += new Vector2(0, gameSpeed*2);	
				plankSprite.Position += new Vector2(0, gameSpeed*2);
			}
		}
	
		override public void Reset(float x)
		{			
			missedBridge = true;
			trap.SetXPos(x);
			
			// Update Bounds 5 = paddding, 55 = Left 3D offset
			ropeBLOffset = 	plankBounds.Point00 + new Vector2(  5,  5);
			ropeBROffset = 	plankBounds.Point10 + new Vector2(-65,  5);
			ropeTLOffset = 	plankBounds.Point01 + new Vector2( 55, -5);
			ropeTROffset = 	plankBounds.Point11 + new Vector2(-25, -5);	
			
			plankSprite.Position = new Vector2(x, 470);
			ropeSprite1.Position = new Vector2(x, 470) + ropeBLOffset;
			ropeSprite2.Position = new Vector2(x, 470) + ropeTLOffset;
			ropeSprite3.Position = new Vector2(x, 470) + ropeTROffset;
			ropeSprite4.Position = new Vector2(x, 470) + ropeBROffset;
			
		}
	}
}

