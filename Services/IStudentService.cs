using Student_Management_System.Models;

namespace Student_Management_System.Services
{
    public interface IStudentService
    {
        List<StudentGetModel> StudetDetailsGet();
        string AddStudentDetails(StudentAddModel oStudentAddModel);
        string UpdateStudentDetails(StudentEditModel oStudentEditModel);
        string DeleteStudentDetails(int id);
        LoginModel ValidateUser(string username, string password);
        string AddUser(string username, string password);
    }
}
