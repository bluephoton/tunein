namespace TuneIn.Models
{
    interface ITuneInModel
    {
        string SelectedProvider { get; }
        void StartListening();
        void StopListening();
        void ClearLogs();
        void AddTrace(TraceData trace);
    }
}
