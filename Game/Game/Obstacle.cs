using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace Game
{
	public class Obstacle
	{
		public Obstacle ()
		{
			
		}
				
		public virtual void Update(float speed)
		{
			// Overvide this
		}
		
		public virtual void Dispose()
		{
			// Overvide this
		}
		
		public virtual void ReleasePlunger()
		{
			// Overvide this	
		}
		
		public virtual void ReleaseSpring(bool b)
		{
			// Overvide this	
		}
		
		public virtual void Reset(float x)
		{
			// Overvide this	
		}
		
		public virtual float GetEndPosition()
		{
			// Overvide this	
			return 0;
		}
	}
}

