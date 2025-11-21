namespace CrystalArena
{
    public static class Mana
    {
        public static readonly ManaAmount Zero = new ZeroManaAmount();
        public static readonly ManaAmount Any = new SingleColorManaAmount(ManaColor.Any, 1);
        public static readonly ManaAmount Light = new SingleColorManaAmount(ManaColor.Light, 1);
        public static readonly ManaAmount Water = new SingleColorManaAmount(ManaColor.Water, 1);
        public static readonly ManaAmount Dark = new SingleColorManaAmount(ManaColor.Dark, 1);
        public static readonly ManaAmount Fire = new SingleColorManaAmount(ManaColor.Fire, 1);
        public static readonly ManaAmount Wind = new SingleColorManaAmount(ManaColor.Wind, 1);

        public static ManaAmount Colored(
            bool isWhite = false,
            bool isBlue = false,
            bool isBlack = false,
            bool isRed = false,
            bool isGreen = false,
            int count = 1
        )
        {
            return new SingleColorManaAmount(
                new ManaColor(isWhite, isBlue, isBlack, isRed, isGreen),
                count
            );
        }

        public static ManaAmount Colored(ManaColor color, int count)
        {
            if (count == 0)
                return Zero;

            return new SingleColorManaAmount(color, count);
        }

        public static ManaAmount Parse(this string str)
        {
            return ManaParser.ParseMana(str);
        }

        public static ManaAmount Colorless(this int value)
        {
            return new SingleColorManaAmount(ManaColor.Colorless, value);
        }

        public static ManaAmount Repeat(this ManaAmount amount, int count)
        {
            var result = amount;

            for (var i = 1; i < count; i++)
            {
                result = result.Add(amount);
            }

            return result;
        }

        public static ManaAmount GetBasicBackupMana(string name)
        {
            switch (name)
            {
                case ("plains"):
                    return Light;
                case ("island"):
                    return Water;
                case ("swamp"):
                    return Dark;
                case ("mountain"):
                    return Fire;
                case ("forest"):
                    return Wind;
            }

            return Colorless(1);
        }
    }
}
