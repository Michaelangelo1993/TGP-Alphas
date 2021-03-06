using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace Game
{
	public class Spring : Obstacle
	{
		private Trap trap;
		private Pit pit;
		private bool missedSpring;
		private bool magmaTrap;
		private Random rand;
		
		private bool ready;
		private bool beingPushed;
		private bool springReleased;
		private SpriteUV springTopSprite;
		private TextureInfo springTopTextureInfo;
		public float springTopHeight;
		public float springTopWidth;
		
		private SpriteUV springSprite;
		private SpriteUV springSprite2;
		private TextureInfo springTextureInfo;
		private float springOriginalHeight;
		private float springCurrentHeight;
		public float springWidth;
		
		public bool IsReady { get { return ready; }}
		public bool SetReady { set { ready = value; }}
		
		private Vector2		_min, _max;
		private Bounds2		_box;
				
		public float GetOriginalHeight { get { return springOriginalHeight; }}
		public bool BeingPushed { get { return beingPushed; }}
		public bool MissedSpring { get { return missedSpring; }}
		public bool IsReleased { get { return springReleased; }}
		public Vector2 GetPosition { get { return springSprite.Position; }}
		public float GetSpringWidth { get { return springWidth; }}
		public float GetTop { get { return (springTopSprite.Position.Y + springTopHeight); }}
		public Bounds2 GetBox() { return _box; }
		
		
		override public void ReleaseSpring(bool b) { springReleased = b; }
		public void MissSpring() { missedSpring = true; }
		public void PushSpring() { beingPushed = true; }
		override public float GetEndPosition() { return (trap.GetEndPosition()); }
		
		public Spring (Scene scene, Vector2 position)
		{
			rand = new Random();
			springReleased = false;
			missedSpring = false;
			beingPushed = false;
			magmaTrap = true;
			
			// Initialise spring texture and sprite, get bounds and set position minus height offset
			springTextureInfo 		= new TextureInfo("/Application/textures/Spring.png");
			
			springSprite 			= new SpriteUV(springTextureInfo);
			springSprite2			= new SpriteUV(springTextureInfo);
			
			springSprite.Quad.S 	= springTextureInfo.TextureSizef;
			springSprite2.Quad.S 	= springTextureInfo.TextureSizef;
			
			Bounds2 springBounds 	= springSprite.Quad.Bounds2 ();
			springWidth 			= springBounds.Point10.X;
			springOriginalHeight 	= springBounds.Point01.Y;
			springCurrentHeight 	= springBounds.Point01.Y;	
			
			
			// Initialise spring texture and sprite, get bounds and set position minus height offset
			springTopTextureInfo = new TextureInfo("/Application/textures/SpringTop.png");
			springTopSprite = new SpriteUV(springTopTextureInfo);
			springTopSprite.Quad.S = springTopTextureInfo.TextureSizef;
			Bounds2 springTopBounds = springTopSprite.Quad.Bounds2 ();
			springTopHeight = springTopBounds.Point01.Y;
			springTopWidth = springTopBounds.Point10.X;
			float sizeDifference = 12; // Spring width is 100, spring top is 76, half it for x offset
			
			springSprite.Position 	= new Vector2(position.X, position.Y);
			springSprite2.Position  = springSprite.Position + new Vector2(58.0f, 48.0f); // Offset second spring for 3d effect
			
			trap = new Trap(scene, new Vector2((position.X + 125 + sizeDifference), 60));	
			pit = new Pit(scene, new Vector2((position.X + 125 + sizeDifference), 60));	
			
			springTopSprite.Position = new Vector2(position.X + sizeDifference, springSprite.Position.Y + springBounds.Point01.Y - 20);
			
			// Add sprites to scene
			scene.AddChild(springSprite);
			scene.AddChild(springSprite2);
			scene.AddChild(springTopSprite);
		}
		
		override public void Dispose(Scene scene)
		{	
			trap.Dispose(scene);
			pit.Dispose(scene);
			scene.RemoveChild(springSprite, true);
			scene.RemoveChild(springSprite2, true);
			scene.RemoveChild(springTopSprite, true);
			springTextureInfo.Dispose();
			springTopTextureInfo.Dispose();
		}
		
		public void WindSpring(float gameSpeed)
		{
			if(!springReleased && springCurrentHeight > 10)
			{
				float yPos = AppMain.GetTouchPosition().Y;
				beingPushed = true;
				springCurrentHeight = yPos - springSprite.Position.Y;
				springSprite.Scale = new Vector2(springSprite.Scale.X, springCurrentHeight/springOriginalHeight);
				springSprite2.Scale = new Vector2(springSprite2.Scale.X, springCurrentHeight/springOriginalHeight);
				springTopSprite.Position = new Vector2(springTopSprite.Position.X, springSprite.Position.Y + springCurrentHeight - 20);
			}
		}
		
		override public void Update(float speed)
		{			
			springSprite.Position = new Vector2(springSprite.Position.X - speed, springSprite.Position.Y);
			springSprite2.Position = new Vector2(springSprite2.Position.X - speed, springSprite2.Position.Y);
			springTopSprite.Position = new Vector2(springTopSprite.Position.X - speed, springTopSprite.Position.Y);
			
			trap.Update(speed);
			pit.Update(speed);
			
			if(springReleased)
			{		
				// Spring can move too fast for collisions, split it up
				int iterations = (int)FMath.Ceiling(speed/3.0f);
				float speedPerCycle = speed/iterations;
				
				for(int i=0;i<iterations;i++)
				{
					// Update collision box
					_min.X			= springTopSprite.Position.X ;
					_min.Y			= springTopSprite.Position.Y ;
					_max.X			= springTopSprite.Position.X + springTopTextureInfo.TextureSizef.X;
					_max.Y			= springTopSprite.Position.Y + springTopTextureInfo.TextureSizef.Y;
					_box.Min 		= _min;			
					_box.Max 		= _max;
				
					// Check for collision with player
					if(AppMain.GetPlayer().GetBottomBox().Overlaps(_box))
					{
						missedSpring = false;						
						AppMain.GetPlayer().DoJump();
					}
					
					// Update spring height
					if(springCurrentHeight < springOriginalHeight)
					{
						springTopSprite.Position = new Vector2(springTopSprite.Position.X, springTopSprite.Position.Y+(speedPerCycle*5));
						springCurrentHeight+=(speedPerCycle*5);
						springSprite.Scale = new Vector2(springSprite.Scale.X, springCurrentHeight/springOriginalHeight);
						springSprite2.Scale = new Vector2(springSprite2.Scale.X, springCurrentHeight/springOriginalHeight);
					}
					
					if(springCurrentHeight <=55)
			  			AppMain.GetSoundManager().PlayJump();
					
					else
						springReleased = false;	
				}
			}
			else if(beingPushed)
				WindSpring(speed);
	
			Vector2 touchPos = AppMain.GetTouchPosition();
				
			if((touchPos.X-100 < springSprite.Position.X) &&
			   (touchPos.X+125 > springSprite2.Position.X + springWidth) &&
			   (touchPos.Y-(springTopHeight/5) > springTopSprite.Position.Y)) // Touching spring
			{
				PushSpring();
			}
			
			if(Touch.GetData(0).ToArray().Length <= 0)
				ReleaseSpring(true);
			
			if(magmaTrap && missedSpring && AppMain.GetPlayer().GetPos().X > springSprite2.Position.X + springTopWidth*1.5)
					AppMain.GetPlayer().KillByFire();
			
			
			if(!magmaTrap && missedSpring && AppMain.GetPlayer().GetPos().X > springSprite2.Position.X + springTopWidth*1.5)
					AppMain.GetPlayer().KillByFire();
		}
		
		override public void Reset(float x)
		{
			int randomNum = (rand.Next(0, 2));
			
			if(randomNum == 0)
			{
				// Magma
				trap.Visible(true);
				pit.Visible(false);
				magmaTrap = true;
			}
			else
			{
				// Magma
				trap.Visible(false);
				pit.Visible(true);
				magmaTrap = false;
			}
			
			springReleased = true;
			missedSpring = true;
			beingPushed = false;
			
			float sizeDifference = (springTopWidth - springWidth)/2;
			springSprite.Position = new Vector2(x, springSprite.Position.Y);
			springSprite2.Position = springSprite.Position + new Vector2(58.0f, 48.0f);
			trap.SetXPos(x + 125 + sizeDifference);
			pit.SetXPos(x + 125 + sizeDifference);
			springTopSprite.Position = new Vector2(x + sizeDifference, springTopSprite.Position.Y);
		}
	}
}