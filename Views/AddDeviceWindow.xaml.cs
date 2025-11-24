using DevicesControlApp.Data;
using DevicesControlApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;



namespace DevicesControlApp.Views
{
    /// <summary>
    /// Interaction logic for DevicesWindow.xaml
    /// </summary>
    public partial class AddDeviceWindow : Window
    {
        public Device CreatedDevice{ get; private set; }

        public AddDeviceWindow()
        {
            InitializeComponent();
        }
        private void AddDevice_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên thiết bị!");
                return;
            }

            CreatedDevice = new Device
            {
                Name = txtName.Text,
                Type = txtType.Text,
                Description = txtDescription.Text,
                Status = "Tắt",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            DialogResult = true;
            Close();

            MessageBox.Show("Them thanh cong!");
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        
    }
}

