using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UCSClientPatcher
{
    internal class Utils
    {
        internal static byte[] HexToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        internal static string AssemblyVersion
        {
            get
            {
                return "v" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        internal static void WriteCenter(string _String)
        {
            Console.SetCursorPosition((Console.WindowWidth - _String.Length) / 2, Console.CursorTop);
            Console.WriteLine(_String);
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
        }      
    }
}
