/*
MIT License

Copyright (c) 2019 Grega Mohorko

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

Project: GM.Utility
Created: 2018-4-4
Author: GregaMohorko
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GM.Utility
{
	/// <summary>
	/// Utilities for working with colors.
	/// <para>Color conversions follow the procedures found at http://www.easyrgb.com/en/math.php .</para>
	/// </summary>
	public static class ColorUtility
	{
		/// <summary>
		/// Returns either black or white, depending on the luminosity of the specified background color.
		/// <para>Follows W3C recommendations (found at https://www.w3.org/TR/WCAG20/ ).</para>
		/// </summary>
		/// <param name="background">The color of the background.</param>
		public static Color GetFontColorToBackground(Color background)
		{
			// https://stackoverflow.com/questions/3942878/how-to-decide-font-color-in-white-or-black-depending-on-background-color

			// The formula given for contrast in the W3C Recommendations is (L1 + 0.05) / (L2 + 0.05), where L1 is the luminance of the lightest color and L2 is the luminance of the darkest on a scale of 0.0-1.0.
			// The luminance of black is 0.0 and white is 1.0, so substituting those values lets you determine the one with the highest contrast.
			// If the contrast for black is greater than the contrast for white, use black, otherwise use white.

			double r = background.R;
			double g = background.G;
			double b = background.B;

			// compute L (the luminance of the background color)
			double L;
			{
				// The formula is also given in the guidelines ( http://www.w3.org/TR/WCAG20/#relativeluminancedef ) and it looks like the conversion from sRGB to linear RGB followed by the ITU-R recommendation BT.709 ( http://en.wikipedia.org/wiki/Luma_(video)#Rec._601_luma_versus_Rec._709_luma_coefficients ) for luminance.
				double rgbToSrgbITUR(double c)
				{
					c /= 255.0;
					if(c <= 0.03928) {
						return c / 12.92;
					} else {
						return Math.Pow((c + 0.055) / 1.055, 2.4);
					}
				}
				r = rgbToSrgbITUR(r);
				g = rgbToSrgbITUR(g);
				b = rgbToSrgbITUR(b);
				L = 0.2126 * r + 0.7152 * g + 0.0722 * b;
			}

			// Given the luminance of the background color as L the test becomes:
			// if (L + 0.05) / (0.0 + 0.05) > (1.0 + 0.05) / (L + 0.05) use #000000 else use #ffffff
			// This simplifies down algebraically to:
			// if L > sqrt(1.05 * 0.05) - 0.05
			// Or approximately:
			return L > 0.179 ? Color.Black : Color.White;
		}

		/// <summary>
		/// Converts from RGB to HSV.
		/// </summary>
		/// <param name="color">The color to convert.</param>
		/// <param name="H">Hue (from 0 to 1).</param>
		/// <param name="S">Saturation (from 0 to 1).</param>
		/// <param name="V">Value (from 0 to 1).</param>
		public static void RGBToHSV(Color color, out double H, out double S, out double V)
		{
			if(color == null) {
				throw new ArgumentNullException(nameof(color));
			}
			RGBToHSV(color.R, color.G, color.B, out H, out S, out V);
		}

		/// <summary>
		/// Converts from RGB to HSV.
		/// </summary>
		/// <param name="H">Hue (from 0 to 1).</param>
		/// <param name="S">Saturation (from 0 to 1).</param>
		/// <param name="V">Value (from 0 to 1).</param>
		public static Color HSVToRGB(double H, double S, double V)
		{
			HSVToRGB(H, S, V, out byte R, out byte G, out byte B);
			return Color.FromArgb(R, G, B);
		}

		/// <summary>
		/// Converts from RGB to HSV.
		/// </summary>
		/// <param name="R">Red.</param>
		/// <param name="G">Green.</param>
		/// <param name="B">Blue.</param>
		/// <param name="H">Hue (from 0 to 1).</param>
		/// <param name="S">Saturation (from 0 to 1).</param>
		/// <param name="V">Value (from 0 to 1).</param>
		public static void RGBToHSV(byte R, byte G, byte B, out double H, out double S, out double V)
		{
			double Rd = R / 255d;
			double Gd = G / 255d;
			double Bd = B / 255d;

			RGBToHSV(Rd, Gd, Bd, out H, out S, out V);
		}

		/// <summary>
		/// Converts from HSV to RGB.
		/// </summary>
		/// <param name="H">Hue (from 0 to 1).</param>
		/// <param name="S">Saturation (from 0 to 1).</param>
		/// <param name="V">Value (from 0 to 1).</param>
		/// <param name="R">Red.</param>
		/// <param name="G">Green.</param>
		/// <param name="B">Blue.</param>
		public static void HSVToRGB(double H, double S, double V, out byte R, out byte G, out byte B)
		{
			HSVToRGB(H, S, V, out double Rd, out double Gd, out double Bd);

			R = (byte)(Rd * 255);
			G = (byte)(Gd * 255);
			B = (byte)(Bd * 255);
		}

		/// <summary>
		/// Converts from RGB to HSV.
		/// </summary>
		/// <param name="R">Red (from 0 to 1).</param>
		/// <param name="G">Green (from 0 to 1).</param>
		/// <param name="B">Blue (from 0 to 1).</param>
		/// <param name="H">Hue (from 0 to 1).</param>
		/// <param name="S">Saturation (from 0 to 1).</param>
		/// <param name="V">Value (from 0 to 1).</param>
		public static void RGBToHSV(double R, double G, double B, out double H, out double S, out double V)
		{
			// RGB -> HSV
			// http://www.easyrgb.com/en/math.php

			double rgbMin = Math.Min(R, Math.Min(G, B));
			double rgbMax = Math.Max(R, Math.Max(G, B));
			double rgbDelta = rgbMax - rgbMin;

			V = rgbMax;

			if(rgbDelta == 0) {
				// this is a gray, no chroma ...
				H = 0;
				S = 0;
				return;
			}

			S = (rgbMax == 0) ? 0 : rgbDelta / rgbMax;

			double rgbDeltaHalf = rgbDelta / 2;

			double deltaR = (((rgbMax - R) / 6) + rgbDeltaHalf) / rgbDelta;
			double deltaG = (((rgbMax - G) / 6) + rgbDeltaHalf) / rgbDelta;
			double deltaB = (((rgbMax - B) / 6) + rgbDeltaHalf) / rgbDelta;

			if(R == rgbMax) {
				H = deltaB - deltaG;
			} else if(G == rgbMax) {
				H = ONE_THIRD + deltaR - deltaB;
			} else {
				H = TWO_THIRDS + deltaG - deltaR;
			}

			if(H < 0) {
				H += 1;
			} else if(H > 1) {
				H -= 1;
			}
		}

		/// <summary>
		/// Converts from HSV to RGB.
		/// </summary>
		/// <param name="H">Hue (from 0 to 1).</param>
		/// <param name="S">Saturation (from 0 to 1).</param>
		/// <param name="V">Value (from 0 to 1).</param>
		/// <param name="R">Red (from 0 to 1).</param>
		/// <param name="G">Green (from 0 to 1).</param>
		/// <param name="B">Blue (from 0 to 1).</param>
		public static void HSVToRGB(double H, double S, double V, out double R, out double G, out double B)
		{
			// HSV -> RGB
			// http://www.easyrgb.com/en/math.php

			if(S == 0) {
				R = V;
				G = V;
				B = V;
				return;
			}

			H *= 6;
			if(H == 6) {
				H = 0;
			}

			int i = (int)H;
			double v1 = V * (1 - S);
			double v2 = V * (1 - S * (H - i));
			double v3 = V * (1 - S * (1 - (H - i)));

			switch(i) {
				case 0:
					R = V;
					G = v3;
					B = v1;
					break;
				case 1:
					R = v2;
					G = V;
					B = v1;
					break;
				case 2:
					R = v1;
					G = V;
					B = v3;
					break;
				case 3:
					R = v1;
					G = v2;
					B = V;
					break;
				case 4:
					R = v3;
					G = v1;
					B = V;
					break;
				default:
					R = V;
					G = v1;
					B = v2;
					break;
			}
		}

		/// <summary>
		/// Converts from RGB to CIE-L*ab.
		/// <para>Uses Noon Daylight (D65) illuminant for reference.</para>
		/// </summary>
		/// <param name="color">The color to convert.</param>
		/// <param name="L">Lightness.</param>
		/// <param name="a">Green-red color component.</param>
		/// <param name="b">Blue-yellow color component.</param>
		public static void RGBToCIELAB(Color color, out double L, out double a, out double b)
		{
			if(color == null) {
				throw new ArgumentNullException(nameof(color));
			}
			RGBToCIELAB(color.R, color.G, color.B, out L, out a, out b);
		}

		/// <summary>
		/// Converts from CIE-L*ab to RGB.
		/// <para>Assumes that Noon Daylight (D65) illuminant was used for reference.</para>
		/// </summary>
		/// <param name="L">Lightness.</param>
		/// <param name="a">Green-red color component.</param>
		/// <param name="b">Blue-yellow color component.</param>
		public static Color CIELABToRGB(double L, double a, double b)
		{
			CIELABToRGB(L, a, b, out byte R, out byte G, out byte B);
			return Color.FromArgb(R, G, B);
		}

		/// <summary>
		/// Converts from RGB to CIE-L*ab.
		/// <para>Uses Noon Daylight (D65) illuminant for reference.</para>
		/// </summary>
		/// <param name="R">Red.</param>
		/// <param name="G">Green.</param>
		/// <param name="B">Blue.</param>
		/// <param name="L">Lightness.</param>
		/// <param name="a">Green-red color component.</param>
		/// <param name="b">Blue-yellow color component.</param>
		public static void RGBToCIELAB(byte R, byte G, byte B, out double L, out double a, out double b)
		{
			double Rd = R / 255d;
			double Gd = G / 255d;
			double Bd = B / 255d;
			RGBToCIELAB(Rd, Gd, Bd, out L, out a, out b);
		}

		/// <summary>
		/// Converts from CIE-L*ab to RGB.
		/// <para>Assumes that Noon Daylight (D65) illuminant was used for reference.</para>
		/// </summary>
		/// <param name="L">Lightness.</param>
		/// <param name="a">Green-red color component.</param>
		/// <param name="b">Blue-yellow color component.</param>
		/// <param name="R">Red.</param>
		/// <param name="G">Green.</param>
		/// <param name="B">Blue.</param>
		public static void CIELABToRGB(double L, double a, double b, out byte R, out byte G, out byte B)
		{
			CIELABToRGB(L, a, b, out double Rd, out double Gd, out double Bd);

			R = (byte)(Rd * 255);
			G = (byte)(Gd * 255);
			B = (byte)(Bd * 255);
		}

		/// <summary>
		/// Converts from RGB to CIE-L*ab.
		/// <para>Uses Noon Daylight (D65) illuminant for reference.</para>
		/// </summary>
		/// <param name="R">Red (from 0 to 1).</param>
		/// <param name="G">Green (from 0 to 1).</param>
		/// <param name="B">Blue (from 0 to 1).</param>
		/// <param name="L">Lightness.</param>
		/// <param name="a">Green-red color component.</param>
		/// <param name="b">Blue-yellow color component.</param>
		public static void RGBToCIELAB(double R, double G, double B, out double L, out double a, out double b)
		{
			RGBToXYZ(R, G, B, out double X, out double Y, out double Z);
			XYZToCIELAB(X, Y, Z, out L, out a, out b);
		}

		/// <summary>
		/// Converts from CIE-L*ab to RGB.
		/// <para>Assumes that Noon Daylight (D65) illuminant was used for reference.</para>
		/// </summary>
		/// <param name="L">Lightness.</param>
		/// <param name="a">Green-red color component.</param>
		/// <param name="b">Blue-yellow color component.</param>
		/// <param name="R">Red (from 0 to 1).</param>
		/// <param name="G">Green (from 0 to 1).</param>
		/// <param name="B">Blue (from 0 to 1).</param>
		public static void CIELABToRGB(double L, double a, double b, out double R, out double G, out double B)
		{
			CIELABToXYZ(L, a, b, out double X, out double Y, out double Z);
			XYZToRGB(X, Y, Z, out R, out G, out B);
		}

		/// <summary>
		/// Converts from RGB to XYZ color space.
		/// </summary>
		/// <param name="color">The color to convert.</param>
		/// <param name="X">X.</param>
		/// <param name="Y">Y.</param>
		/// <param name="Z">Z.</param>
		public static void RGBToXYZ(Color color, out double X, out double Y, out double Z)
		{
			if(color == null) {
				throw new ArgumentNullException(nameof(color));
			}
			RGBToXYZ(color.R, color.G, color.B, out X, out Y, out Z);
		}

		/// <summary>
		/// Converts from XYZ to RGB color space.
		/// <para>X, Y and Z input should refer to a D65/2° standard illuminant.</para>
		/// </summary>
		/// <param name="X">X.</param>
		/// <param name="Y">Y.</param>
		/// <param name="Z">Z.</param>
		public static Color XYZToRGB(double X, double Y, double Z)
		{
			XYZToRGB(X, Y, Z, out byte R, out byte G, out byte B);
			return Color.FromArgb(R, G, B);
		}

		/// <summary>
		/// Converts from RGB to XYZ color space.
		/// </summary>
		/// <param name="R">Red.</param>
		/// <param name="G">Green.</param>
		/// <param name="B">Blue.</param>
		/// <param name="X">X.</param>
		/// <param name="Y">Y.</param>
		/// <param name="Z">Z.</param>
		public static void RGBToXYZ(byte R, byte G, byte B, out double X, out double Y, out double Z)
		{
			double Rd = R / 255d;
			double Gd = G / 255d;
			double Bd = B / 255d;

			RGBToXYZ(Rd, Gd, Bd, out X, out Y, out Z);
		}

		/// <summary>
		/// Converts from XYZ to RGB color space.
		/// <para>X, Y and Z input should refer to a D65/2° standard illuminant.</para>
		/// </summary>
		/// <param name="X">X.</param>
		/// <param name="Y">Y.</param>
		/// <param name="Z">Z.</param>
		/// <param name="R">Red.</param>
		/// <param name="G">Green.</param>
		/// <param name="B">Blue.</param>
		public static void XYZToRGB(double X, double Y, double Z, out byte R, out byte G, out byte B)
		{
			RGBToXYZ(X, Y, Z, out double Rd, out double Gd, out double Bd);

			R = (byte)(Rd * 255);
			G = (byte)(Gd * 255);
			B = (byte)(Bd * 255);
		}

		private const double D65_X = 95.047;
		private const double D65_Y = 100;
		private const double D65_Z = 108.883;

		private const double SIXTEEN_DIV_HUNDREDSIXTEEN = 16d / 116d;
		private const double ONE_THIRD = 1d / 3d;
		private const double TWO_THIRDS = 2d / 3d;

		/// <summary>
		/// Converts from RGB to XYZ color space.
		/// <para>X, Y and Z output refer to a D65/2° standard illuminant.</para>
		/// </summary>
		/// <param name="R">Red (from 0 to 1).</param>
		/// <param name="G">Green (from 0 to 1).</param>
		/// <param name="B">Blue (from 0 to 1).</param>
		/// <param name="X">X.</param>
		/// <param name="Y">Y.</param>
		/// <param name="Z">Z.</param>
		public static void RGBToXYZ(double R, double G, double B, out double X, out double Y, out double Z)
		{
			// Standard-RGB -> XYZ
			// http://www.easyrgb.com/en/math.php

			if(R > 0.04045) {
				R = Math.Pow((R + 0.055) / 1.055, 2.4);
			} else {
				R /= 12.92;
			}
			if(G > 0.04045) {
				G = Math.Pow((G + 0.055) / 1.055, 2.4);
			} else {
				G /= 12.92;
			}
			if(B > 0.04045) {
				B = Math.Pow((B + 0.055) / 1.055, 2.4);
			} else {
				B /= 12.92;
			}

			R *= 100;
			G *= 100;
			B *= 100;

			X = R * 0.4124 + G * 0.3576 + B * 0.1805;
			Y = R * 0.2126 + G * 0.7152 + B * 0.0722;
			Z = R * 0.0193 + G * 0.1192 + B * 0.9505;
		}

		/// <summary>
		/// Converts from XYZ to RGB color space.
		/// <para>X, Y and Z input should refer to a D65/2° standard illuminant.</para>
		/// </summary>
		/// <param name="X">X.</param>
		/// <param name="Y">Y.</param>
		/// <param name="Z">Z.</param>
		/// <param name="R">Red (from 0 to 1).</param>
		/// <param name="G">Green (from 0 to 1).</param>
		/// <param name="B">Blue (from 0 to 1).</param>
		public static void XYZToRGB(double X, double Y, double Z, out double R, out double G, out double B)
		{
			// XYZ -> Standard-RGB
			// http://www.easyrgb.com/en/math.php

			X /= 100;
			Y /= 100;
			Z /= 100;

			R = X * 3.2406 + Y * -1.5372 + Z * -0.4986;
			G = X * -0.9689 + Y * 1.8758 + Z * 0.0415;
			B = X * 0.0557 + Y * -0.2040 + Z * 1.0570;

			if(R > 0.0031308) {
				R = 1.055 * (Math.Pow(R, 1 / 2.4)) - 0.055;
			} else {
				R *= 12.92;
			}
			if(G > 0.0031308) {
				G = 1.055 * (Math.Pow(G, 1 / 2.4)) - 0.055;
			} else {
				G *= 12.92;
			}
			if(B > 0.0031308) {
				B = 1.055 * (Math.Pow(B, 1 / 2.4)) - 0.055;
			} else {
				B *= 12.92;
			}
		}

		/// <summary>
		/// Returns a color at the specified offset in the specified 2-color linear gradient.
		/// <para>Scales in the HSV space.</para>
		/// </summary>
		/// <param name="start">The color at the start of the gradient (offset 0).</param>
		/// <param name="end">The color at the end of the gradient (offset 1).</param>
		/// <param name="offset">The offset. Must be [0.0, 1.0].</param>
		public static Color ScaleLinear(Color start, Color end, double offset)
		{
			if(offset < 0 || offset > 1) {
				throw new ArgumentOutOfRangeException(nameof(offset), $"Must be [0.0, 1.0]. '{offset}' was given.");
			}
			RGBToHSV(start, out double startH, out double startS, out double startV);
			RGBToHSV(end, out double endH, out double endS, out double endV);
			double invertedOffset = 1 - offset;
			double A = (start.A * invertedOffset + end.A * offset);
			double H = startH * invertedOffset + endH * offset;
			double S = startS * invertedOffset + endS * offset;
			double V = startV * invertedOffset + endV * offset;
			HSVToRGB(H, S, V, out byte R, out byte G, out byte B);
			return Color.FromArgb((int)A, R, G, B);
		}

		/// <summary>
		/// Converts from XYZ to CIE-L*ab.
		/// <para>Uses Noon Daylight (D65) illuminant for reference.</para>
		/// </summary>
		/// <param name="X">X.</param>
		/// <param name="Y">Y.</param>
		/// <param name="Z">Z.</param>
		/// <param name="L">Lightness.</param>
		/// <param name="a">Green-red color component.</param>
		/// <param name="b">Blue-yellow color component.</param>
		public static void XYZToCIELAB(double X, double Y, double Z, out double L, out double a, out double b)
		{
			// Standard XYZ -> CIE-L*ab
			// http://www.easyrgb.com/en/math.php

			X /= D65_X;
			Y /= D65_Y;
			Z /= D65_Z;

			if(X > 0.008856) {
				X = Math.Pow(X, ONE_THIRD);
			} else {
				X = (7.787 * X) + SIXTEEN_DIV_HUNDREDSIXTEEN;
			}
			if(Y > 0.008856) {
				Y = Math.Pow(Y, ONE_THIRD);
			} else {
				Y = (7.787 * Y) + SIXTEEN_DIV_HUNDREDSIXTEEN;
			}
			if(Z > 0.008856) {
				Z = Math.Pow(Z, ONE_THIRD);
			} else {
				Z = (7.787 * Z) + SIXTEEN_DIV_HUNDREDSIXTEEN;
			}

			L = (116 * Y) - 16;
			a = 500 * (X - Y);
			b = 200 * (Y - Z);
		}

		/// <summary>
		/// Converts from CIE-L*ab to XYZ.
		/// <para>Assumes that Noon Daylight (D65) illuminant was used for reference.</para>
		/// </summary>
		/// <param name="L">Lightness.</param>
		/// <param name="a">Green-red color component.</param>
		/// <param name="b">Blue-yellow color component.</param>
		/// <param name="X">X.</param>
		/// <param name="Y">Y.</param>
		/// <param name="Z">Z.</param>
		public static void CIELABToXYZ(double L, double a, double b, out double X, out double Y, out double Z)
		{
			// CIE-L*ab -> XYZ
			// http://www.easyrgb.com/en/math.php

			Y = (L + 16d) / 116d;
			X = a / 500d + Y;
			Z = Y - b / 200d;

			double tmp = Math.Pow(Y, 3);
			if(tmp > 0.008856) {
				Y = tmp;
			} else {
				Y = (Y - SIXTEEN_DIV_HUNDREDSIXTEEN) / 7.787;
			}
			tmp = Math.Pow(X, 3);
			if(tmp > 0.008856) {
				X = tmp;
			} else {
				X = (X - SIXTEEN_DIV_HUNDREDSIXTEEN) / 7.787;
			}
			tmp = Math.Pow(Z, 3);
			if(tmp > 0.008856) {
				Z = tmp;
			} else {
				Z = (Z - SIXTEEN_DIV_HUNDREDSIXTEEN) / 7.787;
			}

			X *= D65_X;
			Y *= D65_Y;
			Z *= D65_Z;
		}
	}
}
