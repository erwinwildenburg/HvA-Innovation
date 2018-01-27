#!/usr/bin/env bash
service ssh start

[ -z "$ASPNETCORE_URLS" ] && export ASPNETCORE_URLS=http://*:"$PORT"

# If there is any command line argument specified, run it
[ $# -ne 0 ] && exec "$@"

# Copy appsettings.json if it's provided by Amazon
FILE=/var/app/current/appsettings.json
if [ -f $FILE ]; then
	cp $FILE /defaulthome/hostingstart/
fi

cd /defaulthome/hostingstart
exec dotnet API.dll
