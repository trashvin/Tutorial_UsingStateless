using System;
using Xunit;

using Lightswitch;
using System.ComponentModel;

namespace Stateless.Tests
{
    public class LightswitchTests
    {
        [Theory]
        [InlineData(false, ALightSwitch.State.ON)]
        [InlineData(true, ALightSwitch.State.OFF)]
        public void TraditionalSwitchWithoutConstraintTest(bool initialState,
                                         Lightswitch.ALightSwitch.State expected)
        {
            TraditionalLightSwitch testSwitch = new TraditionalLightSwitch("Traditional Switch", initialState);

            testSwitch.ToggleSwitch();

            Assert.Equal(expected, testSwitch.CurrentState);
        }

        [Theory]
        [InlineData(false, ALightSwitch.State.ON)]
        [InlineData(true, ALightSwitch.State.OFF)]
        public void StatelessSwitchWithoutConstraintTest(bool initialState, ALightSwitch.State expected)
        {
            StatelessLightSwitch testSwitch = new StatelessLightSwitch("Stateless Switch", initialState);

            testSwitch.ToggleSwitch();

            Assert.Equal(expected, testSwitch.CurrentState);
        }

        [Theory]
        [InlineData(false, "4:0:0", ALightSwitch.State.ON)]
        [InlineData(false, "9:0:0", ALightSwitch.State.OFF)]
        [InlineData(true, "3:0:0", ALightSwitch.State.OFF)]
        [InlineData(true, "11:0:0", ALightSwitch.State.OFF)]
        [InlineData(true, "19:0:0", ALightSwitch.State.OFF)]
        [InlineData(false, "22:0:0", ALightSwitch.State.ON)]
        public void TraditionalSwitchWithConstraintTest(bool initialState, string testTime, ALightSwitch.State expected)
        {
            TraditionalLightSwitch testSwitch = new TraditionalLightSwitch("Traditinal Switch", initialState, true);
            TimeSpan time = TimeSpan.Parse(testTime);

            testSwitch.SetTimeOfDay(time);
            testSwitch.ToggleSwitch();

            Assert.Equal(expected, testSwitch.CurrentState);

        }

        [Theory]
        [InlineData(false, "4:0:0", ALightSwitch.State.ON)]
        [InlineData(false, "9:0:0", ALightSwitch.State.OFF)]
        [InlineData(true, "3:0:0", ALightSwitch.State.OFF)]
        [InlineData(true, "11:0:0", ALightSwitch.State.OFF)]
        [InlineData(true, "19:0:0", ALightSwitch.State.OFF)]
        [InlineData(false, "22:0:0", ALightSwitch.State.ON)]
        public void StatelessSwitchWithConstraintTest(bool initialState, string testTime, ALightSwitch.State expected)
        {
            StatelessLightSwitch testSwitch = new StatelessLightSwitch("Traditinal Switch", initialState, true);
            TimeSpan time = TimeSpan.Parse(testTime);

            testSwitch.SetTimeOfDay(time);
            testSwitch.ToggleSwitch();

            Assert.Equal(expected, testSwitch.CurrentState);
        }
    }
}
