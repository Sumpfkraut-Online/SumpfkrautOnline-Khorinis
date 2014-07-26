using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GMPStarter
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            else
            {
                String ip = args[0];
                int port = 3396;
                int pTrenner = ip.IndexOf(":");
                if (pTrenner > 0)
                {
                    ip = ip.Substring(0, pTrenner);
                    Int32.TryParse(args[0].Substring(pTrenner + 1), out port);
                }

                Form1 f = new Form1();
                f.textBox2.Text = ip;//IP
                f.textBox3.Text = ""+port;//Port
                f.mBStart_Click(null, null);
            }
        }
    }
}
