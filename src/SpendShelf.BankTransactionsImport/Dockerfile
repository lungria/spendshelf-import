FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine3.10 as build

# this needed to make TimeZoneConverter work
RUN apk add --no-cache tzdata  

WORKDIR /src
COPY . .
RUN dotnet publish -c Release --self-contained true -r linux-musl-x64 -o ./deploy /p:ShowLinkerSizeComparison=true

FROM mcr.microsoft.com/dotnet/core/runtime-deps:3.1-alpine3.10 as runtime

WORKDIR /app
COPY --from=build /src/deploy .

CMD ["./SpendShelf.BankTransactionsImport"]