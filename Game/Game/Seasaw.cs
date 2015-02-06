using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace Game
{
	public class Seasaw
	{
		private static SpriteUV 	_sprite;
		private static TextureInfo	_textureInfo;
		private static Vector2		_min, _max;
		private static Bounds2		_box;
		private static float 		_scale, _angle, _angle2, _rotationSpeed,
									_adjacent, _opposite, _hypotenuse;
		private static bool 		_rotateLeft;
		
		
		private static Vector2 oldTouchPos = new Vector2( 0.0f, 0.0f ); // Position of first touch on screen
		private static Vector2 newTouchPos = new Vector2( 0.0f, 0.0f ); // Position of last touch on screen
		
		public Seasaw (Scene scene, Vector2 position)
		{
			//Initialise Variables
			_scale = 1.00f;
			_rotationSpeed = -0.015f;
			_rotateLeft = false;

			//SpriteSheet Info
			_textureInfo  = new TextureInfo("/Application/textures/Seasaw.png");
			
			//Create Sprite
			_sprite	 				= new SpriteUV();
			_sprite 				= new SpriteUV(_textureInfo);
			_sprite.Quad.S 			= _textureInfo.TextureSizef;
			_sprite.Scale			= new Vector2(_scale, _scale);
			_sprite.CenterSprite();
			_sprite.Position 		= position;
			
			
			_hypotenuse 			= (_textureInfo.TextureSizef.X *_scale);
			_angle 					= 0.55f;
			_angle2					= (float)((System.Math.PI/2)-_angle);	
			_opposite 				= (float)System.Math.Cos(_angle2) * _hypotenuse;
			_adjacent 				= (float)System.Math.Tan(_angle) * _opposite;
			_sprite.Angle 			= _angle;
			
			//Add to the current scene.
			scene.AddChild(_sprite);
		}
		
		public void Dispose()
		{
			_textureInfo.Dispose();
		}
		
		public void Update(float deltaTime, float speed)
		{
			//_sprite.Rotate(0.1f);
			CheckInput();
			UpdateAngle();
			
			//Storing Bounds2 box data for collisions
			_min.X			= _sprite.Position.X - (_adjacent *_scale);
			_min.Y			= _sprite.Position.Y - (_opposite *_scale);
			_max.X			= _sprite.Position.X + (_adjacent *_scale);
			_max.Y			= _sprite.Position.Y + (_opposite *_scale);
			_box.Min 		= _min;			
			_box.Max 		= _max;
		}
		
		private void CheckInput()
		{
			var touches = Touch.GetData(0);
			
			//If tapped, do something
			if(touches.Count > 0)
			{				
				_rotateLeft = true;								
			}					
			
			
		}
		
		private void UpdateAngle()
		{
			
			if(_rotateLeft)
				_sprite.Rotate(-_rotationSpeed);
			else
				_sprite.Rotate(_rotationSpeed);
			
			if (_sprite.Angle > 0.55f)
			{
				_sprite.Angle = 0.55f;
				_rotateLeft = false;
			}
			
			if (_sprite.Angle < -0.55f)
			{
				_sprite.Angle = -0.55f;
			}
			
			_angle = _sprite.Angle;
		}
		
		//Get and set the rotation of the player
		public void SetAngle(float angle) { _angle = angle; }
		public float GetAngle(){ return _angle; }
		
		//Get the collision box of the player
		public Bounds2 GetBox() { return _box; }
	}
}

