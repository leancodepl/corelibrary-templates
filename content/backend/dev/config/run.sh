#!/usr/bin/env bash

source /app/config/config.sh
exec -a LncdApp.Api dotnet /app/bin/LncdApp.MainApp.dll
