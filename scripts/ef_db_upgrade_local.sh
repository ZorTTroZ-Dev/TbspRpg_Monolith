#!/bin/bash

CONNECTION_STRING="User ID=tbsprpg_api;Password=tbsprpg_api;Server=localhost;Port=5432;Database=tbsprpgapi;" DATABASE_SALT="X" ADMIN_PASSWORD="admin" TEST_PASSWORD="test" dotnet ef database update
