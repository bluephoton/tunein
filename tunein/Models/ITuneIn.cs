namespace TuneIn.Models
{
    interface ITuneIn
    {
        void StartListening();
        void StopListening();
        void ClearLogs();
        void AddTrace(TraceData trace);
    }
}
