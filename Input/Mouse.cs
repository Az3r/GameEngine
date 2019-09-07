using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GameEngine.Exception;
namespace GameEngine.Input
{
    public class Mouse
    {
        public const int MAX_EVENT_BUFFER_SIZE = byte.MaxValue;
        public const int MOUSE_WHEEL_DELTA = 120;
        public int Delta { get; private set; }
        public Point Location { get; private set; }
        public bool OverWindow { get; private set; }
        public bool HasFocus { get; private set; }
        public Mouse()
        {
            MouseBuffer = new Dictionary<MouseButtons, ButtonState>
            {
                { MouseButtons.Left, ButtonState.None },
                { MouseButtons.Right, ButtonState.None },
                { MouseButtons.Middle, ButtonState.None },
                { MouseButtons.XButton1, ButtonState.None },
                { MouseButtons.XButton2, ButtonState.None }
            };
            SupportedButtons = new List<MouseButtons>(MouseBuffer.Keys);
        }
        /// <summary>
        /// All mouse events are not read immediately but stored in an events buffer, call <seealso cref="ReadInput"/> to read all mouse events
        /// </summary>
        public void ReadInput()
        {
            ClearReleasedState();
            while (EventsBuffer.Count > 0)
            {
                MouseEventArgs e = EventsBuffer.Dequeue();
                switch (e.Type)
                {
                    case EventType.Down:
                        OnMouseDown(e.Buttons);
                        break;
                    case EventType.Released:
                        OnMouseReleased(e.Buttons);
                        break;
                    case EventType.Enter:
                        OnMouseEnter(e.Location);
                        break;
                    case EventType.Leave:
                        OnMouseLeave(e.Location);
                        break;
                    case EventType.Move:
                        OnMouseMove(e.Location);
                        break;
                    case EventType.Wheel:
                        OnMouseWheel(e.Delta);
                        break;
                    default:
                        GameException.Raise($"Event type is {e.Type}");
                        break;
                }
            }
        }
        /// <summary>
        /// set any buttons whose state is <see cref="ButtonState.Released"/> after previous call to ReadInput to <see cref="ButtonState.None"/>
        /// </summary>
        private void ClearReleasedState()
        {
            Delta = 0;
            foreach (MouseButtons key in SupportedButtons)
                if (MouseBuffer[key] == ButtonState.Released) MouseBuffer[key] = ButtonState.None;
        }
        public ButtonState GetStates(MouseButtons buttons) => MouseBuffer[buttons];
        public bool IsReleased(MouseButtons buttons)
        {
            return Convert.ToBoolean(MouseBuffer[buttons] & ButtonState.Released);
        }
        public bool IsDown(MouseButtons buttons)
        {
            return Convert.ToBoolean(MouseBuffer[buttons] & ButtonState.Down);
        }
        public void ClearEventsBuffer() { EventsBuffer.Clear(); }
        public void Reset()
        {
            MouseBuffer[MouseButtons.Left] = ButtonState.None;
            MouseBuffer[MouseButtons.Right] = ButtonState.None;
            MouseBuffer[MouseButtons.Middle] = ButtonState.None;
            MouseBuffer[MouseButtons.XButton1] = ButtonState.None;
            MouseBuffer[MouseButtons.XButton2] = ButtonState.None;
        }

        public bool QueueEvents(MouseEventArgs e)
        {
            if (EventsBuffer.Count >= MAX_EVENT_BUFFER_SIZE) return false;
            EventsBuffer.Enqueue(e);
            return true;
        }
        protected virtual void OnMouseDown(MouseButtons buttons)
        {
            MouseBuffer[buttons] = ButtonState.Down;
        }
        protected virtual void OnMouseReleased(MouseButtons buttons)
        {
            MouseBuffer[buttons] = ButtonState.Released;
        }
        protected virtual void OnMouseMove(Point location)
        {
            Location = location;
        }
        protected virtual void OnMouseWheel(int delta)
        {
            Delta = delta;
        }
        protected virtual void OnMouseEnter(Point enterLocation) { OverWindow = true; }
        protected virtual void OnMouseLeave(Point leaveLocation) { OverWindow = false; }
        protected virtual void OnGainFocus() { HasFocus = true; OnFocusChanged(); }
        protected virtual void OnLostFocus() { HasFocus = false; OnFocusChanged(); }
        protected virtual void OnFocusChanged() { }

        private readonly Dictionary<MouseButtons, ButtonState> MouseBuffer;
        private readonly List<MouseButtons> SupportedButtons;
        private readonly Queue<MouseEventArgs> EventsBuffer = new Queue<MouseEventArgs>();

        public class MouseEventArgs
        {
            public MouseEventArgs()
            {
                Type = EventType.None;
                Buttons = MouseButtons.None;
                Location = Point.Empty;
                Delta = 0;
            }
            public MouseEventArgs(EventType type, MouseButtons buttons, Point location)
            {
                Type = type;
                Location = location;
                Buttons = buttons;
                Delta = 0;
            }
            /// <summary>
            /// EventArgs for mouse wheel event
            /// </summary>
            public MouseEventArgs(EventType type, MouseButtons buttons, Point location, int delta) : this(type, buttons, location)
            {
                Delta = delta;
            }
            public EventType Type { get; private set; }
            public Point Location { get; private set; }
            public MouseButtons Buttons { get; private set; }
            public int Delta { get; private set; }

        }
        public enum EventType
        {
            Down, Released, Enter, Leave, Move, Wheel, Capture, None
        }
        public enum ButtonState
        {
            None = 0,
            Down = 1,
            Released = 2
        }
    }
}
