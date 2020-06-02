
using System.Globalization;
using System.Windows.Media;

namespace TWETTY_CHAT
{
    public static class FormattedString
    {
        /// <summary>
        /// Measures the specified string when drawn with the specified Font.
        /// </summary>
        /// <param name="text">String to measure.</param>
        /// <param name="fontFamily">Font that defines the text format of the string.</param>
        /// <param name="fontSize">Font size that defines the text format of the string.</param>
        /// <returns>Formatted text (width, height, MaxTextWidth, MaxLineCout, etc.)</returns>
        public static FormattedText Measure(string text, string fontFamily, double fontSize)
        {
#pragma warning disable 0618
            return new FormattedText(
                text,
                CultureInfo.CurrentCulture,
                System.Windows.FlowDirection.LeftToRight,
                new Typeface(fontFamily),
                fontSize,
                System.Windows.Media.Brushes.Black);

        }
    }
}
