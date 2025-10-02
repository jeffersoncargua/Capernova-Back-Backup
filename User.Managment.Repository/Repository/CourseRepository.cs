// <copyright file="CourseRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoMapper;
using System.Net;
using User.Managment.Data.Data;
using User.Managment.Data.Models.Course;
using User.Managment.Data.Models.Course.DTO;
using User.Managment.Data.Models.ProductosServicios;
using User.Managment.Repository.Models;
using User.Managment.Repository.Repository.IRepository;

namespace User.Managment.Repository.Repository
{
    public class CourseRepository : Repository<Course>, ICourseRepositoty
    {
        private readonly ApplicationDbContext _db;
        private readonly IProductoRepository _repoProducto;
        private readonly IMapper _mapper;
        protected ResponseDto _response;

        public CourseRepository(ApplicationDbContext db, IMapper mapper, IProductoRepository repoProducto)
            : base(db)
        {
            _db = db;
            _mapper = mapper;
            _repoProducto = repoProducto;
            this._response = new();
        }

        public async Task<ResponseDto> CreateCourse(CourseDto course)
        {
            try
            {
                if (course.CategoriaId == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Debe seleccionar una categoría para el curso";
                }
                else
                {
                    if (await this.GetAsync(u => u.Titulo!.ToLower() == course.Titulo!.ToLower() || u.Codigo!.ToLower() == course.Codigo!.ToLower(), tracked: false) != null)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.Message = "El curso ya está registrado con un titulo o código similar!!";
                    }
                    else
                    {
                        // Se genera el modelo del curso que se va a enviar a almacenar en la base de datos
                        await this.CreateAsync(_mapper.Map<Course>(course));

                        Producto producto = new()
                        {
                            Codigo = course.Codigo!,
                            ImagenUrl = course.ImagenUrl,
                            Titulo = course.Titulo!,
                            Detalle = course.Detalle,
                            Precio = course.Precio,
                            Tipo = "curso",
                            Cantidad = 1,
                            CategoriaId = course.CategoriaId,
                        };

                        await _repoProducto.CreateAsync(producto);

                        _response.IsSuccess = true;
                        _response.StatusCode = HttpStatusCode.Created;
                        _response.Message = "El curso ha sido registrado con éxito";
                    }
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> DeleteCourse(int id)
        {
            try
            {
                var course = await this.GetAsync(u => u.Id == id, tracked: false);
                var producto = await _repoProducto.GetAsync(u => u.Codigo == course.Codigo, tracked: false);
                if (course == null || producto == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Ha ocurrido un error inesperado al eliminar el registro";
                }
                else
                {
                    await this.RemoveAsync(course);

                    await _repoProducto.RemoveAsync(producto);

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "El curso ha sido eliminado con éxito";
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> GetAllCourses(string? search)
        {
            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    var coursesQuery = await this.GetAllAsync(u => u.Titulo!.ToLower().Contains(search), tracked: false);
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de Cursos";
                    _response.Result = _mapper.Map<List<CourseDto>>(coursesQuery);
                }
                else
                {
                    var courses = await this.GetAllAsync(); // devuelve una lista con los cursos
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido la lista de Cursos";
                    _response.Result = _mapper.Map<List<CourseDto>>(courses);
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> GetCourse(int id)
        {
            try
            {
                var course = await this.GetAsync(u => u.Id == id, tracked: false); // devuelve el curso cuyo id sea igual al Id del curso
                if (course == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "El registro no existe!";
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido el curso con éxito!";
                    _response.Result = _mapper.Map<CourseDto>(course);
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> GetCourseCode(string code)
        {
            try
            {
                var course = await this.GetAsync(u => u.Codigo == code, tracked: false); // devuelve el curso cuyo id sea igual al Id del curso
                if (course == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "El registro no existe!";
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Message = "Se ha obtenido el curso con éxito!";
                    _response.Result = _mapper.Map<CourseDto>(course);
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        public async Task<ResponseDto> UpdateAsync(int id, CourseDto course)
        {
            try
            {
                if (course.CategoriaId == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Debe seleccionar una categoría para el curso";
                }
                else
                {
                    var courseFromDb = await this.GetAsync(u => u.Id == course.Id, tracked: false);
                    var productoFromDb = await _repoProducto.GetAsync(u => u.Codigo == courseFromDb.Codigo, tracked: false);
                    if (courseFromDb == null || course == null || id != course.Id || productoFromDb == null)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.Message = "Ha ocurrido un error y no se pudo actualizar el registro";
                    }
                    else
                    {
                        if ((courseFromDb.Codigo!.ToLower() != course.Codigo!.ToLower() && productoFromDb.Codigo!.ToLower() != course.Codigo!.ToLower()) || (courseFromDb.Titulo!.ToLower() != course.Titulo!.ToLower() && productoFromDb.Titulo!.ToLower() != course.Titulo.ToLower()))
                        {
                            var coursesCode = await this.GetAllAsync(u => u.Titulo!.ToLower() == course.Titulo!.ToLower() || u.Codigo!.ToLower() == course.Codigo!.ToLower(), tracked: false);
                            foreach (var item in coursesCode)
                            {
                                if (item.Id != id)
                                {
                                    _response.IsSuccess = false;
                                    _response.StatusCode = HttpStatusCode.BadRequest;
                                    _response.Message = "Error. No se puede actualizar el curso con un código o titulo que ya existe!";
                                }
                            }
                        }
                        else
                        {
                            _db.CourseTbl.Update(_mapper.Map<Course>(course));
                            await _db.SaveChangesAsync();

                            if (course.CategoriaId == 0)
                            {
                                Producto producto = new()
                                {
                                    Id = productoFromDb.Id,
                                    Codigo = course.Codigo,
                                    ImagenUrl = course.ImagenUrl,
                                    Titulo = course.Titulo,
                                    Detalle = course.Detalle,
                                    Precio = course.Precio,
                                    Tipo = productoFromDb.Tipo,
                                    Cantidad = productoFromDb.Cantidad,
                                    CategoriaId = productoFromDb.CategoriaId,
                                };

                                _db.ProductoTbl.Update(producto);
                                await _db.SaveChangesAsync();
                            }
                            else
                            {
                                Producto producto = new()
                                {
                                    Id = productoFromDb.Id,
                                    Codigo = course.Codigo,
                                    ImagenUrl = course.ImagenUrl,
                                    Titulo = course.Titulo,
                                    Detalle = course.Detalle,
                                    Precio = course.Precio,
                                    Tipo = productoFromDb.Tipo,
                                    Cantidad = productoFromDb.Cantidad,
                                    CategoriaId = course.CategoriaId,
                                };

                                _db.ProductoTbl.Update(producto);
                                await _db.SaveChangesAsync();
                            }

                            _response.IsSuccess = true;
                            _response.StatusCode = HttpStatusCode.OK;
                            _response.Message = "El curso ha sido actualizado con éxito";
                        }
                    }
                }

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors = new List<string>() { ex.ToString() };
            }

            return _response;
        }
    }
}