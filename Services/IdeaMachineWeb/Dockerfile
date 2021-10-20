#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ADD ../../AzuriteCert/cert.pem /usr/local/share/ca-certificates/azuritecert.crt
RUN chmod 644 /usr/local/share/ca-certificates/azuritecert.crt && update-ca-certificates

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/IdeaMachineWeb/IdeaMachineWeb.csproj", "Services/IdeaMachineWeb/"]
COPY ["Modules/IdeaMachine.Modules.Idea/IdeaMachine.Modules.Idea.csproj", "Modules/IdeaMachine.Modules.Idea/"]
COPY ["Common/IdeaMachine.Common.Eventing.Abstractions/IdeaMachine.Common.Eventing.Abstractions.csproj", "Common/IdeaMachine.Common.Eventing.Abstractions/"]
COPY ["Common/IdeaMachine.Common.Core/IdeaMachine.Common.Core.csproj", "Common/IdeaMachine.Common.Core/"]
COPY ["Modules/IdeaMachine.Modules.Session/IdeaMachine.Modules.Session.csproj", "Modules/IdeaMachine.Modules.Session/"]
COPY ["Modules/IdeaMachine.ModulesServiceBase/IdeaMachine.ModulesServiceBase.csproj", "Modules/IdeaMachine.ModulesServiceBase/"]
COPY ["Modules/IdeaMachine.Modules.Account.Abstractions/IdeaMachine.Modules.Account.Abstractions.csproj", "Modules/IdeaMachine.Modules.Account.Abstractions/"]
COPY ["Modules/IdeaMachine.Modules.Session.Abstractions/IdeaMachine.Modules.Session.Abstractions.csproj", "Modules/IdeaMachine.Modules.Session.Abstractions/"]
COPY ["Common/IdeaMachine.Common.IPC/IdeaMachine.Common.IPC.csproj", "Common/IdeaMachine.Common.IPC/"]
COPY ["Common/IdeaMachine.Common.Eventing/IdeaMachine.Common.Eventing.csproj", "Common/IdeaMachine.Common.Eventing/"]
COPY ["Common/IdeaMachine.Common.SignalR/IdeaMachine.Common.SignalR.csproj", "Common/IdeaMachine.Common.SignalR/"]
COPY ["Common/IdeaMachine.Common.DI/IdeaMachine.Common.DI.csproj", "Common/IdeaMachine.Common.DI/"]
COPY ["Common/IdeaMachine.Common.Database/IdeaMachine.Common.Database.csproj", "Common/IdeaMachine.Common.Database/"]
COPY ["Modules/IdeaMachine.Modules.Account/IdeaMachine.Modules.Account.csproj", "Modules/IdeaMachine.Modules.Account/"]
COPY ["Common/IdeaMachine.Common.AspNetIdentity/IdeaMachine.Common.AspNetIdentity.csproj", "Common/IdeaMachine.Common.AspNetIdentity/"]
COPY ["Modules/IdeaMachine.Modules.Idea.Abstractions/IdeaMachine.Modules.Idea.Abstractions.csproj", "Modules/IdeaMachine.Modules.Idea.Abstractions/"]
COPY ["Modules/IdeaMachine.Modules.Reaction.Abstractions/IdeaMachine.Modules.Reaction.Abstractions.csproj", "Modules/IdeaMachine.Modules.Reaction.Abstractions/"]
COPY ["Common/IdeaMachine.Common.Logging/IdeaMachine.Common.Logging.csproj", "Common/IdeaMachine.Common.Logging/"]
COPY ["Services/IdeaMachine.Service.Base/IdeaMachine.Service.Base.csproj", "Services/IdeaMachine.Service.Base/"]
COPY ["Common/IdeaMachine.Common.Grpc/IdeaMachine.Common.Grpc.csproj", "Common/IdeaMachine.Common.Grpc/"]
COPY ["Common/IdeaMachine.Common.RuntimeSerialization/IdeaMachine.Common.RuntimeSerialization.csproj", "Common/IdeaMachine.Common.RuntimeSerialization/"]
COPY ["Common/IdeaMachine.Common.Web/IdeaMachine.Common.Web.csproj", "Common/IdeaMachine.Common.Web/"]
COPY ["Modules/IdeaMachine.Modules.Email/IdeaMachine.Modules.Email.csproj", "Modules/IdeaMachine.Modules.Email/"]
COPY ["Common/IdeaMachine.Common.RemotingProxies/IdeaMachine.Common.RemotingProxies.csproj", "Common/IdeaMachine.Common.RemotingProxies/"]
COPY ["Tools/GrpcProxyGenerator/GrpcProxyGenerator.csproj", "Tools/GrpcProxyGenerator/"]
COPY ["Modules/IdeaMachine.Modules.Reaction/IdeaMachine.Modules.Reaction.csproj", "Modules/IdeaMachine.Modules.Reaction/"]
RUN dotnet restore "Services/IdeaMachineWeb/IdeaMachineWeb.csproj"
COPY . .
WORKDIR "/src/Services/IdeaMachineWeb"
RUN dotnet build "IdeaMachineWeb.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IdeaMachineWeb.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdeaMachineWeb.dll"]
