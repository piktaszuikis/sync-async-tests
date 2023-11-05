#!/bin/bash
set -e

function cleanup {
	echo killing pids: $PIDs
	kill $PIDs
	exit
}
trap cleanup SIGHUP SIGINT SIGTERM

echo Warning: Make sure to build before running!

./bin/http-listener-5/HttpListenerTest -port 3050 &
PIDs="$PIDs $!"
./bin/http-listener-6/HttpListenerTest -port 3060 &
PIDs="$PIDs $!"
./bin/http-listener-7/HttpListenerTest -port 3070 &
PIDs="$PIDs $!"
./bin/http-listener-8/HttpListenerTest -port 3080 &
PIDs="$PIDs $!"

./bin/http-listener-5/HttpListenerTest -port 3051 -threaded &
PIDs="$PIDs $!"
./bin/http-listener-6/HttpListenerTest -port 3061 -threaded &
PIDs="$PIDs $!"
./bin/http-listener-7/HttpListenerTest -port 3071 -threaded &
PIDs="$PIDs $!"
./bin/http-listener-8/HttpListenerTest -port 3081 -threaded &
PIDs="$PIDs $!"

wait
