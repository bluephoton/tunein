using System;
using System.Collections.Generic;

namespace TuneIn.Models
{
    public class TraceData
    {
        // Event specific
        public DateTime Timestamp { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }

        // Provider specific
        public string ProviderName { get; set; }
        public Guid ProviderGuid { get; set; }

        // Activity specific
        public Guid ActivityId { get; set; }
        public int ProcessId { get; set; }
        public int ThreadId { get; set; }
        public string Task { get; set; }
        public string Opcode { get; set; }
        public string Message { get; set; }
        public IDictionary<string, string> Payload { get; set; }
    }
}
