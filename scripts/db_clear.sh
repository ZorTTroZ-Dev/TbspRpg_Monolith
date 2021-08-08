#!/bin/bash

psql -U postgres -h localhost -d tbsprpgapi -c "DELETE FROM contents"
psql -U postgres -h localhost -d tbsprpgapi -c "DELETE FROM games"
