namespace CrystalArena.UserInterface.ManaPool
{
  using System;
  using System.Threading;

  public class ViewModel : ViewModelBase, IDisposable
  {
    private Timer _timer;
    private readonly Player _owner;

    public ViewModel(Player owner)
    {
      _owner = owner;
    }
    
    public override void Initialize()
    {
      _timer = new Timer(delegate { Update(); }, null,
        TimeSpan.FromMilliseconds(20),
        TimeSpan.FromMilliseconds(20));
    }

    public virtual int LightCount { get; protected set; }
    public virtual int WaterCount { get; protected set; }
    public virtual int DarkCount { get; protected set; }
    public virtual int FireCount { get; protected set; }
    public virtual int WindCount { get; protected set; }
    public virtual int IceCount { get; protected set; }
    public virtual int EarthCount { get; protected set; }
    public virtual int LightningCount { get; protected set; }
    public virtual int ColorlessCount { get; protected set; }
    public virtual int MultiCount { get; protected set; }
  
    public virtual int CrystalCount { get; protected set; }

    public override object ToJson()
    {
      return new
      {
        Light = LightCount,
        Water = WaterCount,
        Dark = DarkCount,
        Fire = FireCount,
        Wind = WindCount,
        Ice = IceCount,
        Earth = EarthCount,
        Lightning = LightningCount,
        Crystal = CrystalCount,
        Colorless = ColorlessCount,
        Multi = MultiCount
      };
    }

    public void Dispose()
    {
      _timer.Dispose();
    }

    private void Update()
    {
      var pool = _owner.ManaCache.ManaPool;

      Update(() => LightCount != pool.Light, () => LightCount = pool.Light);
      Update(() => WaterCount != pool.Water, () => WaterCount = pool.Water);
      Update(() => DarkCount != pool.Dark, () => DarkCount = pool.Dark);
      Update(() => FireCount != pool.Fire, () => FireCount = pool.Fire);
      Update(() => WindCount != pool.Wind, () => WindCount = pool.Wind);
      Update(() => IceCount != pool.Ice, () => IceCount = pool.Ice);
      Update(() => EarthCount != pool.Earth, () => EarthCount = pool.Earth);
      Update(() => LightningCount != pool.Lightning, () => LightningCount = pool.Lightning);
      Update(() => CrystalCount != pool.Crystal, () => CrystalCount = pool.Crystal);
      Update(() => ColorlessCount != pool.Colorless, () => ColorlessCount = pool.Colorless);
      Update(() => MultiCount != pool.Multi, () => MultiCount = pool.Multi);
    }

    private static void Update(Func<bool> condition, Action update)
    {
      if (condition()) update();
    }
    
    public interface IFactory
    {
      ViewModel Create(Player owner);
    }
  }
}