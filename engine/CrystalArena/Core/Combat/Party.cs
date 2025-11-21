using System.Collections.Generic;
using System.Linq;
using CrystalArena.AI;

namespace CrystalArena;

public class Party : IDamageSource
{
    private readonly IEnumerable<Card> _attackers;
    public IEnumerable<Card> Attackers => _attackers;
    public bool HasFirstStrike => _attackers.All(a => a.HasFirstStrike);
    public bool HasIndestructible => _attackers.All(a => a.Has().Indestructible);
    public bool HasDeathtouch => _attackers.Any(a => a.Has().Deathtouch);
    public Player Controller => _attackers.Select(a => a.Controller).Distinct().Single();

    public bool HasColor(CardColor color)
    {
        return _attackers.Any(a => a.HasColor(color));
    }

    public ITargetType Is()
    {
        return new PartyTargetType();
    }

    public bool IsCategory(string category)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerable<Card> Cards => _attackers;

    public bool CanBeDestroyed => _attackers.Any(a => a.CanBeDestroyed);
    public Card SingleCard => _attackers.Single();

    public Party(IEnumerable<Card> attackers)
    {
        _attackers = attackers;
    }

    public Party(List<CombatEvaluationParameters.CardWithPowerIncrease> attackers)
        : this(attackers.Select(x => x.Card)) { }

    public Party(IEnumerable<Attacker> attackers)
        : this(attackers.Select(a => a.Card)) { }

    public Party(Card card)
        : this(new[] { card }) { }

    public bool CanBeBlockedBy(Card blocker)
    {
        return Attackers.Any(a => a.CanBeBlockedBy(blocker));
    }

    public bool Contains(Card card)
    {
        return Attackers.Contains(card);
    }

    private class PartyTargetType : ITargetType
    {
        public bool Artifact
        {
            get { return false; }
        }

        public bool Attachment
        {
            get { return false; }
        }
        public bool BasicBackup
        {
            get { return false; }
        }
        public bool Forward
        {
            get { return false; }
        }
        public bool Monster
        {
            get { return false; }
        }
        public bool Equipment
        {
            get { return false; }
        }
        public bool Summon
        {
            get { return false; }
        }
        public bool Backup
        {
            get { return false; }
        }
        public bool Legendary
        {
            get { return false; }
        }
        public bool Sorcery
        {
            get { return false; }
        }
        public bool Token
        {
            get { return false; }
        }
        public bool Aura
        {
            get { return false; }
        }
        public bool NonBasicBackup
        {
            get { return false; }
        }
        public bool Planeswalker
        {
            get { return false; }
        }
        public bool Party => true;
    }
}
