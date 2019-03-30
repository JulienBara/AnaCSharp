FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY AnaTelegramBotClient/AnaTelegramBotClient.csproj AnaTelegramBotClient/
COPY AnaCSharp.BLL/AnaCSharp.BLL.csproj AnaCSharp.BLL/
COPY AnaCSharp.DAL/AnaCSharp.DAL.csproj AnaCSharp.DAL/
RUN dotnet restore AnaTelegramBotClient/AnaTelegramBotClient.csproj
COPY . .
WORKDIR /src/AnaTelegramBotClient
RUN dotnet build AnaTelegramBotClient.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish AnaTelegramBotClient.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AnaTelegramBotClient.dll"]
