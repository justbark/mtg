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
            var cards = new List<Card>();
            String json = "";
            try
            {
                using (StreamReader sr = new StreamReader("..\\..\\..\\AllCards-x.json"))
                {
                    json = sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }


            dynamic data = JsonConvert.DeserializeObject(json);
            IDictionary<string, JToken> raw_cards = data;

            Card new_card;
            foreach (var card in raw_cards)
            {
                new_card = JsonConvert.DeserializeObject<Card>(card.Value.ToString());
                cards.Add(new_card);
            }

            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine(cards[i].name);
            }
                Console.ReadLine();

        }
    }
}
