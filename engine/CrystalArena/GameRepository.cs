using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CrystalArena.UserInterface;

namespace CrystalArena;

public static class GameRepository
{
    private static Dictionary<string, IoC> _containers = new Dictionary<string, IoC>();
    
    private static IoC GetContainer(string gameId)
    {
        if (!_containers.TryGetValue(gameId, out var container))
        {
            container = new IoC(IoC.Configuration.Ui);
            _containers.Add(gameId, container);
        }
        return container;
    }

    public static Ui ResolveUi(string id)
    {
        var ui = GetContainer(id).Resolve<Ui>();
        ui.GameId = id;
        return ui;
    }

    public static string NextId()
    {
        return System.Guid.NewGuid().ToString();
    }
}