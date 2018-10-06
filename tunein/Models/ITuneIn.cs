namespace TuneIn.Models
{
    interface ITuneIn
    {
        void StartListening();
        void StopListening();
        void ClearLogs();
        void Write(string text);
    }
}
