using System;
using Android.Graphics;
using WoWonder.Library.Anjo.Share.Abstractions;

namespace WoWonder.Library.Anjo.Share
{
    /// <summary>
    /// Extension class used for color conversion
    /// </summary>
    static class ShareColorExtensions
    {
        /// <summary>
        /// Convert <see cref="ShareColor"/> object to native color
        /// </summary>
        /// <param name="color">The color to convert</param>
        /// <returns>The converted color</returns>
        public static Color ToNativeColor(this ShareColor color)
        {
            return color switch
            {
                null => throw new ArgumentNullException(nameof(color)),
                _ => new Color(color.R, color.G, color.B, color.A)
            };
        }
    }
}