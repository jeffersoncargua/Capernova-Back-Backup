// <copyright file="ICourseRepositoty.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using User.Managment.Data.Models.Course;
using User.Managment.Data.Models.Course.DTO;
using User.Managment.Repository.Models;

namespace User.Managment.Repository.Repository.IRepository
{
    public interface ICourseRepositoty : IRepository<Course>
    {
        Task<ResponseDto> GetAllCourses(string? search);

        Task<ResponseDto> GetCourse(int id);

        Task<ResponseDto> GetCourseCode(string code);

        Task<ResponseDto> CreateCourse(CourseDto course);

        Task<ResponseDto> UpdateAsync(int id, CourseDto course);

        Task<ResponseDto> DeleteCourse(int id);

        // Task<List<Course>> UpdateRangesAsync(List<Course> entities);
    }
}