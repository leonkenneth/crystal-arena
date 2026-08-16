# Building blocks — the reuse catalog

**Search this file before writing any new effect/trigger/modifier/cost.** Almost every mechanic
already exists. Entries show the **class name you instantiate** (not always the filename — see
"Naming gotchas") and the key constructor args. Most `DynParam<T>` args accept a raw literal or a
`P(e => ...)` lambda (see `card-builder.md` → dynamic values).

Contents: [Effects](#effects) · [Triggers](#triggers) · [Modifiers](#modifiers) · [Costs](#costs) ·
[Targeting](#targeting) · [Enums](#enums) · [Mana syntax](#mana--cost-string-syntax) ·
[AI hint rules](#ai-hint-rules) · [Gaps & gotchas](#gaps--naming-gotchas)

Directories: Effects `Core/Effects/` · Triggers `Core/Triggers/` · Modifiers `Core/Modifiers/` ·
Costs `Core/Costs/` · Targeting `Core/Targeting/` + `Core/Zones/` · AI rules `Core/AI/*Rules/`.

---

## Effects

Set as `p.Effect = () => new X(...)`. Effects **don't take targets as ctor args** — they read the
targets defined by `p.TargetSelector.AddEffect(...)`. Suffix conventions: `...ToSelf`/`...Owner` act
on this card; `...ToTargets` act on chosen targets; `...ToCard` act on a specific `DynParam<Card>`;
`...ToPermanents`/`...ToPlayers` act on all cards/players matching a filter; `...ToGame` applies
global `IGameModifier`s.

### Damage
- `DealDamageToTargets(amount, gainLife=false, canBePrevented=null)` — damage each chosen Forward/player.
- `DealDamageToAndTapTargets(amount)` — damage + dull targets.
- `DealDamageToForward(amount, forward)` — damage one specific (non-targeted) `DynParam<Card>` Forward.
- `DealDamageToPlayer(amount, player)` — damage a specific `DynParam<Player>`.
- `DealDamageToForwardsAndPlayers(amountForward, amountPlayer, filterForward, filterPlayer)` — AoE.
- `DealDifferentDamageToTargets(IEnumerable<int> amounts)` — different amount per target, in order.
- `DistributeDamageToTargets()` — split a chosen distribution across targets.
- `DealDamageToForwardWithAttributeSelectIfMoreThanOne(amount, hasAttribute, getAttribute)`.
- `DealDamageToTargetForEachRevealedCard(filter)` · `DealDamageToOpponentEqualToCardDifference()`.
- `DealExistingDamageToPlayer(damage, player)` — redirect an existing `IDamage` to a player.
- `SacrificeToDealDamageToTarget(filter)` · `TapForwardsThatDidntAttackDamagePlayer()` · `FlipCoinDealDamageToItself(amount)`.

### Damage prevention / replacement
- `PreventAllDamageToTargets()` · `PreventAllDamageToTargetsFromChosenColor()`.
- `PreventAllDamageFromSourceUntilEot(preventCombatOnly=false)` — target's damage output → 0 this turn.
- `PreventAllCombatDamage(filter=null)` · `PreventNextXDamageToTargets(amount)`.
- `PreventFirstDamageFromSourceToController()` · `PreventFirstDamageFromSourceToTarget()` · `PreventDamageToEquipedForward(amount)`.
- `ReplaceDamageToTargets(target => new ReplaceDamage(predicate, mutation), untilEot)` — e.g. double/zero damage (see Samurai). `ReplaceDamage` is in `Modifiers/`.
- `RegenerateOwner()` · `RegenerateTarget()`.

### Zone — return to hand
- `ReturnToHand(discard=0, returnOwningCard=false, tag=EffectTag.Bounce)` or `ReturnToHand(card)` — bounce targets / a specific card.
- `PutTargetPermanentToHand()` · `ReturnAllPermanentsToHand(filter)` · `ReturnAllCardsInBreakZoneToHand(filter)`.
- `ReturnCardsFromBreakZoneToHandForEachRevealedCard(revealFilter, breakZoneFilter)` · `ReturnOwnerAndAttachedToHand()`.
- `EachPlayerReturnsCardsToHand(minCount, maxCount, zone, aiOrders, text, filter)`.

### Zone — break / destroy / sacrifice
- `DestroyTargetPermanents(canRegenerate=true)` · `DestroyPermanent(card)` · `DestroyAllPermanents(filter=null, allowToRegenerate=true)`.
- `DestroyAllBackupsOrForwards()` · `DestroyAttachedAttachments(permanent, filter=null)` · `DestroyOwner()` · `PowderKegEffect()`.
- `SacrificeOwner()` · `PlayerSacrificePermanents(count, player, filter, text)` · `PlayersSacrificePermanents(count, text, validator, playerFilter)`.
- `TargetPlayerSacrificesPermanents(count, filter, text)` · `PlayerSelectPermanentsAndSacrificeRest(toUpCount, player, filter, text)`.

### Zone — remove from play (RFG / exile)
- `RemoveFromPlayTargets()` · `RemoveFromPlayCard(card, from)` · `RemoveFromPlayAllCards(from=Battlefield, filter=null)` · `RemoveFromPlaySelectedCards(amount, from, filter=null)`.
- `RemoveFromPlayOwner()` · `RemoveFromPlayOwnerUnlessYouDiscardForwardCard()`.
- `RemoveFromPlayTargetsUntilOwnerLeavesBattlefield()` (blink) · `RemoveFromPlayTargetThenPutIntoPlayUnderOwnersControl()`.
- `RemoveFromPlayCardsWithSameNameAsTargetFromGhl()` · `OpponentRemoveTheirHandFromTheGameUntilEot()`.

### Zone — put into play
- `PutCardToBattlefield(card, from)` · `PutOwnerToBattlefield(from, tap=null)` · `PutTargetsToBattlefield(mustSacForwardOnResolve=false, tapped=false)`.
- `PutSelectedCardsToBattlefield(fromZone, validator, text, after, count, modifiers)` · `PutCardsFromBreakZoneToBattlefield(filter, modify=null, eachPlayer=false)` (reanimate).
- `PutForwardsFromBreakZonesToYourBattlefield()` · `EachPlayerPutsACardToBattlefield(zone, filter=null)` · `CastPermanent(tap=null)` (casting rule).

### Zone — deck top / reorder / mill
- `PutTargetsOnTopOfMainDeck()` · `PutTargetsIntoMainDeckAtPosition(positionFromTheTop)` · `PutAllPermanentsOnTopOfMainDeck()`.
- `ShuffleOwningCardIntoMainDeck()` · `ShuffleTargetBreakZoneIntoMainDeck()` · `PutOnTopOfMainDeckUnlessOpponentSacsBackup()`.
- `ReorderTopCards(count)` · `Scry(count)` · `PlayerPutsTopCardsFromMainDeckToBreakZone(count)` · `EachPlayerPutTopCardsFromMainDeckToBreakZone(count)`.

### Search / dig
- `SearchMainDeckPutToZone(zone, afterPutToZone, maxCount=1, minCount=0, validator, text, ...)` — search deck for matches → zone. **(file `SearchLibraryPutToZone.cs`)**
- `ChooseInBreakZonePutToZone(zone, afterPutToZone, maxCount=1, minCount=0, validator, text, revealCards=true, player, rankingAlgorithm)` — pick from Break Zone → zone (see Terra).
- `SearchAuraAndAttachToOwningCard()`.
- `LookAtTopCardsPutPartInHandRestIntoBreakZone(count, toHandAmount=1)` · `LookAtTopCardsPutPartInHandRestOnBottom(count)` · `RevealTopCardsPutOneInHandOthersIntoBreakZone(cardsToReveal, selector)`.
- `PutFirstCardInPlayPutOtherCardsToZone(toZone, filter=null)` · `PutSelectedCardsIntoBreakZoneOthersOnTop(count, countBreakZone=null)` · `PutSelectedAttackersOnTopRestOnBottom()`.

### Control (change controller)
- `SwitchController()` · `GainControlOfAllPermanents(filter)` · `ExchangeForOpponentsForward()` · `ExchangeCardsInBattlefieldAndBreakZone()`.
- `AttachTargetToOwningCard(gainControl=true)` · `FlipACoinOpponentGainsOwningCard()`.
- For "gain control of 1 Forward until EoT": `new Attach(() => new ChangeController(m => m.SourceCard.Controller) { UntilEot = true })` (see Garland).

### Counters
- `Put11CountersOnTargets(count)` — N +1000/+1000 ("11") counters · `PutDifferentAmountOf11ContersOnTargets(amounts)` · `DistributeCountersAmongTargets(() => new Counter())`.
- `Add11ForEachCounter()` · `ChooseToAddCounter(counterType, chooseAi)` · `PayLifeAddCounters(counterType)`.
- `RemoveAllCountersFromOwner(counterType)` · `RemoveAllCountersFromPermanents(filter, counterType)` · `BecomeMonstrous(counterCount)` · `PutCounterOnYoursAndFightWithOpponentsForward(() => new Counter(), count)`.

### Mana / cost
- `AddManaToPool(amount, usage=Any)` — add `ManaAmount` (e.g. `new AddManaToPool("{Z}".Parse())`
  adds one Crystal, see Samurai). Numeric tokens represent generic CP; `{Z}` is the distinct
  card game's Crystal resource.
- `AddManaForEachRevealedCard(filter, amount)` · `ForwardsOfChosenTypeCostLess(amount)`.

### Life
- `ChangeLife(amount, whos)` · `YourLifeBecomesEqual(amount)` · `ControllerGainsLifeOpponentLoosesLife(amountGained, amountLost)`.
- `GainLifeForEachRevealedCard(filter, amount)` · `TargetPlayerGainsLifeEqualToForwardCount(multiplier=1)` · `ChangeLifeOfEnchantedPermanentsController(amount)`.

### Card advantage (draw / discard / reveal)
- `DrawCards(count, discardCount=0, lifeloss=0, player=null)` · `TargetPlayerDrawsCards(cardCount, lifeLoss=0)` · `DrawCardsEqualToSacrificedPermanentsCount(text, validator)`.
- `DiscardCards(count, player=null)` (see Cecil: `new DiscardCards(1, P(e => e.Controller))`) · `DiscardHand()` · `OpponentDiscardsHand()` · `EachPlayerDiscardsHand()`.
- `OpponentDiscardsCards(randomCount, selectedCount, youChooseDiscardedCards, filter)` · `DiscardCardToDrawCard()` · `DiscardCardOrSacrificeOwner()` · `DiscardAllCardsOfChosenColor()` · `DiscardAndDrawANewHand()`.
- Wheels: `EachPlayerDiscardsHandAndDrawsGreatestDiscardedCount()` · `EachPlayerDiscardsHandAndDrawsThatManyCardsMinusOne()` · `EachPlayerShufflesHandAndBreakZoneIntoMainDeckAndDrawsCards()` · `PlayersReplaceTheirHandWithNewOneUntilEot()` · `OpponentRevealsHand()`.

### Tap / untap / dull / freeze
- `TapTargets()` · `TapPermanents(ctx => cards)` · `DullAndFreezeTargets()` · `DullAndFreezePermanents(ctx => cards)`.
- `UntapTargetPermanents()` · `UntapOwner()` · `UntapAllBackups()` · `UntapSelectedPermanents(minCount, maxCount, validator, text)` · `UntapEachPermanent(filter, controlledBy=Any)` · `TapOrUntapAllArtifactsForwardsOrBackups()`.

### Applying modifiers / buffs / keywords
- `ApplyModifiersToSelf(() => new Mod(...), ...)` — on this card. · `ApplyModifiersToCard(card, () => new Mod(...))` — on a specific `DynParam<Card>`.
- `ApplyModifiersToTargets(() => new Mod(...))` — on chosen targets (see Zorn & Thorn: grant `Static.MustBlockIfPossible`). Overload takes `PlayerModifierFactory[]`.
- `ApplyModifiersToSelfAndToTargets(self, target, ...)` · `ApplyModifiersToPermanents(selector, () => new Mod(...))` · `ApplyModifiersToPlayer(selector, ...)`.
- `ApplyModifiersToGame(() => new AddCostModifier(...), ...)` — global (cost reducers, static rules; see Terra).
- `Attach(() => new Mod(...))` — attach this card carrying modifiers · `RemoveModifier(typeof(SomeModifier))`.
- `ForwardsOfChosenTypeGainPT(power, toughness, controlledBy=Any)` · `ForwardGetsPwtForEachRevealedCard(power, toughness, filter)`.
- `TargetGainsProtectionFromChosenColor()` · `TargetLoosesChosenAbility(Static.A, ...)` · `OwnerGainsBraveLifelinkOrHaste()` · `BecomeCopyOfTargetCard()` · `CreateEmblem(text, score, controller, () => new Mod(...))`.

### Tokens / counterspells / fight / extra turns
- `CreateTokens(tokenTemplate, ...)` / `CreateTokens(count, token, afterComesToPlay, controller, params)`.
- `CounterTargetSpell(p => {...})` · `CounterThatSpell(spell, doNotCounterCost=null)` · `CounterTargetSpellUnlessControllerPays1ForEachRevealedCard()` · `CastCardWithoutpayingIfOpponentGuessedWrong(question, chooseAnswer, isCorrectAnswer)`.
- `Fight(card => order)` · `TakeExtraTurn()` · `TargetPlayerTakesExtraTurn()`.
- `ActivePlayerPaysLifeOrReturnSelectedPermanentToHand(life)` · `PayLifeOrTapBackup(life)` · `SacrificeForwardOrPayLifeOrOpponentDrawsCard(lifeAmount)`.

### Composition & control-flow (build compound behavior from these)
- `CompoundEffect(eff1, eff2, ...)` — run several effects together, sharing `Targets` (see Cecil, Terra).
- `ModalEffect(maxCount, ceap => { ceap.Text=...; ceap.Effect=()=>...; ceap.TargetSelector.AddEffect(...); }, ceap => {...})` — "choose up to N modes", each a full `AbilityParameters`.
- `PayManaThen(amount, innerEffect, parameters=null)` · `PayLifeThen(amount, innerEffect, parameters=null)` (see Cecil: `new PayLifeThen(1, new DullAndFreezeTargets())`) — pay a cost, then run inner effect if paid. Base `PayThen` with `Parameters { ExecuteIfPaid, Message }`.
- `FerociousEffect(normal[], ferocious[], instead=false)` · `ApplyActionToPermanentOrApplyActionToOwner(...)`.
- **Conditional "if"**: there is **no** `ConditionalEffect`. Gate at the ability level (`p.Condition`, the trigger, a `StaticAbility.Condition`), or use base-`Effect` delegates `ShouldResolve` / `BeforeResolve` / `AfterResolve`, or `PayThen` / `ModalEffect`.
- Coin flips: `FlipACoinReturnToHand()` · `FlipACoinReturnToHandOrSacrifice()`.

---

## Triggers

Add with `p.Trigger(new X(...))` inside `.TriggeredAbility(p => ...)`. Multiple `p.Trigger` calls
**OR** together (e.g. "enters the field **or** attacks"). Shared context on predicates: `ctx.OwningCard`,
`ctx.You`, `ctx.Opponent`, `ctx.Turn`, `ctx.Combat`, `ctx.You.IsActive`.

### Zone / ETB / LTB
- `OnZoneChanged(from=None, to=None, selector=null)` — **the workhorse**. ETB = `to: Zone.Battlefield`; field→Break = `from: Battlefield, to: BreakZone`. `selector (card, ctx)` defaults to self; pass one to watch other cards.
- `WhenPermanentLeavesPlay(Card permanent)` — a specific referenced permanent leaves.
- `OnForwardDamagedBySelfDiesInSameTurn()`.

### Phase / step
- `OnStepStart(Step step, activeTurn=true, passiveTurn=false, onlyOnceAfterActivated=false)` — start of a step, gated by turn.

### Combat
- `AfterAttackersAreDeclared(ctx => bool)` · `WhenThisAttacks(p => bool)` (`p.Attacker`) ·
  `WhenAForwardAttacks(p => bool)` (`p.You`, `p.Opponent`, `p.AttackerHas(...)`). In the last
  trigger, `p.You` means the attacker is controlled by your opponent (it attacks you), while
  `p.Opponent` means the attacker is controlled by you (it attacks your opponent).
- `WhenThisBlocks()` · `WhenThisBecomesBlocked(bool triggerForEveryBlocker)`.

### Cast / activate / spell
- `OnCastedSpell(selector=null)` — a spell hits the stack, e.g. `new OnCastedSpell((c, ctx) => c.Is().Summon)`.
- `OnCounteredSpell(filter=null)` · `OnBackupPlayed(filter)` (pass a non-null filter) · `OnEffectResolved(filter)` (required) · `OnBeingTargetedBySpellOrAbility(predicate=null)`.

### Counters / levels / tap
- `OnLevelChanged(int level)` · `OnOwnerGetsTapped()` · `OnPermanentGetsTapped(filter)`.

### Damage / life
- `OnDamageDealt(p => bool)` — rich `p`: `.IsDealtToPlayer/Forward/You/Opponent/OwningCard`, `.IsDealtBy...`, `.IsCombat`, `.Source`.
- `OnLifeChanged(f => bool)` — `f.IsYours/IsOpponents/IsGain/IsLoss` · `OnLifepointsLeft(ability => bool)` — re-checks life on any change/activation.

### Player actions / attachments
- `WhenPlayerDiscardsCard(cond=null)` · `WhenPlayerDrawsCard(filter=null)` · `WhenPlayerSearchesMainDeck(cond=null)`.
- `OnAttachmentAttached(cond=null)` · `OnAttachmentDetached()`.

Ability-level trigger flags: `p.TriggerOnlyIfOwningCardIsInPlay`, `p.TriggerOnlyOncePerTurn`, `p.UsesStack`.

---

## Modifiers

Instantiated in factory lambdas: `ApplyModifiersToSelf(() => new X(...) { UntilEot = true })`,
`.Modifier = () => new X()`, or `card.AddModifier(new X(...), params)`. `Value` is implicit-`int`
with `Value.PlusX`/`Value.MinusX`.

### Power / toughness
- `AddPowerAndToughness(power, toughness)` — **most common** (also `Func<Player,Value>` overload). · `SetPowerAndToughness(power, toughness)` · `SwitchPowerAndToughness()`.
- `Add11ForEachOtherForward()` · `ModifyPowerToughnessForEachPermanent(power?, toughness?, filter, () => modifier, controlledBy=SpellOwner)`.
- `ModifyPowerToughnessEqualToControllersLife()` · `ModifyPowerToughnessEqualToTotalHandsCount()` · `IncreasePowerToughnessEqualToAttachedCounterCount(counterType)`.
- `ChangeToForward(power, toughness, type, colors=null)` (Monster→Forward) · `ChangeToMonster()`.

### Counters (types live in `Core/Counter/`)
- `AddCounters(() => new Counter(), count)` — attach N counters (also `GetCount(ctx)` overload).
- Counter kinds: `PowerToughness(power, toughness)` (a +P/+T counter) · `SimpleCounter(CounterType)` (marker) · `IncreaseLevel()`.

### Abilities
- `AddSimpleAbility(Static)` (e.g. `Static.Haste`) · `AddTriggeredAbility(TriggeredAbility)` · `AddStaticAbility(StaticAbility)` · `AddActivatedAbility(ActivatedAbility)` (see Cecil).
- `AddContiniousEffect(ContinuousEffect)` [player] · `AddCostModifier(CostModifier)` [game] · `AddNamedGameModifier(Static)` [game].
- `RemoveAbility(Static)` · `DisableAllAbilities(activated=false, simple=false, triggered=false)` · `ChangeBasicBackupSubtype(subtype, replace)`.

### Control / protection / type / color
- `ChangeController(Player)` or `ChangeController(m => player)` — usually `{ UntilEot = true }` (see Garland).
- `AddProtectionFromColors(CardColor)` (also `IEnumerable`/`Func` overloads) · `AddProtectionFromOpponentRemoveFromGameEffects()` (see Terra).
- `SetColors(CardColor)` · `ChangeCardTemplate(CardTemplate)`.

### Mana / backup / combat / status
- `IncreaseManaOutput(ManaAmount)` · `IncreaseBackupLimit(amount=1)` [player] · `PreventPlayingBackups()` [player].
- `IncreaseCombatCost(value)` · `SetMinBlockerCount(amount)`.
- `Freeze()` — grants `Static.DoesNotUntap`, auto-expires next Untap · `AddDamagePrevention(...)` [game] · `AddDamageRedirection(...)` [game].

### Object-initializer flags & lifetimes
- **`{ UntilEot = true }`** is the one common initializer flag (auto-adds an end-of-turn lifetime). `SourceCard`/`Owner`/`X` come from `ModifierParameters`, not initializers.
- `.AddLifetime(...)` adds an extra expiry: `LevelLifetime(min, max?)`, `EndOfStep(step, filter?)`, `OwningCardLifetime()`, `AttachmentLifetime(selector?)`, `ModifierSourceGetsUntapedLifetime()`, `OwnerControlsPermamentsLifetime(sel)`, `PermanentLeavesBattlefieldLifetime(sel)`, `PlayerCastsForwardLifetime()`, `EmblemLifetime(e)`, `ManualLifetime`, `OwnerHasCardsInRemovedFromPlay(sel)`.
- `CardModifierFactory` = `() => ICardModifier` (siblings `ModifierFactory`/`GameModifierFactory`/`PlayerModifierFactory`); `L(...)` bundles arrays.

---

## Costs

Set as `p.Cost = new X(...)`; combine with `AggregateCost(...)`. An optional `Validator`
(TargetValidator) scopes *which* permanent is paid.

- **Mana/life**: `PayMana(amount, hasX=false, supportsRepetitions=false)` (most common; `"{R}".Parse()`) · `PayLife(amount, supportsRepetitions=false)` (or `Func<Card,int>`).
- **Tap**: `Tap()` (self or Validator-matched) · `TapOwner()` (self only).
- **Sacrifice/remove**: `Sacrifice()` · `SacrificeThis()` (self) · `SacrificeTarget()` (chosen other, Validator-scoped) · `RemoveFromPlayOwnerCost()`.
- **Discard/reveal/return**: `DiscardThis()` · `DiscardTarget()` · `DiscardRandom()` · `Reveal()` · `RevealLimitBreakCards(level)` (Limit Break) · `ReturnToHand()` (self).
- **Counters**: `AddCountersCost(counterType, count?, hasX=false)` · `RemoveCounters(counterType, count?, hasX=false)`.
- **Composition**: `AggregateCost(params Cost[])` — e.g. `new AggregateCost(new Tap(), new SacrificeThis(), new SacrificeTarget())` (see Notsugo). At most one `PayMana` child; no nested `AggregateCost`. · `NoCost()`.
- **Cost modifiers** (applied via `AddCostModifier` game modifier, not as `p.Cost`): `ChangeManaCostOfSpellsOrAbilities(amount, (card, costType, mod) => bool)` (see Terra) · `ChangeManaCostOfEnchantedForwardsAbilities(amount)`.
- **X / repetition**: `hasX:true` enables `{X}`; `supportsRepetitions:true` lets a cost be paid multiple times to repeat the effect.

> Note: `SacrificeOwner` and `TapTargets` are **Effects** (`Core/Effects/`), not costs. The cost-side equivalents are `SacrificeThis`/`SacrificeTarget` and `TapOwner`.

---

## Targeting

`p.TargetSelector.AddEffect(trg => trg.Is.<predicate>.<On|In>.<zone>(), cfg => { ... })` defines a
target the effect acts on. `AddCost(...)` defines a target consumed to pay a cost (sacrifice/discard/
tap) — pair it with a matching cost object. `.On` and `.In` are identical aliases (readability only).

### `Is.` predicate builders (`Core/Targeting/IsValidTargetBuilder.cs`)
- `Forward(controlledBy=null, canTargetSelf=true)` — a Forward. · `ForwardWith(x => bool)` — e.g. `x => x.Power <= 9000`.
- `Monster()` · `Card(x => bool, controlledBy=null, canTargetSelf=true)` — **the general workhorse** (predicate on the `Card`).
- `Card(x => bool)` overload with full `IsValidTargetParameters` context.
- `Player()` · `Opponent()` · `ForwardOrPlayer()` — target players (no zone clause needed).
- `AttackerOrBlocker()` · `CounterableSpell(filter=null)` · `ValidEquipmentTarget()`.
- **There is no `Backup()`/`Summon()`/`Character()` builder** — express via `Card(x => x.Is().Backup)` etc. Predicate flags on `x.Is()`: `Forward, Backup, Monster, Summon, Character, BasicBackup, NonBasicBackup, Equipment, Artifact, Attachment, Aura, Legendary, Sorcery, Token, Party`. Also `x.IsTapped`, `x.Power`, `x.Name`, `x.HasJob("...")`, `x.Controller`.

### `ControlledBy` (`Core/Player/ControlledBy.cs`)
`SpellOwner` (= "you"), `Opponent`, `Any`. **No literal `You` — use `SpellOwner`.**

### Zone locators after `.On`/`.In` (`Core/Zones/IsValidZoneBuilder.cs`)
`Battlefield()` · `BattlefieldOrStack()` · `Stack()` · `BreakZone()` · `YourBreakZone()` · `OwnersHand()` · `OwnersLBDeck()`.

### Count / config (the optional second lambda `cfg => ...`, `TargetValidatorParameters`)
- `cfg.MinCount` / `cfg.MaxCount` (both default 1; auto-convert from int) · `cfg.Message = "..."`.
- `cfg.GetMinCount` / `cfg.GetMaxCount` (`Func<GetTargetCountParameters, Value>`) — dynamic counts.
- Idioms: exact N → `Min=Max=N`; **"up to N"** → `Min=0; Max=N`; **optional single** → `Min=0`. Example (Cecil): `cfg => cfg.MaxCount = 2`.
- `AddEffect` = effect targets (targetable). `AddCost` = cost targets (`MustBeTargetable` forced false).

---

## Enums

- **`Zone`** (`Core/Zones/Zone.cs`): `None, MainDeck, Hand, Battlefield` (the field), `BreakZone` (graveyard), `Stack, RemovedFromPlay` (exile), `DamageZone` (life pile), `LimitBreak` (LB deck).
- **`CardColor`** (`Core/Card/CardColor.cs`): `Fire, Ice, Wind, Lightning, Water, Earth, Light, Dark, Colorless, None`. The legacy `CardColors.All` contains only Light/Water/Dark/Fire/Wind; do not use it when all six standard Elements are intended.
- **`CounterType`** (`Core/Counter/CounterType.cs`): `Generic, Charge, Page, Fungus, PowerToughness, Verse, Petal, Soot, Fake, Infection, Arrow, Fuse, Growth, Gem, Loyality`.
- **`CostType`** (`Core/Costs/CostType.cs`): `Spell`, `Ability` (set by the system: Spell when casting, Ability when activating).
- **`EffectTag`** (`Core/AI/EffectTag.cs`) — AI hints via `.SetTags(...)`: `Generic, Destroy, DealDamage, Bounce, IncreaseToughness, IncreasePower, Shroud, Protection, ReduceToughness, CombatDisabler, Regenerate, CannotRegenerate, RemoveFromPlay, GainReach, ChangeController, Indestructible, Humble, ForwardsOnly`.
- **`Static`** (`Core/Card/Static.cs`) — keyword abilities via `.SimpleAbilities(...)`. Common values: `Brave, Haste, FirstStrike, MustBlockIfPossible`, `DoesNotUntap`; many other values exist. Use `rg` on the enum for the exact member name.
- **`Step`** (`Core/State/Step.cs`): `Untap, Upkeep, Draw, FirstMain, BeginningOfCombat, DeclareAttackers, DeclareBlocker, FirstStrikeCombatDamage, CombatDamage, EndOfCombat, SecondMain, EndOfTurn, CleanUp`. `step.IsMain()` = First||Second.
- **`ManaUsage`** (`[Flags]`): `None=0, Spells=1, Abilities=2, Any`. **`ManaColor`** is a class: singletons `Light(W) Water(U) Dark(B) Fire(R) Wind(G) Ice(I) Earth(Y) Lightning(P) Colorless(C) Any`; `IsCrystal` (Z); `FromCardColor(...)`.

---

## Mana / cost string syntax

Parser `Core/Mana/ManaParser.cs`; `"{...}".Parse()` → `ManaAmount`. String = sequence of `{...}`
tokens, case-insensitive:
- `{1}`,`{2}`,… — that many **generic/colorless** (numeric only; a literal `{C}` throws).
- `{W}`=Light `{U}`=Water `{B}`=Dark `{R}`=Fire `{G}`=Wind `{I}`=Ice `{Y}`=Earth `{P}`=Lightning.
- `{Z}` — the **Crystal** resource (`AddManaToPool("{Z}".Parse())`, `PayMana("{Z}".Parse())`),
  distinct from CP. Numeric tokens are the generic portion of a CP cost.
- `{WU}` etc. — hybrid. **Unknown symbols throw.** So `{T}`, `{S}`, `{EX BURST}` are
  **display-only** (`.Text(...)`) — model them with `new Tap()`, the same-name discard target built
  by `.SpecialAbility(...)`, and `p.ExBurst()`, respectively.
- `.Cost(cp, "R")` sugar → `{leftover}{colors…}`: each char → one `{c}`, `leftover = cp − colors.Length` prepended if > 0. `Cost(2,"R")`→`{1}{R}`; `Cost(3,"II")`→`{1}{I}{I}`; `Cost(2,"WU")`→`{W}{U}`; `Cost(1,"R")`→`{R}`.

---

## AI hint rules

Attached via `p.TargetingRule(...)` / `p.TimingRule(...)`. **AI heuristics only — never change legality
or game rules.** They tell the machine player which legal target to pick and whether/when to auto-play.
Add one matching your effect's intent so the AI plays the card sensibly.

- **TargetingRules** (`Core/AI/TargetingRules/`): `EffectDestroy`, `EffectDealDamage(n)`, `EffectGainControl` (see Garland), `EffectBounce`, `EffectTapForward`, `EffectRemoveFromPlayBattlefield`, `EffectCounterspell`, `EffectPumpSummon`, `EffectReduceToughness`, `EffectCombatMonster`, `EffectOpponent`/`EffectYou`/`EffectSelectPlayer`, `EffectCannotBlockAttack`, `EffectPreventNextDamageToTargets`, `EffectOrCostRankBy(score)`.
- **TimingRules** (`Core/AI/TimingRules/`): `OnFirstMain`/`OnSecondMain` (most common), `OnYourTurn`/`OnOpponentsTurn`/`OnEndOfOpponentsTurn`, `OnStep(Step)`, `WhenStackIsEmpty`/`WhenStackIsNotEmpty`, `WhenTopSpellIsCounterable`, `WhenYouHaveMana`, `TargetRemovalTimingRule(removalTag:)`, `MassRemovalTimingRule`, `PumpOwningCardTimingRule(p,t)`, `DefaultForwardsTimingRule`/`DefaultBackupsTimingRule` (auto-applied by card type). Cast-window helpers surface on `p.TimingRule` inside `.Cast`.

---

## Gaps & naming gotchas

- Treat this catalog as a navigation aid, not an API contract. Before using an unfamiliar entry,
  run `rg "class <Name>|<Name>\\(" engine/CrystalArena` and inspect its current constructor.
- **Filename ≠ class name** in several effect files: examples include `SearchMainDeckPutToZone`
  (file `SearchLibraryPutToZone.cs`), `CounterThatSpell` (file `CounterTopSpell.cs`), and
  `ApplyModifiersToPlayer` (file `ApplyModifiersToPlayers.cs`). Instantiate the **class** name.
- **Typos are baked into the public API**: `CastCardWithoutpaying…`, `…LoosesLife`, `TargetLoosesChosenAbility`, `PutDifferentAmountOf11Conters…`, `AddContiniousEffect`, `Loyality`. Match them exactly.
- **Current `.Damage(...)` bug:** `CardTemplate.Damage` never invokes its `set` callback. Use a
  `StaticAbility` with `cond.YouHaveLessThanXLife(7 - n)` until the helper is repaired.
- **Generic icon ≠ rarity:** the engine's `multiplayable: false` path uses an internal
  `"Legendary"` type label, but the printed same-name restriction is determined by the generic-card
  icon, never the serial's rarity suffix.
- **No general conditional/`if` effect** and **damage prevention is fragmented** (~9 near-duplicate `Prevent*`). If your card needs one of these and nothing composes cleanly, that's a candidate to *generalize* — follow SKILL.md → "When a building block is missing" (propose to the user before adding a core file).
- **Reveal-scaled** (`*ForEachRevealedCard`) and **"choose a color/zone" (`CustomizableEffect`)** are parallel families — reuse the closest sibling rather than writing a new one.
