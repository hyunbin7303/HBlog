#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HBlog.Api/HBlog.Api.csproj", "HBlog.Api/"]
COPY ["HBlog.Domain/HBlog.Domain.csproj", "HBlog.Domain/"]
COPY ["HBlog.Application/HBlog.Application.csproj", "HBlog.Application/"]
COPY ["HBlog.Shared/HBlog.Contract.csproj", "HBlog.Shared/"]
COPY ["HBlog.Infrastructure/HBlog.Infrastructure.csproj", "HBlog.Infrastructure/"]
RUN dotnet restore "HBlog.Api/HBlog.Api.csproj"
COPY . .
WORKDIR "/src/HBlog.Api"
RUN dotnet build "HBlog.Api.csproj" -c %BUILD_CONFIGURATION% -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HBlog.Api.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HBlog.Api.dll"]