using System.Linq;

namespace CrystalArena;

public class PlayerDeckLoader
{
  private Player _player;
  private Deck _deck;
  private Game _game;

  public PlayerDeckLoader(Game game, Player player, Deck deck)
  {
    _game = game;
    _player = player;
    _deck = deck;
  }
    public void LoadDeck()
    {
      var cards = _deck.Select(cardInfo =>
        {
          var card = Cards.Create(cardInfo.Name);
          card.Rarity = cardInfo.Rarity;
          card.Serial = cardInfo.Serial;
          card.Set = cardInfo.Set;

          card.Initialize(_player, _game);

          return card;
        });

      foreach (var card in cards)
      {
        AddCardToMainDeckOrLimitBreak(card);
      }
    }

    private void AddCardToMainDeckOrLimitBreak(Card card)
    {
      if (card.IsLimitBreak)
      {
        _player.AddToLimitBreakZone(card);
        return;
      }
      _player.PutOnBottomOfMainDeck(card);
    }
}