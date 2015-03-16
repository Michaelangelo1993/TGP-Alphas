using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace Game
{
	public enum PopUp
	{
		HowToPlay,
		Spring,
		Seasaw,
		Spinning,
		Geiser,
		TNT
	}
	
	public class TutorialManager
	{
		private PopUp _popUp;
		private bool  _popUpActive;
		private bool  _tutorialsEnabled;
		private bool  _ready; // Makes sure 1-tap doesnt spam through popup windows
		
		
		public bool HasPopUp() { return (_popUpActive); }
		public bool IsReady() { return _ready; }
		public void SetReady() {_ready = true; }
		
		// Popup sprite stuff
		private SpriteUV _popUpSprite;
		private TextureInfo _popUpTextureInfo;
		
		public TutorialManager (Scene scene)
		{
			_tutorialsEnabled = true;
			_popUpActive = true;
			_popUp = PopUp.HowToPlay;
			_ready = false;
			
			_popUpTextureInfo = new TextureInfo("/Application/textures/tutorial/gamePopUpTutorialsOn.png");
			_popUpSprite = new SpriteUV(_popUpTextureInfo);
			_popUpSprite.Scale = new Vector2(800.0f, 500.0f);
			_popUpSprite.Position = new Vector2(80.0f, 22.0f);
			
			scene.AddChild(_popUpSprite);
		}
		
		public void Dispose(Scene scene)
		{
			scene.RemoveChild(_popUpSprite, true);
			_popUpTextureInfo.Dispose();
		}
		
		public void ClosePopUp(Scene scene)
		{
			_ready = false;
			
			if(_tutorialsEnabled)
			{	
				scene.RemoveChild(_popUpSprite,false);
				
				// Load next popup
				switch(_popUp)
				{
					case PopUp.HowToPlay:
						_popUpTextureInfo.Dispose();
						_popUpTextureInfo = new TextureInfo("/Application/textures/tutorial/springPopUp.png");
						_popUpSprite = new SpriteUV(_popUpTextureInfo);
						_popUpSprite.Scale = new Vector2(800.0f, 500.0f);
						_popUpSprite.Position = new Vector2(80.0f, 22.0f);
						scene.AddChild(_popUpSprite);
						_popUp = PopUp.Spring;
						break;
					case PopUp.Spring:
						_popUpTextureInfo.Dispose();
						_popUpTextureInfo = new TextureInfo("/Application/textures/tutorial/seaSawPopUp.png");
						_popUpSprite = new SpriteUV(_popUpTextureInfo);
						_popUpSprite.Scale = new Vector2(800.0f, 500.0f);
						_popUpSprite.Position = new Vector2(80.0f, 22.0f);
						scene.AddChild(_popUpSprite);
						_popUp = PopUp.Seasaw;
						break;
					case PopUp.Seasaw:
						_popUpTextureInfo.Dispose();
						_popUpTextureInfo = new TextureInfo("/Application/textures/tutorial/spinningPopUp.png");
						_popUpSprite = new SpriteUV(_popUpTextureInfo);
						_popUpSprite.Scale = new Vector2(800.0f, 500.0f);
						_popUpSprite.Position = new Vector2(80.0f, 22.0f);
						scene.AddChild(_popUpSprite);
						_popUp = PopUp.Spinning;
						break;
					case PopUp.Spinning:
						_popUpTextureInfo.Dispose();
						_popUpTextureInfo = new TextureInfo("/Application/textures/tutorial/geiserPopUp.png");
						_popUpSprite = new SpriteUV(_popUpTextureInfo);
						_popUpSprite.Scale = new Vector2(800.0f, 500.0f);
						_popUpSprite.Position = new Vector2(80.0f, 22.0f);
						scene.AddChild(_popUpSprite);
						_popUp = PopUp.Geiser;
						break;
					case PopUp.Geiser:
						_popUpTextureInfo.Dispose();
						_popUpTextureInfo = new TextureInfo("/Application/textures/tutorial/tntPopUp.png");
						_popUpSprite = new SpriteUV(_popUpTextureInfo);
						_popUpSprite.Scale = new Vector2(800.0f, 500.0f);
						_popUpSprite.Position = new Vector2(80.0f, 22.0f);
						scene.AddChild(_popUpSprite);
						_popUp = PopUp.TNT;
						break;
					case PopUp.TNT: // Last tutorial = remove sprite 
						_popUpActive = false;	
						scene.RemoveChild(_popUpSprite,false);
						break;
				}
			}
			else // Tutorials have been disabled, don't display them
			{
				_popUpActive = false;	
				scene.RemoveChild(_popUpSprite,false);
			}
		}
		
		public void DisableTutorials(Scene scene)
		{
			_ready = false;
			_tutorialsEnabled = !_tutorialsEnabled;
			scene.RemoveChild(_popUpSprite,false);
			
			if(_tutorialsEnabled)
			{
				_popUpTextureInfo = new TextureInfo("/Application/textures/tutorial/gamePopUpTutorialsOn.png");
				_popUpSprite = new SpriteUV(_popUpTextureInfo);
				_popUpSprite.Scale = new Vector2(800.0f, 500.0f);
				_popUpSprite.Position = new Vector2(80.0f, 22.0f);
				scene.AddChild(_popUpSprite);
			}
			else
			{
				_popUpTextureInfo = new TextureInfo("/Application/textures/tutorial/gamePopUpTutorialsOff.png");
				_popUpSprite = new SpriteUV(_popUpTextureInfo);
				_popUpSprite.Scale = new Vector2(800.0f, 500.0f);
				_popUpSprite.Position = new Vector2(80.0f, 22.0f);
				scene.AddChild(_popUpSprite);
			}
		}
	}
}

