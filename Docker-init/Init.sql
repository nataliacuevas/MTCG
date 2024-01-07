-- init.sql
-- when using docker compose up, as indicated in the docker-compose.yml file
-- this sql script is mounted in the folder docker-entrypoint-initdb.d
-- which is automatically executed by the server, creating these databases
CREATE DATABASE simpledatastore;
CREATE DATABASE testsdb;
