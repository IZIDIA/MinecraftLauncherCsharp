using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MinecraftLauncher
{

    static public class POST
    {
        static private MainWindow form;
        static public bool goPost(string link, string login, string password, MainWindow _form)
        {
            form = _form;
            //Оффлайн мод
            if (form.offMode.IsChecked == true)
            {
                Launch.Login = form.tb1.Text;
                Launch.UUID = "1a2b3c4d5e6f7g8h9i0g";
                Launch.accessToken = "1a2b3c4d5e6f7g8h9i0g";
                return true;
            }
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(link);
                var postData = "method=auth";
                postData += "&username=" + login;
                postData += "&password=" + password;
                var dat = Encoding.ASCII.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = dat.Length;
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(dat, 0, dat.Length);
                }
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                if (responseString.Contains("OK"))
                {
                    string[] responseArray = responseString.Split(':');
                    Launch.Login = responseArray[1];
                    Launch.UUID = responseArray[2];
                    Launch.accessToken = responseArray[3];
                    return true;
                }
                else
                {
                    form.topLabel.Content = "Неверный логин или пароль";
                    form.topLabel.Visibility = Visibility.Visible;
                    form.tb2.Password = string.Empty;
                    MainWindow.passwordWrong = true;
                    return false;
                }
            }
            catch
            {
                form.topLabel.Content = "Невозможно подключиться к базе данных...";
                form.topLabel.Visibility = Visibility.Visible;
                MainWindow.passwordWrong = true;
                return false;
            }


        }
    }
}
