using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using static UCSClientPatcher.Utils;

namespace UCSClientPatcher
{
    internal class Patcher
    {
        internal static void Main(string[] args)
        {
            try
            {
                string fileName = "libg.so";

                byte[] searchPattern = HexToByteArray("4102C28189897A48CEDFA8C6E5378F55624F9E8408FA8A376643DBBCE715B21A"); // 8.709.16 - 8.709.24
                byte[] replacePattern = HexToByteArray("72f1a4a4c48e44da0c42310f800e96624e6dc6a641a9d41c3b5039d8dfadc27e");
  
                SetWindowLong(GetConsoleWindow(), -20, (int)GetWindowLong(GetConsoleWindow(), -20) ^ 0x80000);
                SetLayeredWindowAttributes(GetConsoleWindow(), 0, 227, 0x2);

                Console.Title = $"Ultrapowa Client Patcher { AssemblyVersion } - © {DateTime.Now.Year}";

                Console.ForegroundColor = ConsoleColor.Blue;
                WriteCenter(@" ____ ___.__   __                                                  ");
                WriteCenter(@"|    |   \  |_/  |_____________  ______   ______  _  _______       ");
                WriteCenter(@"|    |   /  |\   __\_  __ \__  \ \____ \ /  _ \ \/ \/ /\__  \      ");
                WriteCenter(@"|    |  /|  |_|  |  |  | \// __ \|  |_> >  <_> )     /  / __ \_    ");
                WriteCenter(@"|______/ |____/__|  |__|  (____  /   __/ \____/ \/\_/  (____  /    ");
                WriteCenter(@"                               \/|__|                       \/     ");
                WriteCenter("            ");

                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Blue;
                WriteCenter("+-------------------------------------------------------+");
                Console.ResetColor();
                WriteCenter("|This program is made by the Ultrapowa Development Team.|");
                WriteCenter("|    Ultrapowa is not affiliated to \"Supercell, Oy\".    |");
                WriteCenter("|     This program is licensed under the MIT License    |");
                WriteCenter("|   Visit www.ultrapowa.com daily for News & Updates!   |");
                Console.ForegroundColor = ConsoleColor.Blue;
                WriteCenter("+-------------------------------------------------------+");
                Console.WriteLine();
                Console.ResetColor();

                byte[] fileBytes = File.ReadAllBytes(fileName);

                IEnumerable<int> positions = FindPattern(fileBytes, searchPattern);
                if (positions.Count() == 0)
                {
                    Console.WriteLine("[UCP]     Pattern not found.");
                    Console.Read();
                    return;
                }

                foreach (int pos in positions)
                {
                    Console.WriteLine($"[UCP]    Key offset: 0x{pos.ToString("X8")}");
                    using (BinaryWriter bw = new BinaryWriter(File.Open(fileName, FileMode.Open, FileAccess.Write)))
                    {
                        bw.BaseStream.Seek(pos, SeekOrigin.Begin);
                        bw.Write(replacePattern);
                    }
                    Console.WriteLine($"[UCP]    File: {fileName} patched");
                }
                Console.ReadKey();
            }
            catch (Exception _Exception)
            {
                Console.WriteLine($"[UCP]    Error: {_Exception.Message}");
                Console.ReadKey();
            }
        }

        public static IEnumerable<int> FindPattern(byte[] fileBytes, byte[] searchPattern)
        {
            if ((searchPattern != null) && (fileBytes.Length >= searchPattern.Length))
                for (int i = 0; i < fileBytes.Length - searchPattern.Length + 1; i++)
                    if (!searchPattern.Where((data, index) => !fileBytes[i + index].Equals(data)).Any())
                        yield return i;
        }

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        static extern bool SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetConsoleWindow();
    }
}
