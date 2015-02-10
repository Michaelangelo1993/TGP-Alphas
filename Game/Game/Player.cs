using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace Game
{
	public class Player
	{
		//Private variables.
		private static SpriteUV 	_sprite;
		private static TextureInfo	_textureInfo;
		private static Vector2		_min, _max, _velocity,
									_jumpingVector, _jumpVelocity,
									_movementVector;
		private static Bounds2		_box, _bottomBox;
		private static float 		_scale, _angle, _size,
									_gravity, _force,
									_speed, _defaultYPos,
									_floorHeight;
		private static int 			_frameTime, _animationDelay,
									_noOnSpritesheetWidth,
									_noOnSpritesheetHeight,
									_widthCount, _heightCount;
		private static bool 		_jump;
		
		//Public functions.
		public Player (Scene scene, float floorHeight)
		{
			//Initialise Variables
			_frameTime 				= 0;
			_animationDelay 		= 3;
			_widthCount 			= 0;
			_heightCount 			= 0;	
			_speed 					= 3.0f;
			_size 					= 115.0f;
			_scale 					= 1.00f;
			_angle 					= 0.0f;
			_gravity 				= -1.98f;
			_force 					= 25.0f;
			_floorHeight			= floorHeight;
			_velocity				= new Vector2(0.0f, 0.0f);
			_movementVector 		= new Vector2(0.0f, 0.0f);
			_jumpingVector 			= new Vector2(0.0f, 0.0f);
			_jumpVelocity 			= new Vector2(0.0f, 0.0f);
			_jump  					= false;
			
			//SpriteSheet Info
			_textureInfo  			= new TextureInfo("/Application/textures/stick.png");
			_noOnSpritesheetWidth 	= 4;
			_noOnSpritesheetHeight 	= 2;
			_defaultYPos			= ((_textureInfo.TextureSizef.Y/_noOnSpritesheetHeight)*_scale)*0.5f;
			
			//Create Sprite
			_sprite	 				= new SpriteUV();
			_sprite 				= new SpriteUV(_textureInfo);
			_sprite.UV.S 			= new Vector2(1.0f/_noOnSpritesheetWidth,1.0f/_noOnSpritesheetHeight);
			_sprite.Quad.S 			= new Vector2(_size, _size);
			_sprite.Scale			= new Vector2(_scale, _scale);
			_sprite.Position 		= new Vector2((Director.Instance.GL.Context.GetViewport().Width/2) - 400, _defaultYPos + _floorHeight);
			_sprite.CenterSprite();
			
			//Add to the current scene.
			scene.AddChild(_sprite);
		}
		
		public void Dispose()
		{
			_textureInfo.Dispose();
		}
		
		public void Update(float deltaTime)
		{
			//Move Player
			AnimatePlayer();
			Move ();
			CheckJump();
			
			//Stop player from falling through the ground
			if (_sprite.Position.Y < _defaultYPos + _floorHeight)
			{
				_sprite.Position = new Vector2(_sprite.Position.X, _defaultYPos + _floorHeight);
				_jump 			 = false;
			}
			
			//Rotate Player
			if(_sprite.Angle > _angle)
				_sprite.Rotate(-0.05f);
			else if(_sprite.Angle < _angle)
				_sprite.Rotate(0.05f);
			
			//Reset rotation;
			_angle = 0.0f;
			
			//Storing Bounds2 box data for collisions
			_min.X					= _sprite.Position.X - ((_size*_scale)*0.5f);
			_min.Y					= _sprite.Position.Y - ((_size*_scale)*0.3f);
			_max.X					= _sprite.Position.X + ((_size*_scale)*0.5f);
			_max.Y					= _sprite.Position.Y + ((_size*_scale)*0.3f);
			_box.Min 				= _min;			
			_box.Max 				= _max;
			
			_min.X					= _sprite.Position.X - ((_size*_scale)*0.5f);
			_min.Y					= _sprite.Position.Y - ((_size*_scale)*0.3f);
			_max.X					= _sprite.Position.X + ((_size*_scale)*0.5f);
			_max.Y					= _sprite.Position.Y - ((_size*_scale)*0.3f);
			_bottomBox.Min 				= _min;			
			_bottomBox.Max 				= _max;
		}
		
		private void AnimatePlayer()
		{
			if(!_jump)
			{
				if(_frameTime == _animationDelay)
				{
					if (_widthCount == _noOnSpritesheetWidth)
					{
						_heightCount++;
						_widthCount = 0;
					}
					
					if (_heightCount == _noOnSpritesheetHeight)
					{
						//_widthCount++;
						_heightCount = 0;
					}
					
					_sprite.UV.T = new Vector2((1.0f/_noOnSpritesheetWidth)*_widthCount,(1.0f/_noOnSpritesheetHeight)*_heightCount);
					
					_widthCount++;
					//_heightCount++;
					_frameTime = 0;
				}
				
				_frameTime++;
			}
		}
		
		private void Move()
		{
			if(!_jump)
			{
				_movementVector 	= new Vector2(_sprite.Position.X + 100.0f, _defaultYPos + _floorHeight);
				_velocity			= Normalize(_movementVector, _sprite.Position);
				_velocity 			= new Vector2(_velocity.X * _speed, _velocity.Y * _speed);
				_velocity 			= new Vector2(_velocity.X, _velocity.Y + (_gravity*_speed));
				_sprite.Position 	= new Vector2 (_sprite.Position.X + _velocity.X, _sprite.Position.Y + _velocity.Y);
				_velocity 			= new Vector2(_velocity.X, _velocity.Y * 0.95f);
			}
		}
		
		private void CheckJump()
		{
			if(_jump)
			{
				_jumpVelocity 		= new Vector2(_jumpVelocity.X, _jumpVelocity.Y + _gravity);
				_sprite.Position 	= new Vector2 (_sprite.Position.X + _jumpVelocity.X, _sprite.Position.Y + _jumpVelocity.Y);
				_jumpVelocity 		= new Vector2(_jumpVelocity.X, _jumpVelocity.Y * 0.95f);
			}
		}
		
		private Vector2 Normalize(Vector2 destination, Vector2 position)
		{
			Vector2 vector 			= new Vector2(destination.X - position.X, destination.Y - position.Y);
			float vectorMagnitude 	= FMath.Sqrt(FMath.Pow(vector.X, 2) + FMath.Pow(vector.Y, 2));
			vector 					= new Vector2(vector.X / vectorMagnitude, vector.Y / vectorMagnitude);
	
			return vector;
		}
		
				
		public void DoJump()
		{
			_jumpingVector 			= new Vector2(_sprite.Position.X + 600.0f, _sprite.Position.Y + 1000.0f);
			_jumpVelocity 			= Normalize(_jumpingVector, _sprite.Position);
			_jumpVelocity 			= new Vector2(_jumpVelocity.X * _force, _jumpVelocity.Y * _force);
			_jump 					= true;
		}
		
		//Get and set the size of the player
		public void SetScale(float scale)
		{ 
			_scale 					= scale;
			_sprite.Scale 			= new Vector2(_scale, _scale);
			_sprite.Position 		= new Vector2(_sprite.Position.X, _defaultYPos);
		}
		public float GetScale() { return _scale; }
		
		//Get and set the rotation of the player
		public void SetAngle(float angle) { _angle = angle; }//_sprite.Angle = _angle; }
		public float GetAngle(){ return _angle; }
		
		//Get and set the gravity of the player
		public void SetGravity(int gravity) { _gravity = gravity; }
		public float GetGravity(){ return _gravity; }
		
		//Get and set the upforce of the player
		public void SetUpForce(int upForce) { _force = upForce; }
		public float GetUpForce(){ return _force; }
		
		//Get and set the speed of the player
		public void SetSpeed(float speed) { _speed = speed; }
		public float GetSpeed(){ return _speed; }
		
		//Get the size of the player
		public float GetSize(){ return _size *_scale; }
		
		//Get and Set the position of the player
		public void SetPos(float x, float y) { new Vector2(x, _defaultYPos + y); }
		public Vector2 GetPos() { return _sprite.Position; }
		
		//Set the height of the player
		public void SetYPos(float y) { _sprite.Position = new Vector2(_sprite.Position.X, y); }
		
		//Get the collision box of the player
		public Bounds2 GetBox() { return _box; }
		public Bounds2 GetBottomBox() { return _bottomBox; }
	}
}

