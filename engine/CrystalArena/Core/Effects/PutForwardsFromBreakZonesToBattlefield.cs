namespace CrystalArena.Effects
{
    using System.Linq;
    using CrystalArena.Modifiers;

    public class PutForwardsFromBreakZonesToYourBattlefield : Effect
    {
        protected override void ResolveEffect()
        {
            var forwards = Players.SelectMany(x => x.BreakZone).Where(x => x.Is().Forward).ToList();

            foreach (var card in forwards)
            {
                if (card.Owner != Controller)
                {
                    var modifier = new ChangeController(Controller);

                    var p = new ModifierParameters
                    {
                        SourceEffect = this,
                        SourceCard = Source.OwningCard,
                        X = X,
                    };

                    card.AddModifier(modifier, p);
                }

                Controller.PutCardToBattlefield(card);
            }
        }
    }
}
