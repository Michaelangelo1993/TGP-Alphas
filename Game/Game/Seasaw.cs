using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace Game
{
	public class Seasaw : Obstacle
	{
		private SpriteUV 	_sprite;
		private TextureInfo	_textureInfo;
		private Vector2		_min, _max;
		private Bounds2		_box;
		private float 		_scale, _angle, _angle2, _rotationSpeed,
									_adjacent, _opposite, _hypotenuse,
									_floorHeight, _defaultYPos, _tempScale,
									_scaleLimiter, _scalerValue;
		private bool 		_onObstacle, _rotateLeft;
		
		private Trap			_trap;
		private Pit 			_pit;
		private Random rand;
		
		
		override public float GetEndPosition() { return (_sprite.Position.X + 150); }
		
		public Seasaw (Scene scene, float xPos, float floorHeight)
		{
			//Initialise Variables
			rand = new Random();
			
			_scale 					= 1.00f;
			_rotationSpeed 			= 0.01f;
			_scaleLimiter			= 0.3f;
			_tempScale				= 1.0f;
			_rotateLeft 			= false;
			_onObstacle 			= false;
			_floorHeight			= floorHeight;
			_defaultYPos			= floorHeight + 45.0f;
			

			//SpriteSheet Info
			_textureInfo  			= new TextureInfo("/Application/textures/Seasaw.png");
			
			//Create Sprite
			_sprite	 				= new SpriteUV();
			_sprite 				= new SpriteUV(_textureInfo);
			_sprite.Quad.S 			= _textureInfo.TextureSizef;
			_sprite.Scale			= new Vector2(_scale, _scale);
			_sprite.CenterSprite();
			_sprite.Position 		= new Vector2(xPos + 180, _defaultYPos);
			
			_hypotenuse 			= (_textureInfo.TextureSizef.X *_scale);
			_angle 					= -0.32f;
			_angle2					= (FMath.PI/2)-_angle;	
			_opposite 				= FMath.Cos(_angle2) * _hypotenuse;
			_adjacent 				= FMath.Tan(_angle) * _opposite;
			_sprite.Angle 			= _angle;
			_scalerValue 			= _tempScale/(_angle*10);
			
			_trap = new Trap(scene, new Vector2(xPos, 60.0f));
			_pit = new Pit(scene, new Vector2(xPos, 60));	
			
			//Add to the current scene.
			scene.AddChild(_sprite);
		}
		
		override public void Dispose()
		{
			_textureInfo.Dispose();
		}
		
		override public void Update(float t)
		{
			CheckInput();
			UpdateAngles(t);
			
			_sprite.Position += new Vector2(-t, 0.0f);
			
			_trap.Update(t);
			_pit.Update(t);
			
			_min.X			= _sprite.Position.X - 160;
			_min.Y			= _sprite.Position.Y - 200;
			_max.X			= _sprite.Position.X - 170;
			_max.Y			= _sprite.Position.Y + 200;
			_box.Min 		= _min;			
			_box.Max 		= _max;
			
			if(AppMain.GetPlayer().GetBox().Overlaps(_box))
				if (_angle > 0.3f)
					_onObstacle = true;
			
			if(_onObstacle)
			{
				AppMain.GetPlayer().SetAngle(_angle);
				AppMain.GetPlayer().SetYPos(GetNewPlayerYPos(AppMain.GetPlayer().GetPos()));
			}
		}
		
		override public void Reset(float x)
		{
			SetXPos(x);
		}
		
		private void CheckInput()
		{
			var motionData = Motion.GetData(0);
			
			if(motionData.Acceleration.X<= 0)
				_rotateLeft = true;
			else
				_rotateLeft = false;
		}
		
		private void UpdateAngles(float gameSpeed)
		{
			
			if(_rotateLeft)
				_sprite.Rotate(_rotationSpeed*gameSpeed);
			else
				_sprite.Rotate(-_rotationSpeed*gameSpeed);
			
			//Keep Seasaw from rotating too far left
			if (_sprite.Angle > 0.32f)
			{
				_sprite.Angle 	= 0.32f;
				_rotateLeft 	= false;
			}
			
			//Keep Seasaw from rotating too far Right
			if (_sprite.Angle < -0.32f)
				_sprite.Angle = -0.32f;
			
			//Update the angle variables
			_angle 	= _sprite.Angle;
			_angle2 = (FMath.PI/2) - _angle;
		}
		
		//Get the player Y position for walking on the Seasaw
		public float GetNewPlayerYPos(Vector2 position)
		{
			float distanceBetween = FMath.Sqrt(FMath.Pow((position.X - _sprite.Position.X), 2) + FMath.Pow((position.Y - _sprite.Position.Y), 2));
			if(distanceBetween < 0)
				distanceBetween = -distanceBetween;
			
			if(distanceBetween < (_textureInfo.TextureSizef.X/2)+40 && _onObstacle)
			{
				float adjacent, opposite, hypotenuse;
				
				adjacent 	= _sprite.Position.X - position.X;
				hypotenuse  = adjacent/(FMath.Sin(_angle2));
				opposite 	= (FMath.Cos(_angle2))*hypotenuse;
				
				_tempScale = ((_angle *10) * _scalerValue);
				
				if (_tempScale < 0)
					_tempScale = -_tempScale;
				
				float tempScale =(_scaleLimiter * _tempScale);
				
				return ((_sprite.Position.Y+((_textureInfo.TextureSizef.Y *_scale)*tempScale))- opposite) + (115/2);
			}
			else
				_onObstacle = false;
			
			return position.Y;
		}
		
		//Get and Set if the player is on the seasaw
		public void SetIsOn() { if (_angle > 0.3f) _onObstacle = true; }
		public bool IsOn(){ return _onObstacle; }
		
		//Get and set the rotation of the player
		public void SetAngle(float angle) { _angle = angle; }
		public float GetAngle(){ return _angle; }
		
		//Set X position of the seasaw 
		public void SetXPos(float x)
		{
			int randomNum = (rand.Next(0, 2));
			
			if(randomNum == 0)
			{
				// Magma
				_trap.Visible(true);
				_pit.Visible(false);				
			}
			else
			{
				// Magma
				_trap.Visible(false);
				_pit.Visible(true);					
			}
			
			_sprite.Position = new Vector2(x + 180, _defaultYPos);
			_trap.SetXPos(x);
			_pit.SetXPos(x);
		}
		
		//Get the postion of the seasaw
		public Vector2 GetPos(){ return _sprite.Position; }
		
		//Get the collision box of the player
		public Bounds2 GetBox() { return _box; }
	}
}

