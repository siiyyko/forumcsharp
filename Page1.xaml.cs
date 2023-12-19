using System.IO;
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
            
            string[] dataBase;
            string[] LoginAndPassword;

            if (LoginTextBox.Text != "" && PasswordTextBox.Password != "")
            {
                dataBase = File.ReadAllText("database.txt").Split("\n");
                foreach (var user in dataBase)
                {
                    LoginAndPassword = user.Split(" ");
                    if (LoginAndPassword[0].CompareTo(LoginTextBox.Text) == 0 && LoginAndPassword[1].CompareTo(PasswordTextBox.Password) == 0)
                    {
                        Page2 page2 = new Page2(); // Create an instance of Page2
                        NavigationService.Navigate(page2, LoginAndPassword[0]);
                        break;
                    }
                }

            }
        }

        private void SignUpButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            File.WriteAllText("database.txt", LoginTextBox.Text + " " + PasswordTextBox.Password + "\n");
            Page2 page2 = new Page2(); // Create an instance of Page2
            NavigationService.Navigate(page2, LoginTextBox.Text);
        }

        bool isSignedUp(string userName)
        {
            //open file and search for username

            return false;
        }
    }
}
