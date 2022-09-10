using System.Windows;
using System.Windows.Media;
using Canvas;
using Shape.Model;
using Sim.Core;

namespace EditorApp.WpfApp;

public partial class App : Application
{
    private void ApplicationStartup(object sender, StartupEventArgs e)
    {
        IShapeFactory shapeFactory = new ShapeFactory();
        var background = shapeFactory.GetShape(ShapeTypes.Rectangle, Colors.White) as IRectangle;
        var currsor = shapeFactory.GetShape(ShapeTypes.Circle, Colors.Red, true, 10) as ICircle;
        IDictionary<SpecialShapes, IShape> specialShapes = new Dictionary<SpecialShapes, IShape>
            {
                { SpecialShapes.Background, background },
                { SpecialShapes.Currsor, currsor }
            };
        var subControl = new CanvasConfigDialog(
            specialShapes,
            new SerializerXml(),
            shapeFactory);
        var control = new CanvasEditorControl(subControl);
        var window = new EditorView(control, new DialogView(control));
        MainWindow = window;
        MainWindow.Show();
    }
}