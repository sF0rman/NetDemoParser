FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["DemoParserNET/DemoParserNET.csproj", "DemoParserNET/"]
RUN dotnet restore "DemoParserNET/DemoParserNET.csproj"
COPY . .
WORKDIR "/src/DemoParserNET"
RUN dotnet build "DemoParserNET.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DemoParserNET.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DemoParserNET.dll"]
