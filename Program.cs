using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Practices.Unity; //UnityContainer

namespace ADO_LINQ
{
    static class Program
    {
        public static UnityContainer DiContainer;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DiContainer = new UnityContainer();
            DiContainer.RegisterInstance(typeof(IConfiguration),
                new ConfigurationBuilder()
                .AddJsonFile(@"C:\Users\RTX 3070\source\repos\ADO_LINQ\appsettings.json")
                .Build());
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
