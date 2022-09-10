using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace AppEngine.WpfLib.Controls.ValueSlider;

public partial class ValueControl : UserControl
{
    public event EventHandler<ValueChangeArgs> NotifyOnValueChange;

    public Label _Label;
    public TextBox _Text;
    public Slider _Slider;

    public ValueControl()
    {
        InitializeComponent();

        SetBindings();

        Visibility = Visibility.Collapsed;

        _Label = Label;

        _Text = TextRadius;

        _Slider = SliderRadius;


        TextRadius.TextChanged += TextRadius_TextChanged;

        SliderRadius.ValueChanged += SliderRadius_ValueChanged;
    }

    private void SetBindings()
    {
        Binding b = new Binding
        {
            Converter = new DoubleConverter(),
            Source = SliderRadius,
            Path = new PropertyPath("Value")
        };
        TextRadius.SetBinding(TextBox.TextProperty, b);
    }

    private void SliderRadius_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        ValueChangeArgs arg = new ValueChangeArgs();
        if ((int)e.NewValue > (int)e.OldValue) arg.PositiveDirection = true;
        else if ((int)e.NewValue < (int)e.OldValue) arg.PositiveDirection = false;
        NotifyOnValueChange?.Invoke(sender, arg);
    }

    private void TextRadius_TextChanged(object sender, TextChangedEventArgs e)
    {
        ValueChangeArgs arg = new ValueChangeArgs();
        NotifyOnValueChange?.Invoke(sender, arg);
    }
}