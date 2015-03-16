using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;


namespace Game
{
	public class Background
	{
		//Private variables.
		//volcano
		private SpriteUV 	volcSprite;
		private TextureInfo	volcTextureInfo;
		//walls
		private SpriteUV	wallSprite;
		private SpriteUV	wallSprite2;
		private SpriteUV	entrSprite;
		private TextureInfo	wallTextureInfo;
		private TextureInfo	wall2TextureInfo;
		//smog
		private SpriteUV 	smogSprite;
		private SpriteUV	smogSprite2;
		
		private SpriteUV	smog2Sprite;
		private SpriteUV	smog2Sprite2;
		
		private TextureInfo	smogTextureInfo;
		private TextureInfo	smog2TextureInfo;
		
		//floor
		private SpriteUV	underFloorSprite;
		private TextureInfo underFloorTextureInfo;
		private SpriteUV	underFloor2Sprite;
		
		private SpriteUV	floorSprite;
		private TextureInfo floorTextureInfo;
		private SpriteUV	floor2Sprite;
		
		private SpriteUV	floorOverlay;
		private TextureInfo	floorOTextureInfo;
		private SpriteUV	floor2Overlay;
		
		private float		overHeight = 100.0f;
		
		private float		width;
		private float		height2;
		
		private static Scene tScene;
		
		//Public functions.
		public Background (Scene scene)
		{
			initVolcano ();
			initwalls ();
			initSmogs ();
			
			tScene = scene;
			//Get sprite bounds.
			Bounds2 b = volcSprite.Quad.Bounds2();
			width     = b.Point10.X;
			Bounds2 b2 = volcSprite.Quad.Bounds2();
			height2     = b2.Point11.Y;
			
			//Position background.
			volcSprite.Position 	= new Vector2(0.0f, 0.0f);
			smogSprite.Position	 	= new Vector2(0.0f, 0.0f);
			smogSprite2.Position 	= new Vector2(width-2.0f, 0.0f);	
			
			wallSprite.Position 	= new Vector2(width*1.5f, 0.0f);
			wallSprite2.Position 	= new Vector2(width*2.5f-2.0f, 0.0f);
					
			entrSprite.Position 	= new Vector2(width*0.5f, 0.0f);
			
			underFloorSprite 			= new SpriteUV();			
			underFloorTextureInfo 		= new TextureInfo("/Application/textures/underFloor.png");
			underFloorSprite 			= new SpriteUV(underFloorTextureInfo);
			underFloorSprite.Position 	= new Vector2(0.0f, 0.0f);
			underFloorSprite.Quad.S 	= underFloorTextureInfo.TextureSizef;
			
			underFloor2Sprite 			= new SpriteUV();			
			underFloor2Sprite 			= new SpriteUV(underFloorTextureInfo);
			underFloor2Sprite.Position 	= new Vector2(width, 0.0f);
			underFloor2Sprite.Quad.S 	= underFloorTextureInfo.TextureSizef;
			
			floorSprite 			= new SpriteUV();			
			floorTextureInfo 		= new TextureInfo("/Application/textures/floor.png");
			floorSprite 			= new SpriteUV(floorTextureInfo);
			floorSprite.Position 	= new Vector2(0.0f, underFloorTextureInfo.TextureSizef.Y);
			floorSprite.Quad.S 		= floorTextureInfo.TextureSizef;
			
			floor2Sprite 			= new SpriteUV();			
			floor2Sprite 			= new SpriteUV(floorTextureInfo);
			floor2Sprite.Position 	= new Vector2(width, underFloorTextureInfo.TextureSizef.Y);
			floor2Sprite.Quad.S 	= floorTextureInfo.TextureSizef;
			
			floorOverlay	 		= new SpriteUV();			
			floorOTextureInfo 		= new TextureInfo("/Application/textures/transrocks.png");
			floorOverlay	 		= new SpriteUV(floorOTextureInfo);
			floorOverlay.Position 	= new Vector2(0.0f, overHeight);
			floorOverlay.Quad.S 	= floorOTextureInfo.TextureSizef;
			
			floor2Overlay	 		= new SpriteUV();			
			floor2Overlay	 		= new SpriteUV(floorOTextureInfo);
			floor2Overlay.Position	= new Vector2(width, overHeight);
			floor2Overlay.Quad.S 	= floorOTextureInfo.TextureSizef;
			
			addToScene(scene);

		}	
		
		public void initVolcano()
		{
			//The basic background
			volcSprite	= new SpriteUV();			
			volcTextureInfo  	= new TextureInfo("/Application/textures/volc2.png");
			volcSprite 			= new SpriteUV(volcTextureInfo);
			volcSprite.Quad.S 	= volcTextureInfo.TextureSizef;
		}
		
		public void initSmogs()
		{
			//Create the scrolling smog sprites
			smogSprite			= new SpriteUV();
			smogSprite2 		= new SpriteUV();
			
			smog2Sprite			= new SpriteUV();
			smog2Sprite2 		= new SpriteUV();
			
			//smog textures
			smogTextureInfo  	= new TextureInfo("/Application/textures/dclouds2.png");
			smog2TextureInfo  	= new TextureInfo("/Application/textures/dclouds2.png");
			
			smogSprite 			= new SpriteUV(smogTextureInfo);
			smogSprite.Quad.S 	= smogTextureInfo.TextureSizef;
			
			smogSprite2 		= new SpriteUV(smogTextureInfo);
			smogSprite2.Quad.S 	= smogTextureInfo.TextureSizef;
			
		}
		
		public void initwalls()
		{
			//Create the scrolling wall sprites
			wallSprite	= new SpriteUV();
			wallSprite2 = new SpriteUV();
			
			//wall textures
			wallTextureInfo  = new TextureInfo("/Application/textures/brownwall2.png");
			wall2TextureInfo = new TextureInfo("/Application/textures/cavestart3.png");
			
			wallSprite = new SpriteUV(wallTextureInfo);
			wallSprite2 = new SpriteUV(wallTextureInfo);
			entrSprite = new SpriteUV(wall2TextureInfo);
			wallSprite.Quad.S 	= wallTextureInfo.TextureSizef;
			wallSprite2.Quad.S 	= wallTextureInfo.TextureSizef;
			entrSprite.Quad.S 	= wall2TextureInfo.TextureSizef;
		}
		
		public void addToScene(Scene scene)
		{
			//Add to the current scene.
			scene.AddChild (volcSprite);
			scene.AddChild (smogSprite);
			scene.AddChild (smogSprite2);
			scene.AddChild (wallSprite);
			scene.AddChild (wallSprite2);
			scene.AddChild (entrSprite);
			scene.AddChild (floorOverlay);
			scene.AddChild (floor2Overlay);
			scene.AddChild (floorSprite);
			scene.AddChild (floor2Sprite);

		}
		
		public void addUnderFloor(Scene scene)
		{
			scene.AddChild(underFloorSprite);
			scene.AddChild(underFloor2Sprite);
		}
		
		public void Dispose(Scene scene)
		{
			scene.RemoveChild (volcSprite, true);
			scene.RemoveChild (smogSprite, true);
			scene.RemoveChild (smogSprite2, true);
			scene.RemoveChild (wallSprite, true);
			scene.RemoveChild (wallSprite2, true);
			scene.RemoveChild (entrSprite, true);
			scene.RemoveChild (floorOverlay, true);
			scene.RemoveChild (floor2Overlay, true);
			scene.RemoveChild (floorSprite, true);
			scene.RemoveChild (floor2Sprite, true);
			scene.RemoveChild(underFloorSprite, true);
			scene.RemoveChild(underFloor2Sprite, true);
			volcTextureInfo.Dispose();
			wallTextureInfo.Dispose();
			wall2TextureInfo.Dispose();
			smogTextureInfo.Dispose();
			smog2TextureInfo.Dispose();
			underFloorTextureInfo.Dispose();
			floorTextureInfo.Dispose();
			floorOTextureInfo.Dispose();
		}
		
		public void Update(float speed)
		{	
			UpdateWalls();
			UpdateFloor(speed);
			UpdateSmog();
		}
		
		public void UpdateSmog()
		{
			//Moves the smog overlay so that it's a constant scrolling image
			smogSprite.Position = new Vector2(smogSprite.Position.X+2.0f, smogSprite.Position.Y);
			smogSprite2.Position = new Vector2(smogSprite2.Position.X+2.0f, smogSprite2.Position.Y);
			
			//Resets the position once off screen
			if(smogSprite.Position.X+smogTextureInfo.TextureSizef.X <= volcSprite.Position.X)
				smogSprite.Position = new Vector2(smogSprite2.Position.X + smogTextureInfo.TextureSizef.X, 0.0f);
			if(smogSprite2.Position.X+smogTextureInfo.TextureSizef.X <= volcSprite.Position.X)
				smogSprite2.Position = new Vector2(smogSprite.Position.X + smogTextureInfo.TextureSizef.X, 0.0f);
				
		}
		
		public void UpdateWalls()
		{
			//Moves the wall overlay so that it's a constant scrolling image
//			wallSprite.Position = new Vector2(wallSprite.Position.X-1.5f, wallSprite.Position.Y);
//			wallSprite2.Position = new Vector2(wallSprite2.Position.X-1.5f, wallSprite2.Position.Y);
//			entrSprite.Position = new Vector2(entrSprite.Position.X-1.5f, entrSprite.Position.Y);
			
			//Resets the position once off screen
			if(wallSprite.Position.X+wallTextureInfo.TextureSizef.X <= volcSprite.Position.X)
				wallSprite.Position = new Vector2(wallSprite2.Position.X + wallTextureInfo.TextureSizef.X, 0.0f);
			if(wallSprite2.Position.X+wallTextureInfo.TextureSizef.X <= volcSprite.Position.X)
				wallSprite2.Position = new Vector2(wallSprite.Position.X + wallTextureInfo.TextureSizef.X, 0.0f);
			if(entrSprite.Position.X < -width)
				tScene.RemoveChild (entrSprite, true);
			
			
			
		}
		
		public void UpdateFloor(float speed)
		{
			underFloorSprite.Position = new Vector2(underFloorSprite.Position.X-speed, underFloorSprite.Position.Y);
			underFloor2Sprite.Position = new Vector2(underFloor2Sprite.Position.X-speed, underFloor2Sprite.Position.Y);
			floorSprite.Position = new Vector2(floorSprite.Position.X-speed, floorSprite.Position.Y);
			floor2Sprite.Position = new Vector2(floor2Sprite.Position.X-speed, floor2Sprite.Position.Y);
			
			//Resets the position once off screen
			if(underFloorSprite.Position.X+floorTextureInfo.TextureSizef.X <= volcSprite.Position.X)
				underFloorSprite.Position = new Vector2(underFloor2Sprite.Position.X + underFloorTextureInfo.TextureSizef.X, 0.0f);
			if(underFloor2Sprite.Position.X+underFloorTextureInfo.TextureSizef.X <= volcSprite.Position.X)
				underFloor2Sprite.Position = new Vector2(underFloorSprite.Position.X + underFloorTextureInfo.TextureSizef.X, 0.0f);
			if(floorSprite.Position.X+floorTextureInfo.TextureSizef.X <= volcSprite.Position.X)
				floorSprite.Position = new Vector2(floor2Sprite.Position.X + floorTextureInfo.TextureSizef.X, floorSprite.Position.Y);
			if(floor2Sprite.Position.X+floorTextureInfo.TextureSizef.X <= volcSprite.Position.X)
				floor2Sprite.Position = new Vector2(floorSprite.Position.X + floorTextureInfo.TextureSizef.X, floor2Sprite.Position.Y);
			
			floorOverlay.Position = new Vector2(floorOverlay.Position.X-speed, floorOverlay.Position.Y);
			floor2Overlay.Position = new Vector2(floor2Overlay.Position.X-speed, floor2Overlay.Position.Y);
			
			//Resets the position once off screen
			if(floorOverlay.Position.X+floorOTextureInfo.TextureSizef.X <= volcSprite.Position.X)
				floorOverlay.Position = new Vector2(floor2Overlay.Position.X + floorTextureInfo.TextureSizef.X, overHeight);
			if(floor2Overlay.Position.X+floorTextureInfo.TextureSizef.X <= volcSprite.Position.X)
				floor2Overlay.Position = new Vector2(floorOverlay.Position.X + floorTextureInfo.TextureSizef.X, overHeight);
		}
		
		public void SetVolcanoPosition(float x, float y) { volcSprite.Position = new Vector2(x,y); }
		public Vector2 GetVolcanoPosition() { return volcSprite.Position; }
		
		public float GetFloorHeight() { return floorSprite.Position.Y + floorTextureInfo.TextureSizef.Y; }
	}
}


