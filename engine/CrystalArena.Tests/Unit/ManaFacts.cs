namespace CrystalArena.Tests.Unit
{
  using Infrastructure;
  using Xunit;

  public class ManaFacts : PredefinedScenario
  {
    [Fact]
    public void Consume1()
    {
      Add("{R}{R}{R}{R}{G}{G}{G}");
      Consume("{4}{R}{R}");

      AssertHas(0);
    }

    [Fact]
    public void Consume2()
    {
      Add("{B}{GRB}{R}");
      Consume("{G}{B}{R}");

      AssertHas(0);
    }

    [Fact]
    public void Arena_TooMuch()
    {
      Add("{U}{B}{GRB}{R}");
      AssertNotAvailable("{U}{G}");
    }
    
    [Fact]
    public void Arena_Cristal()
    {
      Add("{Z}{Z}{Z}{Z}");
      AssertAvailable("{Z}{Z}");
    }

    [Fact]
    public void Arena_JustEnough()
    {
      Add("{G}{G}");
      AssertAvailable("{G}");
    }

    [Fact]
    public void DoesNotHave1()
    {
      Add("{R}{R}{R}{G}{G}{G}");

      AssertNotAvailable("{2}{R}{R}{R}{R}");
    }

    [Fact]
    public void DoesNotHave2()
    {
      Add("{2}{WG}{R}");
      AssertNotAvailable("{2}{W}{G}");
    }

    [Fact]
    public void Has1()
    {
      Add("{R}{R}{R}{G}{G}{G}");
      AssertAvailable("{4}{R}{R}");
    }

    [Fact]
    public void Has2()
    {
      Add("{3}{RG}{R}{G}");
      AssertAvailable("{4}{R}{R}");
    }

    [Fact]
    public void Has3()
    {
      Add("{RG}{RG}");
      AssertAvailable("{G}{G}");
    }

    [Fact]
    public void Has4()
    {
      Add("{G}{RG}{RG}{G}");
      AssertAvailable("{2}{G}{G}");
    }

    [Fact]
    public void Has5()
    {
      Add("{R}");
      AssertAvailable("{1}");
    }

    [Fact]
    public void MulticolorSourceBug1()
    {
      Add("{GB}{GW}");
      AssertAvailable("{G}{B}");
    }

    [Fact]
    public void DoNotPayLife()
    {
      P1.Life = 20;
      
      Add("{B}{B}{R}");
      Consume("{PB}{PB}{1}");

      Equal(20, P1.Life);
    }

    [Fact (Skip="Old test")]
    public void Pay2Life()
    {
      P1.Life = 20;

      Add("{B}{R}{B}");
      Consume("{B}{PB}{PB}{1}");

      Equal(18, P1.Life);
    }

    public ManaFacts()
    {
      _cache = new ManaCache(P1);
    }

    private readonly ManaCache _cache;

    private void Add(string manaAmount)
    {
      _cache.AddManaToPool(manaAmount.Parse(), ManaUsage.Any);
    }

    private void Consume(string manaAmount)
    {
      _cache.Consume(manaAmount.Parse(), ManaUsage.Any, ConvokeAndDelveOptions.NoConvokeAndDelve);
    }

    private void AssertHas(int count)
    {
      var amount = _cache.GetAvailableMana(ManaUsage.Any, ConvokeAndDelveOptions.NoConvokeAndDelve).Count;
      Assert.Equal(count, amount);
    }

    private void AssertAvailable(string amount)
    {
      var has = _cache.Has(amount.Parse(), ManaUsage.Any, ConvokeAndDelveOptions.NoConvokeAndDelve);
      Assert.True(has);
    }

    private void AssertNotAvailable(string amount)
    {
      var has = _cache.Has(amount.Parse(), ManaUsage.Any, ConvokeAndDelveOptions.NoConvokeAndDelve);
      Assert.False(has);
    }
  }
}