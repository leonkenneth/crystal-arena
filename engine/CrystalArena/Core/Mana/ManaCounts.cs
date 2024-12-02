namespace CrystalArena
{
  public class ManaCounts
  {
    public ManaCounts(int light, int water, int dark, int fire, int wind, int multi, int colorless, int ice, int earth, int lightning, int crystal)
    {
      Light = light;
      Water = water;
      Dark = dark;
      Fire = fire;
      Wind = wind;
      Multi = multi;
      Ice = ice;
      Earth = earth;
      Lightning = lightning;
      Colorless = colorless;
      Crystal = crystal;
    }

    public int Ice { get; private set; }
    
    public int Earth { get; private set; }
    
    public int Lightning { get; private set; }

    public int Light { get; private set; }
    public int Water { get; private set; }
    public int Dark { get; private set; }
    public int Fire { get; private set; }
    public int Wind { get; private set; }
    public int Multi { get; private set; }
    public int Colorless { get; private set; }
    public int Crystal { get; private set; }
  }
}