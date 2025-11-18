# 请参阅 https://aka.ms/customizecontainer 以了解如何自定义调试容器，以及 Visual Studio 如何使用此 Dockerfile 生成映像以更快地进行调试。

# 此阶段用于在快速模式(默认为调试配置)下从 VS 运行时
FROM mcr.microsoft.com/dotnet/aspnet:8.0
COPY bin/Release/net8.0/publish/ /app/
WORKDIR /app
EXPOSE 5000
EXPOSE 5001
ENTRYPOINT ["dotnet", "YS.CoffeeMachine.API.dll"]