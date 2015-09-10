using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace mtg
{
    class Program
    {
        static void Main(string[] args)
        {
            //string json = @"{
            //    'Email': 'james@example.com',
            //    'Active': true,
            //    'CreatedDate': '2013-01-20T00:00:00Z',
            //    'Roles': [
            //    'User',
            //    'Admin'
            //    ]
            //    }";
            //
            //    Account account = JsonConvert.DeserializeObject<Account>(json);
            //    Console.WriteLine(account.Email);
            //    Console.ReadLine();
                // james@example.com
            String json = "";
            try
            {
                using (StreamReader sr = new StreamReader("..\\..\\..\\AllCards-x.json"))
                {
                    json = sr.ReadToEnd();
                    //Console.WriteLine(line);
                    Console.ReadLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }


            dynamic obj = null;
            if (!String.IsNullOrEmpty(json))
            {
                obj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                foreach (JObject result in obj)
                {
                    foreach (JProperty property in result.Properties())
                    {
                        // do something with the property belonging to result
                        Console.WriteLine(property.Name);
                    }
                }
            }
            else
            {
                Console.WriteLine("JSON string is empty");
                return;
            }

        }
    }
}
