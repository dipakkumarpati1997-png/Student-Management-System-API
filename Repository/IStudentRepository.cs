using Student_Management_System.Models;

namespace Student_Management_System.Repository
{
    public interface IStudentRepository
    {
        List<StudentGetModel> StudetDetailsGet();
        string AddStudentDetails(StudentAddModel oStudentAddModel);
        string UpdateStudentDetails(StudentEditModel oStudentEditModel);
        string DeleteStudentDetails(int id);
        LoginModel ValidateUser(string username, string password);
        string UserAdd(string username, string password);
    }
}
