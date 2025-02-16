using System;
using System.Drawing;

namespace ImageLyre.ImageEngine.ColorTypeConvert;

/// <summary>
///     Provides methods to convert from a color space to an other.
/// </summary>
public static class ColorSpaceHelper
{
    #region Color processing

    /// <summary>
    ///     Gets the "distance" between two colors.
    ///     RGB colors must be normalized (eg. values in [0.0, 1.0]).
    /// </summary>
    /// <param name="r1">First color red component.</param>
    /// <param name="g1">First color green component.</param>
    /// <param name="b1">First color blue component.</param>
    /// <param name="r2">Second color red component.</param>
    /// <param name="g2">Second color green component.</param>
    /// <param name="b2">Second color blue component.</param>
    public static double GetColorDistance(double r1, double g1, double b1, double r2, double g2, double b2)
    {
        var a = r2 - r1;
        var b = g2 - g1;
        var c = b2 - b1;

        return Math.Sqrt(a * a + b * b + c * c);
    }


    /// <summary>
    ///     Gets the "distance" between two colors.
    ///     RGB colors must be normalized (eg. values in [0.0, 1.0]).
    /// </summary>
    /// <param name="color1">First color [r,g,b]</param>
    /// <param name="color2">Second color [r,g,b]</param>
    public static double GetColorDistance(double[] color1, double[] color2)
    {
        return GetColorDistance(color1[0], color1[1], color1[2], color2[0], color2[1], color2[2]);
    }


    /// <summary>
    ///     Gets the "distance" between two colors.
    /// </summary>
    /// <param name="color1">First color.</param>
    /// <param name="color2">Second color.</param>
    public static double GetColorDistance(Color c1, Color c2)
    {
        double[] rgb1 =
        {
            c1.R / 255.0,
            c1.G / 255.0,
            c1.B / 255.0
        };

        double[] rgb2 =
        {
            c2.R / 255.0,
            c2.G / 255.0,
            c2.B / 255.0
        };

        return GetColorDistance(rgb1[0], rgb1[1], rgb1[2], rgb2[0], rgb2[1], rgb2[2]);
    }

    #endregion

    #region Light Spectrum processing

    /// <summary>
    ///     Gets visible colors (color wheel).
    /// </summary>
    /// <param name="alpha">
    ///     The alpha value used for each colors.
    /// </param>
    public static Color[] GetWheelColors(int alpha)
    {
        Color temp;
        var colorCount = 6 * 256;
        var colors = new Color[colorCount];

        for (var i = 0; i < colorCount; i++)
        {
            temp = HSBtoColor((int) (i * 255.0 / colorCount), 255, 255);
            colors[i] = Color.FromArgb(alpha, temp.R, temp.G, temp.B);
        }

        return colors;
    }


    /// <summary>
    ///     Gets visible spectrum colors.
    /// </summary>
    /// <param name="alpha">The alpha value used for each colors.</param>
    public static Color[] GetSpectrumColors(int alpha = 255)
    {
        var colors = new Color[256 * 6];
        //for(int i=127; i<256; i++) colors[i-127] = Color.FromArgb(alpha, i, 0, 0);
        for (var i = 0; i < 256; i++) 
            colors[i] = Color.FromArgb(alpha, 255, i, 0);
        for (var i = 0; i < 256; i++) 
            colors[i + 256] = Color.FromArgb(alpha, 255 - i, 255, 0);
        for (var i = 0; i < 256; i++) 
            colors[i + 256 * 2] = Color.FromArgb(alpha, 0, 255, i);
        for (var i = 0; i < 256; i++) 
            colors[i + 256 * 3] = Color.FromArgb(alpha, 0, 255 - i, 255);
        for (var i = 0; i < 256; i++) 
            colors[i + 256 * 4] = Color.FromArgb(alpha, i, 0, 255);
        for (var i = 0; i < 256; i++) 
            colors[i + 256 * 5] = Color.FromArgb(alpha, 255, 0, 255 - i);
        //for(int i=0; i<128; i++) colors[i+(128+256*6)] = Color.FromArgb(alpha, 255-i, 0, 0);

        return colors;
    }

    #endregion

    #region Hexa convert

    /// <summary>
    ///     Gets the int equivalent for a hexadecimal value.
    /// </summary>
    private static int GetIntFromHex(string strHex)
    {
        switch (strHex)
        {
            case "A":
            {
                return 10;
            }
            case "B":
            {
                return 11;
            }
            case "C":
            {
                return 12;
            }
            case "D":
            {
                return 13;
            }
            case "E":
            {
                return 14;
            }
            case "F":
            {
                return 15;
            }
            default:
            {
                return int.Parse(strHex);
            }
        }
    }


    /// <summary>
    ///     Converts a Hex color to a .net Color.
    /// </summary>
    /// <param name="hexColor">The desired hexadecimal color to convert.</param>
    public static Color HexToColor(string hexColor)
    {
        string r, g, b;

        if (hexColor != string.Empty)
        {
            hexColor = hexColor.Trim();
            if (hexColor[0] == '#') hexColor = hexColor.Substring(1, hexColor.Length - 1);

            r = hexColor.Substring(0, 2);
            g = hexColor.Substring(2, 2);
            b = hexColor.Substring(4, 2);

            r = Convert.ToString(16 * GetIntFromHex(r.Substring(0, 1)) + GetIntFromHex(r.Substring(1, 1)));
            g = Convert.ToString(16 * GetIntFromHex(g.Substring(0, 1)) + GetIntFromHex(g.Substring(1, 1)));
            b = Convert.ToString(16 * GetIntFromHex(b.Substring(0, 1)) + GetIntFromHex(b.Substring(1, 1)));

            return Color.FromArgb(Convert.ToInt32(r), Convert.ToInt32(g), Convert.ToInt32(b));
        }

        return Color.Empty;
    }

    /// <summary>
    ///     Converts a RGB color format to an hexadecimal color.
    /// </summary>
    /// <param name="r">The Red value.</param>
    /// <param name="g">The Green value.</param>
    /// <param name="b">The Blue value.</param>
    public static string RGBToHex(int r, int g, int b)
    {
        return $"#{r:x2}{g:x2}{b:x2}";
    }


    /// <summary>
    ///     Converts a RGB color format to an hexadecimal color.
    /// </summary>
    /// <param name="r">The color to convert.</param>
    public static string RGBToHex(Color c)
    {
        return RGBToHex(c.R, c.G, c.B);
    }

    #endregion

    #region HSB convert

    /// <summary>
    ///     Converts HSB to RGB.
    /// </summary>
    /// <param name="hsv">The HSB structure to convert.</param>
    public static RGB HSBtoRGB(HSB hsb)
    {
        double r = 0;
        double g = 0;
        double b = 0;

        if (hsb.Saturation == 0)
        {
            r = g = b = hsb.Brightness;
        }
        else
        {
            // the color wheel consists of 6 sectors. Figure out which sector you're in.
            var sectorPos = hsb.Hue / 60.0;
            var sectorNumber = (int) Math.Floor(sectorPos);
            // get the fractional part of the sector
            var fractionalSector = sectorPos - sectorNumber;

            // calculate values for the three axes of the color. 
            var p = hsb.Brightness * (1.0 - hsb.Saturation);
            var q = hsb.Brightness * (1.0 - hsb.Saturation * fractionalSector);
            var t = hsb.Brightness * (1.0 - hsb.Saturation * (1 - fractionalSector));

            // assign the fractional colors to r, g, and b based on the sector the angle is in.
            switch (sectorNumber)
            {
                case 0:
                    r = hsb.Brightness;
                    g = t;
                    b = p;
                    break;
                case 1:
                    r = q;
                    g = hsb.Brightness;
                    b = p;
                    break;
                case 2:
                    r = p;
                    g = hsb.Brightness;
                    b = t;
                    break;
                case 3:
                    r = p;
                    g = q;
                    b = hsb.Brightness;
                    break;
                case 4:
                    r = t;
                    g = p;
                    b = hsb.Brightness;
                    break;
                case 5:
                    r = hsb.Brightness;
                    g = p;
                    b = q;
                    break;
            }
        }

        return new RGB(
            Convert.ToInt32(double.Parse(string.Format("{0:0.00}", r * 255.0))),
            Convert.ToInt32(double.Parse(string.Format("{0:0.00}", g * 255.0))),
            Convert.ToInt32(double.Parse(string.Format("{0:0.00}", b * 255.0)))
        );
    }

    /// <summary>
    ///     Converts HSB to RGB.
    /// </summary>
    /// <param name="H">Hue value.</param>
    /// <param name="S">Saturation value.</param>
    /// <param name="V">Brigthness value.</param>
    public static RGB HSBtoRGB(double h, double s, double b)
    {
        return HSBtoRGB(new HSB(h, s, b));
    }


    /// <summary>
    ///     Converts HSB to Color.
    /// </summary>
    /// <param name="hsv">the HSB structure to convert.</param>
    public static Color HSBtoColor(HSB hsb)
    {
        var rgb = HSBtoRGB(hsb);

        return Color.FromArgb(rgb.Red, rgb.Green, rgb.Blue);
    }

    /// <summary>
    ///     Converts HSB to a .net Color.
    /// </summary>
    /// <param name="h">Hue value (must be between 0 and 360).</param>
    /// <param name="s">Saturation value (must be between 0 and 1).</param>
    /// <param name="b">Brightness value (must be between 0 and 1).</param>
    public static Color HSBtoColor(double h, double s, double b)
    {
        return HSBtoColor(new HSB(h, s, b));
    }

    /// <summary>
    ///     Converts HSB to Color.
    /// </summary>
    /// <param name="h">Hue value.</param>
    /// <param name="s">Saturation value.</param>
    /// <param name="b">Brightness value.</param>
    public static Color HSBtoColor(int h, int s, int b)
    {
        double hue = 0, sat = 0, val = 0;

        // Scale Hue to be between 0 and 360. Saturation and value scale to be between 0 and 1.
        if (h > 360 || s > 1 || b > 1)
        {
            hue = h / 255.0 * 360.0 % 360.0;
            sat = s / 255.0;
            val = b / 255.0;
        }

        return HSBtoColor(new HSB(hue, sat, val));
    }


    /// <summary>
    ///     Converts HSB to HSL.
    /// </summary>
    public static HSL HSBtoHSL(double h, double s, double b)
    {
        var rgb = HSBtoRGB(new HSB(h, s, b));

        return RGBtoHSL(rgb.Red, rgb.Green, rgb.Blue);
    }

    /// <summary>
    ///     Converts HSB to CMYK.
    /// </summary>
    public static CMYK HSBtoCMYK(double h, double s, double b)
    {
        var rgb = HSBtoRGB(new HSB(h, s, b));

        return RGBtoCMYK(rgb.Red, rgb.Green, rgb.Blue);
    }

    /// <summary>
    ///     Converts HSB to CMYK.
    /// </summary>
    public static YUV HSBtoYUV(double h, double s, double b)
    {
        var rgb = HSBtoRGB(new HSB(h, s, b));

        return RGBtoYUV(rgb.Red, rgb.Green, rgb.Blue);
    }

    #endregion

    #region HSL convert

    /// <summary>
    ///     Converts HSL to RGB.
    /// </summary>
    /// <param name="h">Hue, must be in [0, 360].</param>
    /// <param name="s">Saturation, must be in [0, 1].</param>
    /// <param name="l">Luminance, must be in [0, 1].</param>
    public static RGB HSLtoRGB(double h, double s, double l)
    {
        if (s == 0)
            // achromatic color (gray scale)
            return new RGB(
                Convert.ToInt32(double.Parse($"{l * 255.0:0.00}")),
                Convert.ToInt32(double.Parse($"{l * 255.0:0.00}")),
                Convert.ToInt32(double.Parse($"{l * 255.0:0.00}"))
            );

        var q = l < 0.5 ? l * (1.0 + s) : l + s - l * s;
        var p = 2.0 * l - q;

        var Hk = h / 360.0;
        var T = new double[3];
        T[0] = Hk + 1.0 / 3.0; // Tr
        T[1] = Hk; // Tb
        T[2] = Hk - 1.0 / 3.0; // Tg

        for (var i = 0; i < 3; i++)
        {
            if (T[i] < 0) T[i] += 1.0;
            if (T[i] > 1) T[i] -= 1.0;

            if (T[i] * 6 < 1)
                T[i] = p + (q - p) * 6.0 * T[i];
            else if (T[i] * 2.0 < 1) //(1.0/6.0)<=T[i] && T[i]<0.5
                T[i] = q;
            else if (T[i] * 3.0 < 2) // 0.5<=T[i] && T[i]<(2.0/3.0)
                T[i] = p + (q - p) * (2.0 / 3.0 - T[i]) * 6.0;
            else T[i] = p;
        }

        return new RGB(
            Convert.ToInt32(double.Parse($"{T[0] * 255.0:0.00}")),
            Convert.ToInt32(double.Parse($"{T[1] * 255.0:0.00}")),
            Convert.ToInt32(double.Parse($"{T[2] * 255.0:0.00}"))
        );
    }

    /// <summary>
    ///     Converts HSL to RGB.
    /// </summary>
    /// <param name="hsl">The HSL structure to convert.</param>
    public static RGB HSLtoRGB(HSL hsl)
    {
        return HSLtoRGB(hsl.Hue, hsl.Saturation, hsl.Luminance);
    }


    /// <summary>
    ///     Converts HSL to .net Color.
    /// </summary>
    /// <param name="hsl">The HSL structure to convert.</param>
    public static Color HSLtoColor(double h, double s, double l)
    {
        var rgb = HSLtoRGB(h, s, l);

        return Color.FromArgb(rgb.Red, rgb.Green, rgb.Blue);
    }

    /// <summary>
    ///     Converts HSL to .net Color.
    /// </summary>
    /// <param name="hsl">The HSL structure to convert.</param>
    public static Color HSLtoColor(HSL hsl)
    {
        return HSLtoColor(hsl.Hue, hsl.Saturation, hsl.Luminance);
    }


    /// <summary>
    ///     Converts HSL to HSB.
    /// </summary>
    public static HSB HSLtoHSB(double h, double s, double l)
    {
        var rgb = HSLtoRGB(h, s, l);

        return RGBtoHSB(rgb.Red, rgb.Green, rgb.Blue);
    }

    /// <summary>
    ///     Converts HSL to CMYK.
    /// </summary>
    public static CMYK HSLtoCMYK(double h, double s, double l)
    {
        var rgb = HSLtoRGB(h, s, l);

        return RGBtoCMYK(rgb.Red, rgb.Green, rgb.Blue);
    }

    /// <summary>
    ///     Converts HSL to YUV.
    /// </summary>
    public static YUV HSLtoYUV(double h, double s, double l)
    {
        var rgb = HSLtoRGB(h, s, l);

        return RGBtoYUV(rgb.Red, rgb.Green, rgb.Blue);
    }

    #endregion

    #region RGB convert

    /// <summary>
    ///     Converts RGB to HSL.
    /// </summary>
    /// <param name="red">Red value, must be in [0,255].</param>
    /// <param name="green">Green value, must be in [0,255].</param>
    /// <param name="blue">Blue value, must be in [0,255].</param>
    public static HSL RGBtoHSL(int red, int green, int blue)
    {
        double h = 0, s = 0, l = 0;

        // normalizes red-green-blue values
        var nRed = red / 255.0;
        var nGreen = green / 255.0;
        var nBlue = blue / 255.0;

        var max = Math.Max(nRed, Math.Max(nGreen, nBlue));
        var min = Math.Min(nRed, Math.Min(nGreen, nBlue));

        // hue
        if (max == min)
            h = 0; // undefined
        else if (max == nRed && nGreen >= nBlue)
            h = 60.0 * (nGreen - nBlue) / (max - min);
        else if (max == nRed && nGreen < nBlue)
            h = 60.0 * (nGreen - nBlue) / (max - min) + 360.0;
        else if (max == nGreen)
            h = 60.0 * (nBlue - nRed) / (max - min) + 120.0;
        else if (max == nBlue) h = 60.0 * (nRed - nGreen) / (max - min) + 240.0;

        // luminance
        l = (max + min) / 2.0;

        // saturation
        if (l == 0 || max == min)
            s = 0;
        else if (0 < l && l <= 0.5)
            s = (max - min) / (max + min);
        else if (l > 0.5) s = (max - min) / (2 - (max + min)); //(max-min > 0)?

        return new HSL(
            double.Parse($"{h:0.##}"),
            double.Parse($"{s:0.##}"),
            double.Parse($"{l:0.##}")
        );
    }

    /// <summary>
    ///     Converts RGB to HSL.
    /// </summary>
    public static HSL RGBtoHSL(RGB rgb)
    {
        return RGBtoHSL(rgb.Red, rgb.Green, rgb.Blue);
    }

    /// <summary>
    ///     Converts Color to HSL.
    /// </summary>
    public static HSL RGBtoHSL(Color c)
    {
        return RGBtoHSL(c.R, c.G, c.B);
    }


    /// <summary>
    ///     Converts RGB to HSB.
    /// </summary>
    public static HSB RGBtoHSB(int red, int green, int blue)
    {
        var r = red / 255.0;
        var g = green / 255.0;
        var b = blue / 255.0;

        var max = Math.Max(r, Math.Max(g, b));
        var min = Math.Min(r, Math.Min(g, b));

        var h = 0.0;
        if (max == r && g >= b)
        {
            if (max - min == 0) h = 0.0;
            else h = 60 * (g - b) / (max - min);
        }
        else if (max == r && g < b)
        {
            h = 60 * (g - b) / (max - min) + 360;
        }
        else if (max == g)
        {
            h = 60 * (b - r) / (max - min) + 120;
        }
        else if (max == b)
        {
            h = 60 * (r - g) / (max - min) + 240;
        }

        var s = max == 0 ? 0.0 : 1.0 - min / max;

        return new HSB(h, s, max);
    }

    /// <summary>
    ///     Converts RGB to HSB.
    /// </summary>
    public static HSB RGBtoHSB(RGB rgb)
    {
        return RGBtoHSB(rgb.Red, rgb.Green, rgb.Blue);
    }

    /// <summary>
    ///     Converts RGB to HSB.
    /// </summary>
    public static HSB RGBtoHSB(Color c)
    {
        return RGBtoHSB(c.R, c.G, c.B);
    }


    /// <summary>
    ///     Converts RGB to CMYK
    /// </summary>
    /// <param name="red">Red vaue must be in [0, 255].</param>
    /// <param name="green">Green vaue must be in [0, 255].</param>
    /// <param name="blue">Blue vaue must be in [0, 255].</param>
    public static CMYK RGBtoCMYK(int red, int green, int blue)
    {
        var c = (double) (255 - red) / 255;
        var m = (double) (255 - green) / 255;
        var y = (double) (255 - blue) / 255;

        var min = Math.Min(c, Math.Min(m, y));
        if (min == 1.0)
            return new CMYK(0, 0, 0, 1);
        return new CMYK((c - min) / (1 - min), (m - min) / (1 - min), (y - min) / (1 - min), min);
    }

    /// <summary>
    ///     Converts RGB to CMYK
    /// </summary>
    public static CMYK RGBtoCMYK(Color c)
    {
        return RGBtoCMYK(c.R, c.G, c.B);
    }

    /// <summary>
    ///     Converts RGB to CMYK
    /// </summary>
    public static CMYK RGBtoCMYK(RGB rgb)
    {
        return RGBtoCMYK(rgb.Red, rgb.Green, rgb.Blue);
    }


    /// <summary>
    ///     Converts RGB to YUV.
    /// </summary>
    /// <param name="red">red must be in [0, 255].</param>
    /// <param name="green">green must be in [0, 255].</param>
    /// <param name="blue">blue must be in [0, 255].</param>
    public static YUV RGBtoYUV(int red, int green, int blue)
    {
        var yuv = new YUV();

        // normalizes red/green/blue values
        var nRed = red / 255.0;
        var nGreen = green / 255.0;
        var nBlue = blue / 255.0;

        // converts
        yuv.Y = 0.299 * nRed + 0.587 * nGreen + 0.114 * nBlue;
        yuv.U = -0.1471376975169300226 * nRed - 0.2888623024830699774 * nGreen + 0.436 * nBlue;
        yuv.V = 0.615 * nRed - 0.5149857346647646220 * nGreen - 0.1000142653352353780 * nBlue;

        return yuv;
    }

    /// <summary>
    ///     Converts RGB to YUV.
    /// </summary>
    public static YUV RGBtoYUV(Color c)
    {
        return RGBtoYUV(c.R, c.G, c.B);
    }

    /// <summary>
    ///     Converts RGB to YUV.
    /// </summary>
    public static YUV RGBtoYUV(RGB rgb)
    {
        return RGBtoYUV(rgb.Red, rgb.Green, rgb.Blue);
    }


    /// <summary>
    ///     Converts RGB to CIE XYZ (CIE 1931 color space)
    /// </summary>
    /// <param name="red">Red must be in [0, 255].</param>
    /// <param name="green">Green must be in [0, 255].</param>
    /// <param name="blue">Blue must be in [0, 255].</param>
    public static CIEXYZ RGBtoXYZ(int red, int green, int blue)
    {
        // normalize red, green, blue values
        var rLinear = red / 255.0;
        var gLinear = green / 255.0;
        var bLinear = blue / 255.0;

        // convert to a sRGB form
        var r = rLinear > 0.04045 ? Math.Pow((rLinear + 0.055) / (1 + 0.055), 2.2) : rLinear / 12.92;
        var g = gLinear > 0.04045 ? Math.Pow((gLinear + 0.055) / (1 + 0.055), 2.2) : gLinear / 12.92;
        var b = bLinear > 0.04045 ? Math.Pow((bLinear + 0.055) / (1 + 0.055), 2.2) : bLinear / 12.92;

        // converts
        return new CIEXYZ(
            r * 0.4124 + g * 0.3576 + b * 0.1805,
            r * 0.2126 + g * 0.7152 + b * 0.0722,
            r * 0.0193 + g * 0.1192 + b * 0.9505
        );
    }

    /// <summary>
    ///     Converts RGB to CIEXYZ.
    /// </summary>
    public static CIEXYZ RGBtoXYZ(RGB rgb)
    {
        return RGBtoXYZ(rgb.Red, rgb.Green, rgb.Blue);
    }

    /// <summary>
    ///     Converts RGB to CIEXYZ.
    /// </summary>
    public static CIEXYZ RGBtoXYZ(Color c)
    {
        return RGBtoXYZ(c.R, c.G, c.B);
    }


    /// <summary>
    ///     Converts RGB to CIELab.
    /// </summary>
    public static CIELab RGBtoLab(int red, int green, int blue)
    {
        return XYZtoLab(RGBtoXYZ(red, green, blue));
    }

    /// <summary>
    ///     Converts RGB to CIELab.
    /// </summary>
    public static CIELab RGBtoLab(RGB rgb)
    {
        return XYZtoLab(RGBtoXYZ(rgb.Red, rgb.Green, rgb.Blue));
    }

    /// <summary>
    ///     Converts RGB to CIELab.
    /// </summary>
    public static CIELab RGBtoLab(Color color)
    {
        return XYZtoLab(RGBtoXYZ(color.R, color.G, color.B));
    }

    #endregion

    #region CMYK convert

    /// <summary>
    ///     Converts CMYK to RGB.
    /// </summary>
    /// <param name="c">Cyan value (must be between 0 and 1).</param>
    /// <param name="m">Magenta value (must be between 0 and 1).</param>
    /// <param name="y">Yellow value (must be between 0 and 1).</param>
    /// <param name="k">Black value (must be between 0 and 1).</param>
    public static Color CMYKtoColor(float c, float m, float y, float k)
    {
        return CMYKtoColor(c, m, y, (double) k);
    }

    /// <summary>
    ///     Converts CMYK to RGB.
    /// </summary>
    /// <param name="c">Cyan value (must be between 0 and 1).</param>
    /// <param name="m">Magenta value (must be between 0 and 1).</param>
    /// <param name="y">Yellow value (must be between 0 and 1).</param>
    /// <param name="k">Black value (must be between 0 and 1).</param>
    public static Color CMYKtoColor(double c, double m, double y, double k)
    {
        return CMYKtoColor(new CMYK(c, m, y, k));
    }

    /// <summary>
    ///     Converts CMYK to RGB.
    /// </summary>
    /// <param name="cmyk"></param>
    public static Color CMYKtoColor(CMYK cmyk)
    {
        var red = Convert.ToInt32((1 - cmyk.Cyan) * (1 - cmyk.Black) * 255);
        var green = Convert.ToInt32((1 - cmyk.Magenta) * (1 - cmyk.Black) * 255);
        var blue = Convert.ToInt32((1 - cmyk.Yellow) * (1 - cmyk.Black) * 255);

        return Color.FromArgb(red, green, blue);
    }


    /// <summary>
    ///     Converts CMYK to RGB.
    /// </summary>
    /// <param name="c">Cyan value (must be between 0 and 1).</param>
    /// <param name="m">Magenta value (must be between 0 and 1).</param>
    /// <param name="y">Yellow value (must be between 0 and 1).</param>
    /// <param name="k">Black value (must be between 0 and 1).</param>
    public static RGB CMYKtoRGB(double c, double m, double y, double k)
    {
        var red = Convert.ToInt32((1.0 - c) * (1.0 - k) * 255.0);
        var green = Convert.ToInt32((1.0 - m) * (1.0 - k) * 255.0);
        var blue = Convert.ToInt32((1.0 - y) * (1.0 - k) * 255.0);

        return new RGB(red, green, blue);
    }

    /// <summary>
    ///     Converts CMYK to RGB.
    /// </summary>
    /// <param name="cmyk"></param>
    public static RGB CMYKtoRGB(CMYK cmyk)
    {
        return CMYKtoRGB(cmyk.Cyan, cmyk.Magenta, cmyk.Yellow, cmyk.Black);
    }


    /// <summary>
    ///     Converts CMYK to HSL.
    /// </summary>
    public static HSL CMYKtoHSL(double c, double m, double y, double k)
    {
        var rgb = CMYKtoRGB(c, m, y, k);

        return RGBtoHSL(rgb.Red, rgb.Green, rgb.Blue);
    }

    /// <summary>
    ///     Converts CMYK to HSB.
    /// </summary>
    public static HSB CMYKtoHSB(double c, double m, double y, double k)
    {
        var rgb = CMYKtoRGB(c, m, y, k);

        return RGBtoHSB(rgb.Red, rgb.Green, rgb.Blue);
    }

    /// <summary>
    ///     Converts CMYK to YUV.
    /// </summary>
    public static YUV CMYKtoYUV(double c, double m, double y, double k)
    {
        var rgb = CMYKtoRGB(c, m, y, k);

        return RGBtoYUV(rgb.Red, rgb.Green, rgb.Blue);
    }

    #endregion

    #region YUV convert

    /// <summary>
    ///     Converts YUV to RGB.
    /// </summary>
    /// <param name="y">Y must be in [0, 1].</param>
    /// <param name="u">U must be in [-0.436, +0.436].</param>
    /// <param name="v">V must be in [-0.615, +0.615].</param>
    public static RGB YUVtoRGB(double y, double u, double v)
    {
        var rgb = new RGB();

        rgb.Red = Convert.ToInt32((y + 1.139837398373983740 * v) * 255);
        rgb.Green = Convert.ToInt32((y - 0.3946517043589703515 * u - 0.5805986066674976801 * v) * 255);
        rgb.Blue = Convert.ToInt32((y + 2.032110091743119266 * u) * 255);

        return rgb;
    }

    /// <summary>
    ///     Converts YUV to RGB.
    /// </summary>
    public static RGB YUVtoRGB(YUV yuv)
    {
        return YUVtoRGB(yuv.Y, yuv.U, yuv.V);
    }


    /// <summary>
    ///     Converts YUV to a .net Color.
    /// </summary>
    /// <param name="y">Y must be in [0, 1].</param>
    /// <param name="u">U must be in [-0.436, +0.436].</param>
    /// <param name="v">V must be in [-0.615, +0.615].</param>
    public static Color YUVtoColor(double y, double u, double v)
    {
        var rgb = YUVtoRGB(y, u, v);

        return Color.FromArgb(rgb.Red, rgb.Green, rgb.Blue);
    }

    /// <summary>
    ///     Converts YUV to a .net Color.
    /// </summary>
    public static Color YUVtoColor(YUV yuv)
    {
        var rgb = YUVtoRGB(yuv);

        return Color.FromArgb(rgb.Red, rgb.Green, rgb.Blue);
    }


    /// <summary>
    ///     Converts YUV to HSL.
    /// </summary>
    /// <param name="y">Y must be in [0, 1].</param>
    /// <param name="u">U must be in [-0.436, +0.436].</param>
    /// <param name="v">V must be in [-0.615, +0.615].</param>
    public static HSL YUVtoHSL(double y, double u, double v)
    {
        var rgb = YUVtoRGB(y, u, v);

        return RGBtoHSL(rgb.Red, rgb.Green, rgb.Blue);
    }

    /// <summary>
    ///     Converts YUV to HSB.
    /// </summary>
    /// <param name="y">Y must be in [0, 1].</param>
    /// <param name="u">U must be in [-0.436, +0.436].</param>
    /// <param name="v">V must be in [-0.615, +0.615].</param>
    public static HSB YUVtoHSB(double y, double u, double v)
    {
        var rgb = YUVtoRGB(y, u, v);

        return RGBtoHSB(rgb.Red, rgb.Green, rgb.Blue);
    }

    /// <summary>
    ///     Converts YUV to CMYK.
    /// </summary>
    /// <param name="y">Y must be in [0, 1].</param>
    /// <param name="u">U must be in [-0.436, +0.436].</param>
    /// <param name="v">V must be in [-0.615, +0.615].</param>
    public static CMYK YUVtoCMYK(double y, double u, double v)
    {
        var rgb = YUVtoRGB(y, u, v);

        return RGBtoCMYK(rgb.Red, rgb.Green, rgb.Blue);
    }

    #endregion

    #region CIE XYZ convert

    /// <summary>
    ///     Converts CIEXYZ to RGB structure.
    /// </summary>
    public static RGB XYZtoRGB(double x, double y, double z)
    {
        var Clinear = new double[3];
        Clinear[0] = x * 3.2410 - y * 1.5374 - z * 0.4986; // red
        Clinear[1] = -x * 0.9692 + y * 1.8760 - z * 0.0416; // green
        Clinear[2] = x * 0.0556 - y * 0.2040 + z * 1.0570; // blue

        for (var i = 0; i < 3; i++)
            Clinear[i] = Clinear[i] <= 0.0031308
                ? 12.92 * Clinear[i]
                : (1 + 0.055) * Math.Pow(Clinear[i], 1.0 / 2.4) - 0.055;

        return new RGB(
            Convert.ToInt32(double.Parse(string.Format("{0:0.00}", Clinear[0] * 255.0))),
            Convert.ToInt32(double.Parse(string.Format("{0:0.00}", Clinear[1] * 255.0))),
            Convert.ToInt32(double.Parse(string.Format("{0:0.00}", Clinear[2] * 255.0)))
        );
    }

    /// <summary>
    ///     Converts CIEXYZ to RGB structure.
    /// </summary>
    public static RGB XYZtoRGB(CIEXYZ xyz)
    {
        return XYZtoRGB(xyz.X, xyz.Y, xyz.Z);
    }


    /// <summary>
    ///     XYZ to L*a*b* transformation function.
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    private static double Fxyz(double t)
    {
        return t > 0.008856 ? Math.Pow(t, 1.0 / 3.0) : 7.787 * t + 16.0 / 116.0;
    }

    /// <summary>
    ///     Converts CIEXYZ to CIELab structure.
    /// </summary>
    public static CIELab XYZtoLab(double x, double y, double z)
    {
        var lab = CIELab.Empty;

        lab.L = 116.0 * Fxyz(y / CIEXYZ.D65.Y) - 16;
        lab.A = 500.0 * (Fxyz(x / CIEXYZ.D65.X) - Fxyz(y / CIEXYZ.D65.Y));
        lab.B = 200.0 * (Fxyz(y / CIEXYZ.D65.Y) - Fxyz(z / CIEXYZ.D65.Z));

        return lab;
    }

    /// <summary>
    ///     Converts CIEXYZ to CIELab structure.
    /// </summary>
    public static CIELab XYZtoLab(CIEXYZ xyz)
    {
        return XYZtoLab(xyz.X, xyz.Y, xyz.Z);
    }

    #endregion

    #region CIE L*a*b* convert

    /// <summary>
    ///     Converts CIELab to CIEXYZ.
    /// </summary>
    public static CIEXYZ LabtoXYZ(double l, double a, double b)
    {
        var theta = 6.0 / 29.0;

        var fy = (l + 16) / 116.0;
        var fx = fy + a / 500.0;
        var fz = fy - b / 200.0;

        return new CIEXYZ(
            fx > theta ? CIEXYZ.D65.X * (fx * fx * fx) : (fx - 16.0 / 116.0) * 3 * (theta * theta) * CIEXYZ.D65.X,
            fy > theta ? CIEXYZ.D65.Y * (fy * fy * fy) : (fy - 16.0 / 116.0) * 3 * (theta * theta) * CIEXYZ.D65.Y,
            fz > theta ? CIEXYZ.D65.Z * (fz * fz * fz) : (fz - 16.0 / 116.0) * 3 * (theta * theta) * CIEXYZ.D65.Z
        );
    }

    /// <summary>
    ///     Converts CIELab to CIEXYZ.
    /// </summary>
    public static CIEXYZ LabtoXYZ(CIELab lab)
    {
        return LabtoXYZ(lab.L, lab.A, lab.B);
    }


    /// <summary>
    ///     Converts CIELab to RGB.
    /// </summary>
    public static RGB LabtoRGB(double l, double a, double b)
    {
        return XYZtoRGB(LabtoXYZ(l, a, b));
    }

    /// <summary>
    ///     Converts CIELab to RGB.
    /// </summary>
    public static RGB LabtoRGB(CIELab lab)
    {
        return XYZtoRGB(LabtoXYZ(lab));
    }

    #endregion
}