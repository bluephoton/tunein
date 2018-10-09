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
        private readonly ITuneInModel model;

        internal ListenerThread(ITuneInModel model)
        {
            this.model = model;
        }

        internal static ListenerThread Listen(ITuneInModel model)
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

                        session.EnableProvider(this.model.SelectedProvider);
                        source.Process();
                        var t = source.CanReset;
                    }
                }
            });

            this.eventSinkThread.Start();
        }

        private TraceData TraceEventToTraceData(TraceEvent data)
        {
            var payload = this.GetPayload(data);

            string message;
            // Not sure where FormattedMessage come from! If its not there, I'll use payload["Message"] and remove it from payload. Otherwise, will not do anything to payload.
            if(payload.TryGetValue("Message", out message) && string.IsNullOrWhiteSpace(data.FormattedMessage))
            {
                payload.Remove("Message");
            }

            return new TraceData
            {
                Timestamp = data.TimeStamp.ToUniversalTime(),
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

                Message = string.IsNullOrWhiteSpace(data.FormattedMessage) ? message : data.FormattedMessage,
                Payload = payload
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
