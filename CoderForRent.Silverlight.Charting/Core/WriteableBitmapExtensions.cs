using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CoderForRent.Charting.Core
{
	public static class WriteableBitmapExtensions
	{
		

		public static void SetPixel(this WriteableBitmap instance, int x, int y, int color)
		{
			int index = (instance.PixelWidth * (y - 1)) + x;
#if SILVERLIGHT
			instance.Pixels[index] = color;
#else
		    instance.WritePixels(new Int32Rect(x, y, 1, 1), new[] {color}, 0, 0);
#endif
		}
		public static int ConvertColor(this WriteableBitmap instance, Color color)
		{
			return (color.A << 24) | (color.R << 16) | (color.G << 8) | (color.B);
		}

	}
}
