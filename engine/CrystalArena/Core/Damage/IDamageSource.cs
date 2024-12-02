using System.Collections.Generic;

namespace CrystalArena;

public interface IDamageSource
{
    bool HasDeathtouch { get; }
    Player Controller { get;  }
    bool HasColor(CardColor color);
    ITargetType Is();
    bool IsCategory(string category);
    IEnumerable<Card> Cards { get; }
}