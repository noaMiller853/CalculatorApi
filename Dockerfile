# Construction Phase
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CalculatorApi.csproj", "."]
RUN dotnet restore "CalculatorApi.csproj"
COPY . .
RUN dotnet build "CalculatorApi.csproj" -c Release -o /app/build

# Publishing Phase
FROM build AS publish
RUN dotnet publish "CalculatorApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Launch Phase
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "CalculatorApi.dll"]
