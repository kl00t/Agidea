using System.Collections.Generic;
using Agidea.Core.Models;

namespace Agidea.Core.Interfaces
{
    public interface IEmailRepository
    {
        List<Mail> GetEmails();
    }
}