// <copyright file="ITeacherRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Http;
using User.Managment.Data.Models.Managment;
using User.Managment.Data.Models.Managment.DTO;
using User.Managment.Repository.Models;

namespace User.Managment.Repository.Repository.IRepository
{
    public interface ITeacherRepository : IRepository<Teacher>
    {
        Task<ResponseDto> GetAllTeacherAsync();

        Task<ResponseDto> GetAllCourseAsync(string id, string? search);

        Task<ResponseDto> GetAllStudentAsync(int? cursoId, string? search, string start, string end);

        Task<ResponseDto> GetTeacherAsync(string id);

        Task<ResponseDto> UpdateTeacherAsync(string id, TeacherDto teacherDto);

        Task<ResponseDto> UpdateImageTeacherAsync(string id, IFormFile? file);

        Task<ResponseDto> UpdateGradeTaskAsync(int? id, string studentId, string? calificacion);

        Task<ResponseDto> UpsertGradeTestAsync(int id, string studentId, string? calificacion);

        Task<ResponseDto> UpdateGradeMatriculaAsync(int id, string? notaFinal);

        Task<ResponseDto> UpdateMatriculaStateAsync(int id, string studentId, bool isActive);
    }
}