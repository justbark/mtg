using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mtg
{
    public class Deck
    {
        public List<Card> deckCardList = new List<Card>();
        public List<double> weights = new List<double>();
        string deckName;
        public void PrintWeights()
        {
            for (int i = 0; i < Shared.numOfTypes; i++)
            {
                Console.WriteLine("weights[" + i + "] = " + weights[i] + "\n");
            }
        }
        public Deck()
        {
            for (int i = 0; i < Shared.numOfTypes; i++)
            {
                weights.Add(1.0 / (double)(Shared.numOfTypes));
            }
        }
    }
}
