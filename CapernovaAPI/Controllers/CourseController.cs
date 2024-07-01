using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using User.Managment.Data.Data;
using User.Managment.Data.Models;
using User.Managment.Data.Models.Managment;
using User.Managment.Data.Models.Managment.DTO;

namespace CapernovaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        protected ApiResponse _respose;
        public CourseController(ApplicationDbContext db)
        {
            _db = db;
            this._respose = new();
        }

        [HttpGet]
        [Route("getAllCourse")]
        public async Task<ActionResult<ApiResponse>> GetAll([FromQuery] string? search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                var coursesQuery = _db.CourseTbl.Where(u => u.Titulo.ToLower().Contains(search));
                _respose.isSuccess = true;
                _respose.StatusCode = HttpStatusCode.OK;
                _respose.Message = "Se ha obtenido la lista de Cursos";
                _respose.Result = coursesQuery;
                return Ok(_respose);
            }

            var courses = _db.CourseTbl.ToList();
            //List<CourseDto> courseDtos = new List<CourseDto>();
            
            //foreach (var item in courses)
            //{
            //    CapituloDto capitulos = JsonConvert.DeserializeObject<CapituloDto>(item.Capitulos);
            //    CourseDto course = new() {
            //        Id = item.Id,
            //        ImageUrl = item.ImageUrl,
            //        Titulo = item.Titulo,
            //        Descripcion = item.Descripcion,
            //        Price = item.Price,
            //        isActive = item.isActive,
            //        CapituloList = capitulos
            //    };
            //}
            _respose.isSuccess = true;
            _respose.StatusCode = HttpStatusCode.OK;
            _respose.Message = "Se ha obtenido la lista de Cursos";
            _respose.Result = courses;
            return Ok(_respose);
        }

        [HttpGet("getCourse/{id:int}", Name ="getCourse")]
        public async Task<ActionResult<ApiResponse>> GetCourse(int id)
        {
            var course = _db.CourseTbl.FirstOrDefault(u => u.Id == id);
            if (course == null)
            {
                _respose.isSuccess = false;
                _respose.StatusCode = HttpStatusCode.BadRequest;
                _respose.Message = "El registro no existe!";
                return BadRequest(_respose);
            }

            _respose.isSuccess = true;
            _respose.StatusCode = HttpStatusCode.OK;
            _respose.Message = "Se ha obtenido el curso con exito!";
            _respose.Result = course;
            return Ok(_respose);
        }

        [HttpPost]
        [Route("createCourse")]
        public async Task<ActionResult<ApiResponse>> CreateCourse([FromBody] CourseDto course)
        {
            if (_db.CourseTbl.FirstOrDefault(u => u.Id == course.Id) != null)
            {
                _respose.isSuccess = false;
                _respose.StatusCode = HttpStatusCode.BadRequest;
                _respose.Message = "El curso ya esta registrado";
                return BadRequest(_respose);
            }

            var capitulos = JsonConvert.SerializeObject(course.CapituloList);

            Course model = new()
            {
                ImageUrl = course.ImageUrl,
                Titulo = course.Titulo,
                Descripcion = course.Descripcion,
                Price = course.Price,
                isActive = course.isActive,
                Capitulos = capitulos
            };

            await _db.CourseTbl.AddAsync(model);
            await _db.SaveChangesAsync();

            _respose.isSuccess = true;
            _respose.StatusCode = HttpStatusCode.Created;
            _respose.Message = "El curso ha sido registrado con exito";
            return Ok(_respose);
        }


        [HttpPut("updateCourse/{id:int}", Name ="UpdateCourse")]
        public async Task<ActionResult<ApiResponse>> UpdateCourse(int id, [FromBody] CourseDto course)
        {
            var courseFromDb = _db.CourseTbl.AsNoTracking().FirstOrDefault(u => u.Id == course.Id);
            if (courseFromDb == null || course==null || id != course.Id)
            {
                _respose.isSuccess = false;
                _respose.StatusCode = HttpStatusCode.BadRequest;
                _respose.Message = "El registro ha actualizar no existe";
                return BadRequest(_respose);
            }

            var capitulos = JsonConvert.SerializeObject(course.CapituloList);

            Course model = new()
            {
                Id = course.Id,
                ImageUrl = course.ImageUrl,
                Titulo = course.Titulo,
                Descripcion=course.Descripcion,
                Price = course.Price,
                isActive = course.isActive,
                Capitulos = capitulos,
            };

            _db.CourseTbl.Update(model);
            await _db.SaveChangesAsync();

            _respose.isSuccess = true;
            _respose.StatusCode = HttpStatusCode.OK;
            _respose.Message = "El curso ha sido actualizado con exito";
            return Ok(_respose);
        }

        [HttpDelete("deleteCourse/{id:int}", Name = "deleteCourse")]
        public async Task<ActionResult<ApiResponse>> DeleteCourse(int id)
        {
            var course = _db.CourseTbl.FirstOrDefault(u => u.Id == id);
            if (course == null)
            {
                _respose.isSuccess = false;
                _respose.StatusCode = HttpStatusCode.BadRequest;
                _respose.Message = "Ha ocurrido un error inesperado al eliminar el registro";
                return BadRequest(_respose);
            }

            _db.CourseTbl.Remove(course);
            await _db.SaveChangesAsync();

            _respose.isSuccess = true;
            _respose.StatusCode = HttpStatusCode.OK;
            _respose.Message = "El curso ha sido eliminado con exito";
            return Ok(_respose);
        }
    }
}
