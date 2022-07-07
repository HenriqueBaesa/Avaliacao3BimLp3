
using SchoolManager.Database;
using SchoolManager.Models;
using SchoolManager.Repositories;
using Microsoft.Data.Sqlite;

var databaseConfig = new DatabaseConfig();
var databaseSetup = new DatabaseSetup(databaseConfig);

var studentRepository = new StudentRepository(databaseConfig);

// Routing
var modelName = args[0];
var modelAction = args[1];

if (modelName == "Student")
{
    if (modelAction == "List")
    {
        if (!studentRepository.CountByFormed().Any())
        {
            Console.WriteLine("Nenhum estudante cadastrado");
        }
        else
        {
            foreach (var student in studentRepository.GetAll())
            {
                string situation = (student.Former) ? "formado" : "não formado";
                Console.WriteLine("{0}, {1}, {2}, {3}", student.Registration, student.Name, student.City, situation);
            }
        }
    }


    if (modelAction == "New")
    {
        string registration = args[2];
        string name = args[3];
        string city = args[4];

        if (studentRepository.StudentExists(registration) == true)
        {
            Console.WriteLine("Estudante com Id " + registration + " já existe");
        }
        else
        {
            var student = new Student(registration, name, city, false);
            studentRepository.Save(student);

            Console.WriteLine("Estudante " + name + " cadastrado com sucesso");
        }
    }

    if (modelAction == "MarkAsFormed")
    {
        string registration = args[2];

        if (studentRepository.StudentExists(registration) == false)
        {
            Console.WriteLine("Estudante " + registration + " não encontrado");
        }
        else
        {
            studentRepository.MarkAsFormed(registration);
            Console.WriteLine("Estudante " + registration + " definido como formado");
        }


    }

    if (modelAction == "Delete")
    {
        string registration = args[2];

        if (studentRepository.StudentExists(registration) == false)
        {
            Console.WriteLine("Estudante " + registration + " não encontrado");
        }
        else
        {
            studentRepository.Delete(registration);
            Console.WriteLine("Estudante " + registration + " removido com sucesso");
        }
    }

    if (modelAction == "ListByCity")
    {
        string city = args[2];

        if (!studentRepository.GetAllStudentByCity(city).Any())
        {
            Console.WriteLine("Nenhum estudante cadastrado");
        }
        else
        {
            foreach (var student in studentRepository.GetAllStudentByCity(city))
            {
                string situation = (student.Former) ? "formado" : "não formado";
                Console.WriteLine("{0}, {1}, {2}, {3}", student.Registration, student.Name, student.City, situation);
            }
        }


    }

    if (modelAction == "ListFormed")
    {
        if (!studentRepository.CountByCities().Any())
        {
            Console.WriteLine("Nenhum estudante cadastrado");
        }
        else
        {
            foreach (var student in studentRepository.GetAllFormed())
            {
                string situation = (student.Former) ? "formado" : "não formado";
                Console.WriteLine("{0}, {1}, {2}", student.Name, student.City, situation);
            }
        }
    }

    if (modelAction == "ListByCities")
    {
        var cities = args;
        cities = cities.Where((source, index) => index != 0 && index != 1).ToArray();

        if (!studentRepository.GetAllByCities(cities).Any())
        {
            Console.WriteLine("Nenhum estudante cadastrado");
        }
        else
        {
            foreach (var student in studentRepository.GetAllByCities(cities))
            {
                string situation = (student.Former) ? "formado" : "não formado";
                Console.WriteLine("{0}, {1}, {2}, {3}", student.Registration, student.Name, student.City, situation);
            }
        }
    }

    if (modelAction == "Report")
    {
        string modelType = args[2];

        if (modelType == "CountByCities")
        {
            if (!studentRepository.CountByCities().Any())
            {
                Console.WriteLine("Nenhum estudante cadastrado");
            }
            else
            {
                foreach (var city in studentRepository.CountByCities())
                {

                    Console.WriteLine("{0}, {1}", city.AttributeName, city.StudentNumber);
                }
            }
        }

        if (modelType == "CountByFormed")
        {
            if (!studentRepository.CountByFormed().Any())
            {
                Console.WriteLine("Nenhum estudante cadastrado");
            }
            else
            {
                foreach (var former in studentRepository.CountByFormed())
                {
                    string situation = (former.AttributeName == "0") ? "Formados" : "Não formados";
                    Console.WriteLine("{0}, {1}", situation, former.StudentNumber);
                }
            }
        }
    }
}
