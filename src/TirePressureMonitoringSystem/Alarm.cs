namespace TDDMicroExercises.TirePressureMonitoringSystem
{
    /* Single resp
     * Open closed
     * Liskov subst
     * Interface segregation
     * Dependency inversion
     * ------------
     * Open/Closed ppl violation:
     *  there's no way to alter behavior of Alarm without changing it.
     *  Sensor is not abstracted, and so we cannot change it's behavior and cannot extend alarm unless we change alarm code itself
     *  Also, maybe thresholds may come in as a dependency and again we could extend it's behavior through that, or at least
     *  if it were an abstract class and have those thresh as protected..
     *  Same thing may apply to the algorithm of check(), could abstract that and have it even more open/closed compliant
     * DI ppl violation:
     *  by stating new on Sensor, then the details of this Alarm class depend on details ( the concrete Sensor class)
     *  instead of abstractions (the ISensor abstraction)
     *  
     */
    public class Alarm
    {
        private const double LowPressureThreshold = 17;
        private const double HighPressureThreshold = 21;

        Sensor _sensor = new Sensor();

        bool _alarmOn = false;
        private long _alarmCount = 0;


        public void Check()
        {
            double psiPressureValue = _sensor.PopNextPressurePsiValue();

            if (psiPressureValue < LowPressureThreshold || HighPressureThreshold < psiPressureValue)
            {
                _alarmOn = true;
                _alarmCount += 1;
            }
        }

        public bool AlarmOn
        {
            get { return _alarmOn; }
        }
    }
}
