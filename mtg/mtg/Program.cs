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
        public static int numOfTypes = 7;
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
            //generate Weights
            //================================================================
            mutate(5, newDeck);
            newDeck.PrintWeights();
            //================================================================
            //landSection
            //================================================================
            int maxLand = 30;
            int minLand = 23;
            int landQuantity = rand.Next(minLand, maxLand);
            Console.WriteLine("land quatity = " + landQuantity);
            int cardsRemaining = numCards - landQuantity;
            Console.WriteLine(" Cards remaining = " + cardsRemaining);
            //this is just determining the number of lands for the deck. not actually selecting them

            //================================================================
            //deck quantities
            //===============================================================
            int maxCopyOfCard = 4;
            int maxColors = 2;
            //================================================================
            //percentages for deck 
            //================================================================

            /*--------minimum percentages----------
            double minPercInstant = 0.17;
            double minPercSorcery = 0.17;
            double minPercCreature = 0.24;
            double minPercEnchantment = 0.17;
            double minPercPlaneswalker = 0.05;
            double minPercArtifact = 0.10;
            double minPercTribal = 0.10;

            //---------maximum percentages---------
            double maxPercInstant = 0.20;
            double maxPercSorcery = 0.20;
            double maxPercCreature = 0.50;
            double maxPercEnchantment = 0.20;
            double maxPercPlaneswalker = 0.10;
            double maxPercArtifact = 0.50;
            double maxPercTribal = 0.20;*/

            //================================================================
            //card selection 
            //================================================================
            int quantInstant = (int)Math.Round(newDeck.weights[0] * cardsRemaining);
            int quantSorcery = (int)Math.Round(newDeck.weights[1] * cardsRemaining);
            int quantCreature = (int)Math.Round(newDeck.weights[2] * cardsRemaining);
            int quantEnchantment = (int)Math.Round(newDeck.weights[3] * cardsRemaining);
            int quantPlaneswalker = (int)Math.Round(newDeck.weights[4] * cardsRemaining);
            int quantArtifact = (int)Math.Round(newDeck.weights[5] * cardsRemaining);
            int quantTribal = (int)Math.Round(newDeck.weights[6] * cardsRemaining);

            //get the number of cards for the deck via the random deck quantities
            int totalCards = (quantInstant + quantSorcery + quantCreature + quantEnchantment + quantPlaneswalker + quantArtifact + quantTribal);
            Console.WriteLine("total cards = " + totalCards);
            //get the total number of cards including land
            int numGeneratedCards = landQuantity + totalCards;
            Console.WriteLine("numGeneratedCards = " + numGeneratedCards);
            //this is the number of cards that the deck is missing, due to decimals
            int requiredAdjustment = numCards - numGeneratedCards; // FIX ME. do something more intelligent with this value.
            Console.WriteLine("requiredAdjustment = " + requiredAdjustment);
            Console.WriteLine("Joes total cards =\n qI=" + quantInstant + "\n qS=" + quantSorcery + "\n qC="  + quantCreature + "\n qE="  + quantEnchantment + "\n qP="  + quantPlaneswalker + "\n qA="  + quantArtifact + "\n qT="  + quantTribal);
            Console.WriteLine("land quantity = " + landQuantity);

            while (quantInstant != 0)
            {
                int randomCardIndex = rand.Next(0, Shared.instant.Count);
                Card randomCard = Shared.instant[randomCardIndex];
                //create a subset of random cards. This keeps track to see if there are multiples of 1 card
                if (newDeck.deckCardList.Where(x => x == randomCard).Count() >= maxCopyOfCard)
                {
                    Console.WriteLine("Oops I picked a card too many times, moving on.");
                    continue;
                }
                if (!randomCard.colors.Contains(primaryColor))
                {
                    Console.WriteLine("this card is not the correct color. Moving on.");
                    continue;
                }
                newDeck.deckCardList.Add(randomCard);
                quantInstant--;
                Console.WriteLine("Ive selected " + randomCard.name + "\n" + "the deckSize is now " + newDeck.deckCardList.Count());
                    
            }

            while (quantSorcery != 0)
            {
                int randomCardIndex = rand.Next(0, Shared.instant.Count);
                Card randomCard = Shared.instant[randomCardIndex];
                //create a subset of random cards. This keeps track to see if there are multiples of 1 card
                if (newDeck.deckCardList.Where(x => x == randomCard).Count() >= maxCopyOfCard)
                {
                    Console.WriteLine("Oops I picked a card too many times, moving on.");
                    continue;
                }
                if (!randomCard.colors.Contains(primaryColor))
                {
                    Console.WriteLine("this card is not the correct color. Moving on.");
                    continue;
                }
                newDeck.deckCardList.Add(randomCard);
                quantSorcery--;
                Console.WriteLine("Ive selected " + randomCard.name + "\n" + "the deckSize is now " + newDeck.deckCardList.Count());

            }

            while (quantCreature != 0)
            {
                int randomCardIndex = rand.Next(0, Shared.instant.Count);
                Card randomCard = Shared.instant[randomCardIndex];
                //create a subset of random cards. This keeps track to see if there are multiples of 1 card
                if (newDeck.deckCardList.Where(x => x == randomCard).Count() >= maxCopyOfCard)
                {
                    Console.WriteLine("Oops I picked a card too many times, moving on.");
                    continue;
                }
                if (!randomCard.colors.Contains(primaryColor))
                {
                    Console.WriteLine("this card is not the correct color. Moving on.");
                    continue;
                }
                newDeck.deckCardList.Add(randomCard);
                quantCreature--;
                Console.WriteLine("Ive selected " + randomCard.name + "\n" + "the deckSize is now " + newDeck.deckCardList.Count());

            }

            while (quantEnchantment != 0)
            {
                int randomCardIndex = rand.Next(0, Shared.instant.Count);
                Card randomCard = Shared.instant[randomCardIndex];
                //create a subset of random cards. This keeps track to see if there are multiples of 1 card
                if (newDeck.deckCardList.Where(x => x == randomCard).Count() >= maxCopyOfCard)
                {
                    Console.WriteLine("Oops I picked a card too many times, moving on.");
                    continue;
                }
                if (!randomCard.colors.Contains(primaryColor))
                {
                    Console.WriteLine("this card is not the correct color. Moving on.");
                    continue;
                }
                newDeck.deckCardList.Add(randomCard);
                quantEnchantment--;
                Console.WriteLine("Ive selected " + randomCard.name + "\n" + "the deckSize is now " + newDeck.deckCardList.Count());

            }

            while (quantPlaneswalker != 0)
            {
                int randomCardIndex = rand.Next(0, Shared.instant.Count);
                Card randomCard = Shared.instant[randomCardIndex];
                //create a subset of random cards. This keeps track to see if there are multiples of 1 card
                if (newDeck.deckCardList.Where(x => x == randomCard).Count() >= maxCopyOfCard)
                {
                    Console.WriteLine("Oops I picked a card too many times, moving on.");
                    continue;
                }
                if (!randomCard.colors.Contains(primaryColor))
                {
                    Console.WriteLine("this card is not the correct color. Moving on.");
                    continue;
                }
                newDeck.deckCardList.Add(randomCard);
                quantPlaneswalker--;
                Console.WriteLine("Ive selected " + randomCard.name + "\n" + "the deckSize is now " + newDeck.deckCardList.Count());

            }

            while (quantArtifact != 0)
            {
                int randomCardIndex = rand.Next(0, Shared.instant.Count);
                Card randomCard = Shared.instant[randomCardIndex];
                //create a subset of random cards. This keeps track to see if there are multiples of 1 card
                if (newDeck.deckCardList.Where(x => x == randomCard).Count() >= maxCopyOfCard)
                {
                    Console.WriteLine("Oops I picked a card too many times, moving on.");
                    continue;
                }
                if (!randomCard.colors.Contains(primaryColor))
                {
                    Console.WriteLine("this card is not the correct color. Moving on.");
                    continue;
                }
                newDeck.deckCardList.Add(randomCard);
                quantArtifact--;
                Console.WriteLine("Ive selected " + randomCard.name + "\n" + "the deckSize is now " + newDeck.deckCardList.Count());

            }

            while (quantTribal != 0)
            {
                int randomCardIndex = rand.Next(0, Shared.instant.Count);
                Card randomCard = Shared.instant[randomCardIndex];
                //create a subset of random cards. This keeps track to see if there are multiples of 1 card
                if (newDeck.deckCardList.Where(x => x == randomCard).Count() >= maxCopyOfCard)
                {
                    Console.WriteLine("Oops I picked a card too many times, moving on.");
                    continue;
                }
                if (!randomCard.colors.Contains(primaryColor))
                {
                    Console.WriteLine("this card is not the correct color. Moving on.");
                    continue;
                }
                newDeck.deckCardList.Add(randomCard);
                quantTribal--;
                Console.WriteLine("Ive selected " + randomCard.name + "\n" + "the deckSize is now " + newDeck.deckCardList.Count());

            }
            //======================================================
            //adding land to the deck
            //======================================================

            Card landCard = Shared.land.Where(x => x.colors.Contains(primaryColor)).First();
            if (landCard != null)
            {
                for (int i = 0; i < landQuantity; i++)
                {
                    newDeck.deckCardList.Add(landCard);
                }
            }
            else
            {
                Console.WriteLine("there has been an error. No mana card found");
                return;
            }
                

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
        public static void mutate(int passes, Deck deck)
        {
            double fudge = 0.33; // this adjusts our mutation amplitude
            Random rand = new Random();
            for (int i = 0; i < passes; i++)
            {

                int rand_a = 0;
                int rand_b = 0;
                while (rand_a == rand_b)
                {
                    rand_a = rand.Next(0, deck.weights.Count - 1);
                    rand_b = rand.Next(0, deck.weights.Count - 1);
                }
                float mutationMagnitude = (float)(rand.NextDouble() * deck.weights.Average() * fudge);
                //Console.WriteLine("mutation magnitude = " + mutationMagnitude);
                if (deck.weights[rand_a] - mutationMagnitude <= 0.1 || deck.weights[rand_b] + mutationMagnitude >= 0.90)
                {
                    continue;
                }
                deck.weights[rand_a] -= mutationMagnitude;
                deck.weights[rand_b] += mutationMagnitude;
            }
        }

        public static double GetRandomNumber(double minimum, double maximum)
        {
            //this is used only when a fractional number is needed
            Random random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

        static void retrieveCard(string selectedCard)
        {
            //user can type the name of a card to display
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
