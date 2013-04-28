using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;

namespace CoderForRent.Silverlight.Charting.Core
{
	public static class WriteableBitmapExtensions
	{
		

		public static void SetPixel(this WriteableBitmap instance, int x, int y, int color)
		{
			int index = (instance.PixelWidth * (y - 1)) + x;
			instance.Pixels[index] = color;
		}
		public static int ConvertColor(this WriteableBitmap instance, Color color)
		{
			return (color.A << 24) | (color.R << 16) | (color.G << 8) | (color.B);
		}

	}
}
