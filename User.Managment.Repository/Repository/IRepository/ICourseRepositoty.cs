using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using User.Managment.Data.Models.Managment;

namespace User.Managment.Repository.Repository.IRepository
{
    public interface ICourseRepositoty : IRepository<Course>
    {
        Task<Course> UpdateAsync(Course entity);
        Task<List<Course>> UpdateRangesAsync(List<Course> entities);

    }
}
