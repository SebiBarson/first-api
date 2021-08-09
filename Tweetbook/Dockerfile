FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

COPY "Tweetbook.csproj" /build/

RUN dotnet restore ./build/Tweetbook.csproj

COPY . ./build
WORKDIR /build/
RUN dotnet publish ./Tweetbook.csproj -c $BUILDCONFIG -o out /p:Version=5.0.302

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app

COPY --from=build /build/out .

ENTRYPOINT ["dotnet", "Tweetbook.dll"]