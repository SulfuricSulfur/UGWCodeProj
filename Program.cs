#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace UGWProjCode
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// Unlimited Game Works 
    /// 5/13/15
    /// //Bierre
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
                game.Run();
        }
    }
#endif
}
