using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace TuneIn.Models
{
    class TuneInModel : ITuneIn, INotifyPropertyChanged
    {
        private string log;
        public string Log
        {
            get { return log; }
            set
            {
                this.log = value;
                this.FirePropertyChanged();
            }
        }

        private ObservableCollection<string> providers;
        public ObservableCollection<string> Providers
        {
            get { return this.providers; }
            set
            {
                this.providers = value;
                this.FirePropertyChanged();
            }
        }

        private string selectedProvider;
        public string SelectedProvider
        {
            get { return this.selectedProvider; }
            set
            {
                this.selectedProvider = value;
                this.FirePropertyChanged();
            }
        }

        public ObservableCollection<TraceData> Traces
        {
            get
            {
                #region remove later
                var traces = new TraceData[]
                {
                    new TraceData {
                        Name="hello", Level = 5, Timestamp = DateTime.Now, ActivityId = Guid.NewGuid(), Task = "Shopping", Opcode = "boring",
                            Properties = new Dictionary<string, string>()
                            {
                                { "foo", "bar"}
                            }
                    },
                    new TraceData {
                        Name="from", Level = 5, Timestamp = DateTime.Now, ActivityId = Guid.NewGuid(), Task = "slacking", Opcode = "boring",
                            Message = "the brown fox jumped into the black box",
                            Properties = new Dictionary<string, string>()
                            {
                                { "foo", "bar"}
                            }
                    },
                    new TraceData {
                        Name="the", Level = 4, Timestamp = DateTime.Now, ActivityId = Guid.NewGuid(), Task = "eating", Opcode = "boring",
                        Message = "the brown fox jumped into the black box",
                            Properties = new Dictionary<string, string>()
                            {
                                { "foo", "bar"}
                            }
                    },
                    new TraceData {
                        Name="Other", Level = 5, Timestamp = DateTime.Now, ActivityId = Guid.NewGuid(), Task = "drinking", Opcode = "boring",
                        Message = "the brown fox jumped into the black box",
                            Properties = new Dictionary<string, string>()
                            {
                                { "foo", "bar"}
                            }
                    },
                    new TraceData {
                        Name="Side", Level = 3, Timestamp = DateTime.Now, ActivityId = Guid.NewGuid(), Task = "sleepin!", Opcode = "boring",
                        Message = "the brown fox jumped into the black box",
                            Properties = new Dictionary<string, string>()
                            {
                                { "foo", "bar"}
                            }
                    },
                    new TraceData {
                        Name="Side", Level = 4, Timestamp = DateTime.Now, ActivityId = Guid.NewGuid(), Task = "sleepin!", Opcode = "boring",
                        Message = "the brown fox jumped into the black box",
                            Properties = new Dictionary<string, string>()
                            {
                                { "foo", "bar"}
                            }
                    },
                    new TraceData {
                        Name="Side", Level = 2, Timestamp = DateTime.Now, ActivityId = Guid.NewGuid(), Task = "sleepin!", Opcode = "boring",
                        Message = "the brown fox jumped into the black box",
                            Properties = new Dictionary<string, string>()
                            {
                                { "foo", "bar"}
                            }
                    },
                    new TraceData {
                        Name="Side", Level = 5, Timestamp = DateTime.Now, ActivityId = Guid.NewGuid(),  Task = "sleepin!", Opcode = "boring",
                            Message = "the brown fox jumped into the black box",
                            Properties = new Dictionary<string, string>()
                            {
                                { "foo", "bar"}
                            }
                    },
                    new TraceData {
                        Name="Side", Level = 1, Timestamp = DateTime.Now, ActivityId = Guid.NewGuid(), Task = "sleepin!", Opcode = "boring",
                            Message = "the brown fox jumped into the black box",
                            Properties = new Dictionary<string, string>()
                            {
                                { "foo", "bar"}
                            }
                    },
                };
                #endregion
                return new ObservableCollection<TraceData>(traces);
            }
        }

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
            this.Log = string.Empty;
        }

        public void StartListening()
        {
            throw new System.NotImplementedException();
        }

        public void StopListening()
        {
            throw new System.NotImplementedException();
        }

        public void Write(string text)
        {
            this.Log += text;
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
