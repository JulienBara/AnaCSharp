using AnaCsharp.Dal.Interfaces.Repositories.Commands;
using AnaCsharp.Dal.Interfaces.Repositories.Queries;
using AnaCSharp.Bll.Interfaces.Services.Queries;
using AnaCSharp.BLL.Services;
using AnaCSharp.DAL;
using AnaCSharp.DAL.Repositories;
using AnaCSharpPolling;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


// with local dev database creating using docker command
// docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Your_password123" -p 1433:1433 mcr.microsoft.com/mssql/server
var connection = @"Server=localhost,1433; Database = master; User = sa; Password = Your_password123"; // TODO: configure SQL

Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services
            .AddHostedService<Worker>()
            .AddDbContext<AnaContext>(
                options => options.UseSqlServer(connection))

            // services
            .AddTransient<IAnswerQueryService, AnaService>()

            // repositories
            .AddTransient<IDeterminedWordCommandRepository, DeterminedWordRepository>()
            .AddTransient<IDeterminedWordQueryRepository, DeterminedWordRepository>()
            .AddTransient<IDeterminingStateCommandRepository, DeterminingStateRepository>()
            .AddTransient<IDeterminingStateQueryRepository, DeterminingStateRepository>()
            .AddTransient<IWordCommandRepository, WordRepository>()
            .AddTransient<IWordQueryRepository, WordRepository>()
            .AddTransient<DeterminingStateRepository>()
            .AddTransient<WordRepository>()
        ;
    })
    .Build()
    .Run();
