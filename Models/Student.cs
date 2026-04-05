using System.ComponentModel.DataAnnotations;

namespace Student_Management_System.Models
{
    public class StudentAddModel
    {
        public string? Student_Name { get; set; }
        [EmailAddress]
        public string? Student_Email { get; set; }
        public int Student_Age { get; set; }
        public string? Student_Course { get; set; }
    }
    public class StudentEditModel
    {
        public int? Student_Id { get; set; }
        public string? Student_Name { get; set; }
        [EmailAddress]
        public string? Student_Email { get; set; }
        public int Student_Age { get; set; }
        public string? Student_Course { get; set; }
    }
    public class StudentGetModel
    {
        public int? Student_Id { get; set; }
        public string? Student_Name { get; set; }
        public string? Student_Email { get; set; }
        public int Student_Age { get; set; }
        public string? Student_Course { get; set; }
        public string? Created_Date { get; set; }
    }
    public class ResultModel
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public int ID { get; set; }
    }
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
