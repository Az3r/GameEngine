using System.Runtime.CompilerServices;
using System.Diagnostics;
namespace GameEngine.Exception
{
    public static class GameException
    {
        public static void Raise(string what, [CallerLineNumber] int line = 0, [CallerFilePath] string file = null)
        {
            string message = string.Format($"[Line]: {line}\r\n[File]: {file}\r\n[Description]: {what}");
            Trace.Assert(false, message);
        }
        public static void Throw(string what, [CallerLineNumber] int line = 0, [CallerFilePath] string file = null)
        {
            string message = string.Format($"[Line]: {line}\r\n[File]: {file}\r\n[Description]: {what}");
            throw new System.Exception(message);
        }
        public static void Raise(System.Exception e, [CallerLineNumber] int line = 0, [CallerFilePath] string file = null)
        {
            string message = string.Format($"[Line]: {line}\r\n[File]: {file}\r\n[Description]: {e.GetType().Name} - {e.Message}");
            Trace.Assert(false, message);
        }
        public static void Throw(System.Exception e, [CallerLineNumber] int line = 0, [CallerFilePath] string file = null)
        {
            string message = string.Format($"[Line]: {line}\r\n[File]: {file}\r\n[Description]: {e.GetType().Name} - {e.Message}");
            throw new System.Exception(message);
        }
    }
}
