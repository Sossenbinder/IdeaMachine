#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["IdeaMachine/IdeaMachine.csproj", "IdeaMachine/"]
COPY ["Common/IdeaMachine.Common.Logging/IdeaMachine.Common.Logging.csproj", "Common/IdeaMachine.Common.Logging/"]
RUN dotnet restore "IdeaMachine/IdeaMachine.csproj"
COPY . .
WORKDIR "/src/IdeaMachine"
RUN dotnet build "IdeaMachine.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IdeaMachine.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdeaMachine.dll"]
