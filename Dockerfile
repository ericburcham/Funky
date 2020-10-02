FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app

#Moving code to folder.
COPY Source /app/Source

#Set working directory.
WORKDIR /app/Source/

#Restore, Build & Test
RUN dotnet restore Funky.sln
RUN dotnet build 
RUN dotnet test --logger:trx 
