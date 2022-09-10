using System.Windows;
using System.Windows.Controls;
using Canvas;
using Sim.Core;

namespace EditorApp.WpfApp;

public partial class EditorView : Window
{
    private readonly ICanvasEditorControl<IShape> _canvasEditorControl;

    private readonly IDialogView _dialogView;

    private CanvasEditorControl CanvasEditorControl { get; }

    private CanvasConfigDialog CanvasDialogSubcontrol { get; }

    private DialogView DialogView { get; }

    public EditorView(ICanvasEditorControl<IShape> canvasEditorControl,
        IDialogView dialog)
    {
        _canvasEditorControl = canvasEditorControl;
        _dialogView = dialog;
        CanvasEditorControl = _canvasEditorControl as CanvasEditorControl;
        CanvasDialogSubcontrol = CanvasEditorControl.Canvas as CanvasConfigDialog;
        DialogView = _dialogView as DialogView;
        InitializeComponent();
        RootLayout.Children.Add(CanvasEditorControl);
        CanvasEditorControl.SetValue(Grid.RowProperty, 1);
        Initialize();
    }

    private void Initialize()
    {
        HandleWindowEvents();
        ConnectMenuWithCanvas();
        ConnectDialogWithCanvas();
    }

    private void HandleWindowEvents() => Closing += (sender, cancelEventArgs) => DialogView.Close();

    private void ConnectMenuWithCanvas()
    {
        LoadMenu.Click += CanvasDialogSubcontrol.Load;
        SaveMenu.Click += CanvasDialogSubcontrol.Save;
        DialogMenu.Click += (sender, routedEventArgs) => DialogView.Show();
        ClearMenu.Click += CanvasDialogSubcontrol.Clear;
        ExitMenu.Click += (sender, routedEventArgs) => Close();
    }

    private void ConnectDialogWithCanvas()
    {
        _dialogView.ShapeSetingEvent += _canvasEditorControl.Canvas.SetingShapeHandler;
        _dialogView.TextFlagSetingEvent += _canvasEditorControl.Canvas.OnSettingTextFlag;
        _dialogView.ContextSetingEvent += _canvasEditorControl.Canvas.OnSettingContext;
        _dialogView.ColorSetingEvent += _canvasEditorControl.Canvas.OnSettingShapeColor;
        _dialogView.FilledSetingEvent += _canvasEditorControl.Canvas.OnSettingFilledFlag;
        _dialogView.ImageSetingEvent += _canvasEditorControl.Canvas.OnSettingImagePath;
        DialogView.ValueControlX.NotifyOnValueChange += _canvasEditorControl.Canvas.OnSettingRadiusWidth;
        DialogView.ValueControlY.NotifyOnValueChange += _canvasEditorControl.Canvas.OnSettingHeight;
    }
}