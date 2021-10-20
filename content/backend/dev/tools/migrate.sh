#!/usr/bin/env bash
set -e

echo "Waiting for MSSQL to start"
sleep 5

echo "Restoring & building"
dotnet restore
dotnet build --no-restore

echo "Configuring"
source /app/code/dev/config/config.sh
echo "Config applied"
if [ -z "$SEED" ]; then
    echo "Migrating"
    dotnet run --no-restore --no-build migrate
else
    echo "Seeding"
    dotnet run --no-restore --no-build seed
fi

echo "All done"
