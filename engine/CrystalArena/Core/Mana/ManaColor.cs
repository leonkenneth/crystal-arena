namespace CrystalArena
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ManaColor : IEquatable<ManaColor>
    {
        public static readonly ManaColor Light = new ManaColor(isWhite: true);

        public static readonly ManaColor Any = new ManaColor(
            isWhite: true,
            isBlue: true,
            isBlack: true,
            isRed: true,
            isGreen: true
        );

        public static readonly ManaColor Water = new ManaColor(isBlue: true);
        public static readonly ManaColor Dark = new ManaColor(isBlack: true);
        public static readonly ManaColor Fire = new ManaColor(isRed: true);
        public static readonly ManaColor Wind = new ManaColor(isGreen: true);

        public static readonly ManaColor Ice = new ManaColor(isIce: true);

        public static readonly ManaColor Earth = new ManaColor(isEarth: true);

        public static readonly ManaColor Lightning = new ManaColor(isPurple: true);
        public static readonly ManaColor Colorless = new ManaColor(isColorless: true);

        private readonly List<int> _colorIndices = new List<int>(9);
        private readonly bool[] _isColor;

        public ManaColor(
            bool isWhite = false,
            bool isBlue = false,
            bool isBlack = false,
            bool isRed = false,
            bool isGreen = false,
            bool isColorless = false,
            bool isIce = false,
            bool isEarth = false,
            bool isPurple = false,
            bool isPhyrexian = false,
            bool isCrystal = false
        )
        {
            IsPhyrexian = isPhyrexian;
            IsCrystal = isCrystal;

            _isColor = new[]
            {
                isWhite,
                isBlue,
                isBlack,
                isRed,
                isGreen,
                isIce,
                isEarth,
                isPurple,
                isColorless,
            };

            for (var i = 0; i < _isColor.Length; i++)
            {
                if (_isColor[i])
                {
                    _colorIndices.Add(i);
                }
            }
        }

        public readonly bool IsPhyrexian;
        public readonly bool IsCrystal;

        public List<int> Indices
        {
            get { return _colorIndices; }
        }

        public bool IsWhite
        {
            get { return _isColor[0]; }
        }
        public bool IsBlue
        {
            get { return _isColor[1]; }
        }
        public bool IsBlack
        {
            get { return _isColor[2]; }
        }
        public bool IsRed
        {
            get { return _isColor[3]; }
        }
        public bool IsGreen
        {
            get { return _isColor[4]; }
        }
        public bool IsIce
        {
            get { return _isColor[5]; }
        }
        public bool IsEarth
        {
            get { return _isColor[6]; }
        }
        public bool IsPurple
        {
            get { return _isColor[7]; }
        }

        public bool IsColorless
        {
            get { return _isColor[8]; }
        }
        public bool IsMulti
        {
            get { return _colorIndices.Count > 1; }
        }

        public bool Equals(ManaColor other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return _colorIndices.SequenceEqual(other._colorIndices)
                && this.IsPhyrexian == other.IsPhyrexian
                && IsCrystal == other.IsCrystal;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(ManaColor))
                return false;
            return Equals((ManaColor)obj);
        }

        public override int GetHashCode()
        {
            var hash = 0;

            foreach (var colorIndex in _colorIndices)
            {
                hash = hash ^ colorIndex * 397;
            }

            return hash;
        }

        public static bool operator ==(ManaColor left, ManaColor right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ManaColor left, ManaColor right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            if (IsWhite)
                return "W";
            if (IsBlue)
                return "U";
            if (IsBlack)
                return "B";
            if (IsRed)
                return "R";
            if (IsGreen)
                return "G";
            if (IsColorless)
                return "C";
            if (IsIce)
                return "I";
            if (IsEarth)
                return "Y";
            if (IsPurple)
                return "P";
            if (IsPhyrexian)
                return "P";
            if (IsMulti)
                return "M";
            if (IsCrystal)
                return "Z";
            throw new Exception("Invalid ManaColor.");
        }

        public static ManaColor FromCardColors(CardColor[] colors)
        {
            return new ManaColor(
                colors.Contains(CardColor.Light),
                colors.Contains(CardColor.Water),
                colors.Contains(CardColor.Dark),
                colors.Contains(CardColor.Fire),
                colors.Contains(CardColor.Wind),
                true
            );
        }

        public static ManaColor FromCardColor(CardColor color)
        {
            return new ManaColor(
                isWhite: color == CardColor.Light,
                isBlue: color == CardColor.Water,
                isBlack: color == CardColor.Dark,
                isRed: color == CardColor.Fire,
                isGreen: color == CardColor.Wind,
                isColorless: color == CardColor.Colorless,
                isPurple: color == CardColor.Lightning,
                isEarth: color == CardColor.Earth,
                isIce: color == CardColor.Ice
            );
        }
    }
}
