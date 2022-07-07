
using SchoolManager.Database;
using SchoolManager.Models;
using Microsoft.Data.Sqlite;
using Dapper;

namespace SchoolManager.Repositories;

class StudentRepository
{
    private DatabaseConfig databaseConfig;

    public StudentRepository(DatabaseConfig databaseConfig)
    {
        this.databaseConfig = databaseConfig;
    }

    // Insere um estudante na tabela
    public Student Save(Student student)
    {
        using var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();

        connection.Execute("INSERT INTO Students VALUES(@Registration, @Name, @City, @Former)",
        student);

        return student;
    }

    // Deleta um estudante na tabela
    public void Delete(string registration)
    {
        using var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();

        connection.Execute("DELETE FROM Students WHERE registration = @Registration;", new { Registration = registration });
    }

    // Marca um estudante como formado
    public void MarkAsFormed(string registration)
    {
        using var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();

        connection.Execute("UPDATE Students SET former = 1 WHERE registration = @Registration;",
        new { Registration = registration });
    }

    // Retorna todos os estudantes
    public IEnumerable<Student> GetAll()
    {
        using var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();

        var students = connection.Query<Student>("SELECT * FROM Students");

        return students;
    }

    // Retorna todos os estudantes formados
    public IEnumerable<Student> GetAllFormed()
    {
        using var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();

        var students = connection.Query<Student>("SELECT * FROM Students WHERE former = 1");

        return students;
    }

    // Retorna todos os estudantes de uma cidade
    public IEnumerable<Student> GetAllStudentByCity(string city)
    {
        using var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();

        var students = connection.Query<Student>("SELECT * FROM Students WHERE city LIKE @City", new { City = "%" + city + "%" });

        return students;
    }

    // Retorna os estudantes das cidades presentes no array cities
    public IEnumerable<Student> GetAllByCities(string[] cities)
    {
        using var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();

        var students = connection.Query<Student>("SELECT * FROM Students WHERE city IN @Cities;", new { Cities = cities });

        return students;
    }

    // Retorna o número de estudantes agrupados por cidade
    public IEnumerable<CountStudentGroupByAttribute> CountByCities()
    {
        using var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();

        var qntByCities = connection.Query<CountStudentGroupByAttribute>("SELECT city AS AttributeName, COUNT(registration) AS StudentNumber FROM Students GROUP BY city;");
        return qntByCities;
    }

    // Retorna o número de estudantes agrupados por formados e não formados
    public IEnumerable<CountStudentGroupByAttribute> CountByFormed()
    {
        using var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();

        var qntByFormed = connection.Query<CountStudentGroupByAttribute>("SELECT former AS AttributeName, COUNT(*) AS StudentNumber FROM Students GROUP BY former ORDER BY 2 ASC;");
        return qntByFormed;
    }

    public bool StudentExists(string registration)
    {
        using var connection = new SqliteConnection(databaseConfig.ConnectionString);
        connection.Open();

        var result = Convert.ToBoolean(connection.ExecuteScalar(
            "SELECT count(registration) FROM Students WHERE registration = @Registration;", new { Registration = registration }));

        return result;
    }
}