FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443
# RUN sed -i '/en_US.UTF-8/s/^# //g' /etc/locale.gen && \
#     locale-gen
# ENV LANG en_US.UTF-8  
# ENV LANGUAGE en_US:en  
# ENV LC_ALL en_US.UTF-8
# ENV LANG=C.UTF-8 LC_ALL=C.UTF-8
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["OrderTrackBlazor/OrderTrackBlazor.csproj", "OrderTrackBlazor/"]
RUN dotnet restore "OrderTrackBlazor/OrderTrackBlazor.csproj"
COPY . .
WORKDIR "/src/OrderTrackBlazor"

RUN dotnet build "OrderTrackBlazor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderTrackBlazor.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderTrackBlazor.dll"]