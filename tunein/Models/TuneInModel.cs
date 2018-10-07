using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace TuneIn.Models
{
    class TuneInModel : ITuneIn, INotifyPropertyChanged
    {
        private ObservableCollection<string> providers;
        private string selectedProvider;
        private SynchronizationContext uiSyncCtx;
        private bool isListening { get; set; }
        private bool isHelpRequested;

        public bool IsHelpRequested
        {
            get { return this.isHelpRequested; }
            set
            {
                this.isHelpRequested = value;
                this.FirePropertyChanged();
            }
        }

        public bool IsListening
        {
            get { return this.isListening; }
            set
            {
                this.isListening = value;
                this.FirePropertyChanged();
            }
        }

        public ObservableCollection<string> Providers
        {
            get { return this.providers; }
            set
            {
                this.providers = value;
                this.FirePropertyChanged();
            }
        }

        public TuneInModel(SynchronizationContext uiSyncCtx)
        {
            this.uiSyncCtx = uiSyncCtx;
        }

        public string SelectedProvider
        {
            get { return this.selectedProvider; }
            set
            {
                this.selectedProvider = value;
                this.FirePropertyChanged();
            }
        }

        public ObservableCollection<TraceData> Traces { get; set; } = new ObservableCollection<TraceData>();

        public event PropertyChangedEventHandler PropertyChanged;

        public void LoadConfig()
        {
            var configFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), $"{Properties.Resources.AppName}.config");
            if(configFile != null && File.Exists(configFile))
            {
                using (StreamReader sr = new StreamReader(new FileStream(configFile, FileMode.Open)))
                {
                    var txt = sr.ReadToEnd();
                    dynamic t = JsonConvert.DeserializeObject(txt);
                    if(t != null)
                    {
                        this.providers = new ObservableCollection<string>(t.Providers.ToObject<string[]>());
                        this.SelectedProvider = t.SelectedProvider.ToObject<string>();
                    }
                }
            }
        }

        public void ClearLogs()
        {
            this.Traces.Clear();
        }

        public void StartListening()
        {
            throw new System.NotImplementedException();
        }

        public void StopListening()
        {
            throw new System.NotImplementedException();
        }

        public void AddTrace(TraceData trace)
        {
            uiSyncCtx.Send(x => this.Traces.Add(trace), null);
        }

        private void FirePropertyChanged([CallerMemberName] string name = default(string))
        {
            if(this.PropertyChanged != null && !string.IsNullOrWhiteSpace(name))
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
