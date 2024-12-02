namespace CrystalArena.AI
{
  using System.Collections.Generic;

  public static class Scores
  {
    public const int BackupInHandCost = 90;

    public static readonly Dictionary<int, int> BackupsOnBattlefieldToBackupScore = new Dictionary<int, int>
      {
        {1, 600},
        {2, 500},
        {3, 450},
        {4, 425},
        {5, 400},
        {6, 375},
      };

    public static readonly Dictionary<int, int> LifeToScore = new Dictionary<int, int>
      {
        {Life.MaxLife, 0},
        {Life.MaxLife - 1, -120},
        {Life.MaxLife - 2, -250},
        {Life.MaxLife - 3, -420},
        {Life.MaxLife - 4, -690},
        {Life.MaxLife - 5, -1150},
        {Life.MaxLife - 6, -2300},
      };

    public static readonly Dictionary<int, int> ManaCostToScore = new Dictionary<int, int>
      {
        {0, 140},
        {1, 150},
        {2, 200},
        {3, 250},
        {4, 300},
        {5, 350},
        {6, 400},
        {7, 450},
      };

    public static readonly Dictionary<int, int> ManaCostToScoreEcho = new Dictionary<int, int>
      {
        {0, 140},
        {1, 190},
        {2, 240},
        {3, 290},
        {4, 340},
        {5, 390},
        {6, 440},
        {7, 490},
      };

    public static readonly Dictionary<int, int> PowerToughnessToScore = new Dictionary<int, int>
      {
        {0, 0},
        {1, 140},
        {2, 160},
        {3, 180},
        {4, 200},
        {5, 220},
        {6, 240},
        {7, 260},
        {8, 280},
        {9, 300},
        {10, 320},
      };
  }
}