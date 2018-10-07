namespace TuneIn.Controls
{
    using System.Windows;
    using System.Windows.Input;
    using TuneIn.Models;

    /// <summary>
    /// Interaction logic for CaptionCtrl.xaml
    /// </summary>
    public partial class CaptionCtrl
    {
        public CaptionCtrl()
        {
            InitializeComponent();
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MaximizeRestoreButton_OnClick(object sender, RoutedEventArgs e)
        {
            switch(Application.Current.MainWindow.WindowState)
            {
                case WindowState.Normal:
                    Application.Current.MainWindow.WindowState = WindowState.Maximized;
                    break;
                case WindowState.Maximized:
                    Application.Current.MainWindow.WindowState = WindowState.Normal;
                    break;
            }
        }

        private void CaptionGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.MainWindow.DragMove();
        }

        private void Minimize_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void Help_OnClick(object sender, RoutedEventArgs e)
        {
            var model = this.DataContext as TuneInModel;
            if(model != null)
            {
                model.IsHelpRequested ^= true;
            }
        }
    }
}
