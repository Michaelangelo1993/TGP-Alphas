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
		private static bool 		_onObstacle, _rotateLeft;
		
		public Seasaw (Scene scene, Vector2 position)
		{
			//Initialise Variables
			_scale 					= 1.00f;
			_rotationSpeed 			= -0.015f;
			_rotateLeft 			= false;
			_onObstacle 			= false;

			//SpriteSheet Info
			_textureInfo  			= new TextureInfo("/Application/textures/Seasaw.png");
			
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
			CheckInput();
			UpdateAngles();
			

			
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
		
		private void UpdateAngles()
		{
			
			if(_rotateLeft)
				_sprite.Rotate(-_rotationSpeed);
			else
				_sprite.Rotate(_rotationSpeed);
			
			//Keep Seasaw from rotating too far left
			if (_sprite.Angle > 0.55f)
			{
				_sprite.Angle 	= 0.55f;
				_rotateLeft 	= false;
			}
			
			//Keep Seasaw from rotating too far Right
			if (_sprite.Angle < -0.55f)
			{
				_sprite.Angle = -0.55f;
			}
			
			//Update the angle variables
			_angle 		= _sprite.Angle;
			_angle2 	= (float)((System.Math.PI/2) - _angle);
		}
		
		//Get the player Y position for walking on the Seasaw
		public float GetNewPlayerYPos(Vector2 position)
		{
			
			float adjacent, opposite, hypotenuse;
			
			adjacent 	= _sprite.Position.X - position.X;
			hypotenuse  = adjacent/(float)(System.Math.Sin(_angle2));
			opposite 	= (float)(System.Math.Cos(_angle2))*hypotenuse;
			
			return (_sprite.Position.Y+((_textureInfo.TextureSizef.Y *_scale)*0.8f))- opposite;
			
		}
		
		//Get and Set if the player is on the seasaw
		public void SetIsOn() { if (_angle > 0.5f) _onObstacle = true; }
		public bool IsOn(){ return _onObstacle; }
		
		//Get and set the rotation of the player
		public void SetAngle(float angle) { _angle = angle; }
		public float GetAngle(){ return _angle; }
		
		//Get the collision box of the player
		public Bounds2 GetBox() { return _box; }
	}
}

