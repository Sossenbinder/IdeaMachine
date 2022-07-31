echo "Starting server"
/opt/mssql/bin/sqlservr
echo "Waiting for startup"
sleep 30s
echo "Running startup script"
/opt/mssql-tools/bin/sqlcmd -S 127.0.0.1 -U sa -P '^dEbX2Ew' -i "/scripts/dbinit.sql"
echo "Falling asleep"
sleep infinity