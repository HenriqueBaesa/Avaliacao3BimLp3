namespace SchoolManager.Models;


class CountStudentGroupByAttribute
{
    public string AttributeName { get; set; }
    public int StudentNumber { get; set; }

    CountStudentGroupByAttribute() { }

    CountStudentGroupByAttribute(string attributeName, int studentNumber)
    {
        AttributeName = attributeName;
        StudentNumber = studentNumber;
    }
}
