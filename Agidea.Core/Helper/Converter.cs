using Agidea.Core.Models;
using Newtonsoft.Json;

namespace Agidea.Core.Helper
{
    public class Converter
    {
        // TODO: Pass parameter in as a type.

        public static string ConvertToJson(Message message)
        {
            return JsonConvert.SerializeObject(message);
        }

        public static string ConvertToJson(Mail mail)
        {
            return JsonConvert.SerializeObject(mail);
        }
    }
}
