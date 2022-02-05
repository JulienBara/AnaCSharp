# AnaCSharp

Ana is a conversational bot for [Telegram](https://telegram.org/).  
It is a simple implementation of the [Markov chain](https://en.wikipedia.org/wiki/Markov_chain).

Disclaimer: This is a personal projet. I work on it for fun and to joke with my friends.   
Please don't try to conquer the world with it.

## Environment

- .NET 5 runtime installed
- docker installed

### Setting up the bot token

Retrieve Telegram token cf. [documentation](https://core.telegram.org/bots/api#authorizing-your-bot).

Create an `appsettings.json` file at the location `/AnaCSharp/Apps/AnaCSharpPolling/appsettings.json`
```
{
    "botToken": "my-token-here"
}
``` 


### Setting up the database

First start a docker container with SQL Server.

```
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Your_password123" -p 1433:1433 mcr.microsoft.com/mssql/server
```

The connection string is `Server=localhost,1433; Database = ana; User = sa; Password = Your_password123`.

Connect to the database using a dbms (e.g. [Azure Data Studio](https://docs.microsoft.com/en-us/sql/azure-data-studio/download-azure-data-studio?view=sql-server-ver15) or the [SQL Server VSCode extension](https://marketplace.visualstudio.com/items?itemName=ms-mssql.mssql)).  
Run the script from the file `/AnaCSharp/Dal/AnaCSharp.Dal.SqlServer/Scripts/init.sql`

## Running the bot

```
cd /Apps/Apps/AnaCSharpPolling
dotnet run
```

## Fuel the bot memory from a Telegram conversation

Export a Telegram chat history in HTML format under `/AnaCSharp/Apps/AnaTelegramImport/Input`.

```
cd /Apps/AnaTelegramImport
dotnet run
```