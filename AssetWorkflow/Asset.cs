using System;
using Stateless;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;

namespace AssetWorkflow
{
    public class Asset
    {
        public enum State { New, Available, Allocated, UnderMaintenance,Unavailable, Decommissioned };
        public enum Trigger { Tested, Assigned, Released, RequestRepair, RequestUpdate,Transferred, Repaired, Lost, Discarded, Found };

        protected State _state;
        protected StateMachine<State, Trigger> _machine;
        protected State _previousState;
        protected bool _isSuccesful;

        public AssetInformation AssetData { get; set; }
        public Person OwnerData { get; set; }

        protected StateMachine<State, Trigger>.TriggerWithParameters<Person> _assignTrigger;
        protected StateMachine<State, Trigger>.TriggerWithParameters<Person> _transferTrigger;


        public State AssetState
        {
            get
            {
                return _state;
            }
            set
            {
                _previousState = _state;
                _state = value;
                Console.WriteLine("------------------------");
                Console.WriteLine($"Asset No : {AssetData.AssetID.ToString()}");
                Console.WriteLine($"Previous asset state : {_previousState.ToString()}");
                Console.WriteLine($"New asset state : {_state.ToString()}" );
            }
        }

        public Asset(AssetInformation data)
        {
            InitializeStateMachine();
            AssetData = data;

        }

        private void InitializeStateMachine()
        {
			_state = State.New;


			_machine = new StateMachine<State, Trigger>(() => AssetState, s => AssetState = s);

			_assignTrigger = _machine.SetTriggerParameters<Person>(Trigger.Assigned);
			_transferTrigger = _machine.SetTriggerParameters<Person>(Trigger.Transferred);


			_machine.Configure(State.New)
                    .Permit(Trigger.Tested, State.Available)
                    .OnEntry(()=>OnEntry())
                    .OnActivate(() => OnActivate())
                    .Permit(Trigger.Lost, State.Unavailable)
                    .OnDeactivate(()=>OnDeactivate())
                    .OnExit(() => OnExit());

            _machine.Configure(State.Available)
                     .OnEntry(() => OnEntry())
                     .OnActivate(() => OnActivate())
                     .Permit(Trigger.Assigned, State.Allocated)
                     .Permit(Trigger.Lost, State.Unavailable)
                     .OnExit(() => OnExit())
                     .OnEntryFrom(Trigger.Found,()=> ProcessFound())
                    .OnEntryFrom(Trigger.Released,() => ProcessDecommission())
                     .OnDeactivate(() => OnDeactivate());


            _machine.Configure(State.Allocated)
                    .OnEntry(()=>OnEntry())
                    .OnEntryFrom(_assignTrigger, owner => SetOwner(owner))
                    .OnEntryFrom(_transferTrigger, owner => SetOwner(owner))
                    .OnActivate(()=> OnActivate())
                    .OnExit(()=>OnExit())
                    .OnDeactivate(()=>OnDeactivate())
                    .PermitReentry(Trigger.Transferred)
                    .Permit(Trigger.Released, State.Available)
                    .Permit(Trigger.RequestRepair, State.UnderMaintenance)
                    .Permit(Trigger.RequestUpdate, State.UnderMaintenance)
                    .Permit(Trigger.Lost, State.Unavailable);

			_machine.Configure(State.UnderMaintenance)
					.OnEntry(() => OnEntry())
					.OnActivate(() => OnActivate())
					.OnExit(() => OnExit())
					.OnDeactivate(() => OnDeactivate())
                    .Permit(Trigger.Repaired, State.Allocated)
                    .Permit(Trigger.Lost,State.Unavailable)
                    .Permit(Trigger.Discarded, State.Decommissioned);


            _machine.Configure(State.Unavailable)
					.OnEntry(() => OnEntry())
					.OnActivate(() => OnActivate())
					.OnExit(() => OnExit())
					.OnDeactivate(() => OnDeactivate())
                    .PermitIf(Trigger.Found, State.Available,()=>(_previousState != State.New))
                    .PermitIf(Trigger.Found,State.New,()=>(_previousState == State.New));

            _machine.Configure(State.Decommissioned)
                    .OnEntry(() => ProcessDecommission())
                    .OnActivate(() => OnActivate())
                    .OnExit(() => OnExit())
                    .OnDeactivate(() => OnDeactivate());




		}

        private void SetOwner(Person owner)
        {
            AssetData.Owner = owner;
        }

        private void ProcessDecommission()
        {
            Console.WriteLine("Clearing owner date..");
            AssetData.Owner = null;
            OnEntry();
        }

        private void ProcessFound()
        {
            if (AssetData.Owner != null)
            {
                Console.WriteLine("Clearing the owner data...");
                AssetData.Owner = null;
            }    
        }

        private void OnEntry()
        {
            Console.WriteLine($"Entering {_state.ToString()} ...");
        }

        private void OnActivate()
        {
            Console.WriteLine($"Activating {_state.ToString()} ...");
        }

        private void OnDeactivate()
        {
            Console.WriteLine($"Deactivating {_state.ToString()} ...");
        }

        private void OnExit()
        {
            Console.WriteLine($"Exiting {_state.ToString()} ...");
        }

        public void Fire(Trigger trigger)
        {
            _isSuccesful = false;
            try
            {
                _machine.Fire(trigger);           
                _isSuccesful = true;
            }
            catch
            {
                Console.WriteLine("Error during state transition.");
                _isSuccesful = false;
            }
        }

        public void FinishedTesting()
        {
            Fire(Trigger.Tested);
        }

        public void Assign(Person owner)
        {
			_isSuccesful = false;
            try
            {
                _machine.Fire(_assignTrigger, owner);
                _isSuccesful = true;
            }
            catch
            {
                Console.WriteLine("Error during state transition.");
                _isSuccesful = false;
            }
        }

        public void Release()
        {
            Fire(Trigger.Released); 
        }

        public void Repaired()
        {
            Fire(Trigger.Repaired);
        }

        public void RequestRepair()
        {
            Fire(Trigger.RequestRepair);
        }

        public void RequestUpdate()
        {
            Fire(Trigger.RequestUpdate);
        }

        public void Transfer(Person owner)
        {
            _isSuccesful = false;
            try
            {
                _machine.Fire(_transferTrigger,owner);
                _isSuccesful = true;
            }
			catch
			{
				Console.WriteLine("Error during state transition.");
				_isSuccesful = false;
			}
        }

        public void Lost()
        {
            Fire(Trigger.Lost);
        }

        public void Found()
        {
            Fire(Trigger.Found);
        }

        public void Discard()
        {
            Fire(Trigger.Discarded);
        }

        public string GetDOTGraph()
        {
            return _machine.ToDotGraph();
        }

        public bool IsSuccessful()
        {
            return _isSuccesful;
        }

    }
}
