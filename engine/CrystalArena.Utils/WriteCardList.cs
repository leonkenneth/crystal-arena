using System;
using System.Linq;

namespace CrystalArena.Utils
{
    public class WriteCardList : Task
    {
        public override bool Execute(Arguments arguments)
        {
            foreach (var cardName in Cards.All.OrderBy(x => x.Name).Select(x => x.Name))
            {
                Console.WriteLine(cardName);
            }

            return true;
        }

        public override void Usage()
        {
            Console.WriteLine(
                "usage: uCrystalArena list\n\nWrites available card names to stdout."
            );
        }
    }
}
