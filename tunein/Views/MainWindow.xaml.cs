﻿using System;
using System.Threading;
using System.Windows;
using TuneIn.Models;

namespace TuneIn.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly TuneInModel model;
        private ListenerThread listener;
        public MainWindow()
        {
            InitializeComponent();

            var model = new TuneInModel(SynchronizationContext.Current);
            model.LoadConfig();
            this.model = model;
            this.DataContext = model;

            foreach (var column in this.gridTraces.Columns)
            {
                if(model.HiddenColumns.Contains(column.Header.ToString()))
                {
                    column.Visibility = Visibility.Collapsed;
                }
            }

            
        }

        protected override void OnClosed(EventArgs e)
        {
            if (this.listener != null)
            {
                this.listener.StopListening();
            }

            base.OnClosed(e);
        }

        private void ToggleListeningButton_Click(object sender, RoutedEventArgs e)
        {
            if (!this.model.IsListening)
            {
                this.listener = ListenerThread.Listen(this.model);
                this.model.IsListening = true;
            }
            else
            {
                if (this.listener != null)
                {
                    this.listener.StopListening();
                    this.model.IsListening = false;
                }
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.model.ClearLogs();
        }
    }
}
