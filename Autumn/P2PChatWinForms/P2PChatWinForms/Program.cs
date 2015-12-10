using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P2PChatWinForms
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string [] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length == 1)
            {
                //we are in a forked process
                Application.Run(new Form1(args[0]));
            }
            else
            {
                //we are in the parent process
                Application.Run(new Form1(""));
            }
        }
    }
}
