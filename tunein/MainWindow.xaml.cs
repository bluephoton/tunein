using System;
using System.Windows;
using tunein.Models;

namespace tunein
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ListenerThread listener;
        public MainWindow()
        {
            InitializeComponent();

            var model = new TuneIn();
            this.DataContext = model;

            this.listener = ListenerThread.Listen(model);
        }

        protected override void OnClosed(EventArgs e)
        {
            if (this.listener != null)
            {
                this.listener.StopListening();
            }

            base.OnClosed(e);
        }
    }
}
