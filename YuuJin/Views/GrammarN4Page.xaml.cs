using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace YuuJin.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GrammarN4Page : Page
    {
        public GrammarN4Page()
        {
            this.InitializeComponent();

            /*            for (var i = 0; i < 5; i++)
                        {
                            MyPanel.Children.Add(new Rectangle
                            {
                                Width = 100,
                                Height = 20,
                                StrokeThickness = 1,
                                Stroke = new SolidColorBrush(Colors.Black),
                                Margin = new Thickness(5)
                            });
                        }*/

            for (var i = 26; i <= 50; i++)
            {
                Border br = new Border();
                br.BorderBrush = new RevealBorderBrush();
                br.BorderThickness = new Thickness(2);

                Image img = new Image();
                img.Width = 400;
                img.Margin = new Thickness(5);
                img.Source = new BitmapImage(new Uri(base.BaseUri, @"/Data/Grammar/N4/" + i + ".png"));

                br.Child = img;

                WrapPanel.Children.Add(br);
            }
        }
    }
}
