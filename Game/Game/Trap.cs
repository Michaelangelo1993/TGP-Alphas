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
		
		public Bounds2 GetBox { get { return _box; }}
		
		public Trap (Scene scene, Vector2 position)
		{	
			_textureInfo = new TextureInfo("/Application/textures/Trap.png");	
			_sprite = new SpriteUV(_textureInfo);
			_sprite.Position = position;
			_sprite.Quad.S = _textureInfo.TextureSizef;
			_box = _sprite.Quad.Bounds2 ();
			
			scene.AddChild(_sprite);
		}
		
		public void Update(float deltaTime, float speed)
		{
			_sprite.Position = new Vector2(_sprite.Position.X - speed, _sprite.Position.Y);	
		}
		
		public void Reset()
		{
			_sprite.Position = new Vector2(_sprite.Position.X + 2500, _sprite.Position.Y);
			
		}
	}
}

