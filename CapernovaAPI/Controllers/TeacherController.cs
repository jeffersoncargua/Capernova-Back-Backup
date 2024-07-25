using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using User.Managment.Data.Data;
using User.Managment.Data.Models;
using User.Managment.Repository.Repository.IRepository;

namespace CapernovaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ICourseRepositoty _dbCourse;
        protected ApiResponse _response;

        public TeacherController(ApplicationDbContext db, ICourseRepositoty dbCourse)
        {
            _db = db;
            _dbCourse = dbCourse;
            this._response = new();
        }

        [HttpGet(Name = "getAllCourse")]
        public async Task<ActionResult<ApiResponse>> GetAllCourse([FromQuery]string id)
        {
            try
            {
                var teacherCourse = await _dbCourse.GetAllAsync(u => u.TeacherId == id);
                if (teacherCourse == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Message = "No se han encontrado cursos asiganados";
                    return BadRequest(_response);
                }

                var teacher = await _db.TeacherTbl.FirstOrDefaultAsync(u => u.Id == id);
                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = $"Se ha obtenido la lista de cursos asignados al profesor";
                _response.Result = teacherCourse;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };
            }
            return _response;

        }
    }
}
