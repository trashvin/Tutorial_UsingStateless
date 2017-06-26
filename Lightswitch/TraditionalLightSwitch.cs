using System;
using Stateless;

namespace Lightswitch
{
    public class TraditionalLightSwitch : ALightSwitch
    {
        public TraditionalLightSwitch(string name, bool initialState = false, bool enforceTimeConstraint = false)
            :base(name,initialState, enforceTimeConstraint)
        {
            _time = new TimeSpan(0, 0, 0);
        }

        public override void ToggleSwitch()
        {
            
            if (CurrentState == State.ON)
            {
                CurrentState = State.OFF;
            }
            else
            {
                if(_enforceTimeConstraint)
                {
                    if (IsLightNeeded()) CurrentState = State.ON;
                }
                else
                {
                    CurrentState = State.ON;
				}

            }

            Console.WriteLine("Light is " + CurrentState.ToString());
        }
    }
}
