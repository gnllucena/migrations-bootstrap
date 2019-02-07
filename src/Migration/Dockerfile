FROM microsoft/dotnet:2.1-sdk as build-environment
WORKDIR /app
COPY src .

RUN dotnet restore Migrations.sln
RUN dotnet publish ./Migrations/Migrations.csproj -c Release -o /app/out

FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /app
COPY --from=build-environment /app/out .
COPY --from=build-environment /app/wait-for-it.sh . 
COPY --from=build-environment /app/wait-for-oracle-commit.sh .
ENV TZ Etc/GMT 
ENTRYPOINT ["dotnet", "Migrations.dll"]
