#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["TeacherConfiguration.Api/TeacherConfiguration.Api.csproj", "TeacherConfiguration.Api/"]
RUN dotnet restore "TeacherConfiguration.Api/TeacherConfiguration.Api.csproj"
COPY . .
WORKDIR "/src/TeacherConfiguration.Api"
RUN dotnet build "TeacherConfiguration.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TeacherConfiguration.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TeacherConfiguration.Api.dll"]