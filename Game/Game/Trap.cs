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
									_noOnSpritesheetHeight,
									_noOnSpritesheet, _counter,
									_widthCount, _heightCount;
		
		public Bounds2 GetBox { get { return _box; }}
		public float GetEndPosition() { return (_sprite.Position.X + 356); }
		
		public Trap (Scene scene, Vector2 position)
		{	
			_textureInfo 			= new TextureInfo("/Application/textures/TrapSpriteSheet.png");	
			_noOnSpritesheetWidth 	= 4;
			_noOnSpritesheetHeight 	= 3;
			_noOnSpritesheet		= 11;
			_widthCount 			= 0;
			_heightCount 			= _noOnSpritesheetHeight - 1;
			_animationDelay 		= 4;
			_counter				= 1;	
			
			//Create Sprite
			_sprite	 				= new SpriteUV();
			_sprite 				= new SpriteUV(_textureInfo);
			_sprite.UV.S 			= new Vector2(1.0f/_noOnSpritesheetWidth,1.0f/_noOnSpritesheetHeight);
			
			//_sprite.Quad.S 			= new Vector2(_size, _size);
			//_sprite.Scale			= new Vector2(_scale, _scale);
			
			_sprite.Quad.S 			= new Vector2(_textureInfo.TextureSizef.X/_noOnSpritesheetWidth, _textureInfo.TextureSizef.Y/_noOnSpritesheetHeight);
			_sprite.Position 		= position;
			_box = _sprite.Quad.Bounds2 ();
			
			scene.AddChild(_sprite);
		}
		
		public void Dispose(Scene scene)
		{
			scene.RemoveChild(_sprite, true);
			_textureInfo.Dispose();
		}
			
		public void Update(float speed)
		{
			_sprite.Position = new Vector2(_sprite.Position.X - speed, _sprite.Position.Y);	
			
			if(_frameTime == _animationDelay)
			{
				if (_widthCount == _noOnSpritesheetWidth)
					{
						_heightCount--;
						_widthCount = 0;
					}
					
					if (_heightCount < 0)
					{
						_heightCount = _noOnSpritesheetHeight - 1;
					}				
				_sprite.UV.T = new Vector2((1.0f/_noOnSpritesheetWidth)*_widthCount, (1.0f/_noOnSpritesheetHeight)*_heightCount);
				_widthCount++;
				_counter++;
				_frameTime = 0;
				
				if (_counter > _noOnSpritesheet)
				{
					_counter = 1;
					_widthCount = 0;
					_heightCount = _noOnSpritesheetHeight - 1;
				}
			}
			
			_frameTime++;
		}
		
		public void Reset(float x)
		{
			//_sprite.Position = new Vector2(_sprite.Position.X + x, _sprite.Position.Y);
			_sprite.Position = new Vector2(x, _sprite.Position.Y);
		}
		
		public void SetWidth(float width) { _sprite.Quad.S = new Vector2(width, _textureInfo.TextureSizef.Y); }
		public void SetXPos(float x) { _sprite.Position = new Vector2(x, _sprite.Position.Y); }
		public void Visible(bool visible) { _sprite.Visible = visible; }
	}
}

