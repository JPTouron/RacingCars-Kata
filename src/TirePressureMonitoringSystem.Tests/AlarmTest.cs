using Xunit;

namespace TDDMicroExercises.TirePressureMonitoringSystem
{
    public class AlarmTest
    {
        [Fact]
        public void Create_WhenInitializedAlarmIsOff()
        {
            Alarm alarm = new Alarm();
            Assert.False(alarm.AlarmOn);
        }


    }
}