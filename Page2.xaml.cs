using System;
using System.Text;
using System.Windows;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace forumcsharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        string login;
        //private string IP = "127.0.0.1";
        private string IP = "192.168.166.220";
        private int Port = 27015;
        private string Login;

        public Page2(string _Login)
        {
            InitializeComponent();
            //Show();
            if (!setupSocket(IP, Port))
            {
                MessageBox.Show("Something went wrong connecting to a server!");
                //Close();
            }

            Thread listenerThread = new Thread(listenAndPrintMessages);
            listenerThread.Start();
            Login = _Login;
            UsernameTextBlock.Text = $"Username: {Login}";
        }

        private TcpClient client;
        private bool setupSocket(string IP, int Port)
        {
            try
            {
                client = new TcpClient();
                client.Connect(IP, Port);
                ForumWindow.Items.Add($"Connected to the server on {IP}:{Port}");
                return true;
            }
            catch (Exception ex)
            {
                ForumWindow.Items.Add($"Error connecting to the server on {IP}:{Port}");
                return false;
            }
        }

        private bool sendMessage(string message)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] data = Encoding.UTF8.GetBytes(message);

                Array.Resize(ref data, data.Length + 1);
                data[data.Length - 1] = (byte)'\0';

                stream.Write(data, 0, data.Length);
                return true;
            }
            catch (Exception ex)
            {
                ForumWindow.Items.Add($"Error sending message!");
                return false;
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string messageToSend = ForumTextBox.Text;
            if (messageToSend.Length > 0)
            {
                messageToSend = Login + ": " + messageToSend;
                if (!sendMessage(messageToSend))
                    MessageBox.Show("Message has not been sent!");
            }


            ForumTextBox.Text = "";
        }

        private void updateForumChat(string message)
        {
            Dispatcher.Invoke(() =>
            {
                ForumWindow.Items.Add(message);
            });
        }

        private void listenAndPrintMessages()
        {
            NetworkStream stream = client.GetStream();

            while (true)
            {
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead > 0)
                {
                    string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    updateForumChat(receivedData);
                }
            }
        }

        private void ForumTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendButton_Click(sender, e);
            }
        }

    }
}
