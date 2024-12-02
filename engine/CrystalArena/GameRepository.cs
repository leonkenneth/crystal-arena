using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CrystalArena.UserInterface;

namespace CrystalArena;

public static class GameRepository
{
    private static Dictionary<int, IoC> _containers = new Dictionary<int, IoC>();
    
    private static IoC GetContainer(int gameId)
    {
        if (!_containers.TryGetValue(gameId, out var container))
        {
            container = new IoC(IoC.Configuration.Ui);
            _containers.Add(gameId, container);
        }
        return container;
    }

    public static Ui ResolveUi(int id)
    {
        var ui = GetContainer(id).Resolve<Ui>();
        ui.GameId = id;
        return ui;
    }

    public static IEnumerable<int> GameIds()
    {
        return _containers.Keys;
    }

    public static int NextId()
    {
        if (!GameIds().Any()) return 1;
        return GameIds().Max() + 1;
    }
}