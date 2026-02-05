# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Funky is a .NET library providing `Func<>` extension methods, primarily for **memoization** and **thread-safe locking**. It targets net48, netstandard2.1, net5.0, and net6.0.

## Build Commands

```bash
# Build the solution
dotnet build Source/Funky.sln

# Run tests
dotnet test Source/Funky.sln

# Run tests for a specific test (use --filter)
dotnet test Source/Funky.sln --filter "FullyQualifiedName~FuncWith2InputsCanBeMemoized"

# Full CI pipeline via Cake (clean, build, test, pack)
dotnet tool restore
dotnet cake
```

## Git Workflow

- **master**: main/production branch (PR target)
- **develop**: integration branch
- Feature branches merge into `develop`

## Architecture

The library lives entirely in `Source/Funky/` with a single namespace `Funky`:

- **`FuncExtensions`** (static class) - The public API surface. Provides:
  - `InvokeWithWriteLock<TResult>()` - executes a `Func<TResult>` inside a `ReaderWriterLockSlim` write lock
  - `Memoize()` overloads for `Func<>` delegates with 1-16 parameters. Multi-parameter overloads use a **cast-by-example pattern** with anonymous types to reduce to the single-parameter case.

- **`Memoizers/`** - Internal memoization infrastructure:
  - `IMemoizeThings<TKey, TValue>` - interface with `GetOrAdd(TKey)`
  - `MemoizerBase<TKey, TValue, TCachedValue>` - abstract base using `Dictionary` + `ReaderWriterLockSlim` with upgradeable read locks for thread safety
  - `Memoizer<TKey, TValue>` - persistent cache (strong references)
  - `ExpirableMemoizer<TKey, TValue>` - GC-expirable cache using `WeakReference`

The `isExpirable` parameter on `Memoize()` selects between `Memoizer` (strong refs) and `ExpirableMemoizer` (weak refs).

## Test Framework

- **NUnit** with **FluentAssertions**
- Tests in `Source/Funky.UnitTests/`
- Test targets: net5.0, net6.0 only
- Tests verify thread-safety using `Parallel.For` loops
