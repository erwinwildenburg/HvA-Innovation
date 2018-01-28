#!/usr/bin/env bash

# If there is any command line argument specified, run it
[ $# -ne 0 ] && exec "$@"

# Copy appsettings.json if it's provided by Amazon
FILE=/var/app/current/config.json
if [ -f $FILE ]; then
	cp $FILE /defaulthome/hostingstart/
fi

exec node index.js
