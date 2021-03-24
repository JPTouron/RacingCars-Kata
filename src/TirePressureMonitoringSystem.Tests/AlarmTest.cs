using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace TDDMicroExercises.TirePressureMonitoringSystem
{
    public class AlarmTest
    {
        private static IAlarmThresholds thresholds;
        private Alarm sut;

        public AlarmTest()
        {
            int pressureHighThreshold = 21;
            int pressureLowThreshold = 17;

            thresholds = new AlarmThresholds(pressureLowThreshold, pressureHighThreshold);
        }

        [Fact]
        public void AlarmCount_WhenPressureIsOutOfBounds_ThenAlarmCountIncreasesByOne()
        {
            var sensor = new Mock<ISensor>();
            int pressureValueOutOfBounds = 0;

            sensor.Setup(x => x.PopNextPressurePsiValue()).Returns(pressureValueOutOfBounds);
            sut = new Alarm(sensor.Object, thresholds);

            int testCount = 3;
            for (int i = 1; i <= testCount; i++)
            {
                int expectedAlarmCount = i;

                sut.Check();

                Assert.True(sut.AlarmOn);
                Assert.Equal(expectedAlarmCount, sut.AlarmCount);
            }
        }

        [Theory]
        [ClassData(typeof(PressureValuesProvider))]
        public void Check_WhenPressureIsBelowLowPressureThreshold_ThenAlarmIsOn(int pressureValue, bool expectedAlarmState)
        {
            var sensor = new Mock<ISensor>();

            sensor.Setup(x => x.PopNextPressurePsiValue()).Returns(pressureValue);
            sut = new Alarm(sensor.Object, thresholds);

            sut.Check();

            Assert.Equal(expectedAlarmState, sut.AlarmOn);
        }

        [Fact]
        public void Create_WhenInitializedAlarmIsOff()
        {
            var sensor = new Mock<ISensor>();

            sut = new Alarm(sensor.Object, thresholds);
            Assert.False(sut.AlarmOn);
        }

        private class PressureValuesProvider : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                foreach (var testCaseData in PressuresBelowLowThreshold())
                    yield return testCaseData;

                foreach (var testCaseData in PressuresAboveHighThreshold())
                    yield return testCaseData;

                foreach (var testCaseData in PressuresWithinThresholds())
                    yield return testCaseData;
            }

            public double GetRandomNumber(double minimum, double maximum)
            {
                Random random = new Random();
                return random.NextDouble() * (maximum - minimum) + minimum;
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            private IEnumerable<object[]> PressuresAboveHighThreshold()
            {
                const bool expectedAlarmState = true;
                const int pressureJustAboveHighThreshold = 22;
                yield return new object[] { int.MaxValue, expectedAlarmState };
                yield return new object[] { pressureJustAboveHighThreshold, expectedAlarmState };
            }

            private IEnumerable<object[]> PressuresBelowLowThreshold()
            {
                const bool expectedAlarmState = true;
                const int pressureJustBelowLowThreshold = 16;
                yield return new object[] { int.MinValue, expectedAlarmState };
                yield return new object[] { pressureJustBelowLowThreshold, expectedAlarmState };
                yield return new object[] { 0, expectedAlarmState };
            }

            private IEnumerable<object[]> PressuresWithinThresholds()
            {
                const bool expectedAlarmState = false;
                var pressureLowThreshold = thresholds.LowThreshold;
                var pressureHighThreshold = thresholds.HighThreshold;

                var pressureVal = GetRandomNumber(pressureLowThreshold, pressureHighThreshold);

                yield return new object[] { pressureVal, expectedAlarmState };
                yield return new object[] { pressureHighThreshold, expectedAlarmState };
                yield return new object[] { pressureLowThreshold, expectedAlarmState };
            }
        }
    }
}