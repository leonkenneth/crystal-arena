using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CrystalArena.UserInterface;

namespace CrystalArena;

public static class GameRepository
{
    public class GameNotFoundException : Exception
    {
        public GameNotFoundException(string id)
            : base($"Game with id {id} not found") { }
    }

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

    public static bool Exists(string id)
    {
        return _containers.ContainsKey(id);
    }

    public static Ui ResolveUi(string id, bool createIfMissing = false)
    {
        if (!createIfMissing && !Exists(id))
        {
            throw new GameNotFoundException(id);
        }
        var ui = GetContainer(id).Resolve<Ui>();
        ui.GameId = id;
        return ui;
    }

    public static string NextId()
    {
        return System.Guid.NewGuid().ToString();
    }
}
