using System;
using System.Windows.Forms;

namespace Snake
{
    internal static class Program
    {
        //public const string FilePath = ""
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
