using System.Text;
using System.Threading;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Session;
using TuneIn.Models;

namespace TuneIn
{
    class ListenerThread
    {
        private TraceEventSession session;
        private Thread eventSinkThread;
        private readonly ITuneIn model;

        internal ListenerThread(ITuneIn model)
        {
            this.model = model;
        }

        internal static ListenerThread Listen(ITuneIn model)
        {
            var listener = new ListenerThread(model);
            listener.StartListening();
            return listener;
        }

        internal void StopListening()
        {
            if (this.session != null)
            {
                this.session.Stop();
            }
        }

        private void StartListening()
        {
            this.eventSinkThread = new Thread(() => {
                using (var session = new TraceEventSession("mo-session-01"))
                {
                    this.session = session;
                    using (var source = new ETWTraceEventSource("mo-session-01", TraceEventSourceType.Session))
                    {
                        var parser = new DynamicTraceEventParser(source);
                        parser.All += (TraceEvent data) =>
                        {
                            var text = FormatEventData(data);
                            model.Write(text);
                        };

                        session.EnableProvider("GuestAction.Telemetry");
                        source.Process();
                        var t = source.CanReset;
                    }
                }
            });

            this.eventSinkThread.Start();
        }

        private string FormatEventData(TraceEvent data)
        {
            StringBuilder sb = new StringBuilder(500);
            sb.AppendFormat("[TS={0}]", data.TimeStamp.ToString("o"));
            
            sb.AppendFormat("[PG={0}]", data.ProviderGuid);
            sb.AppendFormat("[PN={0}]", data.ProviderName);
            sb.AppendFormat("[CH={0}]", data.Channel);
            sb.AppendFormat("[PID={0}]", data.ProcessID);
            sb.AppendFormat("[TID={0}]", data.ThreadID);

            sb.AppendFormat("[ID={0}]", data.ID);
            sb.AppendFormat("[EV={0}]", data.EventName);
            sb.AppendFormat("[LV={0}]", data.Level);
            sb.AppendFormat("[KW={0}]", data.Keywords);     // since keywords can select events across activities/tasks I'm putting it as an event thing

            sb.AppendFormat("[AID={0}]", data.ActivityID);
            sb.AppendFormat("[TSK={0}]", data.TaskName);
            sb.AppendFormat("[OP={0}]", data.OpcodeName);

            sb.AppendFormat("[MSG={0}]", data.FormattedMessage);

            foreach (var name in data.PayloadNames)
            {
                var index = data.PayloadIndex(name);
                sb.AppendFormat("[{0}={1}]", name, data.PayloadString(index));
            }
            
            sb.AppendLine();
            return sb.ToString();
        }
    }
}
