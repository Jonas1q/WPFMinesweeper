
using System.Windows;


namespace WPFMinesweeper
{
  
    public partial class SettingsDlg : Window
    {
        public SettingsDlg()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MainWindow w1 = (MainWindow)Owner;
            w1.nRows = int.Parse(textBox1.Text);
            w1.nCols = int.Parse(textBox1.Text);
            w1.NewGame_Click(null,null);
            Close();
        }
    }
}
