using System;
using System.IO;
using Sce.PlayStation.HighLevel.GameEngine2D;

namespace Game
{
	public class HighScoreManager
	{		
		private string[]	_scores;
		private string		_filePath;
		
		public HighScoreManager (Scene scene)
		{
			_scores = new string[5];
			_filePath = @"/Documents/Volcano_Havoc_highscores.txt";
			
			// If highscores doesnt exist, create and populate it
			if (!File.Exists(_filePath))
			{
				using(StreamWriter sw = File.AppendText(_filePath))
				{
					for(int i =0; i < 5; i++)
						sw.WriteLine("< Empty >");
				}
			}
			
			// Read in scores into array
			_scores = ReadHighScores();
		}
		
		public void SaveScore(int newScore)
		{
            EmptyHighScores();
			string score = newScore.ToString ();
			
			for(int i = 0; i < _scores.Length; i++)
			{
				if(newScore > Convert.ToInt32(_scores[i]))
				{
					// Shuffle scores down
                    for (int j = _scores.Length-1; j > i; j--)
						_scores[j] = _scores[j-1];
					
					// Set the new score in its place
					_scores[i] = newScore.ToString();

                    // Empty & Repopulate the file
                    EmptyHighScores();
                    if (!File.Exists(_filePath))
                    {
                        using (StreamWriter sw = File.AppendText(_filePath))
                        {
                            for (int k = 0; k < _scores.Length; k++)
                                sw.WriteLine(_scores[k]);
                        }
                    }

					break;
				}
			}
		}
		
		public string GetHighScores()
		{
			string scores = "";
			
			foreach (string score in _scores)
				scores += score + "\n";
			
			return scores;
		}
		
		public void EmptyHighScores()
		{
			File.Delete(_filePath);
			
			using(StreamWriter sw = File.AppendText(_filePath))
			{
				// Empty stored scores, and write to file
				for(int i = 0; i < _scores.Length; i++)
				{
					sw.WriteLine(_scores[i]);
				} 
            }
		}
		
		public void ClearHighScores()
		{
			File.Delete(_filePath);
			
			using(StreamWriter sw = File.AppendText(_filePath))
			{
				// Empty stored scores, and write to file
				for(int i = 0; i < _scores.Length; i++)
				{
					_scores[i] = "0";
					sw.WriteLine(_scores[i]);
				} 
            }
		}
		
		private string[] ReadHighScores()
		{
			string[] scores = new string[5];
			string score;
			int pos = 0;
			
			try
			{
				using(StreamReader sr = new StreamReader(_filePath))
				{
					while((score = sr.ReadLine ()) != null)
					{
						scores[pos] = score;
						pos++;
					}
				}
			}
			catch (Exception e) {}
			
			return scores;
		}
	}
}

