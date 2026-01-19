using DevicesControlApp.Data;
using DevicesControlApp.Models;
using System.Windows;
using System.Windows.Controls;

namespace DevicesControlApp.Views
{
    /// <summary>
    /// Interaction logic for DeviceDetailsWindow.xaml
    /// </summary>
    public partial class DeviceDetailsWindow : Window
    {
        private Device _device;
        public DeviceDetailsWindow(Device device)
        {
            InitializeComponent();
            DataContext = device;
            _device = device;

            IntensitySlider.Value = device.Intensity;
            switch (device.Type)
            {
                case "TV":
                    txtIntensityLabel.Text = "Âm lượng";
                    break;
                case "Quạt":
                    txtIntensityLabel.Text = "Mức quạt";
                    break;
                case "Đèn":
                    txtIntensityLabel.Text = "Độ sáng";
                    break;
                default:
                    IntensitySlider.Visibility = Visibility.Collapsed;
                    txtIntensityLabel.Text = "Thiết bị không điều chỉnh";
                    break;
            }


        }
        private void IntensitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sender is Slider slider && slider.DataContext is Device device)
            {
                int newIntensity = (int)slider.Value;

                device.Intensity = newIntensity;

                DatabaseHelper.UpdateDeviceIntensity(device.ID, newIntensity);
            }
        }
    }
}
