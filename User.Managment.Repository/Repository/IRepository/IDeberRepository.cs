using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Managment.Data.Models.Course;

namespace User.Managment.Repository.Repository.IRepository
{
    public interface IDeberRepository: IRepository<Deber>
    {
        Task<Deber> UpdateAsync(Deber entity);
        Task<List<Deber>> UpdateRangesAsync(List<Deber> entities);
    }
}
