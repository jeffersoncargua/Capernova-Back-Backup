// <copyright file="IStudentRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Http;
using User.Managment.Data.Models.Course.DTO;
using User.Managment.Data.Models.PaypalOrder.Dto;
using User.Managment.Data.Models.Student;
using User.Managment.Data.Models.Student.DTO;
using User.Managment.Repository.Models;

namespace User.Managment.Repository.Repository.IRepository
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<ResponseDto> GetStudentAsync(string? id);

        Task<ResponseDto> GetCursosAsync(string? id);

        Task<ResponseDto> GetCursoAsync(int id);

        Task<ResponseDto> GetCapitulosAsync(int? id);

        Task<ResponseDto> GetVideosAsync(int? id);

        Task<ResponseDto> GetAllTaskAsync(int? id);

        Task<ResponseDto> GetGradeTaskAsync(int? id, string studentId);

        Task<ResponseDto> GetViewVideosAsync(string? studentId, int cursoId);

        Task<ResponseDto> GetAllTestAsync(int? id);

        Task<ResponseDto> GetGradeTestAsync(int? id, string studentId);

        Task<ResponseDto> GetCertificateAsync(string? studentId, int? cursoId);

        Task<ResponseDto> CreateViewVideoAsync(EstudianteVideoDto estudianteVideoDto);

        Task<ResponseDto> CreateCommentaryAsync(ComentarioDto comentarioDto);

        Task<ResponseDto> UpdateStudentAsync(string? id, StudentDto studentDto);

        Task<ResponseDto> UpdateMatriculaAsync(int id, MatriculaDto matriculaDto);

        Task<ResponseDto> UpdateImageStudentAsync(string id, IFormFile? file);

        Task<ResponseDto> UpsertGradeTaskAsync(int id, string studentId, IFormFile? file);
    }
}