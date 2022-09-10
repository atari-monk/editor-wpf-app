using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using Sim.Core;

namespace EditorApp.WpfApp;

public partial class DialogView 
    : Window
        , IDialogView
{
    public event EventHandler<EventArgs> ShapeSetingEvent;

    public event EventHandler<EventArgs> ContextSetingEvent;

    public event EventHandler<EventArgs> ColorSetingEvent;

    public event EventHandler<EventArgs> TextFlagSetingEvent;

    public event EventHandler<EventArgs> FilledSetingEvent;

    public event EventHandler<ImageEventArgs> ImageSetingEvent;

    private readonly ICanvasEditorControl<IShape> _canvasControl;
    private OpenFileDialog _openFileDialog;
    private UserControl[] _valueControls;
    private UserControl[] _circle;
    private UserControl[] _rectangle;
    private ShapeTypes _selectedShape;

    public DialogView(ICanvasEditorControl<IShape> canvasControl)
    {
        _canvasControl = canvasControl;
        InitializeComponent();
        SetValueControls();
        Initialize();
        WireControlEvents();
    }

    private void SetValueControls()
    {
        _valueControls = new[] { ValueControlX, ValueControlY };
        _circle = new[] { ValueControlX };
        _rectangle = new[] { ValueControlX, ValueControlY };
    }

    private void Initialize()
    {
        SetData();
        SetDefaults();
        SetControlsForSelectedShape();
    }

    private void SetData()
    {
        SetShapes();
        SetContexts();
    }

    private void SetShapes()
    {
        ComboBoxShape.Items.Add(ShapeTypes.Circle);
        ComboBoxShape.Items.Add(ShapeTypes.Rectangle);
        ComboBoxShape.Items.Add(ShapeTypes.Line);
    }

    private void SetContexts()
    {
        ComboBoxContext.Items.Add(Context.Logic);
        ComboBoxContext.Items.Add(Context.Physic);
    }

    private void SetDefaults()
    {
        SetDefaultColor();
        SetDefaultContext();
        SetDefaultShape();
        SetDefaultShapeSelection();
    }

    private void SetDefaultColor() { }
    //ColorPicker.SelectedColor = _canvasControl.Canvas.Color;

    private void SetDefaultContext() =>
        ComboBoxContext.SelectedIndex =
        ComboBoxContext.Items.IndexOf(_canvasControl.Canvas.Context);

    private void SetDefaultShape() =>
        ComboBoxShape.SelectedIndex =
        ComboBoxShape.Items.IndexOf(_canvasControl.Canvas.SelectedShape);

    private void SetDefaultShapeSelection() => _selectedShape = _canvasControl.Canvas.SelectedShape;

    private void SetControlsForSelectedShape()
    {
        var selected = (ShapeTypes)ComboBoxShape.SelectedIndex;
        if (selected >= 0)
            _selectedShape = selected;
        HideValueControls(_valueControls);
        switch (_selectedShape)
        {
            case ShapeTypes.Circle:
                ValueControlX._Label.Content = "Radius";
                ShowValueControls(_circle);
                break;
            case ShapeTypes.Rectangle:
                ValueControlX._Label.Content = "Width";
                ValueControlY._Label.Content = "Height";
                ShowValueControls(_rectangle);
                break;
            default:
                break;
        }
    }

    private void HideValueControls(UserControl[] controls)
    {
        foreach (var control in controls)
        {
            control.Visibility = Visibility.Collapsed;
        }
    }

    private void ShowValueControls(UserControl[] controls)
    {
        foreach (var control in controls)
        {
            control.Visibility = Visibility.Visible;
        }
    }

    private void WireControlEvents()
    {
        ComboBoxShape.SelectionChanged += ShapeSelectionHandler;
        ComboBoxContext.SelectionChanged += ContextSelectionHandler;
        //ColorPicker.SelectedColorChanged += ColorSelectionHandler;
        TextBoxTextFlag.TextChanged += TextFlagHandler;
        CheckBoxFilled.Unchecked += NotFilledHandler;
        CheckBoxFilled.Checked += FilledHandler;
        ButtonSetImage.Click += SetImageHandler;
    }

    private void ShapeSelectionHandler(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
    {
        SetControlsForSelectedShape();
        ShapeSetingEvent.Invoke(sender, selectionChangedEventArgs);
        ColorSetingEvent.Invoke(sender, selectionChangedEventArgs);
    }

    private void ContextSelectionHandler(object sender,
        SelectionChangedEventArgs selectionChangedEventArgs) =>
       ContextSetingEvent.Invoke(sender, selectionChangedEventArgs);

    private void ColorSelectionHandler(object sender,
        RoutedPropertyChangedEventArgs<Color?> routedPropertyChangedEventArgs)
    {
        ColorSetingEvent.Invoke(sender, routedPropertyChangedEventArgs);
        ImageSetingEvent.Invoke(sender, new ImageEventArgs
        {
            ImagePath = ""
        });
    }

    private void TextFlagHandler(object sender, TextChangedEventArgs textChangedEventArgs) =>
        TextFlagSetingEvent.Invoke(sender, textChangedEventArgs);

    void NotFilledHandler(object sender, RoutedEventArgs routedEventArgs) =>
        FilledSetingEvent.Invoke(sender, routedEventArgs);

    void FilledHandler(object sender, RoutedEventArgs routedEventArgs) =>
        FilledSetingEvent.Invoke(sender, routedEventArgs);

    private void SetImageHandler(object sender, RoutedEventArgs routedEventArgs)
    {
        CreateOpenFileDialog();
        if (IsImageFromDialog())
        {
            InvokeImageChangeEvent(sender);
        }
    }

    private void CreateOpenFileDialog() => _openFileDialog = new OpenFileDialog
    {
        FileName = "Image",
        DefaultExt = ".jpg",
        Filter = "images (.jpg)|*.jpg;*.bmp",
        InitialDirectory = @"Images\"
    };

    private bool IsImageFromDialog() => _openFileDialog.ShowDialog() == true;

    private void InvokeImageChangeEvent(object sender)
        => ImageSetingEvent.Invoke(sender, new ImageEventArgs
        {
            ImagePath = _openFileDialog.FileName
        });
}