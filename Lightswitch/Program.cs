using System;

namespace Lightswitch
{
    class Program
    {
        static void Main(string[] args)
        {
            TraditionalLightSwitch mySwitch1 = new TraditionalLightSwitch("Traditional Switch",true,true);
            StatelessLightSwitch stateSwitch = new StatelessLightSwitch("Stateless Switch",true,true);

            Console.WriteLine("\n Toggling traditional switch at predefined time....\n");
            StartSwitchAtDefinedTime(mySwitch1);

            Console.WriteLine("\n Toggling stateless switch at predefined time...\n");
            StartSwitchAtDefinedTime(stateSwitch);
            Console.WriteLine("Graph >>>>>>>>>>>.");
            Console.WriteLine(stateSwitch.ShowStateMachine());


            Console.Read();
        }

        static void StartSwitchAtDefinedTime(ALightSwitch mySwitch)
        {
            TimeSpan[] times =
            {
                new TimeSpan(4,30,30),
                new TimeSpan(9,45,0),
                new TimeSpan(14,2,6),
                new TimeSpan(19,21,34),
                new TimeSpan(23,46,0)
            };

            foreach(var time in times)
            {
                Console.WriteLine("Toggling switch at " + time.ToString());
                mySwitch.SetTimeOfDay(time);
                mySwitch.ToggleSwitch();
            }
        }

        static void StartSwitchNow(ALightSwitch mySwitch)
        {
            for (int i = 0; i < 3; i++ )
            {
                Console.WriteLine("Toggling switch now : "+ DateTime.Now.TimeOfDay.ToString());
                mySwitch.ToggleSwitch();
            }
        }
    }
}
