using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace Game
{
	public enum Screens
	{
		Splash,
		Menu,
		Game,
		GameOver,
		HighScores
	}
	
	public class ScreenManager
	{
		private bool 	_transitioning;
		private bool 	_fading;
		private Screens _screen;
		private Screens _nextScreen;
		
		public Screens GetScreen() { return _screen; }
		public bool IsTransitioning() { return (_transitioning); }
		
		// Background sprite stuff
		private SpriteUV _bgSprite;
		private TextureInfo _bgTextureInfo;
		
		public ScreenManager(Scene scene)
		{
			_screen = Screens.Splash;
			_nextScreen = Screens.Splash;
			_transitioning = true;
			_fading = false;
			
			_bgTextureInfo = new TextureInfo("/Application/textures/splash.png");
			_bgSprite = new SpriteUV(_bgTextureInfo);
			_bgSprite.Scale = new Vector2(960.0f, 544.0f);
			_bgSprite.Color.A = 0.0f;
			
			
			
			scene.AddChild(_bgSprite);
		}
		
		public void Update(Scene scene)
		{
			// If screen coming off/on, decrease/increase transparancy of background, if fully faded, change image
			if(_transitioning)
			{
				if(_fading)
				{
					if(_bgSprite.Color.A <= 0.0f)
						UpdateSprite(scene);
					else
						_bgSprite.Color.A -= 0.01f;
				}
				else
				{
					if(_bgSprite.Color.A >= 1.0f)
						_transitioning = false;
					else
						_bgSprite.Color.A += 0.01f;	
				}
			}
		}
		
		public void ChangeScreenTo(Screens nextScreen)
		{
			_nextScreen = nextScreen;
			_transitioning = true;
			_fading = true;
			AppMain.SetUISystem("null");
		}
		
		private void UpdateSprite(Scene scene)
		{
			_fading = false;
			_screen = _nextScreen;
			scene.RemoveChild(_bgSprite, false);
						
			switch(_screen)
			{
			case Screens.Splash:
				_bgTextureInfo = new TextureInfo("/Application/textures/splash.png");
				_bgSprite = new SpriteUV(_bgTextureInfo);
				_bgSprite.Scale = new Vector2(960.0f, 544.0f);
				_bgSprite.Color.A = 0.0f;
				scene.AddChild(_bgSprite);
				break;
			case Screens.Menu:
				_bgTextureInfo = new TextureInfo("/Application/textures/menu.png");
				_bgSprite = new SpriteUV(_bgTextureInfo);
				_bgSprite.Scale = new Vector2(960.0f, 544.0f);
				_bgSprite.Color.A = 0.0f;
				scene.AddChild(_bgSprite);
				break;
			case Screens.Game:
				_bgTextureInfo = new TextureInfo("/Application/textures/game.png");
				_bgSprite = new SpriteUV(_bgTextureInfo);
				_bgSprite.Scale = new Vector2(960.0f, 544.0f);
				_bgSprite.Color.A = 0.0f;
				scene.AddChild(_bgSprite);
				AppMain.SetUISystem("game");
				break;
			case Screens.GameOver:
				_bgTextureInfo = new TextureInfo("/Application/textures/gameOver.png");
				_bgSprite = new SpriteUV(_bgTextureInfo);
				_bgSprite.Scale = new Vector2(960.0f, 544.0f);
				_bgSprite.Color.A = 0.0f;
				scene.AddChild(_bgSprite);
				AppMain.SetUISystem("highscores");
				break;
			case Screens.HighScores:
				_bgTextureInfo = new TextureInfo("/Application/textures/highscores.png");
				_bgSprite = new SpriteUV(_bgTextureInfo);
				_bgSprite.Scale = new Vector2(960.0f, 544.0f);
				_bgSprite.Color.A = 0.0f;
				scene.AddChild(_bgSprite);
				AppMain.SetUISystem("highscores");
				break;
			}			
		}
	}
}

