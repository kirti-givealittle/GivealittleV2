using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GivealittleV2.Application.Interfaces.Email
{
    public interface IEmailSender
    {
        Task SendAsync(EmailSendRequestDTO req);
    }
}
