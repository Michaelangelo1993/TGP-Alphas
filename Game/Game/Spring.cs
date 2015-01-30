using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace Game
{
	public class Spring
	{
		private bool springRelease;
		private SpriteUV springTopSprite;
		private TextureInfo springTopTextureInfo;
		public float springTopHeight;
		
		private SpriteUV springSprite;
		private TextureInfo springTextureInfo;
		private float springOriginalHeight;
		private float springCurrentHeight;
		
		private SpriteUV trapSprite;
		private TextureInfo trapTextureInfo;
		public float trapWidth;
		
		public Vector2 GetPosition { get { return trapSprite.Position; }}
		public float GetWidth { get { return trapWidth; }}
		public float GetTop { get { return (springTopSprite.Position.Y + springTopHeight); }}
		
		public Spring (Scene scene, Vector2 position)
		{
			springRelease = false;
			
			// Initialise trap texture and sprite, get bounds and set width
			trapTextureInfo = new TextureInfo("/Application/Trap.png");			
			trapSprite = new SpriteUV(trapTextureInfo);
			trapSprite.Position = position;
			trapSprite.Quad.S = trapTextureInfo.TextureSizef;
			Bounds2 trapBounds = trapSprite.Quad.Bounds2 ();
			trapWidth = trapBounds.Point10.X;
			
			// Initialise spring texture and sprite, get bounds and set position minus height offset
			springTextureInfo = new TextureInfo("/Application/Spring.png");
			springSprite = new SpriteUV(springTextureInfo);
			springSprite.Quad.S = springTextureInfo.TextureSizef;
			Bounds2 springHeight = springSprite.Quad.Bounds2 ();
			springSprite.Position = new Vector2(position.X-100, position.Y);
			
			// Initialise spring texture and sprite, get bounds and set position minus height offset
			springTopTextureInfo = new TextureInfo("/Application/SpringTop.png");
			springTopSprite = new SpriteUV(springTopTextureInfo);
			springTopSprite.Quad.S = springTopTextureInfo.TextureSizef;
			Bounds2 springTopBounds = springTopSprite.Quad.Bounds2 ();
			springTopHeight = springTopBounds.Point01.Y;
			springTopSprite.Position = new Vector2(position.X-100, springSprite.Position.Y + springHeight.Point01.Y);
			//springTopSprite. = new Vector2(1f,1f);
			
			// Add sprites to scene
			scene.AddChild(springSprite);
			scene.AddChild(trapSprite);
			scene.AddChild(springTopSprite);
		}
		
		public void ReleaseSpring()
		{
			springRelease = true;
		}
		
		public void WindSpring()
		{
			if(!springRelease)
			{
				if(springCurrentHeight > 50)
				{
					springTopSprite.Position = new Vector2(springTopSprite.Position.X, springTopSprite.Position.Y-1);
					springCurrentHeight--;
					springSprite.Scale = new Vector2(springSprite.Scale.X, springCurrentHeight);
				}
				else
				{
					springRelease = true;	
				}
			}
		}
		
		public void Update()
		{
			if(springRelease)
			{
				if(springCurrentHeight < springOriginalHeight)
				{
					springTopSprite.Position = new Vector2(springTopSprite.Position.X, springTopSprite.Position.Y+1);
					springCurrentHeight++;
					springSprite.Scale = new Vector2(springSprite.Scale.X, springCurrentHeight);
				}
				else
				{
					springRelease = false;	
				}
			}
		}
	}
}

