using Microsoft.AspNetCore.Mvc;
using System.Net;
using User.Managment.Data.Models;
using User.Managment.Data.Models.Course;
using User.Managment.Data.Models.Course.DTO;
using User.Managment.Repository.Repository.IRepository;

namespace CapernovaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepositoty _dbCourse;
        protected ApiResponse _respose;
        public CourseController(ICourseRepositoty dbCourse)
        {
            _dbCourse = dbCourse;
            this._respose = new();
        }
        
        /// <summary>
        /// Este controlador permite obtener todos los cursos registrados hasta el momento en la base de datos
        /// </summary>
        /// <param name="search">Es el modelo que permite obtener un curso en especifico de acuerdo al titulo del curso</param>
        /// <returns>Retorna una lista de cursos y en el caso de tener una busqueda especifica retorna un listado con los cursos especificos</returns>
        [HttpGet]
        [Route("getAllCourse")]
        public async Task<ActionResult<ApiResponse>> GetAll([FromQuery] string? search)
        {
            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    var coursesQuery = await _dbCourse.GetAllAsync(u => u.Titulo.ToLower().Contains(search));
                    _respose.isSuccess = true;
                    _respose.StatusCode = HttpStatusCode.OK;
                    _respose.Message = "Se ha obtenido la lista de Cursos";
                    _respose.Result = coursesQuery;
                    return Ok(_respose);
                }

                var courses = await _dbCourse.GetAllAsync(); //devuelve una lista con los cursos 
                //var courses = await _db.CourseTbl.ToListAsync(); //si funciona la linea anterior se elimina esta linea
                
                _respose.isSuccess = true;
                _respose.StatusCode = HttpStatusCode.OK;
                _respose.Message = "Se ha obtenido la lista de Cursos";
                _respose.Result = courses;
                return Ok(_respose);
            }
            catch (Exception ex)
            {
                _respose.isSuccess = false;
                _respose.Errors = new List<string>() { ex.ToString() }; 
            }

            return _respose;
  
        }

        /// <summary>
        /// Este controlador permite obtener el curso de acuerdo al id del curso que se desea obtener
        /// </summary>
        /// <param name="id">Es el que contiene el identificador a comparar para obtener el curso</param>
        /// <returns></returns>
        [HttpGet("getCourse/{id:int}", Name ="getCourse")]
        public async Task<ActionResult<ApiResponse>> GetCourse(int id)
        {
            try
            {
                var course = await _dbCourse.GetAsync(u => u.Id == id);// devuelve el curso cuyo id sea igual al Id del curso
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
            catch (Exception ex)
            {
                _respose.isSuccess = false;
                _respose.Errors = new List<string>() { ex.ToString() };
            }
            return _respose;
        }

        [HttpPost]
        [Route("createCourse")]
        public async Task<ActionResult<ApiResponse>> CreateCourse([FromBody] CourseDto course)
        {
            try
            {
                if (await _dbCourse.GetAsync(u => u.Id == course.Id) != null)
                {
                    _respose.isSuccess = false;
                    _respose.StatusCode = HttpStatusCode.BadRequest;
                    _respose.Message = "El curso ya esta registrado";
                    return BadRequest(_respose);
                }

                //var capitulos = JsonConvert.SerializeObject(course.CapituloList);
                //var deberes = JsonConvert.SerializeObject(course.Deberes);
                //var pruebas = JsonConvert.SerializeObject(course.Pruebas);

                Course model = new()
                {
                    Codigo = course.Codigo,
                    ImagenUrl = course.ImagenUrl,
                    Titulo = course.Titulo,
                    Detalle = course.Detalle,
                    //State = course.State,
                    //Deberes = deberes,
                    //Pruebas = pruebas,
                    //NotaFinal = 0,
                    Precio = course.Precio,
                 //   FolderId = course.FolderId,
                //    IsActive = course.IsActive,
                //    Capitulos = capitulos,
                    
                };

                await _dbCourse.CreateAsync(model);
                await _dbCourse.SaveAsync();

                _respose.isSuccess = true;
                _respose.StatusCode = HttpStatusCode.Created;
                _respose.Message = "El curso ha sido registrado con exito";
                return Ok(_respose);
            }
            catch (Exception ex)
            {
                _respose.isSuccess = false;
                _respose.Errors = new List<string>() { ex.ToString() };
            }
            return _respose;

        }


        [HttpPut("updateCourse/{id:int}", Name ="UpdateCourse")]
        public async Task<ActionResult<ApiResponse>> UpdateCourse(int id, [FromBody] CourseDto course)
        {
            try
            {
                //var courseFromDb =await _db.CourseTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Id == course.Id);
                var courseFromDb = await _dbCourse.GetAsync(u => u.Id == course.Id, tracked: false);
                if (courseFromDb == null || course == null || id != course.Id)
                {
                    _respose.isSuccess = false;
                    _respose.StatusCode = HttpStatusCode.BadRequest;
                    _respose.Message = "El registro ha actualizar no existe";
                    return BadRequest(_respose);
                }

                //var capitulos = JsonConvert.SerializeObject(course.CapituloList);
                //var deberes = JsonConvert.SerializeObject(course.Deberes);
                //var pruebas = JsonConvert.SerializeObject(course.Pruebas);

                //double notaFinal = 0;


                Course model = new()
                {
                    Id = course.Id,
                    Codigo = course.Codigo,
                    ImagenUrl = course.ImagenUrl,
                    Titulo = course.Titulo,
                    Detalle = course.Detalle,
                    FolderId = course.FolderId,
                    //Pruebas = pruebas,
                    //Deberes = deberes,
                    //NotaFinal = notaFinal,
                    Precio = course.Precio,
                    //IsActive = course.IsActive,
                    //Capitulos = capitulos,
                    TeacherId = course.TeacherId
                };

                await _dbCourse.UpdateAsync(model);
                await _dbCourse.SaveAsync();

                _respose.isSuccess = true;
                _respose.StatusCode = HttpStatusCode.OK;
                _respose.Message = "El curso ha sido actualizado con exito";
                return Ok(_respose);
            }
            catch (Exception ex)
            {
                _respose.isSuccess = false;
                _respose.Errors = new List<string>() { ex.ToString() };
            }
            return _respose;
   
        }

        [HttpDelete("deleteCourse/{id:int}", Name = "deleteCourse")]
        public async Task<ActionResult<ApiResponse>> DeleteCourse(int id)
        {
            try
            {
                var course = await _dbCourse.GetAsync(u => u.Id == id);
                if (course == null)
                {
                    _respose.isSuccess = false;
                    _respose.StatusCode = HttpStatusCode.BadRequest;
                    _respose.Message = "Ha ocurrido un error inesperado al eliminar el registro";
                    return BadRequest(_respose);
                }

                await _dbCourse.RemoveAsync(course);
                await _dbCourse.SaveAsync();

                _respose.isSuccess = true;
                _respose.StatusCode = HttpStatusCode.OK;
                _respose.Message = "El curso ha sido eliminado con exito";
                return Ok(_respose);
            }
            catch (Exception ex)
            {
                _respose.isSuccess = false;
                _respose.Errors = new List<string>() { ex.ToString() };
            }
            return _respose;

        }

    }
}
