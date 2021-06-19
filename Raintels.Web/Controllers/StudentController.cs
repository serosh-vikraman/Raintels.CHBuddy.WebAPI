using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Raintels.Entity.ViewModel;
using Raintels.Service.ServiceInterface;
using Serilog;

namespace Raintels.CHBuddy.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> logger;
        private readonly IStudentService studentService;
        public StudentController(ILogger<StudentController> _logger, IStudentService _studentService)
        {
            logger = _logger;
            studentService = _studentService;
        }

        [HttpGet("getall")]
        public ResponseDataModel<IEnumerable<StudentViewModel>> GetAllStudents()
        {
            try
            {
                var students = studentService.GetStudents().Result;
                var response = new ResponseDataModel<IEnumerable<StudentViewModel>>()
                {
                    Status = HttpStatusCode.OK,
                    Response = students
                };
                return response;
            }
            catch (Exception ex)
            {
                return new ResponseDataModel<IEnumerable<StudentViewModel>>()
                {
                    Status = HttpStatusCode.InternalServerError,
                    Response = null,
                    ErrorMessage = new ErrorResponse()
                    {
                        Message = ex.Message
                    }

                };
            }
        }

        [HttpGet("getbyid/{studentId}")]
        public ResponseDataModel<StudentViewModel> GetStudent(int studentId)
        {
            try
            {
                var result = studentService.GetStudentDetailsById(studentId).Result;
                var response = new ResponseDataModel<StudentViewModel>()
                {
                    Status = HttpStatusCode.OK,
                    Response = result
                };
                return response;
            }
            catch (Exception ex)
            {
                return new ResponseDataModel<StudentViewModel>()
                {
                    Status = HttpStatusCode.InternalServerError,
                    Response = null,
                    ErrorMessage = new ErrorResponse()
                    {
                        Message = ex.Message
                    }

                };
            }
        }

        [HttpPost("savestudent")]
        public void SaveStudent(StudentViewModel student)
        {
            Log.Information("SaveStudent");
            studentService.CreateStudent(student);
            Log.Information("EndSaveStudent");
        }

        [HttpPost("updatestudent")]
        public void UpdateStudent(StudentViewModel student)
        {
            studentService.UpdateStudent(student);
        }

        [HttpGet("delete/{studentId}")]
        public void DeleteStudent(int studentId)
        {
            studentService.DeleteStudent(studentId);
        }

        public async Task<IActionResult> Upload()
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
