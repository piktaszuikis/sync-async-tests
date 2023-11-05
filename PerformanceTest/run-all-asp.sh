#!/bin/bash
set -e

function cleanup {
	echo killing pids: $PIDs
	kill $PIDs
	exit
}
trap cleanup SIGHUP SIGINT SIGTERM

echo Warning: Make sure to build before running!

./bin/asp5/AspPerformanceTest --urls http://127.0.0.1:3052 -threads 17 &
PIDs="$PIDs $!"
./bin/asp6/AspPerformanceTest --urls http://127.0.0.1:3062 -threads 17 &
PIDs="$PIDs $!"
./bin/asp7/AspPerformanceTest --urls http://127.0.0.1:3072 -threads 17 &
PIDs="$PIDs $!"
./bin/asp8/AspPerformanceTest --urls http://127.0.0.1:3082 -threads 17 &
PIDs="$PIDs $!"

wait
