using System.Collections.Generic;
using System.Linq;

namespace CrystalArena;

public class PartyDamage
{
    Dictionary<Card, int> _contributions = new Dictionary<Card, int>();
    private readonly AggregateDamage.AggregateType _aggregateType;
    public Dictionary<Card, int> Contributions => _contributions;
    public int Total
    {
        get
        {
            if (_aggregateType == AggregateDamage.AggregateType.Sum)
                return _contributions.Select(x => x.Value).Sum();
            return _contributions.Select(x => x.Value).Max();
        }
    }

    public PartyDamage(AggregateDamage.AggregateType aggregateType = AggregateDamage.AggregateType.Sum)
    {
        _aggregateType = aggregateType;
    }
    
    public void Add(Card card, int damage)
    {
        if (_contributions.ContainsKey(card))
            _contributions[card] += damage;
        else
            _contributions.Add(card, damage);
    }
}