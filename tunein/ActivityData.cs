using System;
using System.Collections.Generic;

namespace tunein
{
    public class ActivityData
    {
        public Guid Id { get; set; }
        public int ProcessId { get; set; }
        public int ThreadId { get; set; }
        public string Task { get; set; }
        public string Opcode { get; set; }
        public string Message { get; set; }
        public IDictionary<string, string> Properties { get; set; }
    }
}
