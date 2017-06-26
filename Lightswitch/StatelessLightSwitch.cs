using System;
using Stateless;

namespace Lightswitch 
{
    public class StatelessLightSwitch : ALightSwitch
    {
        
        enum Trigger { TOGGLE };

        StateMachine<ALightSwitch.State, Trigger> _machine;

		public StatelessLightSwitch(string name, bool initialState = false, bool enforceTimeConstraint = false)
			: base(name, initialState, enforceTimeConstraint)
		{
			_time = new TimeSpan(0, 0, 0);

			_machine = new StateMachine<State, Trigger>(() => CurrentState, s => CurrentState = s);

			_machine.Configure(State.ON)
					.Permit(Trigger.TOGGLE, State.OFF);

			_machine.Configure(State.OFF)
					.PermitIf(Trigger.TOGGLE, State.ON, () => IsLightNeeded(), "Toggle allowed")
					.PermitReentryIf(Trigger.TOGGLE, () => !IsLightNeeded(), "Toggle not allowed");


		}

        public override void ToggleSwitch()
        {
            _machine.Fire(Trigger.TOGGLE);

            Console.WriteLine("Switch is " + CurrentState.ToString());
        }

        public string ShowStateMachine()
        {
            return _machine.ToDotGraph();
        }
    }
}
