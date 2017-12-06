#!/usr/bin/env bash
service ssh start

[ -z "$ASPNETCORE_URLS" ] && export ASPNETCORE_URLS=http://*:8000

# If there is any command line argument specified, run it
[ $# -ne 0 ] && exec "$@"

cd /defaulthome/hostingstart
exec dotnet Login.dll
