namespace CrystalArena
{
    public enum CardColor
    {
        Light = 0,
        Water = 1,
        Dark = 2,
        Fire = 3,
        Wind = 4,
        Colorless = 5,
        None = 6,
        Earth = 7,
        Ice = 8,
        Lightning = 9,
    }

    public static class CardColors
    {
        public static readonly CardColor[] All = new[]
        {
            CardColor.Light,
            CardColor.Water,
            CardColor.Dark,
            CardColor.Fire,
            CardColor.Wind,
        };
    }
}
