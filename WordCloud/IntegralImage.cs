using System;

namespace WordCloud
{
	internal class IntegralImage
	{
		public IntegralImage(int width, int height)
		{
			Integral = new uint[width,height];
			Width = width;
			Height = height;
		}

		public void Update(FastImage image)
		{
			Array.Clear(Integral, 0, Integral.Length);

			for (int i = 1; i < image.Height; ++i)
			{
				for (int j = 1; j < image.Width; ++j)
				{
					byte pixel = 0;
					for (int p = 0; p < image.PixelFormatSize; ++p)
					{
						pixel |= image.Data[i*image.Stride + j*image.PixelFormatSize + p];
					}
					Integral[j, i] = pixel + Integral[j - 1, i] + Integral[j, i - 1] - Integral[j - 1, i - 1];
				}
			}
		}

		public ulong GetArea(int xPos, int yPos, int sizeX, int sizeY)
		{
			ulong area = Integral[xPos, yPos] + Integral[xPos + sizeX, yPos + sizeY];
			area -= Integral[xPos + sizeX, yPos] + Integral[xPos, yPos + sizeY];
			return area;
		}

		public int Width { get; set; }

		public int Height { get; set; }

		protected uint[,] Integral { get; set; }
	}
}
