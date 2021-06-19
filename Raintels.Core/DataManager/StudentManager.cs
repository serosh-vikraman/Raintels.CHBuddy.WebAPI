using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Raintels.Core.Interface;
using Raintels.Entity;
using Raintels.Entity.DataModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Raintels.Core.DataManager
{
    public class StudentManager : IStudentManager
    {

        private readonly string connectionString;
        public StudentManager(IConfiguration _configuration)
        {
            connectionString = _configuration.GetConnectionString("RaintelsDB");
        }
        public async Task<StudentDataModel> CreateStudent(StudentDataModel student)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("InsertNewStudent", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("P_ID", student.Id);
                    cmd.Parameters.AddWithValue("P_NAME", student.Name);
                    con.Open();
                    await cmd.ExecuteNonQueryAsync();
                    con.Close();
                    return student;

                }

            }
        }

        public async Task<int> DeleteStudent(int studentId)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("DeleteStudent", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("P_ID", studentId);
                    con.Open();
                    await cmd.ExecuteNonQueryAsync();
                    return studentId;
                }
            }
        }

        public async Task<StudentDataModel> GetStudentDetailsById(int studentId)
        {
            StudentDataModel studentData = new StudentDataModel();
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("GetStudent", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("P_ID", studentId);
                    con.Open();
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        var reader = await cmd.ExecuteReaderAsync();
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                studentData.Id = (int)reader["student_id"];
                                studentData.Name = (string)reader["student_name"];
                            }
                        }
                    }
                    con.Close();
                }
            }
            return studentData;
        }

        public async Task<List<StudentDataModel>> GetStudents()
        {
            List<StudentDataModel> studentList = new List<StudentDataModel>();
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("GetAllStudents", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        await sda.FillAsync(dt);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            studentList.Add(new StudentDataModel()
                            {
                                Id = Convert.ToInt32(dt.Rows[i][0].ToString()),
                                Name = dt.Rows[i][1].ToString()
                            });
                        }

                    }
                }
            }
            return studentList;
        }

        public async Task<StudentDataModel> UpdateStudent(StudentDataModel student)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("UpdateAllStudents", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("P_ID", student.Id);
                    cmd.Parameters.AddWithValue("P_NAME", student.Name);
                    con.Open();
                    await cmd.ExecuteNonQueryAsync();
                    con.Close();
                    return student;
                }

            }
        }
    }
}
