using System;
using Xamarin.Forms;

namespace XamarinLabelFontAutoSize
{
    struct FontCalc
    {
        public double FontSize { private set; get; }
        public double TextHeight { private set; get; }

        public FontCalc(Label label, double fontSize, double containerWidth)
            : this()
        {
            // Save the font size.
            FontSize = fontSize;

            // Recalculate the Label height.
            label.FontSize = fontSize;
            SizeRequest sizeRequest =
                label.Measure(containerWidth, Double.PositiveInfinity);

            // Save that height.
            TextHeight = sizeRequest.Request.Height;
        }
    }
}
