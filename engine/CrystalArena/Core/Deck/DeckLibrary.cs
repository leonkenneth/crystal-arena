using System;
using System.Collections;
using CrystalArena.CardsMainDeck;

namespace CrystalArena
{
  using System.Collections.Generic;
  using System.Linq;
  using Media;

  public static class DeckLibrary
  {
    private static readonly ResourceFolder Folder = "decks";

    public static IEnumerable<Deck> ReadDecks()
    {
      return Folder.ReadAll().Select(r => DeckFile.Read(r.Name, r.Content));
    }

    public static void Write(Deck deck)
    {
      Folder.WriteFile(deck.Name, DeckFile.Write(deck));
    }

    public static Deck CreateTestFire()
    {
      var cardNames = new []
      {
        "23-001C", "23-002L", "23-003C", "23-004R", "23-005R", "23-006R", "23-007C", "23-008H", "23-009H", "23-010C",
        "23-011L", "23-012C", "23-013C", "23-014H", "23-015C", "23-016R", "23-017C", /*"23-018R",*/ "23-019C",
      };

      return CreateDeck(cardNames);
    }
    
    public static Deck CreateTestIce()
    {
      var cardNames = new []
      {
        "23-020C", "23-021C", "23-022R", "23-023H", "23-024R", "23-025C", "23-026C", "23-027C", "23-028L", "23-029R",
        "23-030C", "23-031C", "23-032H", "23-033C", "23-034R" //, "23-035C", "23-036C", "23-037C", "23-038C", "23-039C",
      };

      return CreateDeck(cardNames);
    }
    
    private static Deck CreateDeck(IEnumerable<string> cardNames)
    {
      var deck = new Deck();

      foreach (string cardName in cardNames)
      {
        for (var i = 0; i <= (60 / cardNames.Count()); i++)
        {
          var cardInfo = new CardInfo(cardName, serial: cardName);
          deck.AddCard(cardInfo);
        }
      }

      return deck;
    }

    public static IEnumerable<Deck> GetDecks()
    {
      return new[] { CreateTestFire(), CreateTestIce() };
    }

    public static Deck RandomDeck()
    {
      var random = (new Random()).Next();
      var decks = GetDecks().ToArray();
      return decks[random % decks.Count()];
    }
  }
  
  
}