# 💎 Crystal Arena

An open-source Final Fantasy Trading Card Game (FFTCG) simulation engine and web application for learning, testing strategies, and playing against AI.

## What is Crystal Arena?

Crystal Arena is a digital implementation of the Final Fantasy Trading Card Game that allows you to:
- Play FFTCG against an AI opponent
- Test deck strategies and card interactions
- Learn the game mechanics in a digital environment
- Simulate game scenarios for educational purposes

The project features a modern web interface built with Next.js, a robust game engine written in C#/.NET, and supports real-time gameplay with card rendering.

## 🎮 Try It

<!-- TODO: Add link to hosted demo once available -->
To run Crystal Arena locally:

```bash
# Clone the repository
git clone https://github.com/leonkenneth/crystal-arena.git
cd crystal-arena

# Run with Docker Compose (recommended)
docker-compose up

# The application will be available at:
# - Client (Web UI): http://localhost:3000
# - Engine (API): http://localhost:5001
# - Image Proxy: http://localhost:4000
```

## 🏗️ Architecture & Components

Crystal Arena consists of three main components:

### 1. Game Engine (`/engine`)
- **Language**: C# / .NET 8
- **Purpose**: Core game logic, rules engine, and AI
- **Features**:
  - Complete FFTCG rules implementation
  - AI opponent using search-based decision making
  - Game state management and validation
  - Card effect processing
  - REST API for client communication

The engine is a **fork of Grove**, a Magic: The Gathering engine, adapted for Final Fantasy TCG. Credit to the original Grove project for providing the foundation for the rules engine architecture.

### 2. Web Client (`/client`)
- **Framework**: Next.js 15 with React 19
- **Purpose**: User interface and game visualization
- **Features**:
  - Modern, responsive UI built with Chakra UI
  - Real-time game state updates
  - Card rendering and battlefield visualization
  - Deck management
  - Game controls and interactions

### 3. Image Proxy (`/image_proxy`)
- **Framework**: Express.js with TypeScript
- **Purpose**: Card image fetching and caching
- **Features**:
  - Proxies requests to card image sources
  - Image optimization and caching
  - Handles missing images gracefully

## 🚀 Getting Started

### Prerequisites
- Docker and Docker Compose (recommended), OR:
- .NET 8 SDK (for engine development)
- Node.js 20+ (for client/proxy development)

### Quick Start with Docker
```bash
docker-compose up
```

### Manual Setup

#### Engine
```bash
cd engine
dotnet restore
dotnet run --project CrystalArena
```

#### Client
```bash
cd client
npm install
npm run dev
```

#### Image Proxy
```bash
cd image_proxy
npm install
npm run dev
```

## 📚 Documentation

<!-- TODO: Add links to additional documentation -->
- Game Rules: See the official FFTCG rules
- API Documentation: <!-- TODO: Add API docs link -->
- Development Guide: See CONTRIBUTING.md

## ❓ FAQ

### Is this affiliated with Square Enix?
No, Crystal Arena is an independent fan project. FINAL FANTASY, SQUARE ENIX and the SQUARE ENIX logo are trademarks or registered trademarks of Square Enix Holdings Co., Ltd.

### Can I play against other players?
Currently, Crystal Arena only supports playing against AI opponents. Multiplayer functionality may be added in the future.

### What card sets are supported?
<!-- TODO: Document supported card sets -->
The engine currently supports cards from Opus 23 and test cards. Additional card sets are being added progressively.

### How does the AI work?
The AI uses a search-based decision-making system that evaluates possible moves and their outcomes to select the best action. It's based on game tree search algorithms adapted from the Grove engine.

### Can I contribute my own cards?
Yes! See the CONTRIBUTING.md file for guidelines on adding new cards to the engine.

## 🐛 Known Issues

- Card image loading may be slow on first access
- Some complex card interactions may not be fully implemented
- AI decision-making can be slow for complex board states
- Limited card set coverage (actively being expanded)
<!-- TODO: Add more specific known issues as they are identified -->

## 🤝 Contributing

We welcome contributions! Please see [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines on how to contribute to the project.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Credits

- **Grove Engine**: Crystal Arena's game engine is forked from Grove, a Magic: The Gathering engine. The original Grove project provided the foundational architecture for rules processing and game state management.
- **Square Enix**: For creating the Final Fantasy Trading Card Game
- **Contributors**: All the developers who have contributed to this project

## ⚖️ Legal Disclaimer

This is an unofficial fan project and is not affiliated with, endorsed, sponsored, or specifically approved by Square Enix. FINAL FANTASY, SQUARE ENIX and the SQUARE ENIX logo are trademarks or registered trademarks of Square Enix Holdings Co., Ltd. All card text, images, and game mechanics are property of their respective owners.

This project is for educational and entertainment purposes only.
