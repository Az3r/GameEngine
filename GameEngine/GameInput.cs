using System;
using GameEngine.Input;
using System.Windows.Forms;

namespace GameEngine
{
    public partial class Game
    {
        #region Mouse Section
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            MouseInput.QueueEvents(new Mouse.MouseEventArgs(Mouse.EventType.Released, e.Button, e.Location));
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            MouseInput.QueueEvents(new Mouse.MouseEventArgs(Mouse.EventType.Down, e.Button, e.Location));

        }
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            MouseInput.QueueEvents(new Mouse.MouseEventArgs(Mouse.EventType.Enter, MouseButtons, System.Drawing.Point.Empty));
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            MouseInput.QueueEvents(new Mouse.MouseEventArgs(Mouse.EventType.Leave, MouseButtons, System.Drawing.Point.Empty));

        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            MouseInput.QueueEvents(new Mouse.MouseEventArgs(Mouse.EventType.Wheel, e.Button, e.Location, e.Delta));
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            MouseInput.QueueEvents(new Mouse.MouseEventArgs(Mouse.EventType.Move, e.Button, e.Location));
        }
        #endregion

        #region Keyboard Section
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            KeyboardInput.QueueEvents(new Keyboard.KeyEventArgs(e.KeyCode, e.Modifiers, Keyboard.KeyStates.Down));
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            KeyboardInput.QueueEvents(new Keyboard.KeyEventArgs(e.KeyCode, e.Modifiers, Keyboard.KeyStates.Released));
        }

        #endregion
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
        }
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            MouseInput.ClearEventsBuffer();
            KeyboardInput.ClearEventsBuffer();
        }
        public Mouse MouseInput { get; set; }
        public Keyboard KeyboardInput { get; set; }
    }
}
