#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Sample.Admin.HttpAggregator/Sample.Admin.HttpAggregator.csproj", "Sample.Admin.HttpAggregator/"]
COPY ["Sample.Admin.Model/Sample.Admin.Model.csproj", "Sample.Admin.Model/"]
COPY ["Core.API/Core.API.csproj", "Core.API/"]
COPY ["CommonModels/Common.Model.csproj", "CommonModels/"]
COPY ["Logger/Logger.csproj", "Logger/"]
COPY ["Common.Enum/Common.Enum.csproj", "Common.Enum/"]
COPY ["Utility/Utility.csproj", "Utility/"]
RUN dotnet restore "Sample.Admin.HttpAggregator/Sample.Admin.HttpAggregator.csproj"
COPY . .
WORKDIR "/src/Sample.Admin.HttpAggregator"
RUN dotnet build "Sample.Admin.HttpAggregator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sample.Admin.HttpAggregator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sample.Admin.HttpAggregator.dll"]