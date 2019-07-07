FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Api2/Api2.csproj", "Api2/"]
RUN dotnet restore "Api2/Api2.csproj"
COPY . .
WORKDIR "/src/Api2"
RUN dotnet build "Api2.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Api2.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Api2.dll"]
