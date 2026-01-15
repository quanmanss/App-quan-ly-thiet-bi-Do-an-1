using DevicesControlApp.Data;
using DevicesControlApp.Models;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;


namespace DevicesControlApp.Views
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirm = ConfirmPasswordBox.Password;
            string email = EmailBox.Text.Trim();

            if (password != confirm)
            {
                MessageBox.Show("Mật khẩu nhập lại không đúng!");
                return;
            }

            try
            {
                Account account = new Account()
                {
                    Username = username,
                    Password = DatabaseHelper.HashPassword(password)

                };

                int accountId = DatabaseHelper.CreateAccount(account);


                var user = new User()
                {
                    Name = username,
                    Email = email,
                    AccountID = accountId
                };

                DatabaseHelper.CreateUser(user);
                
                MessageBox.Show("Đăng ký thành công!");
                this.Close();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                    MessageBox.Show("Username đã tồn tại!");
                else
                    MessageBox.Show("Lỗi SQL: " + ex.Message);
            }
        }
    }
}
