#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FeedRetrievalApi/FeedRetrievalApi.csproj", "FeedRetrievalApi/"]
RUN dotnet restore "FeedRetrievalApi/FeedRetrievalApi.csproj"
COPY . .
WORKDIR "/src/FeedRetrievalApi"
RUN dotnet build "FeedRetrievalApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FeedRetrievalApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FeedRetrievalApi.dll"]