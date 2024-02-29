using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Management.Services.Models;

namespace User.Management.Services.Services
{
    public interface IEmailServices
    {
        public void SendMail(Message message);
    }
}
