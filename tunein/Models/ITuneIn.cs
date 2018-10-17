namespace TuneIn.Models
{
    interface ITuneInModel
    {
        string SelectedProvider { get; }
        void ClearLogs();
        void AddTrace(TraceData trace);
        void ResetActivityIds();
    }
}
