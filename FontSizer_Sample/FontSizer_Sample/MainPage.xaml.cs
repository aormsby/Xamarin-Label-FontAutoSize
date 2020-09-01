using System;
using Xamarin.Forms;
using XamarinLabelFontSizer;

namespace FontSizer_Sample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        void OnStackSizeChanged(object sender, EventArgs args)
        {
            // set frames to fill 1/4 of stack height
            double frameHeight = pageStack.Height / 4;
            frame1.HeightRequest = frameHeight;
            frame2.HeightRequest = frameHeight;
            frame3.HeightRequest = frameHeight;
            frame4.HeightRequest = frameHeight;
            
            // for storing the size results to be passed on to the labels
            double sizingResult;

            // sizing labelOne
            ghostLabel.Text = String.Copy(label1.Text);

            sizingResult = FontSizer.CalculateMaxFontSize(ghostLabel, 10, 100,
                pageStack.Width, pageStack.Height / 4);

            label1.FontSize = sizingResult;

            // sizing labelTwo
            ghostLabel.Text = String.Copy(label2.Text);

            sizingResult = FontSizer.CalculateMaxFontSize(ghostLabel, 10, 100,
                pageStack.Width, pageStack.Height / 4);

            label2.FontSize = sizingResult;

            // sizing labelThree
            ghostLabel.Text = String.Copy(label3.Text);

            sizingResult = FontSizer.CalculateMaxFontSize(ghostLabel, 10, 100,
                pageStack.Width, pageStack.Height / 4);

            label3.FontSize = sizingResult;

            // sizing labelFour
            ghostLabel.Text = String.Copy(label4.Text);

            sizingResult = FontSizer.CalculateMaxFontSize(ghostLabel, 10, 100,
                pageStack.Width, pageStack.Height / 4);

            label4.FontSize = sizingResult;
        }
    }
}
