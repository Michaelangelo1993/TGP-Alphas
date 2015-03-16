using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace Game
{
	public class Pit
	{
		private SpriteUV _sprite;
		private TextureInfo _textureInfo;
		public Bounds2 _box;
		
		public Bounds2 GetBox { get { return _box; }}
		public float GetEndPosition() { return (_sprite.Position.X + 356); }
		
		public Pit (Scene scene, Vector2 position)
		{				
			_textureInfo = new TextureInfo("/Application/textures/Pit.png");	
			
			//Create Sprite
			_sprite 				= new SpriteUV(_textureInfo);
			
			_sprite.Quad.S 			= _textureInfo.TextureSizef;
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
		}
		
		public void Reset(float x)
		{
			_sprite.Position = new Vector2(x, _sprite.Position.Y);
		}
		
		public void SetWidth(float width) { _sprite.Quad.S = new Vector2(width, _textureInfo.TextureSizef.Y); }
		public void SetXPos(float x) { _sprite.Position = new Vector2(x, _sprite.Position.Y); }
		public void Visible(bool visible) { _sprite.Visible = visible; }
	}
}

