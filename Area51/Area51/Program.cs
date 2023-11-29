namespace Area51
{
    public enum SecurityLevel
    {
        Confidential,
        Secret,
        TopSecret
    }
    public enum Floor
    {
        G,
        S,
        T1,
        T2
    }


    class Program
    {

        static void Main()
        {
            const int AgentCount = 10;

            Elevator elevator = new Elevator();
            List<Thread> agentThreads = new List<Thread>(AgentCount);

            for (int i = 0; i < AgentCount; i++)
            {
                Agent agent = new Agent(i.ToString());
                agentThreads.Add(new Thread(() =>
               {
                    agent.CallElevator(elevator);
                    agent.EnterElevator(elevator);
                agent.LeaveElevator(elevator);
                       
               }));
            }
           foreach (var agentThread in agentThreads)
            {
                agentThread.Start();
                Thread.Sleep(10000);
            }

            foreach (var agentThread in agentThreads)
            {
                agentThread.Join();
            }
         
            Console.ReadLine();
        }

    }
}
