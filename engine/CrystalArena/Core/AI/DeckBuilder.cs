namespace CrystalArena.AI
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using CrystalArena.Infrastructure;
  using Newtonsoft.Json;

  public static class DeckBuilder
  {
    private const int MinForwardCount = 16;
    private const int MaxForwardCount = 20;

    private const int DeckCardCount = 40;
    private const int SpellCount = 23;
    private const int BackupCount = 17;
    
    private const double MinNonBasicBackupRating = 2;
    private const double PlayableThreshold = 1.2;

    private static readonly int[] ForwardsManaCurve = new[]
    {
      5, // up to 2
      6, // up to 3
      3, // up to 4
      2, // up to 5
      2  // 6 or more
    };

    public static Deck BuildDeck(IEnumerable<CardInfo> library, CardRatings cardRatings)
    {
      return BuildDeck(library, cardRatings, DeckEvaluator.GetBestDeck);
    }

    public static Deck BuildDeck(
      IEnumerable<CardInfo> library,
      CardRatings cardRatings,
      Func<List<Deck>, Deck> deckEvaluator)
    {
      var cards = library.Select(x => Cards.All[x.Name])
        .ToList();

      var candidates = BuildDecks(cards, cardRatings)
        .Select(x => CreateDeckFromCardList(x, library))
        .ToList();

      return deckEvaluator(candidates);
    }

    private static Deck CreateDeckFromCardList(List<Card> cards, IEnumerable<CardInfo> library)
    {
      var deck = new Deck();

      foreach (var card in cards)
      {
        var info = library.FirstOrDefault(x => x.Name.Equals(card.Name)) ??
          new CardInfo(card.Name, Rarity.C); // basic backups are not included in library

        deck.AddCard(info);
      }

      return deck;
    }

    private static List<List<Card>> BuildDecks(IEnumerable<Card> cards, CardRatings cardRatings)
    {
      var decks = new List<DeckCandidate>();

      for (var i = CardColor.Light; i <= CardColor.Wind; i++)
      {
        var cardWithColorI = cards
          .Where(x => !x.IsMultiColored)
          .Where(x => x.HasColor(i) || x.HasColor(CardColor.Colorless) || x.Is().Backup)
          .ToList();

        var colorCount = cardWithColorI.Count(x => x.HasColor(i));

        if (colorCount > 0)
        {
          var monoColoredDeck = BuildDeck(cardWithColorI, cardRatings);

          if (!decks.Any(x => x.Hash == monoColoredDeck.Hash))
          {
            decks.Add(monoColoredDeck);
          }            
        }        

        for (var j = i + 1; j <= CardColor.Wind; j++)
        {
          var dual = cards
            .Where(x => x.HasColor(i) || x.HasColor(j) || x.HasColor(CardColor.Colorless) || x.Is().Backup)
            .ToList();

          var firstcolorCount = dual.Count(x => Cards.All[x.Name].HasColor(i));
          var secondcolorCount = dual.Count(x => Cards.All[x.Name].HasColor(j));

          if (firstcolorCount > 0 && secondcolorCount > 0)
          {
            var dualDeck = BuildDeck(dual, cardRatings);

            if (!decks.Any(x => x.Hash == dualDeck.Hash))
            {
              decks.Add(dualDeck);
            }            
          }
        }
      }      

      var playable = decks        
        .Where(x => x.SpellCount == SpellCount)
        .OrderByDescending(x => x.Rating)
        .Select(x => x.Cards)
        .ToList();      

      if (playable.Count > 0)
      {
        return playable;
      }

      var nonPlayable = decks
        .OrderByDescending(x => x.SpellCount)
        .Take(1)
        .Select(x => x.Cards).ToList();

      return nonPlayable;
    }

    private static List<Card> AddBackups(List<Card> deck, List<Card> library, CardRatings cardRatings, 
      int backupCount)
    {
      var distribution = GetBackupsDistribution(deck, backupCount);

      AddNonBasicBackups(distribution, library, deck, cardRatings);

      AddCards("Plains", distribution[0], deck);
      AddCards("Island", distribution[1], deck);
      AddCards("Swamp", distribution[2], deck);
      AddCards("Mountain", distribution[3], deck);
      AddCards("Forest", distribution[4], deck);

      return deck;
    }

    private static void AddNonBasicBackups(List<int> distribution, List<Card> library, List<Card> deck,
      CardRatings cardRatings)
    {
      var nonBasicBackups = library.Where(x => x.Is().Backup).ToList();
      var deckColors = distribution.Select((x, i) => x > 0 ? i : -1).Where(x => x >= 0).ToList();
      var maxCountColor = distribution.IndexOf(distribution.Max());
      
      var backupsToReplaceBasic = new List<Card>();

      foreach (var backup in nonBasicBackups)
      {
        var rating = cardRatings.GetRating(backup.Name);
        if (rating < MinNonBasicBackupRating)
          continue;

        var backupColors = backup.ProducableManaColors;

        if (backupColors.Count == 1)
        {
          if (deckColors.Contains(backupColors[0]))
          {
            distribution[backupColors[0]]--;
            backupsToReplaceBasic.Add(backup);
          }         
        }
        else if (backupColors.Count > 1)
        {
          var matchCount = backupColors.Count(deckColors.Contains);

          if (matchCount >= 2)
          {
            distribution[maxCountColor]--;
            backupsToReplaceBasic.Add(backup);
          }
        }
      }      

      deck.AddRange(backupsToReplaceBasic);
    }

    private static List<int> GetBackupsDistribution(IEnumerable<Card> noBackupsDeck, int backupCount)
    {
      var manaCounts = new[] { 0, 0, 0, 0, 0 };

      foreach (var card in noBackupsDeck.Select(x => Cards.All[x.Name]))
      {
        foreach (var colorCount in card.ManaCost)
        {
          if (colorCount.Color.IsColorless)
            continue;

          foreach (var index in colorCount.Color.Indices)
          {
            manaCounts[index]++;
          }
        }
      }

      var totalManaCount = manaCounts.Sum();
      var distribution = manaCounts.Select(x => backupCount * x / totalManaCount).ToList();
      var roundedCount = distribution.Sum();
      var roundingError = backupCount - roundedCount;

      var i = 0;
      while (roundingError > 0)
      {
        var minBackupCount = distribution.Where(x => x > 0).Min();

        for (var j = 0; j < distribution.Count; j++)
        {
          if (distribution[j] == minBackupCount)
          {
            distribution[j]++;
            roundingError--;
            break;
          }
        }

        i++;
      }

      Asrt.True(distribution.Sum() == backupCount, "Backup distribution sum should equal backup count.");
      return distribution;
    }

    private static void AddCards(string name, int count, List<Card> deck)
    {
      Asrt.True(count >= 0, "Count must be >= 0");
      for (var i = 0; i < count; i++)
      {
        deck.Add(Cards.All[name]);
      }
    }

    private class DeckCandidate
    {
      public DeckCandidate(List<Card> cards, int spellCount, double rating)
      {
        Cards = cards.OrderBy(x => x.Name).ToList();
        SpellCount = spellCount;
        Rating = rating;

        Hash = Sha1.Calculate(string.Join(";", Cards.Select(x => x.Name)));
      }
      
      public readonly List<Card> Cards;
      public readonly int SpellCount;
      public readonly double Rating;
      public readonly string Hash;

      public override string ToString()
      {
        return $"{SpellCount}, {Rating}, {Hash}";
      }
    }

    private static DeckCandidate BuildDeck(List<Card> cards, CardRatings cardRatings)
    {
      var deck = new List<Card>();

      var forwards = cards
        .Where(x => x.Is().Forward)
        .Select(x =>
        {
          var convertedCost = x.ConvertedCost;

          if (convertedCost <= 2)
            convertedCost = 2;

          if (convertedCost >= 6)
            convertedCost = 6;

          return new
          {
            Card = x,
            ConvertedCost = convertedCost,
            Rating = cardRatings.GetRating(x.Name)
          };
        })
        .GroupBy(x => x.ConvertedCost)
        .OrderBy(x => x.Key)
        .Select(x =>
        {
          var cost = x.Key;
          var maxForwardCount = ForwardsManaCurve[cost - 2];

          var ordered = x.OrderByDescending(y => y.Rating)            
            .ToList();

          var selected = ordered            
            .Take(maxForwardCount)
            .Where(y => y.Rating > PlayableThreshold)
            .Select(y => new { y.Card, y.Rating })
            .ToList();

          var skipped = ordered
            .Where(y => !selected.Any(z => z.Card == y.Card))
            .Select(y => new { y.Card, y.Rating })
            .ToList();

          return new
          {
            Selected = selected,
            Skipped = skipped
          };
        })
        .ToList();

      var selectedForwards = forwards
        .SelectMany(x => x.Selected)
        .ToList();

      var remaining = forwards 
        .SelectMany(x => x.Skipped)
        .Concat(cards
        .Where(x => !x.Is().Forward && !x.Is().Backup)
        .Select(x => new { Card = x, Rating = cardRatings.GetRating(x.Name) }))
        .OrderByDescending(x => x.Rating)
        .ToList();


      var selectedRemaining = remaining.
        Take(SpellCount - selectedForwards.Count)        
        .ToList();

      var avarageRating = selectedForwards.Concat(selectedRemaining)
        .Average(x => x.Rating);

      deck.AddRange(selectedForwards.Select(x => x.Card));
      deck.AddRange(selectedRemaining.Select(x => x.Card));

      var spellCount = deck.Count;

      var backupCount = DeckCardCount - spellCount;
      deck = AddBackups(deck, cards, cardRatings, backupCount);

      return new DeckCandidate(deck, spellCount, avarageRating);                    
    }


    private class BackupCountRule
    {
      public double Cost;
      public int Count;
    }
  }
}