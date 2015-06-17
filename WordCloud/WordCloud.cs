using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace WordCloud
{
	/// <summary>
	/// Class to draw word clouds.
	/// </summary>
	public class WordCloud
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="WordCloud"/> class.
		/// </summary>
		/// <param name="width">The width of word cloud.</param>
		/// <param name="height">The height of word cloud.</param>
		/// <param name="useRank">if set to <c>true</c> will ignore frequencies for best fit.</param>
		/// <param name="fontColor">Color of the font.</param>
		/// <param name="maxFontSize">Maximum size of the font.</param>
		/// <param name="fontStep">The font step to use.</param>
		public WordCloud(int width, int height, bool useRank = false, Color? fontColor = null, int maxFontSize = -1, int fontStep = 1)
		{
			Map = new OccupancyMap(width, height);
			Image = new FastImage(width, height, PixelFormat.Format32bppArgb);

			MaxFontSize = maxFontSize < 0 ? height : maxFontSize;
			FontStep = fontStep;
			m_fontColor = fontColor;
			UseRank = useRank;
			Random = new Random();
		}


		/// <summary>
		/// Draws the specified word cloud given list of words and frequecies
		/// </summary>
		/// <param name="words">List of words ordered by occurance.</param>
		/// <param name="freqs">List of frequecies.</param>
		/// <returns>Image of word cloud.</returns>
		/// <exception cref="System.ArgumentException">
		/// Arguments null.
		/// or
		/// Must have the same number of words as frequencies.
		/// </exception>
		public Image Draw(List<string> words, List<int> freqs)
		{
			var fontSize = MaxFontSize;
			if (words == null || freqs == null)
			{
				throw new ArgumentException("Arguments null.");
			}
			if (words.Count != freqs.Count)
			{
				throw new ArgumentException("Must have the same number of words as frequencies.");
			}

			using (Graphics g = Graphics.FromImage(Image.Bitmap))
			{
				g.Clear(Color.Transparent);
				g.TextRenderingHint = TextRenderingHint.AntiAlias;
				for(int i = 0; i < words.Count; ++i)
				{
					if (!UseRank)
					{
						fontSize = (int)Math.Min(fontSize, 100 * Math.Log10(freqs[i] + 100));
					}
					var format = StringFormat.GenericTypographic;
					format.FormatFlags &= ~StringFormatFlags.LineLimit;

					int posX, posY;
					bool foundPosition = false;
					Font font;

					do
					{
						font = new Font(FontFamily.GenericSansSerif, fontSize, GraphicsUnit.Pixel);
						SizeF size = g.MeasureString(words[i], font, new PointF(0, 0), format);
						foundPosition = Map.TryFindUnoccupiedPosition((int) size.Width, (int) size.Height, out posX, out posY);
						if (!foundPosition) fontSize -= FontStep;
					} while (fontSize > 0 && !foundPosition);
					if (fontSize <= 0) break;

					g.DrawString(words[i], font, new SolidBrush(FontColor), posX, posY, format);
					Map.Update(Image);
				}
			}

			var result = Image.Bitmap.Clone();
			Image.Dispose();
			return (Image)result;
		}

		private Color FontColor { get { return m_fontColor ?? GetRandomColor(); } set { m_fontColor = value; } }

		private Color? m_fontColor;

		private Color GetRandomColor()
		{
			return Color.FromArgb(Random.Next(0, 255), Random.Next(0, 255), Random.Next(0, 255));
			// The error is here
		}

		private Random Random { get; set; }
		private FastImage Image { get; set; }
		private OccupancyMap Map { get; set; }
		private int MaxFontSize { get; set; }

		private bool UseRank { get; set; }

		private int FontStep { get; set; }
	}
}
