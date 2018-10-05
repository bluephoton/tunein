using System;
using System.Diagnostics.Tracing;
using System.Text;

namespace tunein.Models
{
    class TuneInTraceListener : EventListener
    {
        private readonly ITuneIn model;

        public TuneInTraceListener(ITuneIn model) : base()
        {
            this.model = model;
        }
        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            StringBuilder sb = new StringBuilder(1000);
            sb.AppendFormat("[TS={0}]", DateTimeOffset.UtcNow.ToString("o"));
            sb.AppendFormat("[AID={0}]", eventData.ActivityId);
            sb.AppendFormat("[CH={0}]", eventData.Channel);
            sb.AppendFormat("[ID={0}]", eventData.EventId);
            sb.AppendFormat("[EV={0}]", eventData.EventName);
            sb.AppendFormat("[PG={0}]", eventData.EventSource.Guid);
            sb.AppendFormat("[PN={0}]", eventData.EventSource.Name);
            sb.AppendFormat("[KW={0}]", eventData.Keywords);
            sb.AppendFormat("[LV={0}]", eventData.Level);
            sb.AppendFormat("[MSG={0}]", eventData.Message);
            sb.AppendFormat("[OP={0}]", eventData.Message);
            sb.AppendFormat("[TS={0}]", eventData.Task);
            sb.AppendFormat("[PS={0}]", eventData.Payload);
            sb.AppendLine();
            model.Write(sb.ToString());
        }
    }
}
