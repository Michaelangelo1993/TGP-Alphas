using System;
using Sce.PlayStation.Core.Audio;
using Sce.PlayStation.HighLevel.GameEngine2D;

namespace Game
{
	public class SoundManager
	{
		private BgmPlayer 		_bgmPlayer;
		
		private Sound			_jumpSound;
		private SoundPlayer 	_jumpPlayer;
		
		private Sound			_deathSound;
		private SoundPlayer 	_deathPlayer;
		
		public void PlayJump() { _jumpPlayer.Play(); }
		public void PlayBGM() { /*_bgmPlayer.Play();*/ }
		public void PlayDeath() { _deathPlayer.Play(); }
		
		public SoundManager ()
		{
			_jumpSound  = new Sound("/Application/sounds/jump.wav");
			_jumpPlayer = _jumpSound.CreatePlayer();
			
			_deathSound  = new Sound("/Application/sounds/death.wav");
			_deathPlayer = _deathSound.CreatePlayer();
			
			Bgm bgmMusic = new Bgm("/Application/music/157172__danipenet__distant-world.mp3");
			_bgmPlayer = bgmMusic.CreatePlayer();
		}
	}
}