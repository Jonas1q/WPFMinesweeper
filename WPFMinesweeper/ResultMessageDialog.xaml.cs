using System.Windows;


namespace WPFMinesweeper
{
   
    public partial class ResultMessageDialog : Window
    {
        public ResultMessageDialog()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow w1 = (MainWindow)Owner;
            w1.Close();
        }

        private void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            MainWindow w1 = (MainWindow)Owner;
            w1.NewGame_Click(null, null);
            Close();
        }

        
    }
}
