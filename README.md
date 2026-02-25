# Crystal Arena

A Human vs Computer simulation engine for FFTCG rules.

The goal of this project is to learn how to implement such an engine. If more developed, it could offer a learning and basic playtesting environment for new players.

It's adapted from [magicgrove](https://github.com/pinky39/grove), but as an API + web client that runs on Linux and MacOS instead of being a Windows Desktop app.

# Screenshots and demo

![Screenshot](https://sh-misc.s3.fr-par.scw.cloud/crystal-arena/screen1.png)
[Live demo](https://crystal-arena.vercel.app/)

## Current State

- Very few cards implemented: a couple Opus 23 ones - and even then with quite a few bugs. [See list in codebase](https://github.com/leonkenneth/crystal-arena/tree/main/engine/CrystalArena/FFTCGCards)
- Lots of bugs and known issues (see [meta-issue](https://github.com/leonkenneth/crystal-arena/issues/32))
- Missing major mechanics
- Still a lot of cleaning pending from the MTG origin of the engine

## Contributing

Don't hesitate to open an issue to start a discussion.
Please check or comment on the [known issues and missing mechanics](https://github.com/leonkenneth/crystal-arena/issues/32) meta-issue though: there is _a lot_ missing or buggy.

Help is especially needed to implement cards and related mechanics.

Here is an example of an implementation for Weiss 23-021C:
- [Tests](https://github.com/leonkenneth/crystal-arena/blob/main/engine/CrystalArena.Tests/FFTCGCards/Opus23/Opus23_021C_Weiss.cs)
- [Implementation](https://github.com/leonkenneth/crystal-arena/blob/main/engine/CrystalArena/FFTCGCards/Opus23/Opus23_021C_Weiss.cs)

For new mechanics, don't hesitate to create [test cards](https://github.com/leonkenneth/crystal-arena/tree/main/engine/CrystalArena/FFTCGCards/Test).

## Running Locally

```bash
docker compose up
```

Then open http://localhost:3000.

## Architecture

- **client** — React / Three.JS frontend
- **engine** — C# / .NET game logic server
- **image_proxy** — Express service that generates placeholder card images

## Credits

- FINAL FANTASY is a registered trademark of Square Enix Holdings Co., Ltd. This is only a fan endeavor
- Engine is a modification of https://github.com/pinky39/grove?tab=readme-ov-file#how-ai-is-implemented
- Components are licensed separately — see each subdirectory's LICENSE file
