namespace CrystalArena.AI.TargetingRules
{
    using System.Collections.Generic;
    using System.Linq;

    public class PreventNextDamage
    {
        private readonly int _amount;
        private readonly Game _game;
        private readonly TargetingRuleParameters _p;

        private PreventNextDamage(int amount, TargetingRuleParameters p, Game game)
        {
            _amount = amount;
            _p = p;
            _game = game;
        }

        public static IList<ITarget> GetCandidates(int amount, TargetingRuleParameters p, Game game)
        {
            var preventNextDamage = new PreventNextDamage(amount, p, game);
            return preventNextDamage.GetCandidates();
        }

        private IList<ITarget> GetCandidates()
        {
            if (!_game.Stack.IsEmpty)
            {
                return PreventDamageTopSpellWillDealToForwardOrPlayer();
            }

            if (_game.Turn.Step == Step.DeclareBlocker)
            {
                return _p.Controller.IsActive
                    ? PreventDamageBlockerWillDealToAttacker()
                    : PreventDamageAttackerWillDealToPlayerOrBlocker();
            }

            return new ITarget[] { };
        }

        private List<ITarget> PreventDamageAttackerWillDealToPlayerOrBlocker()
        {
            var playerCandidate = _p.Candidates<Player>(
                    ControlledBy.SpellOwner,
                    selector: c => c.Effect
                )
                .Where(x => _game.Combat.WillAnyAttackerDealDamageToDefender());

            var forwardCandidates = _p.Candidates<Card>(
                    ControlledBy.SpellOwner,
                    selector: c => c.Effect
                )
                .Where(x => x.IsBlocker)
                .Where(x =>
                {
                    var prevented =
                        QuickCombat.GetAmountOfDamageThatNeedsToBePreventedToSafeBlockerFromDying(
                            blocker: x,
                            party: new Party(_game.Combat.Attackers)
                        );

                    return 0 < prevented && prevented <= _amount;
                })
                .OrderByDescending(x => x.Score);

            var candidates = new List<ITarget>();
            candidates.AddRange(playerCandidate);
            candidates.AddRange(forwardCandidates);

            return candidates;
        }

        private List<ITarget> PreventDamageBlockerWillDealToAttacker()
        {
            var candidates = _p.Candidates<Card>(ControlledBy.SpellOwner, selector: c => c.Effect)
                .Where(x => x.IsAttacker)
                .Where(x =>
                {
                    var prevented =
                        QuickCombat.GetAmountOfDamageThatNeedsToBePreventedToSafeAttackerFromDying(
                            party: new Party(_game.Combat.Attackers),
                            attacker: x,
                            blocker: _game.Combat.Blocker?.Card
                        );

                    return 0 < prevented && prevented <= _amount;
                })
                .OrderByDescending(x => x.Score);

            return candidates.Cast<ITarget>().ToList();
        }

        private List<ITarget> PreventDamageTopSpellWillDealToForwardOrPlayer()
        {
            var playerCandidate = _p.Candidates<Player>(
                    ControlledBy.SpellOwner,
                    selector: c => c.Effect
                )
                .Where(x => _game.Stack.GetDamageTopSpellWillDealToPlayer(x) > 0);

            var forwardCandidates = _p.Candidates<Card>(
                    ControlledBy.SpellOwner,
                    selector: c => c.Effect
                )
                .Where(x =>
                {
                    var damageToForward = _game.Stack.GetDamageTopSpellWillDealToForward(x);
                    return (damageToForward >= x.Life) && (damageToForward - _amount < x.Life);
                })
                .OrderByDescending(x => x.Score);

            var candidates = new List<ITarget>();
            candidates.AddRange(playerCandidate);
            candidates.AddRange(forwardCandidates);

            return candidates;
        }
    }
}
