using GameEngine.Exception;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GameEngine.Input
{
    public class Keyboard
    {
        public const int MAX_EVENT_BUFFER_SIZE = byte.MaxValue;
        public bool CapsLock { get => Control.IsKeyLocked(Keys.CapsLock); }
        public bool NumLock { get => Control.IsKeyLocked(Keys.NumLock); }
        public bool ScrollLock { get => Control.IsKeyLocked(Keys.Scroll); }
        public Keyboard()
        {
            KeysBuffer = new List<Key>(256);
            for (int i = 0; i < KeysBuffer.Capacity; ++i)
            {
                KeysBuffer.Add(new Key(i, KeyStates.None));
            }

            EventsBuffer = new Queue<KeyEventArgs>();
            Converter = new KeysConverter();
        }
        public void ReadInput()
        {
            ClearReleasedState();
            while (EventsBuffer.Count > 0)
            {
                KeyEventArgs e = EventsBuffer.Dequeue();
                switch (e.KeyState)
                {
                    case KeyStates.None:
                        break;
                    case KeyStates.Down:
                        OnKeyDown(e);
                        break;
                    case KeyStates.Released:
                        OnKeyUp(e);
                        break;
                    default:
                        GameException.Raise($"Unhandled KeyState: {e.KeyState}");
                        break;
                }
            }
        }
        public bool IsKeyDown(Keys key) => this[key].Down;
        public bool IsKeyReleased(Keys key) => this[key].Released;
        public KeyStates GetKeyState(Keys key)
        {
            Key bufferKey = KeysBuffer[(int)key];
            if (bufferKey.Down)
            {
                return KeyStates.Down;
            }
            else if (bufferKey.Released)
            {
                return KeyStates.Released;
            }
            else
            {
                return KeyStates.None;
            }
        }
        /// <summary>
        /// set any keys whose state is <see cref="KeyStates.Released"/> to <see cref="KeyStates.None"/>
        /// </summary>
        private void ClearReleasedState()
        {
            for (int i = 0; i < KeysBuffer.Count; ++i)
            {
                if (KeysBuffer[i].Released)
                {
                    KeysBuffer[i].SetFalse();
                }
            }
        }
        protected virtual void OnKeyDown(KeyEventArgs e)
        {
            try
            {
                this[e.KeyCode].State = KeyStates.Down;
            }
            catch (ArgumentOutOfRangeException outOfRange)
            {
                GameException.Raise(outOfRange);
            }
            catch (System.Exception exception)
            {
                GameException.Raise(exception);
            }

        }
        protected virtual void OnKeyUp(KeyEventArgs e)
        {
            try
            {
                this[e.KeyCode].State = KeyStates.Released;
            }
            catch (ArgumentOutOfRangeException outOfRange)
            {
                GameException.Raise(outOfRange);
            }
            catch (System.Exception exception)
            {
                GameException.Raise(exception);
            }
        }
        private Key this[Keys keyCode] => KeysBuffer[(int)keyCode];
        public void ClearEventsBuffer() { EventsBuffer.Clear(); }

        /// <summary>
        /// for information about virtual key code, see <seealso cref="https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.keys?view=netframework-4.8"/>
        /// </summary>
        private readonly List<Key> KeysBuffer;
        private readonly Queue<KeyEventArgs> EventsBuffer;
        private readonly KeysConverter Converter;
        public bool QueueEvents(KeyEventArgs e)
        {
            if (EventsBuffer.Count >= MAX_EVENT_BUFFER_SIZE)
            {
                return false;
            }

            EventsBuffer.Enqueue(e);
            return true;
        }
        public class Key
        {
            public bool Down { get => State == KeyStates.Down; }
            public bool Released { get => State == KeyStates.Released; }
            public Keys Code { get; private set; }
            public KeyStates State { get; internal set; }

            internal Key(int keyValue, KeyStates keyState)
            {
                Code = Enum.IsDefined(typeof(Keys), keyValue) ? (Keys)keyValue : Keys.None;
                State = keyState;
            }
            public Key(Keys keyCode, KeyStates keyState)
            {
                Code = keyCode;
                State = keyState;
            }
            public void SetFalse() => State = KeyStates.None;
        }
        public class KeyEventArgs
        {
            public Keys KeyCode { get; private set; }
            public Keys Modifiers { get; private set; }
            public KeyStates KeyState { get; private set; }

            public KeyEventArgs(Keys code, Keys modifiers, KeyStates state)
            {
                KeyCode = code;
                Modifiers = modifiers;
                KeyState = state;
            }
            public KeyEventArgs(System.Windows.Forms.KeyEventArgs e, KeyStates state)
            {
                KeyCode = e.KeyCode;
                Modifiers = e.Modifiers;
                KeyState = state;
            }
        }
        public enum KeyStates
        {
            None = 0,
            Down = 1,
            Released = 2,
        }
    }
}
