FROM mcr.microsoft.com/dotnet/aspnet:8.0 as base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
COPY . /src
WORKDIR /src
RUN dotnet build -c Debug -o /app/build

#FROM build AS publish
#RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
#COPY --from=publish /app/publish .
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "CrystalArena.dll"]
