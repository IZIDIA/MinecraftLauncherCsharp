using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;


namespace MinecraftLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static public string folder = ".eaglecraft";
        static string downloadLink = "http://web1038.craft-host.ru/client.zip";
        static string siteRegisterPage = "http://web1038.craft-host.ru/";
        static string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        static DirectoryInfo dirMain = new DirectoryInfo(appData + "\\" + folder + "\\");
        static DirectoryInfo dirAssets = new DirectoryInfo(appData + "\\" + folder + "\\assets");
        static DirectoryInfo dirLibraries = new DirectoryInfo(appData + "\\" + folder + "\\libraries");
        static DirectoryInfo dirVersions = new DirectoryInfo(appData + "\\" + folder + "\\versions");
        static public bool passwordWrong = false;
        public MainWindow()
        {
            InitializeComponent();
            if (!File.Exists(appData + "\\" + folder + "\\" + "data.txt"))
            {
                using (File.Create(appData + "\\" + folder + "\\" + "data.txt"))
                { };
            }
            string[] data = File.ReadAllLines(appData + "\\" + folder + "\\" + "data.txt");
            //Если есть данные
            if (data.Length == 3)
            {
                tb1.Text = Encoder.EncodeDecrypt(data[0]);
                tb2.Password = Encoder.EncodeDecrypt(data[1]);
                ram.Text = Encoder.EncodeDecrypt(data[2]);
                if (data[0].Length > 1)
                {
                    remember.IsChecked = true;
                }
            }
        }
        public void closeApp()
        {
            this.Close();
        }
        #region STARTGAME
        void LaunchGame(string howLaunch)
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            BLAKE3 blake3 = new BLAKE3();
            var hash = blake3.ComputeHash(Encoding.UTF8.GetBytes(tb2.Password));
            string passwd = "";
            foreach (var b in hash)
            {
                passwd += b.ToString("x2");
            }
            if (POST.goPost("http://web1038.craft-host.ru/auth/launcher.php", tb1.Text, passwd, this))
            {
                switch (howLaunch)
                {
                    case "Default":
                        {
                            dirMain = new DirectoryInfo(appData + "\\" + folder);
                            if (dirMain.Exists)
                            {
                                dirAssets = new DirectoryInfo(appData + "\\" + folder + "\\assets");
                                dirLibraries = new DirectoryInfo(appData + "\\" + folder + "\\libraries");
                                dirVersions = new DirectoryInfo(appData + "\\" + folder + "\\versions");
                                if (dirAssets.Exists && dirLibraries.Exists && dirVersions.Exists) //Only Launch
                                {
                                    Launch.StartMinecraft(this);
                                }
                                else if (!dirAssets.Exists && !dirLibraries.Exists && !dirVersions.Exists) //Clean Load
                                {
                                    ReloadFilesAndStart();
                                }
                                else// Something 
                                {
                                    MessageBox.Show("Отсутствуют некоторые директории игры. Выберите в настройках пункт 'Перекачать клиент' и запустите игру снова.");
                                    buttonStart.IsEnabled = true;
                                }
                            }
                            else
                            {
                                DirectoryInfo dirInfo = new DirectoryInfo(appData + "\\" + folder);
                                dirInfo.Create();
                                ReloadFilesAndStart();
                            }
                            break;
                        }
                    case "ReloadFiles":
                        {
                            dirAssets = new DirectoryInfo(appData + "\\" + folder + "\\assets");
                            dirLibraries = new DirectoryInfo(appData + "\\" + folder + "\\libraries");
                            dirVersions = new DirectoryInfo(appData + "\\" + folder + "\\versions");
                            if (dirAssets.Exists)
                            {
                                dirAssets.Delete(true);
                            }
                            if (dirLibraries.Exists)
                            {
                                dirLibraries.Delete(true);
                            }
                            if (dirVersions.Exists)
                            {
                                dirVersions.Delete(true);
                            }
                            string serversDat = appData + "\\" + folder + "\\servers.dat";
                            FileInfo fileInf = new FileInfo(serversDat);
                            if (fileInf.Exists)
                            {
                                fileInf.Delete();
                            }
                            ReloadFilesAndStart();
                            break;
                        }
                }
            }
            else
            {
                buttonStart.IsEnabled = true;
            }
        }
        void ReloadFilesAndStart()
        {
            Launch.DownloadMinecraftWithStart(downloadLink, this);
        }
        #endregion
        //------------------СОБЫТИЯ-------------------
        private void ExitButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        private void MinButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void ToolBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (passwordWrong)
            {
                passwordWrong = false;
                topLabel.Visibility = Visibility.Hidden;
            }
            if (tb2.Password.Length > 0)
            {
                Watermark.Visibility = Visibility.Collapsed;
            }
            else
            {
                Watermark.Visibility = Visibility.Visible;
            }
        }
        //Переход на сайт
        private void Button_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Process.Start("explorer.exe", siteRegisterPage);
        }
        //Запуск игры
        private void Button_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (tb1.Text.Length >= 3)
            {
                buttonStart.IsEnabled = false;
                if (reloadGame.IsChecked == true)
                {
                    LaunchGame("ReloadFiles");
                }
                else
                {
                    LaunchGame("Default");
                }
            }
        }


        //Опции
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (optGrid.Visibility == Visibility.Hidden)
            {
                optGrid.Visibility = Visibility.Visible;
            }
            else
            {
                optGrid.Visibility = Visibility.Hidden;
            }
        }

        private void ram_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }
    }
}
