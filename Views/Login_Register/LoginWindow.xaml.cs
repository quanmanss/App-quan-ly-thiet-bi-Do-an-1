using DevicesControlApp.Data;
using DevicesControlApp.Models;
using System.Windows;
using System.Windows.Controls;


namespace DevicesControlApp.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Register_btn_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow register = new RegisterWindow();
            register.Show();
        }

        private void Login_btn_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text;
            string passwordInput = PasswordBox.Password;
            string hashedPassword = DatabaseHelper.HashPassword(passwordInput.Trim());
            User? user = DatabaseHelper.Login(username.Trim(), hashedPassword);

            if (user != null)
            {
                int? houseId = DatabaseHelper.GetUserHouseId(user.ID);

                if (houseId.HasValue)
                {
                    MainWindow mainWindow = new MainWindow(houseId.Value, user.ID);
                    mainWindow.Show();
                    MessageBox.Show($"Đăng nhập thành công, chào mừng {user.Name}!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                }

                else
                {
                    MessageBox.Show($"Đăng nhập thành công, chào mừng {user.Name}!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    HouseSetupWindow houseWindow = new HouseSetupWindow(user);
                    houseWindow.Show();
                }
                this.Close();

            }
            else
            {
                MessageBox.Show("Đăng nhập thất bại. Tên đăng nhập hoặc mật khẩu không hợp lệ.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
