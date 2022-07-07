using Microsoft.Data.Sqlite;

namespace SchoolManager.Database;

class DatabaseSetup
{
    private DatabaseConfig databaseConfig;

    public DatabaseSetup(DatabaseConfig databaseConfig)
    {
        this.databaseConfig = databaseConfig;
        CreateStudentTable();
    }

    public void CreateStudentTable()
    {
        var connection = new SqliteConnection("Data Source=database.db");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Students(
                registration varchar(100) not null primary key,
                name varchar(100) not null,
                city varchar(100) not null,
                former bit not null
            );
        ";
        command.ExecuteNonQuery();

        connection.Close();
    }
}