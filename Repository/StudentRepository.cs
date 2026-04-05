using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Student_Management_System.Helpers;
using Student_Management_System.Models;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace Student_Management_System.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly IConfiguration _IConfiguration;

        public StudentRepository(IConfiguration IConfiguration)
        {
            _IConfiguration = IConfiguration;
        }

        public string AddStudentDetails(StudentAddModel oStudentAddModel)
        {
            SqlConnection oSqlConnection = new SqlConnection(_IConfiguration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = oSqlConnection;
            cmd.CommandText = "STUD_SP_Student_Add";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_Student_Name", oStudentAddModel.Student_Name);
            cmd.Parameters.AddWithValue("@p_Student_Email", oStudentAddModel.Student_Email);
            cmd.Parameters.AddWithValue("@p_Student_Age", oStudentAddModel.Student_Age);
            cmd.Parameters.AddWithValue("@p_Student_Course", oStudentAddModel.Student_Course);
            oSqlConnection.Open();
            ResultModel oResultModel = new ResultModel();
            SqlDataReader reader = cmd.ExecuteReader();
            oResultModel = reader.Read() ? MapResult(reader) : new ResultModel();
            oSqlConnection.Close();
            return oResultModel.Message;
        }

        public string UpdateStudentDetails(StudentEditModel oStudentEditModel)
        {
            SqlConnection oSqlConnection = new SqlConnection(_IConfiguration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = oSqlConnection;
            cmd.CommandText = "STUD_SP_Student_Update";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_Student_Id", oStudentEditModel.Student_Id);
            cmd.Parameters.AddWithValue("@p_Student_Name", oStudentEditModel.Student_Name);
            cmd.Parameters.AddWithValue("@p_Student_Email", oStudentEditModel.Student_Email);
            cmd.Parameters.AddWithValue("@p_Student_Age", oStudentEditModel.Student_Age);
            cmd.Parameters.AddWithValue("@p_Student_Course", oStudentEditModel.Student_Course);
            oSqlConnection.Open();
            ResultModel oResultModel = new ResultModel();
            SqlDataReader reader = cmd.ExecuteReader();
            oResultModel = reader.Read() ? MapResult(reader) : new ResultModel();
            oSqlConnection.Close();
            return oResultModel.Message;
        }

        public string DeleteStudentDetails(int id)
        {
            SqlConnection oSqlConnection = new SqlConnection(_IConfiguration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = oSqlConnection;
            cmd.CommandText = "STUD_SP_Student_Delete";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_Student_Id", id);
            oSqlConnection.Open();
            ResultModel oResultModel = new ResultModel();
            SqlDataReader reader = cmd.ExecuteReader();
            oResultModel = reader.Read() ? MapResult(reader) : new ResultModel();
            oSqlConnection.Close();
            return oResultModel.Message;
        }
        public List<StudentGetModel> StudetDetailsGet()
        {
            List<StudentGetModel> oStudentGetModelList = new List<StudentGetModel>();
            SqlConnection oSqlConnection = new SqlConnection(_IConfiguration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = oSqlConnection;
            cmd.CommandText = "STUD_SP_Student_Get";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            oSqlConnection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                StudentGetModel oStudentGetModel = new StudentGetModel
                {
                    Student_Id = Convert.ToInt32(reader["Student_Id"]),
                    Student_Name = reader["Student_Name"]?.ToString(),
                    Student_Email = reader["Student_Email"]?.ToString(),
                    Student_Age = Convert.ToInt32(reader["Student_Age"]),
                    Student_Course = reader["Student_Course"]?.ToString(),
                    Created_Date = reader["Created_Date"]?.ToString()
                };
                oStudentGetModelList.Add(oStudentGetModel);
            }
            oSqlConnection.Close();
            return oStudentGetModelList;
        }

        public static ResultModel MapResult(SqlDataReader reader)
        {
            return new ResultModel
            {
                ID = Convert.ToInt32(reader["ID"]),
                IsSuccess = Convert.ToBoolean(reader["IsSuccess"]),
                Message = reader["Message"]?.ToString()
            };
        }
        public LoginModel ValidateUser(string username, string password)
        {
            LoginModel user = null;
            SqlConnection oSqlConnection = new SqlConnection(_IConfiguration.GetConnectionString("DefaultConnection"));

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = oSqlConnection;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "UA_SP_User_Get";
            cmd.Parameters.AddWithValue("@p_Username", username);
            cmd.Parameters.AddWithValue("@p_Password", password);
            oSqlConnection.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                user = new LoginModel
                {
                    Username = reader["Username"].ToString(),
                    Password = reader["Password"].ToString()
                };
            }


            return user;
        }
        public string UserAdd(string username, string password)
        {
            SqlConnection oSqlConnection = new SqlConnection(_IConfiguration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand("UA_SP_User_Add", oSqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_Username", username);
            cmd.Parameters.AddWithValue("@p_Password", PasswordHelper.HashPassword(password));

            oSqlConnection.Open();
            ResultModel oResultModel = new ResultModel();
            SqlDataReader reader = cmd.ExecuteReader();
            oResultModel = reader.Read() ? MapResult(reader) : new ResultModel();
            oSqlConnection.Close();
            return oResultModel.Message;
        }
    }
}
