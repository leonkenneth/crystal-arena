# Contributing to Crystal Arena

Thank you for your interest in contributing to Crystal Arena! This document provides guidelines and information for contributors.

## 🌟 Ways to Contribute

- **Add new cards**: Implement card definitions in the engine
- **Fix bugs**: Help identify and fix issues in the game logic or UI
- **Improve AI**: Enhance the AI decision-making algorithms
- **Documentation**: Improve or expand documentation
- **Testing**: Write tests for new features or improve test coverage
- **UI/UX**: Enhance the user interface and user experience

## 🚀 Getting Started

1. **Fork the repository** on GitHub
2. **Clone your fork** locally:
   ```bash
   git clone https://github.com/YOUR_USERNAME/crystal-arena.git
   cd crystal-arena
   ```
3. **Create a branch** for your changes:
   ```bash
   git checkout -b feature/your-feature-name
   ```
4. **Set up your development environment** (see README.md)
5. **Make your changes**
6. **Test your changes** thoroughly
7. **Commit your changes** with clear commit messages
8. **Push to your fork** and create a Pull Request

## 📝 Coding Standards

### C# (Engine)
- Follow standard C# naming conventions
- Use meaningful variable and method names
- Add XML documentation comments for public APIs
- Keep methods focused and single-purpose
- Write unit tests for new functionality

### TypeScript/JavaScript (Client & Image Proxy)
- Use TypeScript for type safety
- Follow the existing code style (ESLint configuration)
- Use functional components and React hooks
- Keep components small and reusable
- Add comments for complex logic

## 🃏 Adding New Cards

To add a new card to the game:

1. **Create a card file** in the appropriate directory:
   - FFTCG cards: `engine/CrystalArena/FFTCGCards/OpusXX/`
   - Test cards: `engine/CrystalArena/FFTCGCards/Test/`

2. **Implement the card class**:
   ```csharp
   namespace CrystalArena.CardsMainDeck
   {
     using System.Collections.Generic;
     
     public class YourCardName : CardTemplateSource
     {
       public override IEnumerable<CardTemplate> GetCards()
       {
         yield return Card
           .Named("Card Name")
           .ManaCost("{X}{Y}")
           .Type("Forward")
           .Power(7000)
           .Text("Card text here")
           .FlavorText("Flavor text here")
           // Add abilities, effects, etc.
       }
     }
   }
   ```

3. **Write tests** for the card:
   - Add test cases in `engine/CrystalArena.Tests/`
   - Test card abilities and interactions
   - Verify correct behavior in various scenarios

4. **Test manually** in the game client

## 🧪 Testing

### Running Tests

#### Engine Tests
```bash
cd engine
dotnet test
```

#### Client Tests
```bash
cd client
npm test
```

### Writing Tests
- Write tests for all new functionality
- Test edge cases and error conditions
- Ensure tests are deterministic and isolated
- Use descriptive test names that explain what is being tested

## 📋 Pull Request Process

1. **Ensure all tests pass** before submitting
2. **Update documentation** if you're changing functionality
3. **Describe your changes** clearly in the PR description:
   - What does this PR do?
   - Why is this change needed?
   - How has it been tested?
   - Any known issues or limitations?
4. **Link related issues** using keywords like "Fixes #123"
5. **Be responsive** to code review feedback
6. **Keep PRs focused** - one feature or fix per PR

## 🐛 Reporting Bugs

When reporting bugs, please include:
- **Description**: Clear description of the issue
- **Steps to reproduce**: Detailed steps to reproduce the bug
- **Expected behavior**: What should happen
- **Actual behavior**: What actually happens
- **Environment**: OS, browser version, etc.
- **Screenshots**: If applicable
- **Error logs**: Any relevant error messages

## 💡 Suggesting Features

We welcome feature suggestions! Please:
- Check if the feature has already been requested
- Provide a clear description of the feature
- Explain why it would be useful
- Consider implementation complexity
- Be open to discussion and feedback

## 🎯 Priority Areas

Current priority areas for contributions:
- **Card implementations**: Expanding card set coverage
- **Bug fixes**: Resolving known issues
- **Performance**: Optimizing AI and rendering
- **Documentation**: Improving guides and API docs
- **Testing**: Increasing test coverage

<!-- TODO: Update priority areas as project evolves -->

## 📜 Code of Conduct

<!-- TODO: Add code of conduct -->
- Be respectful and inclusive
- Provide constructive feedback
- Welcome newcomers
- Focus on the issue, not the person
- Assume good intentions

## ❓ Questions?

If you have questions about contributing:
- Open an issue on GitHub
- Check existing documentation
- Review previous discussions and PRs

## 📄 License

By contributing to Crystal Arena, you agree that your contributions will be licensed under the same license as the project.

---

Thank you for contributing to Crystal Arena! 🎮✨
