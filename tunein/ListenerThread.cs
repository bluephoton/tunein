using System.Collections.Generic;
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
                            var trace = TraceEventToTraceData(data);
                            model.AddTrace(trace);
                        };

                        session.EnableProvider("GuestAction.Telemetry");
                        source.Process();
                        var t = source.CanReset;
                    }
                }
            });

            this.eventSinkThread.Start();
        }

        private TraceData TraceEventToTraceData(TraceEvent data)
        {
            return new TraceData
            {
                Timestamp = data.TimeStamp,
                ProviderGuid = data.ProviderGuid,
                ProviderName = data.ProviderName,
                ProcessId = data.ProcessID,
                ThreadId = data.ThreadID,
                //data.Channel

                Id = (int)data.ID,
                Name = data.EventName,
                Level = (int)data.Level,
                //data.Keywords   // since keywords can select events across activities/tasks I'm putting it as an event thing - assuming it means what i think it means!

                ActivityId = data.ActivityID,
                Task = data.TaskName,
                Opcode = data.OpcodeName,

                Message = data.FormattedMessage,
                Properties = this.GetPayload(data)
            };
        }

        private IDictionary<string, string> GetPayload(TraceEvent data)
        {
            var payload = new Dictionary<string, string>();

            foreach (var name in data.PayloadNames)
            {
                var index = data.PayloadIndex(name);
                payload.Add(name, data.PayloadString(index));
            }

            return payload;
        }
    }
}
