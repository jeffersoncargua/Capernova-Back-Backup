using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Managment.Repository.Models;

namespace User.Managment.Repository.Repository.IRepository
{
    public interface IEmailRepository
    {
        void SendEmail(Message message);
    }
}
