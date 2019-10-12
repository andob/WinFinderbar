using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace WinFinderbar
{
    public class DarkThemeBitmapTransformer
    {
        public Bitmap Bitmap;
        public readonly Color MostUsedColor;

        public DarkThemeBitmapTransformer(Bitmap bitmap)
        {
            this.Bitmap=bitmap;
            this.MostUsedColor=bitmap.DetectMostUsedColor();
        }

        public void TransformIfNeeded()
        {
            if (this.MostUsedColor.IsLight())
                this.Bitmap.InvertColors();
        }
    }

    static class DarkThemeBitmapTransformerExtensions
    {
        public static Color DetectMostUsedColor(this Bitmap bitmap)
        {
            Dictionary<Color, int> usagesDictionary=new Dictionary<Color, int>();
            
            for (int x=0; x<bitmap.Width; x++)
            {
                for (int y=0; y<bitmap.Height; y++)
                {
                    Color pixelColor=bitmap.GetPixel(x, y);
                    if (usagesDictionary.ContainsKey(pixelColor))
                        usagesDictionary[pixelColor]++;
                    else usagesDictionary[pixelColor]=0;
                }
            }

            int mostUsedColorNumberOfUsages=usagesDictionary.Max(keyValuePair => keyValuePair.Value);
            return usagesDictionary.First(keyValuePair => keyValuePair.Value==mostUsedColorNumberOfUsages).Key;
        }

        public static bool IsLight(this Color color)
        {
            return color.R*0.2126+color.G*0.7152+color.B*0.0722>255.0/2;
        }

        public static void InvertColors(this Bitmap bitmap)
        {
            for (int x=0; x<bitmap.Width; x++)
            {
                for (int y=0; y<bitmap.Height; y++)
                {
                    Color oldPixelColor=bitmap.GetPixel(x, y);
                    Color newPixelColor=Color.FromArgb(alpha: 255, 
                        red: 255-oldPixelColor.R,
                        green: 255-oldPixelColor.G,
                        blue: 255-oldPixelColor.B);
                    
                    bitmap.SetPixel(x, y, newPixelColor);
                }
            }
        }
    }
}