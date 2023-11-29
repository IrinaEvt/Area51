using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area51
{
    public class Elevator
    {
        private Floor currentFloor;
        private Door door;
        private Agent agentInside;
        private Semaphore semaphore;
       private ManualResetEvent leavingSignal;
        static object objLock = new object();
       

        public Elevator()
        {
            currentFloor = Floor.G;
            door = new Door();
            semaphore = new Semaphore(1, 1);
             leavingSignal = new ManualResetEvent(false);
           
        }

        public void CallElevator(Agent agent)
        {
          
            Console.WriteLine($"Agent {agent.Name} is waiting for the elevator on floor {agent.CurrentFloor}- Destination {agent.Destination}");

            MoveTo(agent.CurrentFloor, agent);
            

        }

        public void PressButton(Floor floor, Agent agent) 
        {
            MoveTo(floor, agent);           

        }

        public void EnterElevator(Agent agent)
        {

            semaphore.WaitOne();
            agentInside = agent;
            Console.WriteLine($"Agent {agent.Name} entered the elevator on floor {agent.CurrentFloor} - Destination {agent.Destination}");

            door.Close();

            PressButton(agent.Destination, agent);


        }



        public void LeaveElevator(Agent agent)
        {
            door.Open(agent);
            if (door.isOpen)
            {
                agentInside = null;

                Console.WriteLine($"Agent {agent.Name} leaves the elevator on floor {agent.Destination}");


                semaphore.Release();



            }
            else
            {
                Console.WriteLine($"Agent {agent.Name} does not have the required credentials to exit the elevator on floor {currentFloor} - Destination {agent.Destination}");
                int intDestination = Random.Shared.Next(Enum.GetValues(typeof(Floor)).Length);
                while ((Floor)intDestination == agent.Destination)
                {
                    intDestination = Random.Shared.Next(Enum.GetValues(typeof(Floor)).Length);

                }
                agent.Destination = (Floor)intDestination;
                Console.WriteLine($"The agent pressed the button for floor - {agent.Destination}");
                leavingSignal.WaitOne();
                PressButton(agent.Destination, agent);
                this.LeaveElevator(agent);
            }
        }

        public void MoveTo(Floor floor, Agent agent)
        {

            lock (objLock)
            {
                while (currentFloor != floor)
                {
                    Thread.Sleep(1000);
                    if (currentFloor < floor)
                    {
                        currentFloor++;
                    }
                    else
                    {
                        currentFloor--;
                    }
                }
                Console.WriteLine($"The elevator reaches the floor {currentFloor} - agent {agent.Name}");
            }

        }

    

        public class Door
        {
            public bool isOpen;

            public Door()
            {
                isOpen = false;
            }

            public bool IsOpen
            {
                get { return isOpen; }
            }

            public void Open(Agent agent)
            {
                Floor destination = agent.Destination;

                /*Confidential can access only G floor
                  Secret can access G and S
                  Top-secret can access G, S, T1 and T2*/

                if (agent.SecurityLevel == SecurityLevel.TopSecret)
                {
                    isOpen = true;
                }
                else if (agent.SecurityLevel == SecurityLevel.Secret)
                {
                    if (destination == Floor.G) { isOpen = true; }
                    else if (destination == Floor.S) { isOpen = true; }
                    else { isOpen = false; }
                }
                else if (agent.SecurityLevel == SecurityLevel.Confidential)
                {
                    if (destination == Floor.G) { isOpen = true; }
                    else { isOpen = false; }
                }
                else
                {
                    isOpen = false;
                }
            }

            public void Close()
            {
                isOpen = false;
            }
        }
    }
}
