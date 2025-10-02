// <copyright file="IManagmentRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using User.Managment.Data.Models.Authentication.SignUp;
using User.Managment.Data.Models.PaypalOrder.Dto;
using User.Managment.Repository.Models;

namespace User.Managment.Repository.Repository.IRepository
{
    public interface IManagmentRepository
    {
        Task<ResponseDto> Register(RegisterUserDto registerUser);

        Task<ResponseDto> GetTalent(string? searchRole, string? searchName);

        Task<ResponseDto> DeleteUser(string id);

        Task<ResponseDto> GetAllStudent(string? search);

        Task<ResponseDto> AssigmentCourseAsync(int id, string teacherId);

        Task<ResponseDto> DeleteAssigmentCourseAsync(int id, string teacherId);

        Task<ResponseDto> GetAllCourse(string? search);

        Task<ResponseDto> GetAllCommentaries(string? search);

        Task<ResponseDto> GetUserAsync(string search);

        Task<ResponseDto> GetMatriculaAsync(string userId);

        Task<ResponseDto> CreateMatriculaAsync(MatriculaDto matriculaDto);

        Task<ResponseDto> DeleteMatriculaAsync(int id);
    }
}