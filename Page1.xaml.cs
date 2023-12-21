using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace forumcsharp
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            if(LoginTextBox.Text == "" || PasswordTextBox.Password == "")
            {
                MessageBox.Show("Please, enter the username and the password!");
                return;
            }

            if(!isSignedUp(LoginTextBox.Text))
            {
                MessageBox.Show("There is no user with such username signed up!");
                return;
            }
            else if(!isPassCorrect(LoginTextBox.Text, PasswordTextBox.Password))
            {
                MessageBox.Show("Wrong password!");
                return;
            }

            NavigationService.Navigate(new Page2(LoginTextBox.Text));
        }

        private void SignUpButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            if (LoginTextBox.Text == "" || PasswordTextBox.Password == "")
            {
                MessageBox.Show("Please, enter the username and the password!");
                return;
            }

            if(!isLoginOk(LoginTextBox.Text))
            {
                MessageBox.Show("This username contains banned symbols!");
                return;
            }

            if (isSignedUp(LoginTextBox.Text))
            {
                MessageBox.Show("This username is already occupied");
                return;
            }

            using (StreamWriter sw = File.AppendText("database.txt"))
            {
                sw.WriteLine(LoginTextBox.Text + " " + PasswordTextBox.Password);
            }

            NavigationService.Navigate(new Page2(LoginTextBox.Text));
        }

        bool isSignedUp(string userName)
        {
            string[] dataBase;
            string[] LoginAndPassword;

            dataBase = File.ReadAllText("database.txt").Split('\n');
            foreach (var user in dataBase)
            {
                LoginAndPassword = user.Split(' ');
                if (LoginAndPassword[0] == userName)
                    return true;
            }

            return false;
        }

        bool isPassCorrect(string userName, string password)
        {
            string[] dataBase;
            string[] LoginAndPassword;

            dataBase = File.ReadAllText("database.txt").Split('\n');

            foreach (var user in dataBase)
            {
                LoginAndPassword = user.TrimEnd('\r').Split(' ');
                if (LoginAndPassword[0] == userName && LoginAndPassword[1] == password)
                    return true;
            }

            return false;
        }

        bool isLoginOk(string userName)
        {
            string badChars = "?!#$;\'\"&*=+";
            for (int i = 0; i < badChars.Length; ++i)
                if (userName.Contains(badChars[i]))
                    return false;
            return true;
        }
    }
}
