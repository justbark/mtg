using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            try
            {
                using (StreamReader sr = new StreamReader("../../AllCards-x.json"))
                {
                    String line = sr.ReadToEnd();
                    Console.WriteLine(line);
                    Console.ReadLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

        }
    }
}
