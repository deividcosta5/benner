namespace DigitalMicrowave.Business.Models;

public enum HeatingStatus { Idle, Running, Paused, Completed }

public class HeatingSession
{
    public int TotalTimeInSeconds { get; set; }
    public int RemainingTimeInSeconds { get; set; }
    public int Power { get; set; }
    public HeatingStatus Status { get; set; } = HeatingStatus.Idle;
    public string HeatingString { get; set; } = string.Empty;
    public HeatingProgram? Program { get; set; }

    public bool IsPreDefinedProgram => Program?.IsPreDefined == true;
    public bool IsRunning => Status == HeatingStatus.Running;
    public bool IsPaused => Status == HeatingStatus.Paused;
    public bool IsIdle => Status == HeatingStatus.Idle;
    public bool IsCompleted => Status == HeatingStatus.Completed;

    public string FormattedRemainingTime
    {
        get
        {
            if (TotalTimeInSeconds >= 60)
            {
                int mins = RemainingTimeInSeconds / 60;
                int secs = RemainingTimeInSeconds % 60;
                return $"{mins}:{secs:D2}";
            }
            return RemainingTimeInSeconds.ToString();
        }
    }
}
