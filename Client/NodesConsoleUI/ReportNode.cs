using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Client.NodesConsoleUI;
using DAL.Models;
using Newtonsoft.Json;

namespace Reports.Clients.NodesConsoleUI
{
    public class ReportNode : AbstractNode
    {
        public ReportNode(AbstractNode parentNode) : base(parentNode, 6)
        {
        }

        public override async Task Launch()
        {
            if (EmployeeNode.CurrentEmployee == null)
            {
                Console.WriteLine("Login first");
                Exit();
            }
            while (true)
            {
                Console.WriteLine("Report node, Menu: \n 1.Create \n 2.Get reports of my department" +
                                  " \n 3.Create report for sprint \n 4.Exit");
                int point = ReadMenuPoint();
                switch (point)
                {
                    case 1:
                        await Create();
                        break;
                    case 2:
                        await Get();
                        break;
                    case 3:
                        await CreateSprintReport();
                        break;
                    case 4:
                        await Exit();
                        break;
                }
            }
        }

        private void WriteReportInfo(ReportModel report)
        {
            Console.WriteLine($"--------Report id: {report.Id}---------");
            Console.WriteLine($"Report creator: {report.Employee.Id}");
            Console.WriteLine("-------------------------------------------------");
        }

        private async Task Create()
        {
            try
            {
                Console.WriteLine("Write description");
                string desc = Console.ReadLine();
                
                HttpResponseMessage response = await Client.PostAsync($"{ServerPath}/reports/full?description={desc}&creatorId={EmployeeNode.CurrentEmployee.Id}", null);
            
                var responseStream = await response.Content.ReadAsStreamAsync();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                var responseString = await readStream.ReadToEndAsync();
                Console.WriteLine(responseString);
                ReportModel report = JsonConvert.DeserializeObject<ReportModel>(responseString);
                
                Console.WriteLine("Report was created");
                WriteReportInfo(report);
            }
            catch (WebException e)
            {
                await Console.Error.WriteLineAsync(e.Message);
            }
        }

        private async Task Get()
        {
            try
            {
                HttpResponseMessage response = await Client.GetAsync($"{ServerPath}/reports?bossId={EmployeeNode.CurrentEmployee.Id}");
            
                var responseStream = await response.Content.ReadAsStreamAsync();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                
                var responseString = await readStream.ReadToEndAsync();
                List<ReportModel> reports = JsonConvert.DeserializeObject<List<ReportModel>>(responseString);
                Console.WriteLine($"You have {reports.Count} reports");
                reports.ForEach(WriteReportInfo);
            }
            catch (WebException e)
            {
                await Console.Error.WriteLineAsync(e.Message);
            }
        }

        private async Task Finish()
        {
            try
            {
                Console.WriteLine("Write report guid:");
                string guidString = Console.ReadLine();
                
                HttpResponseMessage response = await Client
                    .PatchAsync($"{ServerPath}/reports/finish?reportId={guidString}&finisherId={EmployeeNode.CurrentEmployee.Id}", null);
                
                Console.WriteLine("Report was finished");
            }
            catch (WebException e)
            {
                await Console.Error.WriteLineAsync(e.Message);
            }
        }

        private async Task Check()
        {
            try
            {
                Console.WriteLine("Write checking report id: ");
                string guidString = Console.ReadLine();

                HttpResponseMessage response = await Client
                    .PatchAsync($"{ServerPath}/reports/check?reportId={guidString}" +
                               $"&checkerId={EmployeeNode.CurrentEmployee.Id}", null);
                
                Console.WriteLine($"Report was checked");
            }
            catch (WebException e)
            {
                await Console.Error.WriteLineAsync(e.Message);
            }
        }

        private async Task CreateSprintReport()
        {
            try
            {
                HttpResponseMessage response = await Client
                    .PostAsync(
                        $"{ServerPath}/reports/sprintReport?creatorId={EmployeeNode.CurrentEmployee.Id}",
                        null);
            
                Stream responseStream = await response.Content.ReadAsStreamAsync();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = await readStream.ReadToEndAsync();

                if (response.IsSuccessStatusCode)
                {
                    
                    SprintReportModel sprintReport = JsonConvert.DeserializeObject<SprintReportModel>(responseString);
                    Console.WriteLine($"Sprint report {sprintReport.Id} created");
                }
                else
                {
                    Console.WriteLine("Creating was failed");
                }
            }
            catch (WebException e)
            {
                await Console.Error.WriteLineAsync(e.Message);
            }
        }
    }
}