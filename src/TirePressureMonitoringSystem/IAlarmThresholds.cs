namespace TDDMicroExercises.TirePressureMonitoringSystem
{
    public interface IAlarmThresholds
    {
        double HighThreshold { get; }
        double LowThreshold { get; }
    }

    public class AlarmThresholds : IAlarmThresholds
    {
        public AlarmThresholds(double lowThreshold, double highThreshold)
        {
            LowThreshold = lowThreshold;
            HighThreshold = highThreshold;
        }

        public double HighThreshold { get; }

        public double LowThreshold { get; }
    }
}