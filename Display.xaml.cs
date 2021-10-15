using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SubathonWPF
{
    /// <summary>
    /// Interaction logic for Display.xaml
    /// </summary>
    public partial class Display : Window
    {
        
        public Display()
        {
            InitializeComponent();
        }

        public void setText(String timerBarText)
        {
            this.Dispatcher.Invoke(() =>
            {
                ProgressbarText.Content = timerBarText;
            });
        }
        public void setValue(int timerBarValue)
        {
            this.Dispatcher.Invoke(() =>
            {
                Progressbar.Value = timerBarValue;
            });
        }

        public void setMax(int max)
        {
            this.Dispatcher.Invoke(() =>
            {
                Progressbar.Maximum = max;
            });
        }

        private void RadialProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
