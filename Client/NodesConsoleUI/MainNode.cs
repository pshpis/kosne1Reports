using System;
using System.Threading.Tasks;
using Reports.Clients.NodesConsoleUI;

namespace Client.NodesConsoleUI
{
    public class MainNode : AbstractNode
    {
        private readonly EmployeeNode _employeeNode;
        private readonly ProblemNode _problemNode;
        private readonly ReportNode _reportNode;
        public MainNode() : base(null, 4)
        {
            _problemNode = new ProblemNode(this);
            _employeeNode = new EmployeeNode(this);
            _reportNode = new ReportNode(this);
        }

        public override async Task Launch()
        {
            while (true)
            {
                Console.WriteLine(" Main Menu: \n 1.Employees \n 2.Problems \n 3.Reports \n 4.Exit");
                int point = ReadMenuPoint();
                switch (point)
                {
                    case 1:
                        await _employeeNode.Launch();
                        break;
                    case 2:
                        await _problemNode.Launch();
                        break;
                    case 3:
                        await _reportNode.Launch();
                        break;
                    case 4:
                        Exit();
                        break;
                }
            }
        }

        public new void Exit()
        {
            Environment.Exit(0);
        }
    }
}