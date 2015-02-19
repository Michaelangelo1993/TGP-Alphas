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
		private SpriteUV	floorSprite;
		private TextureInfo floorTextureInfo;
		
		private SpriteUV	floor2Sprite;
		
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
			
			floorSprite 			= new SpriteUV();			
			floorTextureInfo 		= new TextureInfo("/Application/textures/floor.png");
			floorSprite 			= new SpriteUV(floorTextureInfo);
			floorSprite.Position 	= new Vector2(0.0f, 0.0f);
			floorSprite.Quad.S 		= floorTextureInfo.TextureSizef;
			
			floor2Sprite 			= new SpriteUV();			
			floor2Sprite 			= new SpriteUV(floorTextureInfo);
			floor2Sprite.Position 	= new Vector2(width, 0.0f);
			floor2Sprite.Quad.S 	= floorTextureInfo.TextureSizef;
			
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
			scene.AddChild(volcSprite);
			scene.AddChild (smogSprite);
			scene.AddChild (smogSprite2);
			scene.AddChild (wallSprite);
			scene.AddChild (wallSprite2);
			scene.AddChild (entrSprite);
			scene.AddChild (floorSprite);
			scene.AddChild (floor2Sprite);
		}
		
		public void Dispose()
		{
			volcTextureInfo.Dispose();
			smogTextureInfo.Dispose();
			wallTextureInfo.Dispose ();
			wall2TextureInfo.Dispose ();
			smog2TextureInfo.Dispose ();
		}
		
		public void Update(float deltaTime, float speed)
		{	
			UpdateWalls(deltaTime);
			UpdateFloor(deltaTime, speed);
			UpdateSmog(deltaTime);
		}
		
		public void UpdateSmog(float deltaTime)
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
		
		public void UpdateWalls(float deltaTime)
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
		
		public void UpdateFloor(float deltaTime, float speed)
		{
			floorSprite.Position = new Vector2(floorSprite.Position.X-speed, floorSprite.Position.Y);
			floor2Sprite.Position = new Vector2(floor2Sprite.Position.X-speed, floor2Sprite.Position.Y);
			
			//Resets the position once off screen
			if(floorSprite.Position.X+floorTextureInfo.TextureSizef.X <= volcSprite.Position.X)
				floorSprite.Position = new Vector2(floor2Sprite.Position.X + floorTextureInfo.TextureSizef.X, 0.0f);
			if(floor2Sprite.Position.X+floorTextureInfo.TextureSizef.X <= volcSprite.Position.X)
				floor2Sprite.Position = new Vector2(floorSprite.Position.X + floorTextureInfo.TextureSizef.X, 0.0f);
		}
		
		public void SetVolcanoPosition(float x, float y) { volcSprite.Position = new Vector2(x,y); }
		public Vector2 GetVolcanoPosition() { return volcSprite.Position; }
		
		public float GetFloorHeight() { return floorTextureInfo.TextureSizef.Y-5; }
	}
}


