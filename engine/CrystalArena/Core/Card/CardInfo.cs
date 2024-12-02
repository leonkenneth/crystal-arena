namespace CrystalArena
{
  using System;

  [Serializable]
  public class CardInfo : IEquatable<CardInfo>
  {
    public CardInfo(string name, Rarity? rarity = null, string set = null, string? serial = null)
    {
      Name = name;
      Rarity = rarity;
      Set = set;
      Serial = serial;
    }

    public string Name { get; private set; }
    public Rarity? Rarity { get; private set; }
    public string Set { get; private set; }
    public string? Serial { get; set; }

    public bool Equals(CardInfo other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return Equals(other.Name, Name);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != typeof (CardInfo)) return false;
      return Equals((CardInfo) obj);
    }

    public override int GetHashCode()
    {
      return (Name != null ? Name.GetHashCode() : 0);
    }

    public override string ToString()
    {
      return Name;
    }
  }
}