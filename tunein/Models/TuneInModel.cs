using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace TuneIn.Models
{
    class TuneInModel : ITuneInModel, INotifyPropertyChanged
    {
        private ObservableCollection<string> providers;
        private string selectedProvider;
        private SynchronizationContext uiSyncCtx;
        private bool isListening { get; set; }
        private bool isHelpRequested;
        private ObservableCollection<string> hiddenColumns;
        private ObservableCollection<string> activityIds;
        private string selectedActivityId;

        public string SelectedActivityId
        {
            get { return this.selectedActivityId; }
            set
            {
                this.selectedActivityId = value;
                this.FirePropertyChanged();
            }
        }

        public ObservableCollection<string> ActivityIds
        {
            get { return this.activityIds; }
            set
            {
                this.activityIds = value;
                this.FirePropertyChanged();
            }
        }

        public ObservableCollection<string> HiddenColumns
        {
            get { return this.hiddenColumns; }
            set
            {
                this.hiddenColumns = value;
                this.FirePropertyChanged();
            }
        }

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

        public TuneInModel(SynchronizationContext uiSyncCtx)
        {
            this.uiSyncCtx = uiSyncCtx;
            this.ResetActivityIds();
        }

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
                        this.providers = new ObservableCollection<string>(t.Providers?.ToObject<string[]>() ?? new string[] { });
                        this.SelectedProvider = t.SelectedProvider?.ToObject<string>() ?? string.Empty;
                        this.HiddenColumns = new ObservableCollection<string>(t.HiddenColumns?.ToObject<string[]>() ?? new string[] { });
                    }
                }
            }
        }

        public void ClearLogs()
        {
            this.Traces.Clear();
            this.ResetActivityIds();
        }

        public void AddTrace(TraceData trace)
        {
            uiSyncCtx.Send(x => this.Traces.Add(trace), null);
            uiSyncCtx.Send(x => {
                string aid = trace.ActivityId.ToString();
                if (!this.ActivityIds.Contains(aid))
                {
                    this.ActivityIds.Add(aid);
                }
            }, null);
        }

        public void ResetActivityIds()
        {
            if(this.ActivityIds == null)
            {
                this.ActivityIds = new ObservableCollection<string>();
            }
            else
            {
                this.ActivityIds.Clear();
            }

            var defaultActivityId = "";
            this.ActivityIds.Add(defaultActivityId);
            this.SelectedActivityId = defaultActivityId;
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
