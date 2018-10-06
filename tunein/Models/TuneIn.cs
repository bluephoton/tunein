using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace tunein.Models
{
    class TuneIn : ITuneIn, INotifyPropertyChanged
    {
        private string log;
        public string Log
        {
            get { return log; }
            set
            {
                this.log = value;
                this.FirePropertyChanged("Log");
            }
        }

        public ObservableCollection<TraceData> Traces
        {
            get
            {
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

                return new ObservableCollection<TraceData>(traces);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

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

        private void FirePropertyChanged(string name)
        {
            if(this.PropertyChanged != null && !string.IsNullOrWhiteSpace(name))
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
