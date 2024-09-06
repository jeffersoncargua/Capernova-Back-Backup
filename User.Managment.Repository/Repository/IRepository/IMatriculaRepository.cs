using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Managment.Data.Models.PaypalOrder;

namespace User.Managment.Repository.Repository.IRepository
{
    public interface IMatriculaRepository : IRepository<Matricula>
    {
        Task<Matricula> UpdateAsync(Matricula entity);
        Task<List<Matricula>> UpdateRangesAsync(List<Matricula> entities);
    }
}
