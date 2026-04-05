using Student_Management_System.Models;
using Student_Management_System.Repository;

namespace Student_Management_System.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _IStudentRepository;  
        public StudentService(IStudentRepository IStudentRepository)
        {
            _IStudentRepository = IStudentRepository;
        }
        public List<StudentGetModel> StudetDetailsGet()
        {
            return _IStudentRepository.StudetDetailsGet();
        }
        public string AddStudentDetails(StudentAddModel oStudentAddModel)
        {
            return _IStudentRepository.AddStudentDetails(oStudentAddModel);
        }
        public string UpdateStudentDetails(StudentEditModel oStudentEditModel)
        {
            return _IStudentRepository.UpdateStudentDetails(oStudentEditModel);
        }
        public string DeleteStudentDetails(int id)
        {
            return _IStudentRepository.DeleteStudentDetails(id);

        }
        public LoginModel ValidateUser(string username, string password)
        {
            return _IStudentRepository.ValidateUser(username, password);
        }
        public string AddUser(string username, string password)
        {
            return _IStudentRepository.UserAdd(username, password);

        }
    }
}
