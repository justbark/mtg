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
    public static class Shared
    {
        public static List<Card> cards = new List<Card>();
        public static List<Card> artifacts = new List<Card>();
        public static List<Card> creature = new List<Card>();
        public static List<Card> enchantment = new List<Card>();
        public static List<Card> instant = new List<Card>();
        public static List<Card> land = new List<Card>();
        public static List<Card> planeswalker = new List<Card>();
        public static List<Card> tribal = new List<Card>();
        public static List<Card> sorcery = new List<Card>();
    }

    class Program
    {
        static void Main(string[] args)
        {
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
                Shared.cards.Add(new_card);
            }

            /*for (int i = 0; i < 100; i++) // just checking if we have data
            {
                //just display the first 100
                Console.WriteLine(cards[i].name);
            }*/


            //=======================================================================================
            //this is where we are going to sort lists
            //=======================================================================================
            

            string line;
            Console.WriteLine(Shared.cards.Count); // total number of cards
            line = Console.ReadLine(); //wait for text to generate all sentences file
            if ( line == "getSentences" )
                getSentences(Shared.cards);
            if (line == "getCard")
            {
                string userSelCard;
                Console.WriteLine("type card name.");
                userSelCard = Console.ReadLine();
                retrieveCard(userSelCard);
            }
            if (line == "generateDeck")
            {
                generateDeck(60, 60);
            }
            


        }

        private static void generateDeck(int minCards, int maxCards)
        {
            Deck newDeck = new Deck();

            Random rand = new Random();
            int numCards = rand.Next(minCards, maxCards);

            int cardIndex;

            for (int i = 0; i < numCards; i++)
            {
                cardIndex = rand.Next(0, Shared.cards.Count());
                newDeck.deckCardList.Add(Shared.cards[cardIndex]);
            }
        }

        private static generateName(Deck deck)
        {
            // name the deck after some card in the deck, or card color.
            // These strings should be informed by the contents of newDeck.cards
            String properNoun;
            String adjective;
            String color;
 
 

            // example: "justin's big black deck"
            newDeck.name = properNoun + "\'s" + " " + adjective + " " + color + " deck";
            return "blah";
        }

        static void retrieveCard(string selectedCard)
        {
            string cardName = selectedCard;
            var cardQuery = from card in Shared.cards
                            where card.name == cardName
                            orderby card.name ascending
                            select card;
            foreach ( Card card in cardQuery )
            {
                Console.WriteLine("-------SelectedCard-------");
                Console.WriteLine("card number = " + card.number);
                Console.WriteLine("card text = " + card.text);
                Console.WriteLine("card power = " + card.power);
                Console.WriteLine("card toughness = " + card.toughness);
                Console.WriteLine("card manaCost = " + card.manaCost);
                Console.WriteLine("card colors = " + String.Join(", " , card.colors));
                Console.WriteLine("------EndSelectedCard-------");
                Console.ReadLine();
            }

            
        }

        static void getSentences(List<Card>mycards)
        {
            var masterList = new List<String>();
            List<String> mySentences;
            for (int i = 0; i < mycards.Count; i++ )
            {
                if (String.IsNullOrEmpty(mycards[i].text))
                    continue;
                mySentences = mycards[i].text.Split('.').Distinct().ToList();
                masterList.AddRange(mySentences);
            }
            File.WriteAllLines("allCardText.txt", masterList.ConvertAll(Convert.ToString));
        }
    }
}
