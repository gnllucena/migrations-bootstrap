#!/usr/bin/env bash

while [ $((curl -s -XGET --unix-socket /var/run/docker.sock "http://localhost:2375/containers/mysql/logs?stdout=1&tail=10" | grep -Eq "READY") && echo 1 || echo 0) -ne 1 ]
do
    echo "WAITING..."
    sleep 1
done
echo "GOOD TO GO"
eval "dotnet Migrations.dll"
