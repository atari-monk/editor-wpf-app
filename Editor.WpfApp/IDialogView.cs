using Sim.Core;

namespace EditorApp.WpfApp;

public interface IDialogView
{
    event EventHandler<EventArgs> ColorSetingEvent;

    event EventHandler<EventArgs> ContextSetingEvent;

    event EventHandler<EventArgs> FilledSetingEvent;

    event EventHandler<ImageEventArgs> ImageSetingEvent;

    event EventHandler<EventArgs> ShapeSetingEvent;

    event EventHandler<EventArgs> TextFlagSetingEvent;
}