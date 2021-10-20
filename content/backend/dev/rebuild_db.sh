#!/usr/bin/env bash

docker-compose down -v
docker-compose run migrations
docker-compose run seed
