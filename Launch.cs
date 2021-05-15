using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.IO.Compression;
using System.IO;

namespace MinecraftLauncher
{
    public class Launch
    /*VERSION 1.8*/
    {
        static private MainWindow form;
        static string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        static string folder = ".eaglecraft";
        static public string Login = string.Empty;
        static public string UUID = string.Empty;
        static public string accessToken = string.Empty;
        static public void DownloadMinecraftWithStart(string link, MainWindow _form)
        {
            try
            {
                form = _form;
                using (var client = new WebClient())
                {
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    client.DownloadFileAsync(new Uri(link), appData + "\\" + folder + "\\client.zip");
                    form.downloadBar.Value = 0;
                    form.downloadPanel.Visibility = Visibility.Visible;
                    
                }
            }
            catch
            {
                form.topLabel.Content = "Ошибка: невоможно скачать файлы клиента...";
                form.topLabel.Visibility = Visibility.Visible;
                MainWindow.passwordWrong = true;
                form.buttonStart.IsEnabled = true;
            }
        }
        static private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            form.downloadBar.Value = e.ProgressPercentage;
            form.downloadText.Text = "Загрузка " + e.ProgressPercentage + "%";
            if (e.ProgressPercentage == 100)
            {
                form.downloadText.Text = "Распаковка...";
            }

        }

        static private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                form.topLabel.Content = "Ошибка: невоможно скачать файлы клиента...";
                form.topLabel.Visibility = Visibility.Visible;
                MainWindow.passwordWrong = true;
                form.downloadPanel.Visibility = Visibility.Hidden;
                form.buttonStart.IsEnabled = true;
            }
            else
            {
                unpackFiles();
                StartMinecraft(form);

            }
        }

        static private void unpackFiles()
        {
            ZipFile.ExtractToDirectory(appData + "\\" + folder + "\\client.zip", appData + "\\" + folder + "\\");
            File.Delete(appData + "\\" + folder + "\\client.zip");
            form.downloadPanel.Visibility = Visibility.Hidden;
        }

        static public void StartMinecraft(MainWindow _form)
        {
            form = _form;

            string ram = "1024";
            if (form.ram.Text.Length > 2)
            {
                int num;
                bool isNum = int.TryParse(form.ram.Text, out num);
                if (isNum && num <= 65536 && num >= 128)
                {
                    ram = form.ram.Text;
                }
            }

            ProcessStartInfo mcStartInfo = new ProcessStartInfo("javaw.exe", "-Xincgc -Xms" + ram + "M -Xmx" + ram + "M" +
            " -Djava.library.path=\"" + appData + "\\" + folder + "\\versions\\1.8\\natives\"" +
            " -cp \"" + appData + "\\" + folder + "\\libraries\\java3d\\vecmath\\1.5.2\\vecmath-1.5.2.jar;" +
             appData + "\\" + folder + "\\libraries\\net\\sf\\trove4j\\trove4j\\3.0.3\\trove4j-3.0.3.jar;" +
             appData + "\\" + folder + "\\libraries\\com\\ibm\\icu\\icu4j-core-mojang\\51.2\\icu4j-core-mojang-51.2.jar;" +
             appData + "\\" + folder + "\\libraries\\net\\sf\\jopt-simple\\jopt-simple\\4.6\\jopt-simple-4.6.jar;" +
             appData + "\\" + folder + "\\libraries\\com\\paulscode\\codecjorbis\\20101023\\codecjorbis-20101023.jar;" +
             appData + "\\" + folder + "\\libraries\\com\\paulscode\\codecwav\\20101023\\codecwav-20101023.jar;" +
             appData + "\\" + folder + "\\libraries\\com\\paulscode\\libraryjavasound\\20101123\\libraryjavasound-20101123.jar;" +
             appData + "\\" + folder + "\\libraries\\com\\paulscode\\librarylwjglopenal\\20100824\\librarylwjglopenal-20100824.jar;" +  //
             appData + "\\" + folder + "\\libraries\\com\\paulscode\\soundsystem\\20120107\\soundsystem-20120107.jar;" +
             appData + "\\" + folder + "\\libraries\\io\\netty\\netty-all\\4.0.15.Final\\netty-all-4.0.15.Final.jar;" +
             appData + "\\" + folder + "\\libraries\\com\\google\\guava\\guava\\17.0\\guava-17.0.jar;" +
             appData + "\\" + folder + "\\libraries\\org\\apache\\commons\\commons-lang3\\3.3.2\\commons-lang3-3.3.2.jar;" +
             appData + "\\" + folder + "\\libraries\\commons-io\\commons-io\\2.4\\commons-io-2.4.jar;" +
             appData + "\\" + folder + "\\libraries\\commons-codec\\commons-codec\\1.9\\commons-codec-1.9.jar;" +
             appData + "\\" + folder + "\\libraries\\net\\java\\jinput\\jinput\\2.0.5\\jinput-2.0.5.jar;" +
             appData + "\\" + folder + "\\libraries\\net\\java\\jutils\\jutils\\1.0.0\\jutils-1.0.0.jar;" +
             appData + "\\" + folder + "\\libraries\\com\\google\\code\\gson\\gson\\2.2.4\\gson-2.2.4.jar;" +
             appData + "\\" + folder + "\\libraries\\com\\mojang\\authlib\\1.5.21\\authlib-1.5.21.jar;" +
             appData + "\\" + folder + "\\libraries\\com\\mojang\\realms\\1.5.4\\realms-1.5.4.jar;" +
             appData + "\\" + folder + "\\libraries\\org\\apache\\commons\\commons-compress\\1.8.1\\commons-compress-1.8.1.jar;" +
             appData + "\\" + folder + "\\libraries\\org\\apache\\httpcomponents\\httpclient\\4.3.3\\httpclient-4.3.3.jar;" +
             appData + "\\" + folder + "\\libraries\\commons-logging\\commons-logging\\1.1.3\\commons-logging-1.1.3.jar;" +
             appData + "\\" + folder + "\\libraries\\org\\apache\\httpcomponents\\httpcore\\4.3.2\\httpcore-4.3.2.jar;" +
             appData + "\\" + folder + "\\libraries\\org\\apache\\logging\\log4j\\log4j-api\\2.0-beta9\\log4j-api-2.0-beta9.jar;" +
             appData + "\\" + folder + "\\libraries\\org\\apache\\logging\\log4j\\log4j-core\\2.0-beta9\\log4j-core-2.0-beta9.jar;" +
             appData + "\\" + folder + "\\libraries\\org\\lwjgl\\lwjgl\\lwjgl\\2.9.1\\lwjgl-2.9.1.jar;" +
             appData + "\\" + folder + "\\libraries\\org\\lwjgl\\lwjgl\\lwjgl_util\\2.9.1\\lwjgl_util-2.9.1.jar;" +
             appData + "\\" + folder + "\\libraries\\tv\\twitch\\twitch\\6.5\\twitch-6.5.jar;" +
             appData + "\\" + folder + "\\versions\\1.8\\1.8.jar\" " +
             " net.minecraft.client.main.Main" +
             " --username " + Login +
             " --version 1.8" +
             " --gameDir " + appData + "\\" + folder +
             " --assetsDir " + appData + "\\" + folder + "\\assets" +
             " --assetIndex 1.8" +
             " --uuid " + UUID +
             " --accessToken " + accessToken +
             " --userProperties {}" +
             " --userType mojang"
            );

            mcStartInfo.WorkingDirectory = appData + "\\" + folder;
            mcStartInfo.ErrorDialog = true;
            Process.Start(mcStartInfo);

            string[] data = new string[3];
            if (form.remember.IsChecked == true)
            {
                data[0] = Encoder.EncodeDecrypt(form.tb1.Text);
                data[1] = Encoder.EncodeDecrypt(form.tb2.Password);
            }
            if (form.ram.Text.Length > 2)
            {
                data[2] = Encoder.EncodeDecrypt(form.ram.Text);
            }
            File.WriteAllLines(appData + "\\" + folder + "\\" + "data.txt", data);

            form.closeApp();
        }
    }
}
