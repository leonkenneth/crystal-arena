namespace CrystalArena.Utils
{
  using Castle.MicroKernel.Registration;

  public class IoC : CrystalArena.IoC
  {
    public IoC() : base(Configuration.Test)
    {      
    }

    protected override void Install(Castle.Windsor.IWindsorContainer container)
    {
      container.Register(Classes.FromThisAssembly()
        .BasedOn(typeof (Task))
        .WithService.Self()
        .LifestyleTransient());
    }
  }
}