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
                    To = new List<string>
                    {
                        "scott_vaughan@hotmail.com"
                    },
                    CC = new List<string>(),
                    BCC = new List<string>(),
                    Subject = "Hello World!",
                    Body = "Hello World!"
                }
            };
        }
    }
}