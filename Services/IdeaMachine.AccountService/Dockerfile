#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/IdeaMachine.AccountService/IdeaMachine.AccountService.csproj", "Services/IdeaMachine.AccountService/"]
COPY ["Services/IdeaMachine.Service.Base/IdeaMachine.Service.Base.csproj", "Services/IdeaMachine.Service.Base/"]
COPY ["Common/IdeaMachine.Common.Grpc/IdeaMachine.Common.Grpc.csproj", "Common/IdeaMachine.Common.Grpc/"]
COPY ["Common/IdeaMachine.Common.IPC/IdeaMachine.Common.IPC.csproj", "Common/IdeaMachine.Common.IPC/"]
COPY ["Common/IdeaMachine.Common.Logging/IdeaMachine.Common.Logging.csproj", "Common/IdeaMachine.Common.Logging/"]
COPY ["Common/IdeaMachine.Common.RuntimeSerialization/IdeaMachine.Common.RuntimeSerialization.csproj", "Common/IdeaMachine.Common.RuntimeSerialization/"]
COPY ["Modules/IdeaMachine.ModulesServiceBase/IdeaMachine.ModulesServiceBase.csproj", "Modules/IdeaMachine.ModulesServiceBase/"]
COPY ["Common/IdeaMachine.Common.Eventing.Abstractions/IdeaMachine.Common.Eventing.Abstractions.csproj", "Common/IdeaMachine.Common.Eventing.Abstractions/"]
COPY ["Common/IdeaMachine.Common.Core/IdeaMachine.Common.Core.csproj", "Common/IdeaMachine.Common.Core/"]
COPY ["Modules/IdeaMachine.Modules.Session.Abstractions/IdeaMachine.Modules.Session.Abstractions.csproj", "Modules/IdeaMachine.Modules.Session.Abstractions/"]
COPY ["Modules/IdeaMachine.Modules.Account.Abstractions/IdeaMachine.Modules.Account.Abstractions.csproj", "Modules/IdeaMachine.Modules.Account.Abstractions/"]
COPY ["Common/IdeaMachine.Common.Eventing/IdeaMachine.Common.Eventing.csproj", "Common/IdeaMachine.Common.Eventing/"]
COPY ["Common/IdeaMachine.Common.SignalR/IdeaMachine.Common.SignalR.csproj", "Common/IdeaMachine.Common.SignalR/"]
COPY ["Common/IdeaMachine.Common.DI/IdeaMachine.Common.DI.csproj", "Common/IdeaMachine.Common.DI/"]
COPY ["Modules/IdeaMachine.Modules.Account/IdeaMachine.Modules.Account.csproj", "Modules/IdeaMachine.Modules.Account/"]
COPY ["Modules/IdeaMachine.Modules.Session/IdeaMachine.Modules.Session.csproj", "Modules/IdeaMachine.Modules.Session/"]
COPY ["Common/IdeaMachine.Common.AspNetIdentity/IdeaMachine.Common.AspNetIdentity.csproj", "Common/IdeaMachine.Common.AspNetIdentity/"]
RUN dotnet restore "Services/IdeaMachine.AccountService/IdeaMachine.AccountService.csproj"
COPY . .
WORKDIR "/src/Services/IdeaMachine.AccountService"
RUN dotnet build "IdeaMachine.AccountService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IdeaMachine.AccountService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdeaMachine.AccountService.dll"]
