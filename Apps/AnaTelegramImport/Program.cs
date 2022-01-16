using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using AnaCsharp.Dal.Interfaces.Repositories.Commands;
using AnaCsharp.Dal.Interfaces.Repositories.Queries;
using AnaCSharp.BLL.Services;
using AnaCSharp.DAL;
using AnaCSharp.DAL.Repositories;
using Microsoft.AspNetCore.Html;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AnaTelegramImport
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            Console.WriteLine($"Current Directory: {currentDirectory}");

            var files = Directory.GetFiles(Path.Combine(currentDirectory, "Input"));

            // prepare AnaService
            var connection = @"Server=localhost,1433; Database = ana; User = sa; Password = Your_password123";

            var services = new ServiceCollection();
            services
                .AddDbContext<AnaContext>(
                    options => options.UseSqlServer(connection), ServiceLifetime.Transient, ServiceLifetime.Transient)

                // services
                .AddTransient<AnaService>()

                // repositories
                .AddTransient<IDeterminedWordCommandRepository, DeterminedWordRepository>()
                .AddTransient<IDeterminedWordQueryRepository, DeterminedWordRepository>()
                .AddTransient<IDeterminingStateQueryRepository, DeterminingStateRepository>()
                .AddTransient<DeterminingStateRepository>()
                .AddTransient<WordRepository>()
                ;

            var serviceProvider = services.BuildServiceProvider();

            foreach (var file in files)
            {
                Console.WriteLine($"Current File: {file}");

                // clean files: current telegram export format is not compliant with XDocument.Load
                Console.WriteLine($"\tclean");
                var lines = File.ReadAllLines(file);
                lines = lines.Select(x => x.Replace("<br>", " ")).ToArray();
                lines = lines.Select(x => x.Replace("&laquo;", "")).ToArray();
                lines = lines.Select(x => x.Replace("&raquo;", "")).ToArray();
                //lines = lines.Select(x => x.Replace("<div class=\"page_body chat_page\">", " ")).ToArray();
                //lines = lines.Select(x => x.Replace("<div class=\"page_wrap\">", " ")).ToArray();
                //lines = lines.Take(lines.Length - 3).ToArray();
                File.WriteAllLines(file, lines);

                // import
                Console.WriteLine($"\timport");
                XDocument doc = XDocument.Load(file);

                var elemList = doc.XPathSelectElements($"//div[@class='text']").ToList();
                var elemListDeHtmlised = elemList.Select(x => new HtmlString(x.Value).Value);

                var previousMessage = "";

                var tasks = new List<Task>();
                var inc = 0;
                var total = elemListDeHtmlised.Count();
                Console.Write("\t{0}% - {1}/{2}    ", inc / total, inc, total);
                foreach (var elem in elemListDeHtmlised)
                {
                    var anaService = serviceProvider.GetService<AnaService>();
                    var task = (Func<Task>)(async () =>
                    {
                        await anaService.LearnAsync(elem.Trim(), previousMessage);
                        inc++;
                        Console.Write("\r\t{0}% - {1}/{2}    ", 100 * inc / total, inc, total);
                    });
                    tasks.Add(task());
                    previousMessage = elem.Trim();
                }

                Task.WaitAll(tasks.ToArray());
                Console.WriteLine("\r\tDone             ");

                // delete imported file
                Console.WriteLine($"\tdelete");
                File.Delete(file);
            }
        }
    }
}
