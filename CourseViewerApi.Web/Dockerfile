#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CourseViewerApi.Web/CourseViewerApi.Web.csproj", "CourseViewerApi.Web/"]
COPY ["CourseViewerApi.Common/CourseViewerApi.Common.csproj", "CourseViewerApi.Common/"]
COPY ["CourseViewerApi.Core/CourseViewerApi.Core.csproj", "CourseViewerApi.Core/"]
COPY ["CourseViewerApi.DataAccess/CourseViewerApi.DataAccess.csproj", "CourseViewerApi.DataAccess/"]
RUN dotnet restore "CourseViewerApi.Web/CourseViewerApi.Web.csproj"
COPY . .
WORKDIR "/src/CourseViewerApi.Web"
RUN dotnet build "CourseViewerApi.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CourseViewerApi.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CourseViewerApi.Web.dll"]