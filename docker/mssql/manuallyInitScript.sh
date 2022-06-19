echo 'CREATE DATABASE JointLessonDB;' > setup.sql
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Your_password123 -d master -i setup.sql