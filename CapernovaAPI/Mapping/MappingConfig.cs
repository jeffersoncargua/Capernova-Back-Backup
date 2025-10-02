// <copyright file="MappingConfig.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoMapper;
using Capernova.Utility;
using User.Managment.Data.Models.Course;
using User.Managment.Data.Models.Course.DTO;
using User.Managment.Data.Models.Managment;
using User.Managment.Data.Models.Managment.DTO;
using User.Managment.Data.Models.ProductosServicios;
using User.Managment.Data.Models.ProductosServicios.Dto;
using User.Managment.Data.Models.Student;
using User.Managment.Data.Models.Student.DTO;
using User.Managment.Data.Models.Ventas;
using User.Managment.Data.Models.Ventas.Dto;
using User.Managment.Repository.Models;

namespace CapernovaAPI.Mapping
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<CapituloDto, Capitulo>().ReverseMap();
            CreateMap<DeberDto, Deber>().ReverseMap();
            CreateMap<PublicidadDto, Publicidad>().ReverseMap();
            CreateMap<ProductoDto, Producto>().ReverseMap();
            CreateMap<CategoriaDto, Categoria>().ReverseMap();
            CreateMap<PruebaDto, Prueba>().ReverseMap();
            CreateMap<CourseDto, Course>().ReverseMap();
            CreateMap<StudentDto, Student>().ReverseMap();
            CreateMap<VideoDto, Video>().ReverseMap();
            CreateMap<NotaDeberDto, NotaDeber>().ReverseMap();
            CreateMap<NotaPruebaDto, NotaPrueba>().ReverseMap();
            CreateMap<ComentarioDto, Comentario>().ReverseMap();
            CreateMap<EstudianteVideoDto, EstudianteVideo>().ReverseMap();
            CreateMap<TeacherDto, Teacher>().ReverseMap();
            CreateMap<PedidoDto, Pedido>().ReverseMap();
            CreateMap<ShoppingCartDto, ShoppingCart>().ReverseMap();
            CreateMap<ResponseDto, ApiResponse>().ReverseMap();
        }
    }
}