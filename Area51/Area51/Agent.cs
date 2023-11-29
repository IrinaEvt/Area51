using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area51
{
    public class Agent
    {
        private Elevator Elevator { get; set; }
        public SecurityLevel SecurityLevel { get; set; }
        public Floor CurrentFloor { get; set; }

        public Floor Destination { get; set; }
        public string Name { get; private set; }




        public Agent(string name){
            Name = name;
            var intSecurityLevel = Random.Shared.Next(Enum.GetValues(typeof(SecurityLevel)).Length);
            SecurityLevel = (SecurityLevel)intSecurityLevel;
            int intCurrentFloor = 0;int intDestination = 0;
            while (intCurrentFloor == intDestination)
            {
                intCurrentFloor = Random.Shared.Next(Enum.GetValues(typeof(Floor)).Length);
                intDestination = Random.Shared.Next(Enum.GetValues(typeof(Floor)).Length);
                
            } ;
            CurrentFloor = (Floor)intCurrentFloor;
            Destination = (Floor)intDestination;
 
            
        }
        public void CallElevator(Elevator elevator)
        {
            
            Elevator = elevator;
            elevator.CallElevator(this);
           
        }

        public void EnterElevator(Elevator elevator)

        {
            Elevator = elevator;
            elevator.EnterElevator(this);
            
            
        }
        public void LeaveElevator(Elevator elevator)
        {
            Elevator = elevator;
            elevator.LeaveElevator(this);
          
        }
    }
}
