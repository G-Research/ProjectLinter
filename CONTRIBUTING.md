# Contributing to ProjectLinter

Thank you for your interest in contributing to ProjectLinter! This document provides guidelines and instructions for contributing to the project.

## Table of Contents
- [Code of Conduct](#code-of-conduct)
- [Getting Started](#getting-started)
- [How to Contribute](#how-to-contribute)
- [Code Quality Standards](#code-quality-standards)
- [Testing Guidelines](#testing-guidelines)
- [Community Involvement](#community-involvement)
- [Communication Guidelines](#communication-guidelines)
- [License](#license)

## Code of Conduct

This project and everyone participating in it is governed by our [Code of Conduct](CODE_OF_CONDUCT.md). By participating, you are expected to uphold this code.

## Getting Started

1. Fork the repository
2. Clone your fork: `git clone https://github.com/your-username/ProjectLinter.git`
3. Create a new branch for your changes: `git checkout -b feature/your-feature-name`
4. Set up the development environment:
   - Ensure you have .NET SDK installed (version specified in [global.json](global.json))
   - Run `dotnet restore` to restore dependencies
   - Run `dotnet build` to build the project
   - Run `dotnet test` to verify your setup

### Development Tools
- Visual Studio 2022 or Visual Studio Code with C# extensions
- .NET SDK (version specified in global.json)
- Git for version control

## How to Contribute

### Issues

Please report issues in the [Issues](https://github.com/G-Research/ProjectLinter/issues) section.

If you find a bug or a potential issue:

- Check the [Issues](https://github.com/G-Research/ProjectLinter/issues) to see if it has already been reported
- If it has not been reported, create a new issue
- Please provide: 
  - A clear and descriptive title
  - A detailed description of the problem
  - A code snippet reproducing the issue
  - Environment details (OS, .NET version, etc.)
  - Any other relevant information

### Pull Requests

Please create a pull request to contribute to the project. Follow these guidelines to ensure a smooth review process:

1. **Before Submitting**
   - Fork the repository
   - Create a new branch for your changes
   - Make your changes
   - Test your changes
   - Commit your changes
   - Push your changes to your fork

2. **PR Content**
   - Keep changes small and focused as practical
   - Use a clear and descriptive title
   - Provide a detailed description of the changes
   - Include code snippets demonstrating the changes when relevant
   - Use the PR template
   - Link to any related issues

3. **Review Process**
   - Address review comments promptly
   - Keep commits focused and atomic
   - Squash commits when requested
   - Maintain a clean commit history
   - Ensure CI checks pass

## Code Quality Standards

- Follow the existing code style and formatting
- Write clear, self-documenting code
- Add comments for complex logic
- Keep functions focused and single-purpose
- Use meaningful variable and function names
- Remove unused code and imports
- Handle errors appropriately
- Follow C# coding conventions
- Include XML documentation for public APIs
- Update documentation for user-facing changes

## Testing Guidelines

- Write unit tests for new features
- Ensure all tests pass before submitting PRs
- Maintain or improve test coverage
- Include integration tests for significant changes
- Test edge cases and error conditions
- Run the full test suite locally: `dotnet test`
- Include both positive and negative test cases
- Test against different .NET versions if applicable

## Community Involvement

We welcome all forms of community participation! Here's how you can get involved:

- Join discussions in GitHub Issues and Pull Requests
- Help review pull requests from other contributors
- Improve documentation
- Share your experience using ProjectLinter
- Help answer questions from other users
- Participate in feature planning and design discussions
- Report bugs and suggest improvements
- Help maintain the project's test suite

## Communication Guidelines

### Channels
- GitHub Issues: For bug reports and feature requests
- Pull Requests: For code changes and reviews
- Discussions: For general questions and community engagement

## License

This project is licensed under the [Apache 2.0 License](LICENSE).

Thank you for using and contributing to ProjectLinter!
