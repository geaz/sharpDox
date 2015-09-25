using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SharpDox.GUI.Controls
{
    public partial class ColorSelector : UserControl
    {
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register("SelectedColor", typeof(string), typeof(ColorSelector), 
            new FrameworkPropertyMetadata("#FFFFFF", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty DefaultHeaderTextProperty =
            DependencyProperty.Register("DefaultHeaderText", typeof(string), typeof(ColorSelector), new PropertyMetadata("Default Colors"));

        public static readonly DependencyProperty CustomHeaderTextProperty =
            DependencyProperty.Register("CustomHeaderText", typeof(string), typeof(ColorSelector), new PropertyMetadata("Custom Color"));

        private bool _userSelectsColor = false;
        private string _previousColor = string.Empty;

        public ColorSelector()
        {
            InitializeComponent();
            InitializeControl();
        }

        private void InitializeControl()
        {
            var descriptor = DependencyPropertyDescriptor.FromProperty(SelectedColorProperty, typeof(ColorSelector));
            descriptor.AddValueChanged(this, SelectedColor_Changed);

            colorSelector.ApplyTemplate();
            InitializeDefaultPicker();
        }

        private void InitializeDefaultPicker()
        {
            DefaultPicker.Items.Clear();
            var defaultColors = GetDefaultColors();
            foreach (var item in defaultColors)
            {
                DefaultPicker.Items.Add(item);
            }
            DefaultPicker.SelectionChanged += (o, s) => SelectedColor = GetHexValue(((Color)((ListBox)o).SelectedValue));
        }

        private IEnumerable<Color> GetDefaultColors()
        {
            var defaultColors = new List<Color>();
            var colorsType = typeof(Colors);
            var colorsProperty = colorsType.GetProperties();

            foreach (PropertyInfo property in colorsProperty)
            {
                defaultColors.Add((Color)ColorConverter.ConvertFromString(property.Name));
            }

            return defaultColors;
        }

        private void ChangePointerPositionAndSetColor()
        {
            var x = Mouse.GetPosition(colorCanvas).X;
            x = x < 0 ? 0 : x;
            x = x >= 200 ? 199 : x;

            var y = Mouse.GetPosition(colorCanvas).Y;
            y = y < 0 ? 0 : y;
            y = y >= 160 ? 159 : y;

            colorPointer.SetValue(Canvas.LeftProperty, x - 5);
            colorPointer.SetValue(Canvas.TopProperty, y - 5);
            colorPointer.InvalidateVisual();
            colorCanvas.InvalidateVisual();

            var color = GetColorFromImage((int)x, (int)y);
            SelectedColor = GetHexValue(color);
        }

        private Color GetColorFromImage(int i, int j)
        {
            var cb = new CroppedBitmap(image.Source as BitmapSource, new Int32Rect(i, j, 1, 1));
            var color = new byte[4];

            cb.CopyPixels(color, 4, 0);

            var colorFromImagePoint = Color.FromArgb(255, color[2], color[1], color[0]);

            return colorFromImagePoint;
        }

        private string GetHexValue(Color color)
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
        }

        private void SelectedColor_Changed(object sender, EventArgs eventArgs)
        {            
            if (SelectedColor != null && CheckValidFormatHtmlColor(SelectedColor))
            {
                _previousColor = SelectedColor;
                var colorPreview = (Rectangle)colorSelector.Template.FindName("colorPreview", colorSelector);
                if (colorPreview != null) colorPreview.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(SelectedColor));
            }
            else
            {
                SelectedColor = _previousColor;
            }
        }

        private void ColorSelector_KeyUp(object sender, KeyEventArgs e)
        {
            if (CheckValidFormatHtmlColor(colorSelector.Text))
            {
                var color = (Color)ColorConverter.ConvertFromString(colorSelector.Text);
                SelectedColor = GetHexValue(color);
            }
        }
        
        private void ColorPointer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _userSelectsColor = true;
        }

        private void colorPicker_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _userSelectsColor = false;
            e.Handled = true;
        }

        private void ColorCanvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_userSelectsColor)
            {
                ChangePointerPositionAndSetColor();
            }
        }

        private void ColorCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _userSelectsColor = true;
            ChangePointerPositionAndSetColor();           
        }

        private void epDefault_Expanded(object sender, RoutedEventArgs e)
        {
            if(epCustom != null)
                epCustom.IsExpanded = false;
        }

        private void epCustom_Expanded(object sender, RoutedEventArgs e)
        {
            if (epDefault != null)
                epDefault.IsExpanded = false;
        }

        private bool CheckValidFormatHtmlColor(string inputColor)
        {
            if (Regex.Match(inputColor, "^#(?:[0-9a-fA-F]{3}){1,2}$").Success)
            {
                return true;
            }

            var result = System.Drawing.Color.FromName(inputColor);
            return result.IsKnownColor;
        }

        public String SelectedColor
        {
            get { return (String)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        public String DefaultHeaderText
        {
            get { return (String)GetValue(DefaultHeaderTextProperty); }
            set { SetValue(DefaultHeaderTextProperty, value); }
        }

        public String CustomHeaderText
        {
            get { return (String)GetValue(CustomHeaderTextProperty); }
            set { SetValue(CustomHeaderTextProperty, value); }
        }
    }

    [ValueConversion(typeof(Color), typeof(Brush))]
    public class ColorToSolidColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new SolidColorBrush((Color)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
