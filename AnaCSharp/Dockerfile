FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY AnaCSharp/AnaCSharp.csproj AnaCSharp/
RUN dotnet restore AnaCSharp/AnaCSharp.csproj
COPY . .
WORKDIR /src/AnaCSharp
RUN dotnet build AnaCSharp.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish AnaCSharp.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AnaCSharp.dll"]
