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
		private static Vector2		_min, _max;
		private static Bounds2		_box;
		private static float 		_scale, _angle, _size,
									_gravity, _upForce,
									_speed;
		private static int 			_frameTime, _animationDelay,
									_noOnSpritesheetWidth,
									_noOnSpritesheetHeight,
									_widthCount, _heightCount;
		
		//Public functions.
		public Player (Scene scene)
		{
			//Initialise Variables
			_frameTime 		= 0;
			_animationDelay = 3;
			_widthCount 	= 0;
			_heightCount 	= 0;	
			_speed 			= 3;
			_size 			= 115.0f;
			_scale 			= 1.00f;
			_angle 			= 0.0f;
			
			//SpriteSheet Info
			_textureInfo  			= new TextureInfo("/Application/textures/stick.png");
			_noOnSpritesheetWidth 	= 4;
			_noOnSpritesheetHeight 	= 2;
			
			//Create Sprite
			_sprite	 				= new SpriteUV();
			_sprite 				= new SpriteUV(_textureInfo);
			_sprite.UV.S 			= new Vector2(1.0f/_noOnSpritesheetWidth,1.0f/_noOnSpritesheetHeight);
			_sprite.Quad.S 			= new Vector2(_size, _size);
			_sprite.Scale			= new Vector2(_scale, _scale);
			_sprite.Position 		= new Vector2((Director.Instance.GL.Context.GetViewport().Width/2) - 400, (((_textureInfo.TextureSizef.Y/_noOnSpritesheetHeight)*_scale)*0.5f) +80);
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

			//System.Diagnostics.Debug.WriteLine(_sprite.Position.X);
			//Player Animation
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
			
			//Move Player
			_sprite.Position = new Vector2(_sprite.Position.X + _speed, _sprite.Position.Y - 9.8f);
			
			//Stop player from falling through the ground
			if (_sprite.Position.Y < ((_textureInfo.TextureSizef.Y/_noOnSpritesheetHeight)*_scale)*0.5f +80)
				_sprite.Position = new Vector2(_sprite.Position.X, ((_textureInfo.TextureSizef.Y/_noOnSpritesheetHeight)*_scale)*0.5f +80);
			
			//Rotate Player
			if(_sprite.Angle > _angle)
				_sprite.Rotate(-0.05f);
			else if(_sprite.Angle < _angle)
				_sprite.Rotate(0.05f);
						
			//Storing Bounds2 box data for collisions
			_min.X			= _sprite.Position.X - ((_size*_scale)*0.5f);
			_min.Y			= _sprite.Position.Y - ((_size*_scale)*0.3f);
			_max.X			= _sprite.Position.X + ((_size*_scale)*0.5f);
			_max.Y			= _sprite.Position.Y + ((_size*_scale)*0.3f);
			_box.Min 		= _min;			
			_box.Max 		= _max;
		}
		
		//Set the height of the player
		public void SetYPos(float y) { _sprite.Position = new Vector2(_sprite.Position.X, y); }
		
		//Get and set the size of the player
		public void SetScale(float scale)
		{ 
			_scale 				= scale;
			_sprite.Scale 		= new Vector2(_scale, _scale);
			_sprite.Position 	= new Vector2(_sprite.Position.X, ((_textureInfo.TextureSizef.Y/_noOnSpritesheetHeight)*_scale)*0.5f);
		}
		public float GetScale() { return _scale; }
		
		//Get and set the rotation of the player
		public void SetAngle(float angle) { _angle = angle; }//_sprite.Angle = _angle; }
		public float GetAngle(){ return _angle; }
		
		//Get and set the gravity of the player
		public void SetGravity(int gravity) { _gravity = gravity; }
		public float GetGravity(){ return _gravity; }
		
		//Get and set the upforce of the player
		public void SetUpForce(int upForce) { _upForce = upForce; }
		public float GetUpForce(){ return _upForce; }
		
		//Get and set the speed of the player
		public void SetSpeed(int speed) { _speed = speed; }
		public float GetSpeed(){ return _speed; }
		
		//Get the size of the player
		public float GetSize(){ return _size; }
		
		//Get and Set the position of the player
		public void SetPos(float x, float y) { new Vector2(x, (((_textureInfo.TextureSizef.Y/_noOnSpritesheetHeight)*_scale)*0.5f) + y); }
		public Vector2 GetPos() { return _sprite.Position; }
		
		//Get the collision box of the player
		public Bounds2 GetBox() { return _box; }
	}
}

