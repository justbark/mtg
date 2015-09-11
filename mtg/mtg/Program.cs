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
                return;
            }


            dynamic data = JsonConvert.DeserializeObject(json);
            IDictionary<string, JToken> raw_cards = data;
            //this is very complicated. Joe will explain later in more detail

            Card new_card;
            foreach (var card in raw_cards)
            {
                new_card = JsonConvert.DeserializeObject<Card>(card.Value.ToString());
                // <Card> this is a template type. You can write a piece of code that can
                //work with any code
                cards.Add(new_card);
            }

            for (int i = 0; i < 100; i++)
            {
                //just display the first 100
                Console.WriteLine(cards[i].name);
            }
            getSentences(cards);
            Console.WriteLine(cards.Count); // total number of cards
            Console.ReadLine();

        }
        void getSentences(List<Card>mycards)
        {
            var masterList = new List<String>();
            for (int i = 0; i < mycards.Count; i++ )
            {
                var mySentences = mycards[i].text.Split('.').Distinct().ToList();
                masterList.AddRange(mySentences);
            }
            File.WriteAllLines("allCardText.txt", masterList.ConvertAll(Convert.ToString));
        }
    }
}
