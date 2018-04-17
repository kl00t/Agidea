using System.Collections.Generic;

namespace Agidea.Core.Interfaces
{
    public interface IMailer
    {
        List<string> ReadFromQueue();
    }
}