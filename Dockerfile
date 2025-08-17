ARG VERSION=3.1-alpine

FROM mcr.microsoft.com/dotnet/sdk:$VERSION AS build
WORKDIR /src

COPY src/IdentityServer/*.csproj ./
RUN dotnet restore

COPY ./src/IdentityServer/ .
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:$VERSION AS runtime
WORKDIR /app

RUN apk update && apk add --no-cache \
  ca-certificates \
  curl \
  icu-libs \
  && rm -rf /var/lib/apt/lists/*

RUN adduser --disabled-password --gecos "" appuser

RUN mkdir -p /app/certs && chown -R appuser:appuser /app/certs

ENV ASPNETCORE_URLS="http://+:8080;https://+:8443"
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

COPY --from=build /app/publish .

RUN chown -R appuser:appuser /app
USER appuser

EXPOSE 8080 8443

HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl --fail http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "IdentityServer.dll"]
