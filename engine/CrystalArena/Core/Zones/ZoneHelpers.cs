namespace CrystalArena
{
  public static class ZoneHelpers
  {
    public static bool IsHiddenZone(this Zone zone)
    {
      return zone == Zone.MainDeck || zone == Zone.Hand;
    }

    public static bool IsPublicZone(this Zone zone)
    {
      return !zone.IsHiddenZone();
    }
  }
}