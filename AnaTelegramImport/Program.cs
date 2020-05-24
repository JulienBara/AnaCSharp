using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
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

            var files = Directory.GetFiles($@"{currentDirectory}\Input");

            // clean files: current telegram export format is not compliant with XDocument.Load
            //foreach (var file in files)
            //{
            //    Console.WriteLine($"Current File: {file}");
            //    var lines = File.ReadAllLines(file);
            //    lines = lines.Select(x => x.Replace("<br>", " ")).ToArray();
            //    lines = lines.Select(x => x.Replace("<div class=\"page_body chat_page\">", " ")).ToArray();
            //    lines = lines.Select(x => x.Replace("<div class=\"page_wrap\">", " ")).ToArray();
            //    lines = lines.Take(lines.Length - 3).ToArray();
            //    File.WriteAllLines(file, lines);
            //}

            // import
            // prepare AnaService
            IUnityContainer container = new UnityContainer();

            var connectionString = "Server=localhost,1434;Database=master;User=sa;Password=Your_password123";
            var contextOptions = SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
            container.RegisterType<AnaContext>(new InjectionConstructor(contextOptions));
            var anaContext = container.Resolve<AnaContext>();
            anaContext.Database.Migrate();

            container.RegisterType<AnaService>();
            container.RegisterType<DeterminedWordRepository>();
            container.RegisterType<DeterminingStateRepository>();

            var anaService = container.Resolve<AnaService>();

            foreach (var file in files)
            {
                Console.WriteLine($"Current File: {file}");
                XDocument doc = XDocument.Load(file);

                var elemList = doc.XPathSelectElements($"//div[@class='text']").ToList();
                var elemListDeHtmlised = elemList.Select(x => new HtmlString(x.Value).Value);

                var lastWord = new List<string>();

                foreach (var elem in elemListDeHtmlised)
                {
                    await anaService.LearnAsync(elem.Trim(), lastWord);
                }
                
            }
        }
    }
}
