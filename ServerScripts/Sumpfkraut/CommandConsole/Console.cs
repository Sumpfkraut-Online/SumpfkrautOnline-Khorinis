using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GUC.Server.Scripts.Sumpfkraut.CommandConsole
{
    class Console
    {

        Thread thread;

        public Console ()
            : this(false)
        { }

        public Console (bool startAtInit)
        {
            this.thread = new Thread(this.Run);
            if (startAtInit)
            {
                this.thread.Start();
            }
        }


        public void Run ()
        {
            string line;
            while ((line = System.Console.ReadLine()) != null){
                System.Console.WriteLine(">>>" + line);
            }
            //int count = 0;
            //while (true)
            //{
            //    System.Console.WriteLine(count);
            //    if (count == 999)
            //    {
            //        System.Console.WriteLine("Yay!");
            //        this.thread.Abort();
            //    }
            //    count++;
            //    // ~ sleep in Java
            //    this.thread.Join(0);
            //}
        }
    }
}
