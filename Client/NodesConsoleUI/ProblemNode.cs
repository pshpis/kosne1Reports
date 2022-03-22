using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using Newtonsoft.Json;

namespace Client.NodesConsoleUI
{
    public class ProblemNode : AbstractNode
    {
        public ProblemNode(AbstractNode parentNode) : base(parentNode, 7)
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
                Console.WriteLine("Problem node, Menu: \n 1.Create \n 2.Get my problems \n 3.Finish" +
                                  " \n 4.Change description \n 5.Rollback changing \n 6.Write comment \n 7.Exit");
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
                        await Finish();
                        break;
                    case 4:
                        await Change();
                        break;
                    case 5:
                        await WriteComment();
                        break;
                    case 6: 
                        await Exit();
                        break;
                }
            }
        }

        private void WriteProblemInfo(TaskModel problem)
        {
            Console.WriteLine($"--------Problem id: {problem.Id}---------");
            Console.WriteLine($"Problem name: {problem.Name}");
            Console.WriteLine($"Problem description: {problem.Description}");
            if (problem.Executor != null)
                Console.WriteLine($"Contractor id: {problem.Executor.Id}");
            Console.WriteLine("-------------------------------------------------");
        }

        private async Task Create()
        {
            try
            {
                Console.WriteLine("Write name and description");
                string name = Console.ReadLine();
                string desc = Console.ReadLine();
                
                HttpResponseMessage response = await Client.PostAsync($"{ServerPath}/problems?name={name}&desc={desc}", null);
            
                var responseStream = await response.Content.ReadAsStreamAsync();
                TaskModel problem = null;
                using (var readStream = new StreamReader(responseStream, Encoding.UTF8))
                {
                    var responseString = await readStream.ReadToEndAsync();
                    var settings = new JsonSerializerSettings
                    {
                        Formatting = Formatting.Indented,
                        DefaultValueHandling = DefaultValueHandling.Include,
                        NullValueHandling = NullValueHandling.Include,
                        TypeNameHandling = TypeNameHandling.Auto
                    };
                    problem = JsonConvert.DeserializeObject<TaskModel>(responseString, settings);
                }
                
                response = await Client.PatchAsync($"{ServerPath}/problems?problemId={problem.Id}&contractorId={EmployeeNode.CurrentEmployee.Id}", null);
                responseStream = await response.Content.ReadAsStreamAsync();
                using (var readStream = new StreamReader(responseStream, Encoding.UTF8))
                {
                    var responseString = await readStream.ReadToEndAsync();
                    problem = JsonConvert.DeserializeObject<TaskModel>(responseString);
                }
                
                Console.WriteLine("Problem created");
                WriteProblemInfo(problem);
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
                HttpResponseMessage response = await Client.GetAsync($"{ServerPath}/problems?contractorId={EmployeeNode.CurrentEmployee.Id}");
            
                var responseStream = await response.Content.ReadAsStreamAsync();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                
                var responseString = await readStream.ReadToEndAsync();
                List<TaskModel> problems = JsonConvert.DeserializeObject<List<TaskModel>>(responseString);
                Console.WriteLine($"You have {problems.Count} problems");
                problems.ForEach(WriteProblemInfo);
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
                Console.WriteLine("Write problem guid:");
                string guidString = Console.ReadLine();
                
                HttpResponseMessage response = await Client
                    .PatchAsync($"{ServerPath}/tasks/finish?taskId={guidString}&changerId={EmployeeNode.CurrentEmployee.Id}", null);
            
                Stream responseStream = await response.Content.ReadAsStreamAsync();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = await readStream.ReadToEndAsync();
                TaskModel problem = JsonConvert.DeserializeObject<TaskModel>(responseString);
                
                WriteProblemInfo(problem);
            }
            catch (WebException e)
            {
                await Console.Error.WriteLineAsync(e.Message);
            }
        }

        private async Task Change()
        {
            try
            {
                Console.WriteLine("Write changing problem id: ");
                string guidString = Console.ReadLine();
                Console.WriteLine("Write new description:");
                string newDescription = Console.ReadLine();
                
                HttpResponseMessage response = await Client
                    .PatchAsync($"{ServerPath}/tasks/description?taskId={guidString}" +
                               $"employeeId={EmployeeNode.CurrentEmployee.Id}&newDescription={newDescription}", null);
            
                Stream responseStream = await response.Content.ReadAsStreamAsync();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = await readStream.ReadToEndAsync();
                TaskChangeModel descriptionChange =
                    JsonConvert.DeserializeObject<TaskChangeModel>(responseString);
                
                Console.WriteLine($"Id of this change: {descriptionChange.Id}");
            }
            catch (WebException e)
            {
                await Console.Error.WriteLineAsync(e.Message);
            }
        }

        private async Task WriteComment()
        {
            try
            {
                Console.WriteLine("Write problem id and comment text");
                string guidString = Console.ReadLine();
                string text = Console.ReadLine();
                
                HttpResponseMessage response = await Client
                    .PostAsync($"{ServerPath}/tasks/comment?taskId={guidString}" +
                               $"&employeeId={EmployeeNode.CurrentEmployee.Id}&text={text}", null);
            
                Stream responseStream = await response.Content.ReadAsStreamAsync();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = await readStream.ReadToEndAsync();
                TaskCommentModel comment =
                    JsonConvert.DeserializeObject<TaskCommentModel>(responseString);
                
                Console.WriteLine($"Was created comment with id: {comment.Id}");
            }
            catch (WebException e)
            {
                await Console.Error.WriteLineAsync(e.Message);
            }
        }
    }
}