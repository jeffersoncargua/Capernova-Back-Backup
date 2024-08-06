using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using User.Managment.Data.Data;
using User.Managment.Data.Models.Course;
//using User.Managment.Data.Models.Managment;
using User.Managment.Repository.Repository.IRepository;

namespace User.Managment.Repository.Repository
{
    public class CourseRepository : Repository<Course>, ICourseRepositoty
    {
        private readonly ApplicationDbContext _db;

        public CourseRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
        public async Task<Course> UpdateAsync(Course entity)
        {
            _db.CourseTbl.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<List<Course>> UpdateRangesAsync(List<Course> entities)
        {
            _db.CourseTbl.UpdateRange(entities);
            await _db.SaveChangesAsync();
            return entities;
        }
    }
}
