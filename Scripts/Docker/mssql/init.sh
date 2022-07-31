#!/bin/bash
echo "init.sh starting"
(/opt/mssql/bin/sqlservr --accept-eula &) | grep -q "Service Broker manager has started" && /opt/mssql-tools/bin/sqlcmd -S 127.0.0.1 -U sa -P '^dEbX2Ew' -i "/scripts/dbinit.sql" && pkill sqlservr
echo "init.sh done"