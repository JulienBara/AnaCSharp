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
using Unity;
using Unity.Injection;

namespace AnaTelegramImport
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            Console.WriteLine($"Current Directory: {currentDirectory}");

            var files = Directory.GetFiles(Path.Combine(currentDirectory, "Input"));

            // clean files: current telegram export format is not compliant with XDocument.Load
            foreach (var file in files)
            {
                Console.WriteLine($"Current File: {file}");
                var lines = File.ReadAllLines(file);
                lines = lines.Select(x => x.Replace("<br>", " ")).ToArray();
                //lines = lines.Select(x => x.Replace("<div class=\"page_body chat_page\">", " ")).ToArray();
                //lines = lines.Select(x => x.Replace("<div class=\"page_wrap\">", " ")).ToArray();
                //lines = lines.Take(lines.Length - 3).ToArray();
                File.WriteAllLines(file, lines);
            }

            // import
            // prepare AnaService
            IUnityContainer container = new UnityContainer();

            var connectionString = "Server=localhost,1433;Database=ana;User=sa;Password=Your_password123";
            var contextOptions = SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
            container.RegisterType<AnaContext>(new InjectionConstructor(contextOptions));
            var anaContext = container.Resolve<AnaContext>();
            container.RegisterType<AnaService>();
            container.RegisterType<DeterminedWordRepository>();
            container.RegisterType<DeterminingStateRepository>();
            container.RegisterType<IDeterminedWordCommandRepository, DeterminedWordRepository>();
            container.RegisterType<IDeterminedWordQueryRepository, DeterminedWordRepository>();
            container.RegisterType<IDeterminingStateQueryRepository, DeterminingStateRepository>();

            var anaService = container.Resolve<AnaService>();

            foreach (var file in files)
            {
                Console.WriteLine($"Current File: {file}");
                XDocument doc = XDocument.Load(file);

                var elemList = doc.XPathSelectElements($"//div[@class='text']").ToList();
                var elemListDeHtmlised = elemList.Select(x => new HtmlString(x.Value).Value);

                var previousMessage = "";

                foreach (var elem in elemListDeHtmlised)
                {
                    await anaService.LearnAsync(elem.Trim(), previousMessage);
                    previousMessage = elem.Trim();
                }
            }
        }
    }
}
