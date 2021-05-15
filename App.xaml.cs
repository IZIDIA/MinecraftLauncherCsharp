using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MinecraftLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string folder = ".eaglecraft";
            DirectoryInfo dirInfo = new DirectoryInfo(appData + "\\" + folder);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
        }
    }
}
