#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["GeometricProgramming.ConsumerAPI/GeometricProgramming.ConsumerAPI.csproj", "GeometricProgramming.ConsumerAPI/"]
RUN dotnet restore "GeometricProgramming.ConsumerAPI/GeometricProgramming.ConsumerAPI.csproj"
COPY . .
WORKDIR "/src/GeometricProgramming.ConsumerAPI"
RUN dotnet build "GeometricProgramming.ConsumerAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GeometricProgramming.ConsumerAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GeometricProgramming.ConsumerAPI.dll"]