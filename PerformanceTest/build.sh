#!/bin/bash
set -e

mkdir -p bin
mkdir -p bin/asp5
mkdir -p bin/asp6
mkdir -p bin/asp7
mkdir -p bin/asp8
mkdir -p bin/http-listener-5
mkdir -p bin/http-listener-6
mkdir -p bin/http-listener-7
mkdir -p bin/http-listener-8

function deploy-asp-item {
	version=$1
	echo
	echo "Deploying asp for .net$version"
	dotnet publish -f net$version.0 --sc false -c Release --os linux -o ./bin/asp$version/  AspPerformanceTest/AspPerformanceTest.csproj
}

function deploy-asp {
	deploy-asp-item 5
	deploy-asp-item 6
	deploy-asp-item 7
	deploy-asp-item 8
}

function deploy-httplistener-item {
	version=$1
	echo
	echo "Deploying http-listener for .net$version"
	dotnet publish -f net$version.0 --sc false -c Release --os linux -o ./bin/http-listener-$version/  HttpListenerTest/HttpListenerTest.csproj
}


function deploy-httplistener {
	deploy-httplistener-item 5
	deploy-httplistener-item 6
	deploy-httplistener-item 7
	deploy-httplistener-item 8
}

deploy-asp
#deploy-httplistener