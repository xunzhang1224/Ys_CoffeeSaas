chcp 65001
:: chcp 936
echo off
set projcetname=YS.CoffeeMachine.Iot.Api
echo ''
echo '===================================Project YS.CoffeeMachine.Iot.Api is ready by release==================================='
echo 'Being released...'
::echo y |del bin 
::echo y |del obj
if exist bin (
	echo 'Directory bin deleting...'
	rmdir /s/q bin
)
if exist obj (
	echo 'Directory obj deleting...'
	rmdir /s/q obj
)
dotnet publish YS.CoffeeMachine.Iot.Api.csproj --configuration Release

echo '===================================Project YS.CoffeeMachine.Iot.Api release completed.==================================='
echo '===================================Project YS.CoffeeMachine.Iot.Api is ready by build docker images==================================='
docker build -t ys.coffeemachine.iot.api .
docker tag ys.coffeemachine.iot.api harbor1.ourvend.com/kfj/ys.coffeemachine.iot.api:k8s
docker push harbor1.ourvend.com/kfj/ys.coffeemachine.iot.api:k8s
docker rmi ys.coffeemachine harbor1.ourvend.com/kfj/ys.coffeemachine.iot.api:k8s
echo '===================================Project YS.CoffeeMachine.Iot.Api docker completed.==================================='
::echo '====================================================='
::docker pull harbor1.ourvend.com/v3new/ys.web.rocketlistener && docker stop ys.web.rocketlistener && docker rm ys.web.rocketlistener &&  docker run --name ys.web.rocketlistener1 -e TZ="Asia/Shanghai" --restart=always -p 7400:4293 -v /data0/root/log/work/iot1:/app/logs  -v /etc/localtime:/etc/localtime:ro -v/data0/root/key/nlog.config:/app/NLog.config -v/data0/root/cert/:/app/cert   -v /data0/root/appsettings/rocketlistener/appsettings.json:/app/appsettings.json  -d harbor1.ourvend.com/v3new/ys.web.rocketlistener
::docker run --name ys.web.rocketlistener0 -e TZ="Asia/Shanghai" --restart=always -p 7400:4293 -v /root/log/rocketlistener/iot0:/app/logs  -v /etc/localtime:/etc/localtime:ro -v/root/key/nlog.config:/app/NLog.config -v/root/cert/:/app/cert   -v /root/appsettings/rocketlistener0/appsettings.json:/app/appsettings.json  -d harbor1.ourvend.com/v3new/ys.web.rocketlistener
::docker run --name ys.web.rocketlistener1 -e TZ="Asia/Shanghai" --restart=always -p 7400:4293 -v /root/log/rocketlistener/iot1:/app/logs  -v /etc/localtime:/etc/localtime:ro -v/root/key/nlog.config:/app/NLog.config -v/root/cert/:/app/cert   -v /root/appsettings/rocketlistener1/appsettings.json:/app/appsettings.json  -d harbor1.ourvend.com/v3new/ys.web.rocketlistener