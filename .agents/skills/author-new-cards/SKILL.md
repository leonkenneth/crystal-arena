---
name: author-new-cards
description: >-
  Implement or fix collectible cards in the CrystalArena C# engine test-first.
  Use when asked to add, write, implement, port, or repair a Forward, Backup, Monster, or Summon;
  reproduce printed card abilities; or add card-focused scenario tests. Covers the card-builder
  DSL, reusable effects, triggers, modifiers, costs, targeting, the scenario test DSL, and
  rule-to-engine mappings.
---

# Authoring new cards

Cards live in the **engine** (`engine/CrystalArena/FFTCGCards/<Set>/`) as small C# classes built
with a fluent DSL, auto-discovered by reflection. Each card is developed **test-first** and reuses
a large library of existing effects, triggers, modifiers, costs, and targeting predicates.

## Golden rules

1. **TDD, always.** Write the scenario test from the card's printed text first, watch it fail, then
   implement the card until it passes. Never write the card before the test.
2. **Reuse before you build.** Almost every mechanic already exists. Search the catalogs
   (`references/building-blocks.md`) for an existing effect/trigger/modifier/cost/target before
   writing any new one.
3. **Don't add core engine files casually.** If a needed effect/trigger/etc. seems missing, first
   check whether an existing one can be *parameterized or generalized*. If a new core file really is
   needed, **stop and confirm the design with the user before creating it** (see "When a building
   block is missing").
4. **Match the surrounding code.** Copy the idioms from a nearby card in the same set. Keep it simple.
5. **Do not duplicate card data in comments.** Do not add a header comment that repeats the serial,
   set, rarity, element, type, cost, job, categories, or ability text. The fluent builder and
   `.Text(...)` are the source of truth. Add comments only when they explain non-obvious behavior.

## Reference docs (read the ones you need)

| File | What's in it |
|---|---|
| `references/card-builder.md` | The `Card.Code(...)` fluent API: types, cost/stats, the six ability builders, and every settable `p.` option on each ability. **Start here.** |
| `references/building-blocks.md` | **The map.** Catalogs of reusable Effects, Triggers, Modifiers, Costs, Targeting predicates, AI rules, and enums — with signatures. Search here before building anything. |
| `references/testing.md` | The scenario test DSL (setup, actions, assertions), file/naming conventions, and how to run a single card's tests. |
| `references/rules-glossary.md` | Card term → engine concept map (CP, Dull/Freeze, Break Zone, EX Burst, Damage N, party/attacks, generic-card icon…) and how to read printed text. |

For a subtle ruling, consult the current advanced rules on the official website. Do not rely on a
remembered version number: rules change. The glossary's section numbers are aligned with v3.3
(effective 2026-08-07) and may drift in later revisions.

## Where things live (the location map)

```
engine/CrystalArena/
  FFTCGCards/<Set>/<Set>_NNNR_Name.cs ← the card you write        (namespace CrystalArena.FFTCGCards.<Set>)
  Core/Card/CardTemplate.cs           ← the fluent builder (.Cost, .Forward, .TriggeredAbility, …)
  Core/Effects/                       ← ~178 effects   (DealDamageToTargets, ReturnToHand, SearchMainDeckPutToZone, …)
  Core/Triggers/                      ← ~25 triggers   (OnZoneChanged, WhenThisAttacks, OnCastedSpell, …)
  Core/Modifiers/                     ← ~68 modifiers  (AddPowerAndToughness, ChangeController, AddCounters, …)
  Core/Costs/                         ← ~26 costs      (PayMana, Tap, Sacrifice, AggregateCost, RevealLimitBreakCards, …)
  Core/Targeting/ + Core/Zones/       ← the trg.Is.Forward(...).On.Battlefield() DSL
  Core/AI/TargetingRules|TimingRules/ ← AI hints attached via p.TargetingRule / p.TimingRule
engine/CrystalArena.Tests/
  FFTCGCards/<Set>/<Set>_NNNR_Name.cs ← the test you write first  (namespace CrystalArena.Tests.FFTCGCards.<Set>)
```

## The workflow

### 1. Understand the card
Get the exact printed text (serial, name, element, cost, type, power, job, categories, abilities)
and whether the card has the generic-card icon. Do not infer the icon from the rarity letter.
Break the ability text into clauses. Map each clause to a mechanic using `references/rules-glossary.md`
and the catalogs. Read 1–2 existing cards that do something similar
(`engine/CrystalArena/FFTCGCards/Opus23/`) and copy their structure.

### 2. Write the test first (RED)
Create `engine/CrystalArena.Tests/FFTCGCards/<Set>/<Class>.cs` (see `references/testing.md`).
Write one `[Fact]` per meaningful clause / branch (the effect happens; an edge case; a "you may"
declined; the generic-icon/same-name rule if relevant). Set up zones, script the cast/activation,
and assert on resulting state. Run it and confirm it **fails** for the right reason:

```bash
cd engine && dotnet test CrystalArena.Tests/CrystalArena.Tests.csproj \
  --filter "FullyQualifiedName~<Class>"
```

Because scenarios resolve cards by serial at runtime, a missing card usually fails during catalog
lookup or scenario initialization rather than compilation. Confirm that failure, then add an
identity-and-stats-only stub and confirm the test now fails on the missing behavior.

### 3. Implement the card (GREEN)
Create `engine/CrystalArena/FFTCGCards/<Set>/<Class>.cs` as `: CardTemplateSource`. Build identity,
cost, type, and stats, then add abilities by **composing existing** effects/triggers/modifiers/costs
(`references/building-blocks.md`). Paste the printed text into `.Text(...)` and into each ability's
`p.Text`. Re-run the filtered tests until green.

### 4. Refine & finish
- Add tests for any branch you missed; keep all green.
- Every Forward must call `.Power(n)` (a global test enforces it).
- Format only the touched card and test files:
  `cd engine && dotnet csharpier format CrystalArena/FFTCGCards/<Set>/<Class>.cs CrystalArena.Tests/FFTCGCards/<Set>/<Class>.cs`
- Run the filtered tests once more. (Commit/push only if the user asks.)

## When a building block is missing

Work through this in order — **do not jump straight to writing a new core file**:

1. **Search harder.** Use `rg` in `Core/Effects`, `Core/Triggers`, `Core/Modifiers`, `Core/Costs` and the
   catalogs for synonyms. Many effects are more general than their name suggests (e.g. `Card(predicate)`
   covers most target shapes; `ApplyModifiersTo*` + a modifier covers most "grant/becomes" effects).
2. **Compose.** Combine existing pieces: `CompoundEffect(a, b)`, `PayLifeThen(...)`,
   `ApplyModifiersToTargets(() => new AddSimpleAbility(...))`, a `StaticAbility` with a `Condition`,
   a `DynParam` (`P(...)`) for a computed value. Most "new" behavior is a new *combination*.
3. **Generalize an existing block.** If one existing effect/trigger is *almost* right, prefer adding
   a parameter/overload to it over creating a sibling. Point out the candidate and the change.
4. **Only then, a new core file.** If steps 1–3 don't cover it, **pause and propose the design to the
   user** before creating anything under `Core/`: name it, show the signature and where it slots in,
   note what it generalizes, and confirm. Keep the new block minimal and consistent with neighbors,
   and give it its own unit coverage.

Prefer a small, ugly composition of existing blocks over an elegant new abstraction the user hasn't seen.

## Minimal end-to-end example

Card **"23-016R Bahamut"** — *Summon, cost 5 Fire, EX Burst: "Choose 1 Forward with 9000 power or
less and up to 1 Forward in your opponent's Break Zone. Remove them from the game."*

Test (`CrystalArena.Tests/FFTCGCards/Opus23/Opus23_016R_Bahamut.cs`):
```csharp
public class Opus23_016R_Bahamut : PredefinedScenario
{
    [Fact]
    public void RemovesBothTargets()
    {
        var onField = C("0-002X"); var inBreak = C("0-002X"); var bahamut = C("23-016R");
        Battlefield(P2, onField); BreakZone(P2, inBreak); Hand(P1, bahamut);
        Exec(
            At(Step.FirstMain, turn: 1).Cast(bahamut, targets: Ts(onField, inBreak)),
            At(Step.SecondMain, turn: 1).Verify(() => {
                True(onField.Card.Zone() == Zone.RemovedFromPlay);
                True(inBreak.Card.Zone() == Zone.RemovedFromPlay);
            }));
    }
}
```

Card (`CrystalArena/FFTCGCards/Opus23/Opus23_016R_Bahamut.cs`):
```csharp
public class Opus23_016R_Bahamut : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card.Code("23-016R").Named("Bahamut").Cost(5, "R").Category("IX").Summon()
            .Text("Choose 1 Forward with 9000 power or less and up to 1 Forward in your opponent's Break Zone. Remove them from the game.")
            .Cast(p =>
            {
                p.ExBurst();
                p.Text = "Choose 1 Forward with 9000 power or less and up to 1 Forward in your opponent's Break Zone. Remove them from the game.";
                p.Effect = () => new RemoveFromPlayTargets();
                p.TargetSelector.AddEffect(trg => trg.Is.ForwardWith(x => x.Power <= 9000).In.Battlefield());
                p.TargetSelector.AddEffect(
                    trg => trg.Is.Forward(ControlledBy.Opponent).In.BreakZone(),
                    cfg => cfg.MinCount = 0
                );
            });
    }
}
```

Note the reuse: `RemoveFromPlayTargets` (existing effect), `ForwardWith`/`Forward` targeting, `ControlledBy.Opponent`,
`p.ExBurst()`. Nothing new was built — that's the target state for most cards.
