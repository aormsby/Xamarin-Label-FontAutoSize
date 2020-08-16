using System.Linq;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Globalization;

namespace XamarinLabelFontSizer
{
    public static class FontSizer
    {
        /// <summary>Sets the char value for splitting strings in specific cultures/locales. Add a culture to specify the word or morpheme delimiting value.</summary>
        static Dictionary<string, char> localeWordDelimiters = new Dictionary<string, char>
        {
            { "en-US", ' ' }
        };

        /// <summary>Calculates the larget possible font size for a label within the given constraints, do not pass values for wordSafe or sizeForWidth</summary>
        /// <param name="label"></param><param name="minFontSize"></param><param name="maxFontSize"></param><param name="containerWidth"></param><param name="containerHeight"></param><param name="wordSafe"></param><param name="sizeForWidth"></param><returns>font size value as double</returns>
        public static double CalculateMaxFontSize(Label label, int minFontSize, int maxFontSize, double containerWidth, double containerHeight, bool wordSafe = true, bool sizeForWidth = false)
        {
            // calculate label sizes based on min and max font sizes
            FontCalc lowerFontCalc = new FontCalc(label, minFontSize, containerWidth);
            FontCalc upperFontCalc = new FontCalc(label, maxFontSize, containerWidth);

            if (wordSafe)
            {
                string savedText = label.Text;

                // FIXME: tested only for Latin-alphabet languages with space-delimited words (morphemes?)
                string[] words = label.Text.Split(GetLocaleWordDelimiter());

                // Find the longest word in the string and set it as label text for width constraint calculations
                if (words.Length > 1)
                {
                    string longestWord = words.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur);
                    label.Text = longestWord;
                }

                // calculate the largest font size for the longest word to fit the container *width* (recurses into this function)
                double constrainedMaxSize = CalculateMaxFontSize(label, minFontSize, maxFontSize, containerWidth, containerHeight, false, true);

                // reset label text to full string
                label.Text = savedText;
                upperFontCalc = new FontCalc(label, constrainedMaxSize, containerWidth);
            }

            while (upperFontCalc.FontSize - lowerFontCalc.FontSize > 1)
            {
                // Get the average font size of the upper and lower bounds.
                double fontSize = (lowerFontCalc.FontSize + upperFontCalc.FontSize) / 2;

                // Use the averaged text size data in size calculations
                FontCalc newFontCalc = new FontCalc(label, fontSize, containerWidth);

                // set conditional data based on width- or height-based constraint calculations
                double calculatedSize, sizeConstraint;
                if (sizeForWidth)
                {
                    // when wordSafe is active, calculate number of lines a word spans (should be less than 2)
                    calculatedSize = newFontCalc.TextHeight / (1 * newFontCalc.FontSize);
                    sizeConstraint = 2;
                }
                else
                {
                    // when testing agains container height, make sure label height fits within the bounds
                    calculatedSize = newFontCalc.TextHeight;
                    sizeConstraint = containerHeight;
                }

                // check size constraints and update values for next iteration
                if (calculatedSize > sizeConstraint)
                    upperFontCalc = newFontCalc;
                else
                    lowerFontCalc = newFontCalc;
            }

            return lowerFontCalc.FontSize;
        }

        /// <summary>Gets the assigned string delimiter for separating words in the current language, based on Current UI Culture setting</summary>
        /// <returns>delimiter value as char, fallback set to ' '</returns>
        static char GetLocaleWordDelimiter()
        {
            if (localeWordDelimiters.TryGetValue(CultureInfo.CurrentUICulture.ToString(), out char delimiter))
                return delimiter;

            return ' ';
        }

        // TODO: use this when maxLines parameter is built into the system
        //double NumLinesIn(Label label, double lineHeight = 1)
        //{
        //    double d = label.Height / (lineHeight * label.FontSize);
        //    return label.Height / (lineHeight * label.FontSize);
        //}
    }
}


// TODO: Optimization - try to find a way to check for both width and height constraints in the same sizing pass (no recursion, fewer iterations)
// TODO: Improve language support - word delimiting is currently based on the space character, not valid in some scripts, wordSafe is not a usable option for them
// TODO: Modularize - separate any localization features from the core sizing method(s) to maintain optimization in non-localized projects (i.e. no imported gloablization libraries)
// TODO: Feature - Add support to maxLines parameter to limit font size to fit on x lines
