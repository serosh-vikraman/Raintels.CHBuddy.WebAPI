using AutoMapper;
using Raintels.Core.Interface;
using Raintels.Entity.DataModel;
using Raintels.Entity.ViewModel;
using Raintels.Service.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Raintels.Service
{
    public class StudentService : IStudentService
    {
        private readonly IStudentManager studentManager;
        private readonly IMapper mapper;
        public StudentService(IMapper _mapper, IStudentManager _studentManager)
        {
            mapper = _mapper;
            studentManager = _studentManager;
             
        }
        public async Task<StudentViewModel> CreateStudent(StudentViewModel studentViewModel)
        {
            studentViewModel.Name = studentViewModel.Name.Encrypt();
            var studentDataModel = mapper.Map<StudentDataModel>(studentViewModel);
            studentDataModel = await studentManager.CreateStudent(studentDataModel);
            studentViewModel = mapper.Map<StudentViewModel>(studentDataModel);
            studentViewModel.Name = studentViewModel.Name.Decrypt();
            return studentViewModel;
        }

        public async Task<int> DeleteStudent(int studentId)
        {
            return await studentManager.DeleteStudent(studentId);
        }

        public async Task<StudentViewModel> GetStudentDetailsById(int studentId)
        {
            var student = await studentManager.GetStudentDetailsById(studentId);
            var studentViewModel = mapper.Map<StudentViewModel>(student);
            studentViewModel.Name.Decrypt();
            return studentViewModel;
        }

        public async Task<List<StudentViewModel>> GetStudents()
        {
            var students = await studentManager.GetStudents();
            List<StudentViewModel> studentsViewModel = new List<StudentViewModel>();
            foreach (var item in students)
            {
                studentsViewModel.Add(
                    new StudentViewModel()
                    {
                        Id = item.Id,
                        Name = item.Name
                    });
            }
            return studentsViewModel;
        }

        public async Task<StudentViewModel> UpdateStudent(StudentViewModel studentViewModel)
        {
            var studentDataModel = new StudentDataModel()
            {
                Name = studentViewModel.Name
            };
            studentDataModel = await studentManager.UpdateStudent(studentDataModel);
            studentViewModel.Id = studentDataModel.Id;
            return studentViewModel;
        }
    }
}
