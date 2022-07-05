#!/usr/bin/env bash
source /app/config/config.sh

cd /app/bin
if [ -z "$SEED" ]; then
    echo "Migrating"
    dotnet LncdApp.Migrations.dll migrate
else
    echo "Seeding"
    dotnet LncdApp.Migrations.dll seed
fi
