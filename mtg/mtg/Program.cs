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
            Shared.artifacts = Shared.cards.Where(x => x.type.Contains("Artifact")).ToList();
            Shared.creature = Shared.cards.Where(x => x.type.Contains("Creature")).ToList();
            Shared.enchantment = Shared.cards.Where(x => x.type.Contains("Enchantment")).ToList();
            Shared.instant = Shared.cards.Where(x => x.type.Contains("Instant")).ToList();
            Shared.land = Shared.cards.Where(x => x.type.Contains("Land")).ToList();
            Shared.planeswalker = Shared.cards.Where(x => x.type.Contains("Planeswalker")).ToList();
            Shared.tribal = Shared.cards.Where(x => x.type.Contains("Tribal")).ToList();
            Shared.sorcery = Shared.cards.Where(x => x.type.Contains("Sorcery")).ToList();

            //=======================================================================================
            //gets user input
            //=======================================================================================
            Console.WriteLine(Shared.cards.Count); // total number of cards
            string line = "";
            while(line != "exit" && line != "quit" && line != "q")
            {
                line = Console.ReadLine(); //wait for text to generate all sentences file
                if (line == "getSentences")
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
                line = "";
            }

            


        }

        public static void generateDeck(int minCards, int maxCards)
        {
            string[] colors = new string[] { "Blue", "Black", "Green", "Red", "White" };
            Deck newDeck = new Deck();

            Random rand = new Random();
            int index = rand.Next(0, colors.Length);
            Console.WriteLine(index);
            string primaryColor = colors[index];
            Console.WriteLine(primaryColor);

            int numCards = rand.Next(minCards, maxCards);
            Console.WriteLine(numCards);

            //================================================================
            //deck quantities
            //================================================================
            int maxLand = 30;
            int minLand = 23;
            int maxCopyOfCard = 4;
            int maxColors = 2;

            //================================================================
            //percentages for deck 
            //================================================================

            //--------minimum percentages----------
            double percInstant = 0.17;
            double percSorcery = 0.17;
            double percCreature = 0.24;
            double percEnchantment = 0.17;
            double percPlaneswalker = 0.05;
            double percArtifact = 0.10;
            double percTribal = 0.10;

            //---------maximum percentages---------


            //================================================================
            //landSection
            //================================================================
            int landQuantity = rand.Next(minLand, maxLand);
            Console.WriteLine("land quatity = " + landQuantity);
            int cardsRemaining = numCards - landQuantity;
            Console.WriteLine(" Cards remaining = " + cardsRemaining);
            //this is just determining the number of lands for the deck. not actually selecting them


            /*quantOLand = 25
            rem  =60- 25 = 35
            max/minArtType
            0.20 0.10
            QuantArtType = rand.Next(minArtType, maxArtType)
            0.1578
            rem*QuantArtType =
            3.8
            4
            rem -= 4
            rem = rem - 4;
             newDeck.cards.Add(Shared.artifacts[whateverIndex]);
             the line above is how you get a new card into the deck list*/




            /*int cardIndex;

            for (int i = 0; i < numCards; i++)
            {
                cardIndex = rand.Next(0, Shared.cards.Count());
                newDeck.deckCardList.Add(Shared.cards[cardIndex]);
            }*/
        }

        
        /*private static generateName(Deck deck)
        {
            // name the deck after some card in the deck, or card color.
            // These strings should be informed by the contents of newDeck.cards
            String properNoun;
            String adjective;
            String color;
 
 

            // example: "justin's big black deck"
            newDeck.name = properNoun + "\'s" + " " + adjective + " " + color + " deck";
            return "blah";
        }*/

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
