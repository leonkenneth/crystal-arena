# Testing cards (the scenario DSL)

Every card is driven test-first with an **xUnit scenario test**. A scenario scripts both
players' decisions turn-by-turn, runs the game, and asserts on the resulting state.

- Test framework: **xUnit** (`[Fact]`). Base class: `PredefinedScenario` (both players
  controlled by the script — `engine/CrystalArena.Tests/Infrastructure/PredefinedScenario.cs`).
- The DSL itself lives in `engine/CrystalArena/Core/Decisions/ScenarioStep.cs` (actions) and
  `engine/CrystalArena.Tests/Infrastructure/Scenario.cs` (setup + assertions).

## File & naming convention

| | Card | Test |
|---|---|---|
| Path | `engine/CrystalArena/FFTCGCards/Opus23/Opus23_017C_Parai.cs` | `engine/CrystalArena.Tests/FFTCGCards/Opus23/Opus23_017C_Parai.cs` |
| Namespace | `CrystalArena.FFTCGCards.Opus23` | `CrystalArena.Tests.FFTCGCards.Opus23` |
| Class | `Opus23_017C_Parai : CardTemplateSource` | `Opus23_017C_Parai : PredefinedScenario` |

The test class name **matches the card class name** — that's what makes `--filter` work (below).
Cards are auto-discovered by reflection (`Cards.cs`), so no registration step: create the file, it exists.

## Skeleton

```csharp
namespace CrystalArena.Tests.FFTCGCards.Opus23
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class Opus23_0XX_Name : PredefinedScenario
    {
        [Fact]
        public void DescribesTheRuleUnderTest()
        {
            var card = C("23-0XXY");                 // create by serial (or by exact name)
            Hand(P1, card);                          // place it in a zone

            Exec(
                At(Step.FirstMain, turn: 1).Cast(card),
                At(Step.SecondMain, turn: 1).Verify(() => Equal(1, P1.Battlefield.Count()))
            );
        }
    }
}
```

For a card with several unrelated abilities, either write several `[Fact]`s in one class, or nest
a `public class Predefined : PredefinedScenario { ... }` inside the outer card class (see
`Opus23_001C_Garland` test).

## Zone setup helpers (from `Scenario.cs`)

Place cards before running. Each takes a player and any number of `ScenarioCard`s (a bare
`"serial"` string implicitly converts to one — see `ScenarioCard.cs`):

- `Hand(P1, card, "23-007C")` — put in hand.
- `Battlefield(P1, card)` — put onto the field, already active, no summoning sickness. Chain
  `.Tap()` for a dull card, `.AddCounters(n, CounterType.X)`, `.IsEnchantedWith(...)`,
  `.IsEquipedWith(...)`, `.IsTrackedBy(...)`.
- `BreakZone(P2, "0-004X")`, `LimitBreak(P1, ...)`, `MainDeck(P1, ...)` — other zones.
- `P1.Life = 2;` — set life directly (drives the `Damage N` mechanic — see `Opus23_004R_Kefka` test).

`C("23-016R")` → a `ScenarioCard` handle you pass to actions. `C(scenarioCard)` → the live `Card`
(for reading state: `C(fwd).Controller`, `fwd.Card.Zone()`, `fwd.Card.Power`).
`Ts(a, b)` → `ITarget[]` for multi-target casts.

## Actions — `At(Step, turn: N).X(...)`

`At(step, turn)` selects when the scripted decision applies; chain actions on it. Key actions
(full list in `ScenarioStep.cs`):

- `.Cast(card)` — cast with no target. Overloads: `.Cast(card, target)`, `.Cast(card, player)`,
  `.Cast(card, targets: Ts(a, b))`, `.Cast(card, Ts(effectTargets), Ts(costTargets))`,
  and `x:` for X-costs, `index:` to pick among multiple cast rules.
- `.Activate(card)` — use an activated ability. Overloads: `.Activate(card, target)`,
  `.Activate(card, costTarget: m, target: f)`, `.Activate(card, Ts(...))`, `abilityIndex:` to
  pick among several activated abilities, `x:` for X.
- `.Target(card)` / `.Target(player)` — supply a target for a **triggered** ability that asks
  for one after it goes on the stack (put this on the step where the trigger resolves).
- `.Answer(true|false)` — answer a "you may…" / optional prompt (e.g. pay life, optional ability).
- `.DeclareAttackers(f1, f2)` — declare a party of attackers. Use at `Step.DeclareAttackers`.
- `.DeclareBlockers(attacker, blocker)` — pairs of (attacker, blocker).
- `.NoValidTarget()` — assert/decide that no legal target exists.
- `.Verify(() => { ... asserts ... })` — assertions (below). Every step's scripted results must
  be consumed or `Exec` fails (`AssertAllWereExecuted`), so unused `.Verify`/`.Cast` = test error.

`Exec(step1, step2, ...)` runs the game (up to turn 5 by default) applying the scripted steps.

## Assertions (from `Scenario.cs`, wrapping xUnit)

`Equal(expected, actual)`, `True(cond)`, `False(cond)`, `Null(obj)`. Assert against live state:

```csharp
.Verify(() =>
{
    Equal(1, P1.Battlefield.Count());
    True(target.Card.Zone() == Zone.RemovedFromPlay);
    Equal(P1, C(opponentForward).Controller);
    Equal(6, P2.Life);
    True(P1.ManaCache.Has("{Z}".Parse(), ManaUsage.Any, new ConvokeAndDelveOptions()));
})
```

## Common `Step` values

`Step.Upkeep`, `Step.Draw`, `Step.FirstMain`, `Step.DeclareAttackers`, `Step.CombatDamage`,
`Step.EndOfCombat`, `Step.SecondMain`, `Step.EndOfTurn`. Cast most things at `FirstMain`/`SecondMain`;
verify ETB/on-cast effects one step later (e.g. cast at `FirstMain`, verify at `SecondMain`).

## Worked examples to copy from

- **ETB + gain control** — `Opus23_001C_Garland` test (`.Cast(g).Target(oppFwd)`, then verify `.Controller`).
- **Summon that removes multiple targets** — `Opus23_016R_Bahamut` test (`.Cast(bahamut, targets: Ts(a, b))`).
- **Activated ability with a cost-target** — `Opus23_015C_Notsugo` (`.Activate(notsugo, costTarget: m, target: f)`).
- **Combat + `Damage N` scaling on low life** — `Opus23_004R_Kefka` test (`P1.Life = 2; ... DeclareAttackers`).
- **Continuous effect on/off** — `Opus23_011L_Terra` test (two facts: protection absent vs. present).
- **Mana / crystal gain** — `Opus23_007C_Samurai` test (`P1.ManaCache.Has(...)`).

## Running the tests

Single card (fast — filter by the test class name):

```bash
cd engine
dotnet test CrystalArena.Tests/CrystalArena.Tests.csproj \
  --filter "FullyQualifiedName~Opus23_017C_Parai"
```

First run builds (~10s); add `--no-build` after a build to re-run instantly. Full suite:
`pnpm run test:engine` (from repo root) or `dotnet test CrystalArena.Tests/CrystalArena.Tests.csproj`.

There is also a **global check** (`engine/CrystalArena.Tests/FFTCGCards/GlobalChecks.cs`) that fails if any Forward
has no Power — it runs as part of the suite, so every Forward you add must call `.Power(n)`.

## Format before committing

The engine is formatted with **CSharpier** and CI checks it (`pnpm run format:engine`). After
writing/editing card + test files:

```bash
cd engine
dotnet csharpier format \
  CrystalArena/FFTCGCards/<Set>/<Class>.cs \
  CrystalArena.Tests/FFTCGCards/<Set>/<Class>.cs
```
