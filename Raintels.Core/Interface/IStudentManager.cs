using Raintels.Entity.DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Raintels.Core.Interface
{
    public interface IStudentManager
    {
        Task<List<StudentDataModel>> GetStudents();
        Task<StudentDataModel> GetStudentDetailsById(int studentId);
        Task<StudentDataModel> CreateStudent(StudentDataModel student);
        Task<StudentDataModel> UpdateStudent(StudentDataModel student);
        Task<int> DeleteStudent(int studentId);
    }
}
