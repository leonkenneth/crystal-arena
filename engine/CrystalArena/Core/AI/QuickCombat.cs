using System.Collections;

namespace CrystalArena.AI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CrystalArena.Infrastructure;

    public class QuickCombat
    {
        public static bool CanAttackerBeDealtLeathalDamage(Card attacker, Card? blocker)
        {
            return CanAttackerBeDealtLeathalDamageWithBoost(attacker, blocker, 0);
        }

        public static bool CanAttackerBeDealtLeathalDamageWithBoost(
            Card attacker,
            Card? blocker,
            int powerIncrease
        )
        {
            if (blocker == null)
                return false;
            return GetAmountOfDamageForward1WillDealToForward2(blocker, attacker, powerIncrease)
                >= attacker.Life;
        }

        public static int GetAmountOfDamageForward1WillDealToForward2(
            Card forward1,
            Card forward2,
            int? forward1PowerIncrease = null
        )
        {
            var powerIncrease =
                forward1PowerIncrease ?? forward1.GetCombatAbilities().PowerIncrease;
            var amountDealt = forward1.CalculateCombatDamageAmount(
                toPlayer: false,
                powerIncrease: powerIncrease
            );
            var preventedReceived = forward2.CalculatePreventedDamageAmount(
                amountDealt,
                forward1,
                isCombat: true
            );

            return amountDealt - preventedReceived;
        }

        public static PartyDamage GetAmountOfDamagePartyWillDealToForward2(
            Party party,
            Card forward2
        )
        {
            var damage = new PartyDamage();
            foreach (var attacker in party.Attackers)
            {
                damage.Add(
                    attacker,
                    GetAmountOfDamageForward1WillDealToForward2(attacker, forward2)
                );
            }

            return damage;
        }

        public static int GetAmountOfDamageThatNeedsToBePreventedToSafeBlockerFromDying(
            Card blocker,
            Party party
        )
        {
            var evaluation = new BlockerEvaluation(
                new CombatEvaluationParameters(party.Attackers, blocker)
            );
            var results = evaluation.Evaluate();

            if (results.ReceivesLeathalDamage)
                return results.DamageDealt;

            return 0;
        }

        public static int GetAmountOfDamageThatNeedsToBePreventedToSafeAttackerFromDying(
            Party party,
            Card attacker,
            Card blocker
        )
        {
            var attackerLife = attacker.Life;
            var blockerLife = blocker.Life;
            var blockerDamage = GetAmountOfDamageForward1WillDealToForward2(blocker, attacker);
            var attackerDamage = GetAmountOfDamagePartyWillDealToForward2(party, blocker);

            if (
                party.HasFirstStrike
                && !blocker.Has().FirstStrike
                && attackerDamage.Total > blockerLife
            )
                return 0;

            return Math.Max(0, blockerDamage - attackerLife);
        }

        public static int CalculateTrampleDamage(CombatEvaluationParameters p)
        {
            return 0;
        }

        public static PartyDamage CalculateDefendingPlayerLifeloss(CombatEvaluationParameters p)
        {
            var damage = new PartyDamage(AggregateDamage.AggregateType.Max);

            if (p.Blocker == null)
            {
                p.Attackers.ForEach(a =>
                {
                    var dealt = a.Card.CalculateCombatDamageAmount(
                        toPlayer: true,
                        singleDamageStep: false,
                        powerIncrease: a.PowerIncrease
                    );
                    var prevented =
                        a.Card.Controller.Opponent.CalculatePreventedReceivedDamageAmount(
                            dealt,
                            a.Card,
                            isCombat: true
                        );
                    damage.Add(a.Card, dealt - prevented);
                });
            }

            return damage;
        }

        public static PartyDamage CalculateDefendingPlayerLifeloss(Party party, Card? blocker)
        {
            var p = new CombatEvaluationParameters(party.Attackers, blocker);
            return CalculateDefendingPlayerLifeloss(p);
        }

        public static int CalculateGainAttackerWouldGetIfPowerAndThoughnessWouldIncrease(
            Card attacker,
            Card? blocker,
            int powerIncrease,
            int toughnessIncrease
        )
        {
            if (blocker == null)
                return 0;

            if (toughnessIncrease < 1 && !attacker.Has().FirstStrike)
                return 0;

            var canBeDealtLeathalDamageWithoutBoost = CanAttackerBeDealtLeathalDamage(
                attacker,
                blocker
            );

            if (canBeDealtLeathalDamageWithoutBoost == false)
                return 0;

            var canBeDealtLeathalDamageWithBoost = CanAttackerBeDealtLeathalDamageWithBoost(
                attacker,
                blocker,
                powerIncrease
            );

            return canBeDealtLeathalDamageWithBoost ? 0 : attacker.Score;
        }

        public static int CalculateGainBlockerWouldGetIfPowerAndThougnessWouldIncrease(
            Party party,
            Card blocker,
            int powerIncrease,
            int toughnessIncrease
        )
        {
            var p = new CombatEvaluationParameters(party.Attackers, blocker);

            var canBeDealtLeathalDamageWithoutBoost = CanBlockerBeDealtLeathalCombatDamage(p);

            if (canBeDealtLeathalDamageWithoutBoost == false)
                return 0;

            p.Blocker.PowerIncrease += powerIncrease;

            var canBeDealtLeathalDamageWithBoost = CanBlockerBeDealtLeathalCombatDamage(p);
            return canBeDealtLeathalDamageWithBoost == false ? blocker.Score : 1;
        }

        public static bool CanBlockerBeDealtLeathalCombatDamage(Card attacker, Card? blocker)
        {
            if (blocker == null)
                return false;
            return CanBlockerBeDealtLeathalCombatDamage(
                new CombatEvaluationParameters(attacker, blocker)
            );
        }

        public static bool CanBlockerBeDealtLeathalCombatDamage(Party party, Card? blocker)
        {
            if (blocker == null)
                return false;
            return CanBlockerBeDealtLeathalCombatDamage(
                new CombatEvaluationParameters(party.Attackers, blocker)
            );
        }

        public static bool CanBlockerBeDealtLeathalCombatDamage(CombatEvaluationParameters p)
        {
            var blockerEvaluation = new BlockerEvaluation(p);
            var results = blockerEvaluation.Evaluate();

            return results.ReceivesLeathalDamage;
        }

        public static int CalculateDefendingPlayerLifeloss(Card argCard, Card? blocker)
        {
            return CalculateDefendingPlayerLifeloss(new Party(argCard), blocker).Total;
        }
    }
}
