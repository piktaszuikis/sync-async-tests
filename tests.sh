#!/bin/bash
set -e
HOST=127.0.0.1
THREADS=16
CONNECTIONS=100

BASELINE_FILE="output_baseline.csv"
ASP_FILE="output_asp.csv"

function show-csv {
	title=$1
	filename=$2
	echo $title:
	echo ------------------------------------
	column "-s;" -t -C left -C right -C right -C right -C right < $filename
	echo ------------------------------------
}

function baseline-item {
	name=$1
	port=$2
	
	echo 
	echo Starting test for $name...
	#warmup
	wrk -t$THREADS -c$CONNECTIONS http://$HOST:$port/ -d 2 > /dev/null
	sleep 1
	#real test
	r="$name;$(wrk -t$THREADS -c$CONNECTIONS http://$HOST:$port/ -s format.lua -d 10 3>&1 1>&2 2>&3)"
	echo $r >> $BASELINE_FILE
	sleep 1
}

function baseline {
	echo Establishing baseline...
	echo "server;req/second;latency min (us);latency avg (us);latency max (us);latency stdev" > $BASELINE_FILE
	baseline-item "bun" 3001
	baseline-item "nginx" 3002
	baseline-item "nim-httpbeast" 3003
	baseline-item "node-hyper-express" 3004
	baseline-item "rust-may_minihttp" 3005
	baseline-item "dotnet-httplistenter-net5" 3050
	baseline-item "dotnet-httplistenter-net5-threaded" 3051
	baseline-item "dotnet-httplistenter-net6" 3060
	baseline-item "dotnet-httplistenter-net6-threaded" 3061
	baseline-item "dotnet-httplistenter-net7" 3070
	baseline-item "dotnet-httplistenter-net7-threaded" 3071
	baseline-item "dotnet-httplistenter-net8" 3080
	baseline-item "dotnet-httplistenter-net8-threaded" 3081
	baseline-item "dotnet-asp-net5" 3052
	baseline-item "dotnet-asp-net6" 3062
	baseline-item "dotnet-asp-net7" 3072
	baseline-item "dotnet-asp-net8" 3082

	show-csv "Baseline summary" $BASELINE_FILE
}

# baseline


function asptest-item() {
	name=$1
	port=$2
	url=$3
	num=$4
	duration=$5
	
	echo 
	echo Starting test for $name, num = $num ...
	
	#warmup
	wrk -t$THREADS -c$CONNECTIONS http://$HOST:$port/$url -d 2 > /dev/null
	wrk -t$THREADS -c$CONNECTIONS http://$HOST:$port/a$url -d 2 > /dev/null
	sleep 1
	
	#real test
	#	for sync
	curl -s "http://$HOST:$port/$url" > /dev/null
	rs="$(wrk -t$THREADS -c$CONNECTIONS http://$HOST:$port/$url  -s format.lua -d $duration --timeout $duration 3>&1 1>&2 2>&3)"
	
	#	for async
	curl -s "http://$HOST:$port/$url" > /dev/null
	ra="$(wrk -t$THREADS -c$CONNECTIONS http://$HOST:$port/a$url -s format.lua -d $duration --timeout $duration 3>&1 1>&2 2>&3)"
	
	echo "$port;$name;$num;$rs;$ra" >> $ASP_FILE
	sleep 5
}

function asp-thread-test {
	for dotnet in 5 6 7 8
	do
		for theads in 17 32
		do
			port="3${dotnet}00"
			echo using port $port
			./PerformanceTest/bin/asp$dotnet/AspPerformanceTest --urls "http://127.0.0.1:$port" -threads $theads &
			PID=$!
			sleep 2
			asptest-item get $port sync/get $theads 10
			asptest-item db-fast-t $port sync/db-fast $theads 10
			kill $PID
			wait
			sleep 1
		done
	done
}

function asptests {
	port=$1
	echo Running asp tests for $port...

	for num in 0 1 5 10 100
	do
		asptest-item primes $port sync/primes?num=$num $num 10
	done
	
	for num in 0 1 5 10 100
	do
		asptest-item db-fast $port sync/db-fast?num=$num $num 10
	done
	
	for num in 0 1 5 10 100
	do
		asptest-item db-slow $port sync/db-slow?num=$num $num 30
	done
}

# sync vs asynv
#	simple get (thread count -> 1000)
#	primes (num -> 100)
#	fast-db (num -> 100)
#	slow-db (num -> 100)

echo "port;name;num;sync req/second;sync latency min (us);sync latency avg (us);sync latency max (us);sync latency stdev;async req/second;async latency min (us);async latency avg (us);async latency max (us);async latency stdev" > $ASP_FILE
#asp-thread-test
for dotnet in 5 6 7 8
do
	asptests "30${dotnet}2"
done

show-csv "ASP tests summary" $ASP_FILE
