using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace Game
{
	public class Trap
	{
		private SpriteUV _sprite;
		private TextureInfo _textureInfo;
		public Bounds2 _box;
		
		private int 			_frameTime, _animationDelay,
									_noOnSpritesheetWidth,
									_widthCount;
		
		public Bounds2 GetBox { get { return _box; }}
		public float GetEndPosition() { return (_sprite.Position.X + 270); }
		
		public Trap (Scene scene, Vector2 position)
		{	
			_animationDelay = 4;
			
			_textureInfo = new TextureInfo("/Application/textures/TrapSpriteSheet.png");	
			_noOnSpritesheetWidth 	= 6;
			
			//Create Sprite
			_sprite	 				= new SpriteUV();
			_sprite 				= new SpriteUV(_textureInfo);
			_sprite.UV.S 			= new Vector2(1.0f/_noOnSpritesheetWidth,1.0f);
			_sprite.Quad.S 			= new Vector2(_textureInfo.TextureSizef.X/_noOnSpritesheetWidth, _textureInfo.TextureSizef.Y);
			_sprite.Position 		= position;
			_box = _sprite.Quad.Bounds2 ();
			
			scene.AddChild(_sprite);
		}
		
		public void Update(float deltaTime, float speed)
		{
			_sprite.Position = new Vector2(_sprite.Position.X - speed, _sprite.Position.Y);	
			
			if(_frameTime == _animationDelay)
			{
				if (_widthCount == _noOnSpritesheetWidth)
					_widthCount = 0;
				
				_sprite.UV.T = new Vector2((1.0f/_noOnSpritesheetWidth)*_widthCount, 0.0f);
				_widthCount++;
				_frameTime = 0;
			}
			
			_frameTime++;
		}
		
		public void Reset(float x)
		{
			//_sprite.Position = new Vector2(_sprite.Position.X + x, _sprite.Position.Y);
			_sprite.Position = new Vector2(x, _sprite.Position.Y);
		}
		
		public void SetWidth(float width) { _sprite.Quad.S = new Vector2(width, _textureInfo.TextureSizef.Y); }
		public void SetXPos(float x) { _sprite.Position = new Vector2(x, 0.0f); }
	}
}

