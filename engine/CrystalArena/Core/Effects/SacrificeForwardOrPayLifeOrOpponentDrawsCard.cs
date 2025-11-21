namespace CrystalArena.Effects
{
    using System.Collections.Generic;
    using System.Linq;
    using AI;
    using Decisions;

    public class SacrificeForwardOrPayLifeOrOpponentDrawsCard
        : CustomizableEffect,
            IProcessDecisionResults<ChosenCards>,
            IChooseDecisionResults<List<Card>, ChosenCards>
    {
        private readonly int _lifeAmount;

        private SacrificeForwardOrPayLifeOrOpponentDrawsCard() { }

        public SacrificeForwardOrPayLifeOrOpponentDrawsCard(int lifeAmount)
        {
            _lifeAmount = lifeAmount;
        }

        protected override Player SelectChoosingPlayer()
        {
            return Controller.Opponent;
        }

        private class ScoreAndOption
        {
            public int Score;
            public EffectOption Option;
        }

        public override ChosenOptions ChooseResult(List<IEffectChoice> candidates)
        {
            var opponent = Controller.Opponent;
            var options = new List<ScoreAndOption>
            {
                new ScoreAndOption
                {
                    Score = ScoreCalculator.CalculateLifelossScore(opponent.Life, _lifeAmount),
                    Option = EffectOption.PayLife,
                },
                new ScoreAndOption
                {
                    Score = ScoreCalculator.HiddenCardInHandScore,
                    Option = EffectOption.OpponentDrawsACard,
                },
            };

            if (candidates[0].Options.Contains(EffectOption.SacrificeAForward))
            {
                options.Add(
                    new ScoreAndOption
                    {
                        Score = SelectAForward().Score,
                        Option = EffectOption.SacrificeAForward,
                    }
                );
            }

            return new ChosenOptions(options.OrderBy(x => x.Score).First().Option);
        }

        private Card SelectAForward()
        {
            return Controller.Opponent.Battlefield.Forwards.OrderBy(x => x.Score).First();
        }

        public override void ProcessResults(ChosenOptions results)
        {
            var option = (EffectOption)results.Options[0];

            if (option == EffectOption.SacrificeAForward)
            {
                Enqueue(
                    new SelectCards(
                        Controller.Opponent,
                        p =>
                        {
                            p.SetValidator(card => card.Is().Forward);
                            p.Zone = Zone.Battlefield;
                            p.Text = "Select a forward to sacrifice.";
                            p.OwningCard = Source.OwningCard;
                            p.ProcessDecisionResults = this;
                            p.ChooseDecisionResults = this;
                            p.MinCount = 1;
                            p.MaxCount = 1;
                        }
                    )
                );
                return;
            }

            if (option == EffectOption.OpponentDrawsACard)
            {
                Controller.DrawCard();
                return;
            }

            Controller.Opponent.Life -= _lifeAmount;
        }

        public ChosenCards ChooseResult(List<Card> candidates)
        {
            return new ChosenCards(SelectAForward());
        }

        public void ProcessResults(ChosenCards results)
        {
            results[0].Sacrifice();
        }

        public override string GetText()
        {
            return "Select an effect: #0.";
        }

        public override IEnumerable<IEffectChoice> GetChoices()
        {
            if (Controller.Opponent.Battlefield.Forwards.Any())
            {
                yield return new DiscreteEffectChoice(
                    EffectOption.PayLife,
                    EffectOption.SacrificeAForward,
                    EffectOption.OpponentDrawsACard
                );
            }
            else
            {
                yield return new DiscreteEffectChoice(
                    EffectOption.PayLife,
                    EffectOption.OpponentDrawsACard
                );
            }
        }
    }
}
