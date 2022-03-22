using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using Newtonsoft.Json;

namespace Client.NodesConsoleUI
{
    public class EmployeeNode : AbstractNode
    {
        public static EmployeeModel CurrentEmployee = null;
        
        public EmployeeNode(AbstractNode parentNode) : base(parentNode, 5)
        {
        }

        public override async Task Launch()
        {
            while (true)
            {
                Console.WriteLine(" Employee node, Menu: \n 1.Signup \n 2.Login \n 3.Take commander \n 4.My info \n 5.Exit");
                int point = ReadMenuPoint();
                switch (point)
                {
                    case 1:
                        await Signup();
                        break;
                    case 2:
                        await Login();
                        break;
                    case 3:
                        await TakeCommander();
                        break;
                    case 4:
                        await GetInfo();
                        break;
                    case 5:
                        await Exit();
                        break;
                }
            }
        }

        private async Task Signup()
        {
            try
            {
                Console.WriteLine("Write your full name");
                string name = Console.ReadLine();

                HttpResponseMessage response = await Client.PostAsync($"{ServerPath}/employees?name={@name}", new StringContent(name));
            
                var responseStream = await response.Content.ReadAsStreamAsync();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                var responseString = await readStream.ReadToEndAsync();

                EmployeeModel employee = JsonConvert.DeserializeObject<EmployeeModel>(responseString);

                if (employee != null)
                {
                    Console.WriteLine("Created employee:");
                    Console.WriteLine($"Id: {employee.Id}");
                    Console.WriteLine($"Name: {employee.Name}");
                    CurrentEmployee = employee;
                }
                else
                {
                    Console.WriteLine("Couldn't create employee");
                }
            }
            catch (WebException e)
            {
                await Console.Error.WriteLineAsync(e.Message);
            }
            
        }

        private async Task Login()
        {
            try
            {
                Console.WriteLine("Write your guid");
                string guidString = Console.ReadLine();

                HttpResponseMessage response =
                    await Client.GetAsync($"{ServerPath}/employees?id={guidString}");
            
                var responseStream = await response.Content.ReadAsStreamAsync();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                var responseString = await readStream.ReadToEndAsync();

                CurrentEmployee = JsonConvert.DeserializeObject<EmployeeModel>(responseString);

                if (CurrentEmployee != null)
                {
                    Console.WriteLine("Your info:");
                    Console.WriteLine($"Id: {CurrentEmployee.Id}");
                    Console.WriteLine($"Name: {CurrentEmployee.Name}");
                    
                    if (CurrentEmployee.Boss != null)
                    {
                        Console.WriteLine($"Commander Id: {CurrentEmployee.Boss.Id}");
                        Console.WriteLine($"Commander Name: {CurrentEmployee.Boss.Name}");
                    }
                    else
                    {
                        Console.WriteLine("Commander: -");
                    }
                }
                else
                {
                    Console.WriteLine("Couldn't get your info");
                }
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private async Task GetInfo()
        {
            try
            {
                if (CurrentEmployee == null)
                {
                    Console.WriteLine("Login please");
                    return;
                }
                
                HttpResponseMessage response =
                    await Client.GetAsync($"{ServerPath}/employees?id={CurrentEmployee.Id}");
            
                var responseStream = await response.Content.ReadAsStreamAsync();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                var responseString = await readStream.ReadToEndAsync();

                var employee = JsonConvert.DeserializeObject<EmployeeModel>(responseString);

                if (employee != null)
                {
                    Console.WriteLine("Your info:");
                    Console.WriteLine($"Id: {employee.Id}");
                    Console.WriteLine($"Name: {employee.Name}");
                    Console.WriteLine($"CommanderId: {employee.Boss?.Id}");
                }
                else
                {
                    Console.WriteLine("Couldn't get your info");
                }
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private async Task TakeCommander()
        {
            try
            {
                Console.WriteLine("Write commander guid");
                string guidString = Console.ReadLine();
            
                HttpResponseMessage response =
                    await Client.PatchAsync($"{ServerPath}/employees/boss?id={CurrentEmployee.Id}&bossId={guidString}", null);
            
                var responseStream = await response.Content.ReadAsStreamAsync();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                var responseString = await readStream.ReadToEndAsync();

                var employee = JsonConvert.DeserializeObject<EmployeeModel>(responseString);

                if (employee != null)
                {
                    Console.WriteLine("Update employee:");
                    Console.WriteLine($"Id: {employee.Id}");
                    Console.WriteLine($"Name: {employee.Name}");
                    Console.WriteLine($"Commander Id: {employee.Boss.Id}");
                    Console.WriteLine($"Commander Name: {employee.Boss.Name}");
                    CurrentEmployee = employee;
                }
                else
                {
                    Console.WriteLine("Couldn't update employee");
                }
            }
            catch (WebException e)
            {
                await Console.Error.WriteLineAsync(e.Message);
            }
        }
    }
}