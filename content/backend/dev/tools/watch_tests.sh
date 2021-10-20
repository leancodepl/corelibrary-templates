#!/usr/bin/env bash
set -e

if [[ -z "$TESTS_FQN" ]]
then
    exec dotnet watch test
else
    exec dotnet watch test --filter "FullyQualifiedName~$TESTS_FQN"
fi
