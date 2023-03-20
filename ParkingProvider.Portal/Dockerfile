#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
RUN apt-get update && apt-get install -y apt-utils libgdiplus libc6-dev
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["ParkingProvider.Portal/ParkingProvider.Portal.csproj", "ParkingProvider.Portal/"]
COPY ["Core.Storage/Core.Storage.csproj", "Core.Storage/"]
COPY ["uhv.Customer.Model/uhv.Customer.Model.csproj", "uhv.Customer.Model/"]
COPY ["Common.Enum/Common.Enum.csproj", "Common.Enum/"]
RUN dotnet restore "ParkingProvider.Portal/ParkingProvider.Portal.csproj"
COPY . .
WORKDIR "/src/ParkingProvider.Portal"
RUN dotnet build "ParkingProvider.Portal.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ParkingProvider.Portal.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["ParkingProvider.Portal/wwwroot", "wwwroot/"]
RUN apt-get update && apt-get install -y apt-utils libgdiplus libc6-dev
ENTRYPOINT ["dotnet", "ParkingProvider.Portal.dll"]