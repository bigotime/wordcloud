using System;
using System.Collections.Generic;

namespace WordCloud
{
	internal class OccupancyMap : IntegralImage
	{
		public OccupancyMap(int width, int height) : base(width, height)
		{
			Rand = new Random();

			XPoses = new List<int>(Width * Height);
			YPoses = new List<int>(Width * Height);
		}

		public bool TryFindUnoccupiedPosition(int sizeX, int sizeY, out int oPosX, out int oPosY)
		{
			oPosX = -1;
			oPosY = -1;

			XPoses.Clear();
			YPoses.Clear();

			for (int i = 1; i < (Height - sizeY); ++i)
			{
				for (int j = 1; j < (Width - sizeX); ++j)
				{
					if (GetArea(j, i, sizeX, sizeY) == 0)
					{
						XPoses.Add(j);
						YPoses.Add(i);
					}
				}
			}

			if (XPoses.Count > 0)
			{
				int posItr = Rand.Next(XPoses.Count);
				oPosX = XPoses[posItr];
				oPosY = YPoses[posItr];
				return true;
			}
			return false;
		}


		private List<int> XPoses { get; set; }

		private List<int> YPoses { get; set; } 

		private Random Rand { get; set; }
	}
}
