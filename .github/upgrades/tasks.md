# Invensys.ExternalApi .NET 10.0 Upgrade Tasks

## Overview

This document tracks the execution of the Invensys.ExternalApi upgrade from .NET 8.0 to .NET 10.0. All 9 projects will be upgraded simultaneously in a single atomic operation, followed by comprehensive testing and validation.

**Progress**: 0/2 tasks complete (0%) ![0%](https://progress-bar.xyz/0)

---

## Tasks

### [▶] TASK-001: Atomic framework and package upgrade
**References**: Plan §Migration Strategy, Plan §Package Update Reference, Plan §Breaking Changes Catalog

- [✓] (1) Verify .NET 10 SDK installed
- [✓] (2) SDK version meets minimum requirements (**Verify**)
- [✓] (3) Check for global.json in repository root
- [✓] (4) Update or remove global.json version constraint if present to allow .NET 10
- [✓] (5) global.json compatible with .NET 10 SDK (**Verify**)
- [✓] (6) Update `<TargetFramework>net10.0</TargetFramework>` in all 9 project files per Plan §Detailed Dependency Analysis (Common, PaySpace.Entities, Odata, Sage300, Testing.Emulator, PaySpace, IntegraFlow, IntegrationTests, UnitTests)
- [✓] (7) All project files updated to net10.0 (**Verify**)
- [✓] (8) Update all package references per Plan §Package Update Reference: Microsoft.Extensions.Http (8.0.1 → 10.0.1), Microsoft.Extensions.Http.Polly (8.0.17 → 10.0.1), Microsoft.AspNetCore.Mvc.Testing (8.0.7 → 10.0.1), Microsoft.Extensions.DependencyInjection (8.0.1 → 10.0.1)
- [✓] (9) All package references updated (**Verify**)
- [✓] (10) Restore all dependencies using `dotnet restore`
- [▶] (11) All dependencies restored successfully (**Verify**)
- [✓] (12) Build solution using `dotnet build` and fix all compilation errors per Plan §Breaking Changes Catalog (focus on ASP.NET Core test host changes, DI service resolution, Polly/HttpClientFactory patterns)
- [▶] (13) Solution builds with 0 errors (**Verify**)
- [ ] (14) Commit changes with message: "TASK-001: Atomic upgrade to .NET 10.0"

---

### [ ] TASK-002: Run full test suite and validate upgrade
**References**: Plan §Testing Strategy, Plan §Breaking Changes Catalog

- [ ] (1) Run unit tests in tests\Invensys.ExternalApi.UnitTests and integration tests in tests\Invensys.ExternalApi.IntegrationTests projects
- [ ] (2) Fix any test failures caused by framework/package changes (reference Plan §Breaking Changes Catalog for common issues: test host lifecycle, DI behavior, Polly policy registration)
- [ ] (3) Re-run all tests after fixes
- [ ] (4) All tests pass with 0 failures (**Verify**)
- [ ] (5) Commit test fixes with message: "TASK-002: Complete testing and validation"

---



















