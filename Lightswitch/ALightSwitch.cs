using System;

namespace Lightswitch
{
    public abstract class ALightSwitch
    {
        protected TimeSpan _time;
        protected bool _enforceTimeConstraint;
        protected string _name;

        public State CurrentState = State.OFF;
        public enum State { ON, OFF };

        public ALightSwitch(string name,bool initialState = false, bool enableTimeConstraint = false)
        {
            if (initialState) CurrentState = State.ON;
            _enforceTimeConstraint = enableTimeConstraint;

			Console.WriteLine("--------------------------------------");
			Console.WriteLine(name + " switch initial state : " + CurrentState.ToString());
            Console.WriteLine("Enable Time Constraint : " + enableTimeConstraint.ToString());

		}

        private TimeSpan GetCurrentTime()
        {
            if (_time == new TimeSpan(0, 0, 0))
            {
                return DateTime.Now.TimeOfDay;
            }
            else
            {
                return _time;
            }
        }

        private bool IsDaylight(TimeSpan time)
        {
            if (time.Hours >= 6 && time.Hours <= 18)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetTimeOfDay(TimeSpan time)
        {
            _time = time;

        }

        public abstract void ToggleSwitch();

        protected bool IsLightNeeded()
        {
            bool result = false;
            if (_enforceTimeConstraint)
            {
                result = !IsDaylight(GetCurrentTime());
            }
            else result = true;

            return result;
        }
    }
}
