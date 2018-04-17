using System.Collections.Generic;
using Agidea.Core.Interfaces;
using Agidea.Core.Models;

namespace Agidea.Repository
{
    public class EmailRepository : IEmailRepository
    {
        public List<Mail> GetEmails()
        {
            return GetMockEmails();
        }

        private static List<Mail> GetMockEmails()
        {
            return new List<Mail>
            {
                new Mail
                {
                    To = { "scott_vaughan@hotmail.com" },
                    CC = { string.Empty },
                    BCC = { string.Empty },
                    Subject = "Hello World!",
                    Body = "Hello World!"
                }
            };
        }
    }
}