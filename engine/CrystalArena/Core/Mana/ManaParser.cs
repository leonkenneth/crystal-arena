namespace CrystalArena
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class ManaParser
    {
        public static ManaAmount ParseMana(string str)
        {
            str = str.ToLowerInvariant();
            var tokens = Regex.Split(str, "}|{").Where(x => x != String.Empty);

            var parsed = new Dictionary<ManaColor, int>();

            foreach (var token in tokens)
            {
                var colorless = ParseColorless(token);

                if (colorless.HasValue)
                {
                    parsed[ManaColor.Colorless] = colorless.Value;
                    continue;
                }

                var color = ParseColored(token);

                if (parsed.ContainsKey(color))
                {
                    parsed[color]++;
                }
                else
                {
                    parsed[color] = 1;
                }
            }

            if (parsed.Count == 1)
                return new SingleColorManaAmount(parsed.Keys.First(), parsed.Values.First());

            return new MultiColorManaAmount(parsed);
        }

        private static ManaColor ParseColored(string token)
        {
            bool isWhite = false,
                isBlue = false,
                isBlack = false,
                isRed = false,
                isGreen = false,
                isPhyrexian = false;
            bool isCrystal = false,
                isIce = false,
                isEarth = false,
                isPurple = false;

            foreach (var ch in token)
            {
                switch (Char.ToUpper(ch))
                {
                    // R,I,G,Y,P,U,W,B
                    case ('R'):
                        isRed = true;
                        break;
                    case ('I'):
                        isIce = true;
                        break;
                    case ('G'):
                        isGreen = true;
                        break;
                    case ('Y'):
                        isEarth = true;
                        break;
                    case ('P'):
                        isPurple = true;
                        break;
                    case ('U'):
                        isBlue = true;
                        break;
                    case ('W'):
                        isWhite = true;
                        break;
                    case ('B'):
                        isBlack = true;
                        break;
                    case ('Z'):
                        isCrystal = true;
                        break;
                    default:
                        throw new ArgumentException("Unknown mana symbol: " + Char.ToUpper(ch));
                }
            }

            return new ManaColor(
                isWhite: isWhite,
                isBlue: isBlue,
                isBlack: isBlack,
                isRed: isRed,
                isGreen: isGreen,
                isIce: isIce,
                isEarth: isEarth,
                isPurple: isPurple,
                isColorless: false,
                isPhyrexian: isPhyrexian,
                isCrystal: isCrystal
            );
        }

        private static int? ParseColorless(string token)
        {
            int count;
            if (Int32.TryParse(token, out count))
                return count;

            return null;
        }
    }
}
