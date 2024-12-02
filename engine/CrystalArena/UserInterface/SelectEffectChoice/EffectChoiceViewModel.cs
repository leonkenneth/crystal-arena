using System;
using System.Collections.Generic;

namespace CrystalArena.UserInterface.SelectEffectChoice
{
  using Effects;

  public class EffectChoiceViewModel
  {
    public EffectChoiceViewModel(IEffectChoice choice)
    {      
      Choice = choice;
      Selected = choice.Options[0];      
    }
    
    public object ToJson()
    {
      return new
      {
        Options = OptionsToJson(Choice.Options),
        Selected
      };
    }
    
    private Dictionary<string, int> OptionsToJson(object[] options)
    {
      var dict = new Dictionary<string, int>();
      
      foreach (object option in options)
      {
        var effectOption = option as EffectOption?;
        if (effectOption != null)
        {
          dict.Add(effectOption.ToString(), (int)effectOption);
        }
        else
        {
          throw new Exception("Unknown option type");
        }
      }

      return dict;
    }

    public IEffectChoice Choice { get; set; }
    public object Selected { get; set; }
  }
}