# The card-builder DSL (`CardTemplate`)

A card is a class `: CardTemplateSource` whose `GetCards()` yields one `CardTemplate`, built with
the fluent `Card.Code(...)` API. `Card` is a property on the base returning a fresh builder.
All builder methods live in `engine/CrystalArena/Core/Card/CardTemplate.cs`.

```csharp
public class Opus23_0XX_Name : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card.Code("23-0XXY")
            .Named("Name")
            .Cost(3, "R")
            .Category("VII")
            .Job("Warrior")
            .Forward()
            .Power(7000)
            .Text("...verbatim printed text...")
            .TriggeredAbility(p => { ... });
    }
}
```

Do not add a `/* ...card data... */` header comment. It duplicates the fluent identity calls and
`.Text(...)`, drifts easily, and adds no implementation context. Use comments only to explain a
non-obvious engine mapping or workaround.

## Identity, cost & stats

- `.Code("23-017C")` — serial. Format `<set>-<number><rarity>` (for example `1-107L` or
  `23-017C`). Drives lookup and filtering.
- `.Named("Parai")` — display name (also used for name-matching in tests/costs).
- `.Cost(3, "R")` — CP cost `3` including element(s) `"R"` → expands to `{2}{R}`. Multi-element:
  `.Cost(4, "RU")` → `{2}{R}{U}`. Or set the raw string with `.ManaCost("{2}{R}")`.
- `.HasXInCost()` — the printed cost contains X (variable). Pair with `x:` in tests.
- `.Power(7000)` — sets **both** Power and Toughness (Forwards use one number). Use
  `.Toughness(n)` only to override toughness separately (rare). **Every Forward must set Power**
  (enforced by `GlobalChecks`).
- `.Colors(CardColor.Fire, ...)` — override colors (normally inferred from mana cost).
- `.Category("VII")` / `.Categories("VII", "FFXIII")` — title/category tags. `" · "`-joined
  strings auto-split. `.Job("Warrior")` — job tag (lower-cased; queried via `card.HasJob("...")`).
- `.FlavorText("...")`, `.Text("...")` — flavor / rules text.

## Card type (pick exactly one)

- `.Forward(multiplayable: false)` — a Forward. `.Backup(...)`, `.Monster(...)` likewise.
- `.Summon()` — a Summon (resolves off the stack, goes to Break Zone).
- **`multiplayable`** maps the printed generic-card icon. `false` (default) prefixes the engine type
  with **"Legendary"**, which is how this engine enforces the same-name field restriction. `true`
  permits multiple copies on the field. The engine label is unrelated to the printed rarity:
  serial suffix `L` means Legend rarity, not "Legendary." Inspect the generic-card icon or trusted
  card data; never infer this argument from `C`/`R`/`H`/`L`.
- `.LimitBreak(level)` — marks a Limit Break card (adds LB-deck reveal cost of `level` cards).
- Timing rules for casting (forward/backup/artifact) are applied automatically by type; override
  only for special cast windows.

## Simple (keyword) abilities

`.SimpleAbilities(Static.FirstStrike, Static.Brave)` — attach printed keyword abilities. Values
come from the `Static` enum (Brave, Haste, FirstStrike, Freeze-related, MustBlockIfPossible, …).
See `references/building-blocks.md` → enums for the full list and which rule keyword each maps to.

## Abilities — the six builders

Each takes a lambda configuring a parameter object `p`. Set `p.Text`, `p.Effect = () => new X()`,
targets, cost, and flags. **Full member reference for every `p` below is in this file's
"Ability parameter surfaces" section.**

- `.TriggeredAbility(p => { p.ExBurst(); p.Trigger(new OnZoneChanged(to: Zone.Battlefield)); p.Effect = () => ...; })`
  — "When/Whenever …". Add multiple `p.Trigger(...)` to OR conditions ("enters the field **or** attacks").
- `.ActivatedAbility(p => { p.Cost = new AggregateCost(new Tap(), new Sacrifice()); p.Effect = ...; })`
  — "{cost}: {effect}". Costs like `{T}` (`new Tap()`), sacrifice, pay mana, discard.
- `.Cast(p => { p.Effect = () => ...; p.TargetSelector.AddEffect(...); })` — the spell/Summon
  effect (what happens when the card resolves). Default Cast is auto-created for permanents.
- `.StaticAbility(p => { p.Condition = cond => ...; p.Modifiers.Add(() => new AddPowerAndToughness(...)); })`
  — a continuous self-modifier gated by `p.Condition` (e.g. "+3000 while you have 2+ X in Break Zone").
- `.ContinuousEffect(p => { p.Selector = (c, ctx) => ...; p.Modifier = () => ...; }, enabledInAllZones: true)`
  — a lasting effect this card radiates onto other cards it selects (e.g. "all your Summons in the
  Break Zone can't be removed").
- `.ManaAbility(p => { ... })` — produces mana. Default backup/discard mana abilities are added
  automatically per color, so you rarely need this.

## Higher-level helpers (compose the above)

- **Do not use `.Damage(...)` in the current engine.** `CardTemplate.Damage` sets its condition but
  fails to invoke the supplied callback, so the requested modifiers are discarded. Until that helper
  is fixed, express Damage N as `.StaticAbility(p => { p.Condition = cond =>
  cond.YouHaveLessThanXLife(7 - n); ... })` and add focused tests at N−1 and N damage.
- `.SpecialAbility(name, init, additionalCost?)` — an ability whose cost is "discard a card named
  <this card>" (`{S}` special actions), plus optional extra cost.
- `.Leveler(cost, tag, Level(...))`, `.IsLeveler()`, `.Loyality(n)` — leveler/loyalty permanents.
- `.Protections(CardColor.Fire)` / `.Protections("Summon")`, `.MinBlockerPower(n)`,
  `.OverrideScore(o => ...)` — misc combat/AI tuning.

## Dynamic values — `P(...)` / `DynParam<T>`

Effects often need a value computed at resolution, not at definition. Use the `P(...)` helper on
`CardTemplateSource` (or `new DynParam<T>(...)` directly):

```csharp
p.Effect = () => new DiscardCards(1, P(e => e.Controller));               // "you" = the controller
p.Effect = () => new DealDamageToPlayer(1, P(e => e.Controller));
new DynParam<Card>((e, _) => e.TriggerMessage<AttackersDeclaredEvent>().Attackers.Single().Card);
```

`P(getter, EvaluateAt.OnResolve | EvaluateAt.OnInit)` controls when the getter runs. `e` is the
`Effect` (with `.Controller`, `.Source`, `.TriggerMessage<T>()`). `L(...)` wraps params into arrays.

---

# Ability parameter surfaces

What you can set on each `p`. Shared base `AbilityParameters`
(`Core/Card/AbilityParameters.cs`) backs Triggered/Activated/Cast/Mana params.

### Shared base (`AbilityParameters`)
- `p.Effect = () => Effect` — the effect factory to run.
- `p.TargetSelector` — target requirements: `.AddEffect(trg => trg.Is.Forward().On.Battlefield(), cfg => cfg.MaxCount = 2)` and `.AddCost(...)`. See `references/building-blocks.md` → targeting.
- `p.Text` — this ability's rules text.
- `p.UsesStack` — bool (default `true`); `false` = resolve immediately, off the stack.
- `p.ExBurst()` — mark as EX Burst.
- `p.PlayZone` — `Zone?` the resulting card is played to (hint).
- `p.DistributeAmount` — amount to spread across multiple targets.
- `p.TimingRule(...)`, `p.TargetingRule(...)`, `p.RepetitionRule(...)`, `p.CostRule(...)` — **AI hints**
  (not game rules): when/what/how-many/how-to-pay. See building-blocks → AI rules.

### `TriggeredAbility.Parameters`
- `p.Trigger(Trigger)` — add a trigger (call repeatedly to OR). Catalog in building-blocks → triggers.
- `p.TriggerOnlyIfOwningCardIsInPlay` — only armed while the owner is on the field.
- `p.TriggerOnlyOncePerTurn` — suppress repeats within a turn.

### `ActivatedAbilityParameters`
- `p.Cost = Cost` — activation cost (`new Tap()`, `new PayMana("{R}".Parse())`, `new AggregateCost(...)`).
- `p.ActivateAsSorcery` — main phase, your turn, empty stack only.
- `p.ActivateOnlyOnceEachTurn`, `p.ActivateOnlyDuringYourTurn` — timing gates.
- `p.ActivationZone` — `Zone` the card must be in (default `Battlefield`; e.g. `Zone.Hand`).
- `p.Condition = (card, game) => bool` — extra activation gate.
- `p.PutToZoneAfterActivation = card => { ... }` — post-activation placement.

### `CastRule.Parameters` (`.Cast`)
- `p.Cost = Cost` — extra/kicker cost beyond printed mana (`CostType.Spell`).
- `p.Condition = (card, game) => bool` — whether the card can be cast.
- `p.AfterResolve = (card, ctx) => { ... }` — override post-resolution zone placement.
- `p.KickerDescription` — string. Inherits `p.Text`/`p.ExBurst()`/timing (auto-defaulted by type).

### `StaticAbilityParameters` (standalone)
- `p.Modifiers.Add(() => new AddPowerAndToughness(+3000, +3000))` — continuous self-modifiers
  (also `AddSimpleAbility`, `AddTriggeredAbility`, `AddActivatedAbility`).
- `p.Condition = cond => bool` — gate. `cond` helpers: `cond.OwnerHasCardInBreakZone(cards => ...)`,
  `cond.OwnerControlsPermanents(...)`, `cond.PermanentExists(...)`, `cond.OwningCardHas(Static)`,
  `cond.YouHaveLessThanXLife(n)`, `cond.OpponentHasCardInRemovedFromPlay()`.
- `p.EnabledInAllZones` — static also works off the battlefield.

### `ContinuousEffectParameters` (`.ContinuousEffect`)
- `p.Selector = (card, ctx) => bool` — which cards it applies to.
- `p.Modifier = () => new SomeModifier(...)` — add one `ICardModifier`; assign repeatedly or use
  `p.Modifiers.Add(...)` for several.
- `p.ApplyOnlyToPermanents` — default `true`; `false` to reach non-battlefield cards (Break Zone, etc.).

### `ManaAbilityParameters`
- `p.ManaAmount(amount)` or `p.ManaAmount(color, filter, controlledBy)` — mana produced.
- `p.Priority`, `p.UsageRestriction` (`ManaUsage`), `p.AdditionalEffects`. Defaults: `Cost = TapOwner`, `UsesStack = false`.
