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
		private SpriteUV 	_sprite, _fireDeathSprite;
		
		private TextureInfo	_textureInfo, _fireDeathTextureInfo;
		
		private Vector2		_min, _max, _velocity,
							_jumpingVector, _jumpVelocity,
							_movementVector;
		
		private Bounds2		_box, _bottomBox, _deathBox;
		private float 		_scale, _angle, _size,
							_gravity, _force,
							_speed, _defaultYPos,
							_floorHeight;
		
		private int 		_frameTime, _animationDelay,
							_noOnSpritesheetWidth,
							_noOnSpritesheetHeight,
							_noOnFDSpritesheetWidth,
							_noOnFDSpritesheetHeight,
							_widthCount, _heightCount,
							_fDwidthCount, _fDheightCount,
							_counter;
		
		private bool 		_jump, _dead, _killed,
							_killedByFire;
		
		
		
		//Public functions.
		public Player (Scene scene, float floorHeight)
		{
			//Initialise Variables
			_frameTime 				= 0;
			_animationDelay 		= 3;
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
			_dead					= false;
			_killed					= false;
			_killedByFire			= false;
			
			//SpriteSheet Info
			_textureInfo  			= new TextureInfo("/Application/textures/stick2.png");
			_noOnSpritesheetWidth 	= 4;
			_noOnSpritesheetHeight 	= 2;
			_defaultYPos			= ((_textureInfo.TextureSizef.Y/_noOnSpritesheetHeight)*_scale)*0.5f;
			_widthCount 			= 0;
			_heightCount 			= _noOnSpritesheetHeight - 1;
			
			_fireDeathTextureInfo  		= new TextureInfo("/Application/textures/deathSpriteFire.png");
			_noOnFDSpritesheetWidth 	= 4;
			_noOnFDSpritesheetHeight 	= 4;
			_counter 					= _noOnFDSpritesheetWidth * _noOnFDSpritesheetHeight;
			_fDwidthCount 				= 0;
			_fDheightCount 				= _noOnFDSpritesheetHeight - 1;
			
			//Create Sprite
			_sprite	 				= new SpriteUV();
			_sprite 				= new SpriteUV(_textureInfo);
			_sprite.UV.S 			= new Vector2(1.0f/_noOnSpritesheetWidth,1.0f/_noOnSpritesheetHeight);
			_sprite.Quad.S 			= new Vector2(_size, _size);
			_sprite.Scale			= new Vector2(_scale, _scale);
			_sprite.Position 		= new Vector2((Director.Instance.GL.Context.GetViewport().Width/2) - 400, _defaultYPos + _floorHeight);
			_sprite.CenterSprite();
			
			_fireDeathSprite	 		= new SpriteUV();
			_fireDeathSprite 			= new SpriteUV(_fireDeathTextureInfo);
			_fireDeathSprite.UV.S 		= new Vector2(1.0f/_noOnFDSpritesheetWidth,1.0f/_noOnFDSpritesheetHeight);
			_fireDeathSprite.Quad.S 	= new Vector2(_size, _size);
			_fireDeathSprite.Scale		= new Vector2(1.5f, 1.5f);
			_fireDeathSprite.Position 	= _sprite.Position;
			_fireDeathSprite.CenterSprite();
			
			//Add to the current scene.
			scene.AddChild(_sprite);
			scene.AddChild(_fireDeathSprite);
			
			_fireDeathSprite.Visible = false;
		}
		
		public void Dispose()
		{
			_textureInfo.Dispose();
		}
		
		public void Update()
		{
			if(!_killed)
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
			}
			else
			{
				if(_killedByFire)
					KillPlayerByFire();
			}
			
			//Storing Bounds2 box data for collisions
			_min.X					= _sprite.Position.X;
			_min.Y					= _sprite.Position.Y - ((_size*_scale)*0.5f);
			_max.X					= _sprite.Position.X + ((_size*_scale)*0.3f);
			_max.Y					= _sprite.Position.Y + ((_size*_scale)*0.5f);
			_box.Min 				= _min;			
			_box.Max 				= _max;
			
			_min.X					= _sprite.Position.X - ((_size*_scale)*0.5f);
			_min.Y					= _sprite.Position.Y - ((_size*_scale)*0.5f);
			_max.X					= _sprite.Position.X + ((_size*_scale)*0.5f);
			_max.Y					= _sprite.Position.Y - ((_size*_scale)*0.5f);
			_bottomBox.Min 			= _min;			
			_bottomBox.Max 			= _max;
			
		}
		
		private void AnimatePlayer()
		{
			if(!_jump)
			{
				if(_frameTime == _animationDelay)
				{
					if (_widthCount == _noOnSpritesheetWidth)
					{
						_heightCount--;
						_widthCount = 0;
					}
					
					if (_heightCount < 0)
					{
						//_widthCount++;
						_heightCount = _noOnSpritesheetHeight - 1;
					}
					
					_sprite.UV.T = new Vector2((1.0f/_noOnSpritesheetWidth)*_widthCount,(1.0f/_noOnSpritesheetHeight)*_heightCount);
					_widthCount++;
					//_heightCount--;
					_frameTime = 0;
				}
				
				_frameTime++;
			}
		}
		
		private void Move()
		{
			if(!_jump)
			{
				_movementVector 	+= new Vector2(100.0f, 0.0f);
				_velocity			= Normalize(_movementVector, _sprite.Position);
				_velocity 			*=  _speed;
				_velocity 			+= new Vector2(0.0f, _gravity *_speed);
				_sprite.Position 	+= _velocity;
				_velocity 			*= new Vector2(0.0f, 0.95f);
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
			if(!_dead)
			{
				_jumpingVector 			= new Vector2(_sprite.Position.X + 800.0f, _sprite.Position.Y + 1200.0f);
				_jumpVelocity 			= Normalize(_jumpingVector, _sprite.Position);
				_jumpVelocity 			= new Vector2(_jumpVelocity.X * _force, _jumpVelocity.Y * _force);
				_jump 					= true;
			}
		}
		
		private void KillPlayerByFire()
		{
			_fireDeathSprite.Position = _sprite.Position;
			
			if(_counter == 0)
			{
				_fireDeathSprite.Visible = false;
				_dead = true;
				_counter = _noOnFDSpritesheetWidth * _noOnFDSpritesheetHeight;
			}
			else
			{
				if(_frameTime == _animationDelay)
				{
					if(_counter == 9)
						_sprite.Visible = false;
					
					_counter--;
					
					//Spritesheet scrolling
					if (_fDwidthCount == _noOnFDSpritesheetWidth)
					{
						_fDheightCount--;
						_fDwidthCount = 0;
					}
				
					if (_fDheightCount < 0)
					{
						_fDheightCount = _noOnFDSpritesheetHeight - 1;
					}
					
					_fireDeathSprite.UV.T = new Vector2((1.0f/_noOnFDSpritesheetWidth)*_fDwidthCount,(1.0f/_noOnFDSpritesheetHeight)*_fDheightCount);
					_fDwidthCount++;
					
					_frameTime = 0;
				}
				
				_frameTime++;
			}
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
		
		//Set if the player was killed by fire
		public void KillByFire() { _killed = true; _killedByFire = true; _fireDeathSprite.Visible = true; _frameTime = 0; _animationDelay = 2; }
		
		//Return if the player is dead
		public bool IsDead() { return _dead; }
		public bool HasBeenKilled() { return _killed; }
		
		public void IsntDead() { _killed = false; _sprite.Visible = true; }
	}
}

