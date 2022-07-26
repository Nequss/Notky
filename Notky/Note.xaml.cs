using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shell;

namespace Notky
{
    public partial class Note : Window
    {
        public Note()
        {
            Ini();
        }

        public Note(double left, double top, double height, double width, double fontSize, string text, string bgColor, string frColor)
        {
            Ini();

            Left = left;
            Top = top;
            Height = height;
            Width = width;
            Text.FontSize = fontSize;
            Text.Text = text;

            BrushConverter bc = new BrushConverter();
            Text.Background = (SolidColorBrush)bc.ConvertFrom(bgColor);
            Text.Foreground = (SolidColorBrush)bc.ConvertFrom(frColor);
            Background = (SolidColorBrush)bc.ConvertFrom(bgColor);
        }

        private void Ini()
        {
            InitializeComponent();

            WindowChrome.SetWindowChrome(this, new WindowChrome());
            WindowChrome.SetIsHitTestVisibleInChrome(optionsPanel, true);

            pinImage.Source = ImageConverter("Notky.Resources.pin.png");
            colorImage.Source = ImageConverter("Notky.Resources.palette.png");
            fontUpImage.Source = ImageConverter("Notky.Resources.fontup.png");
            fontDownImage.Source = ImageConverter("Notky.Resources.fontdown.png");
            closeImage.Source = ImageConverter("Notky.Resources.close.png");
            fontColorImage.Source = ImageConverter("Notky.Resources.fontcolor.png");
            saveDownImage.Source = ImageConverter("Notky.Resources.save.png");
        }

        private void DragEvent(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            grid.RowDefinitions.Clear();
            optionsPanel.Visibility = Visibility.Collapsed;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            RowDefinition row = new RowDefinition();

            row.Height = new GridLength(20);
            grid.RowDefinitions.Add(row);

            row = new RowDefinition();

            row.Height = GridLength.Auto;
            grid.RowDefinitions.Add(row);

            optionsPanel.Visibility = Visibility.Visible;
        }

        private void pinButton_Click(object sender, RoutedEventArgs e)
        {
            Topmost = !Topmost;

            if (Topmost)
                pinImage.Source = ImageConverter("Notky.Resources.redpin.png");
            else
                pinImage.Source = ImageConverter("Notky.Resources.pin.png");
        }

        private void colorPalette_Click(object sender, RoutedEventArgs e)
        {
            ColorPalette colorPalette = new ColorPalette();

            colorPalette.Width = Width;
            colorPalette.Height = 20d;
            colorPalette.Top = Top - colorPalette.Height;
            colorPalette.Left = Left;
            colorPalette.Show();
            colorPalette.Deactivated += ColorPalette_Deactivated;

            foreach (var child in colorPalette.grid.Children)
            {
                (child as Button).Tag = (sender as Button).Name;
                (child as Button).Click += Btn_Click;
            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Tag.ToString() == "backgroundColorButton")
            {
                Background = (sender as Button).Background;
                Text.Background = (sender as Button).Background;
            }
            else
            {
                Text.Foreground = (sender as Button).Background;
            }
        }

        private void ColorPalette_Deactivated(object sender, EventArgs e)
            => (sender as Window).Close();

        private void closeButton_Click(object sender, RoutedEventArgs e)
            => Close();

        private void fontUpButton_Click(object sender, RoutedEventArgs e)
            => Text.FontSize += 2;

        private void fontDownButton_Click(object sender, RoutedEventArgs e)
            => Text.FontSize -= 2;

        private void saveDownButton_Click(object sender, RoutedEventArgs e)
        {
            App currentApp = Application.Current as App;
            currentApp.SaveApp();
        }

        private ImageSource ImageConverter(string file)
            => (ImageSource)new ImageSourceConverter().ConvertFrom(Assembly.GetExecutingAssembly().GetManifestResourceStream(file));
    }
}