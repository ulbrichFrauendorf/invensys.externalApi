# .github/upgrades/plan.md

Table of contents
- Executive Summary
- Migration Strategy
- Detailed Dependency Analysis
- Project-by-Project Plans
- Package Update Reference
- Breaking Changes Catalog
- Testing Strategy
- Risk Management
- Complexity & Effort Assessment
- Source Control Strategy
- Success Criteria
- Appendices


## Executive Summary
### Selected Strategy
**All-At-Once Strategy** - All projects upgraded simultaneously in a single atomic operation.

**Rationale**:
- 9 projects (small/medium solution)
- All projects currently target `net8.0` and are SDK-style
- Total LOC ~7,983 — manageable
- Dependency structure is clear and shallow (depth ? 3)
- Assessment shows 4 NuGet packages require updates and no binary/source API incompatibilities were detected
- Security-related package updates are included in this upgrade

**Scope**: Upgrade all projects to `net10.0` and apply all package updates listed in §Package Update Reference in a single coordinated operation.

**Repo state**:
- Source branch: `develop`
- Upgrade branch: `upgrade-to-NET10` (will be created from `develop`)
- Pending changes: none


## Migration Strategy
- Approach: All projects simultaneously (atomic upgrade).
- Key principle: perform TargetFramework and NuGet package updates across all projects in one pass, restore, build and fix all compilation issues in the same operation.
- Prerequisites executed first: ensure .NET 10 SDK available and `global.json` (if present) adapted.
- Testing: run unit and integration tests after atomic upgrade completes.

Implementation phases (for human understanding):
- Phase 0 — Preparation: validate SDK, create upgrade branch, capture baseline build/tests
- Phase 1 — Atomic Upgrade (single coordinated operation): update all project TargetFramework values, update all package references per §Package Update Reference, restore dependencies, build solution and fix compilation errors
- Phase 2 — Test Validation: run unit and integration tests, address test failures, finalize

Atomic upgrade task structure (to be executed as a single atomic operation):

TASK-001: Atomic framework and package upgrade
- (1) Ensure prerequisites: .NET 10 SDK available; handle `global.json` if present
- (2) Update all project files: set `<TargetFramework>net10.0</TargetFramework>` for all projects listed in §Detailed Dependency Analysis
- (3) Update all package references: apply versions from §Package Update Reference across affected projects
- (4) Restore dependencies (`dotnet restore`)
- (5) Build solution and fix all compilation errors introduced by framework/package updates
- (6) Verify: solution builds with 0 errors

TASK-002: Test execution and validation
- (1) Run unit tests for `Invensys.ExternalApi.UnitTests`
- (2) Run integration tests for `Invensys.ExternalApi.IntegrationTests`
- (3) Address failing tests and document fixes
- (4) Verify: all tests pass or failing tests have remediation plans

Note: Per All-At-Once strategy rules, TASK-001 combines all project and package updates and the initial build/fix pass into a single atomic task.


## Detailed Dependency Analysis
Summary:
- Total projects: 9 (all SDK-style)
- Projects form a shallow dependency graph; several libraries are shared (`Invensys.ExternalApi.Common` is a common leaf dependency)
- No circular dependencies detected in assessment

Topological order (leaf ? root) from repository analysis:
1. `src\Invensys.ExternalApi.Common\Invensys.ExternalApi.Common.csproj`
2. `src\Invensys.ExternalApi.PaySpace.Entities\Invensys.ExternalApi.PaySpace.Entities.csproj`
3. `src\Invensys.ExternalApi.Odata\Invensys.ExternalApi.Odata.csproj`
4. `src\Invensys.ExternalApi.Sage300\Invensys.ExternalApi.Sage300.csproj`
5. `Testing.Emulator\Testing.Emulator.csproj`
6. `src\Invensys.ExternalApi.PaySpace\Invensys.ExternalApi.PaySpace.csproj`
7. `Invensys.ExternalApi.IntegraFlow\Invensys.ExternalApi.IntegraFlow.csproj`
8. `tests\Invensys.ExternalApi.IntegrationTests\Invensys.ExternalApi.IntegrationTests.csproj`
9. `tests\Invensys.ExternalApi.UnitTests\Invensys.ExternalApi.UnitTests.csproj`

Phase groupings (for planning clarity; upgrade will be atomic):
- Phase A (core libraries): `Common`, `PaySpace.Entities`, `Odata`
- Phase B (supporting libraries): `Sage300`, `IntegraFlow`, `Testing.Emulator`
- Phase C (applications & tests): `PaySpace`, `IntegrationTests`, `UnitTests`

Critical path:
- `Common` is a central dependency; ensure its package updates and target framework change are included in the atomic upgrade since many projects depend on it.

Special cases:
- Test projects depend on multiple updated projects; they must be validated after the atomic upgrade completes.


## Project-by-Project Plans
For each project: current/target frameworks, package updates affecting that project, and validation checklist.
 
 ### All projects (applies to each project below)
 - Prerequisites: .NET 10 SDK installed, working `global.json` or update if necessary
 - Project file changes: set `<TargetFramework>net10.0</TargetFramework>` (if project was multi-targeting, append `net10.0` rather than replacing unless assessment recommends otherwise)
 - Package changes: apply package version updates listed in §Package Update Reference
 - Build & fix: restore and build solution; resolve compilation errors caused by framework or package changes
 - Validation: project builds with 0 errors; unit tests (if present) pass
 
 ---
 ### Project: `Invensys.ExternalApi.Common`
 - Current: `net8.0` ? `net10.0`
 - Packages to update: None
 - Risk: Low
 - Actions (for executor):
   1. Update `<TargetFramework>` to `net10.0`
   2. Restore and build to verify no issues (there are no package updates)
   3. Validate: ensure any consuming projects still build and function correctly
 
 ---
 ### Project: `Invensys.ExternalApi.PaySpace.Entities`
 - Current: `net8.0` ? `net10.0`
 - Packages to update:
   - `Invensys.ExternalApi.Common` 8.0.1 ? 10.0.1
 - Risk: Low
 - Actions (for executor):
   1. Update `<TargetFramework>` to `net10.0`
   2. Update `PackageReference` for `Invensys.ExternalApi.Common`
   3. Restore, build and validate no issues
 
 ---
 ### Project: `Invensys.ExternalApi.Odata`
 - Current: `net8.0` ? `net10.0`
 - Packages to update:
   - `Invensys.ExternalApi.Common` 8.0.1 ? 10.0.1
 - Risk: Low
 - Actions (for executor):
   1. Update `<TargetFramework>` to `net10.0`
   2. Update `PackageReference` for `Invensys.ExternalApi.Common`
   3. Restore, build and validate no issues
 
 ---
 ### Project: `Invensys.ExternalApi.Sage300`
 - Current: `net8.0` ? `net10.0`
 - Packages to update:
   - `Invensys.ExternalApi.Common` 8.0.1 ? 10.0.1
 - Risk: Low
 - Actions (for executor):
   1. Update `<TargetFramework>` to `net10.0`
   2. Update `PackageReference` for `Invensys.ExternalApi.Common`
   3. Restore, build and validate no issues
 
 ---
 ### Project: `Testing.Emulator`
 - Current: `net8.0` ? `net10.0`
 - Packages to update:
   - `Invensys.ExternalApi.Common` 8.0.1 ? 10.0.1
 - Risk: Low
 - Actions (for executor):
   1. Update `<TargetFramework>` to `net10.0`
   2. Update `PackageReference` for `Invensys.ExternalApi.Common`
   3. Restore, build and validate no issues
 
 ---
 ### Project: `Invensys.ExternalApi.PaySpace`
 - Current: `net8.0` ? `net10.0`
 - Packages to update:
   - `Invensys.ExternalApi.Common` 8.0.1 ? 10.0.1
 - Risk: Low
 - Actions (for executor):
   1. Update `<TargetFramework>` to `net10.0`
   2. Update `PackageReference` for `Invensys.ExternalApi.Common`
   3. Restore, build and validate no issues
 
 ---
 ### Project: `Invensys.ExternalApi.IntegraFlow`
 - Current: `net8.0` ? `net10.0`
 - Packages to update:
   - `Microsoft.Extensions.Http` 8.0.1 ? 10.0.1
   - `Microsoft.Extensions.Http.Polly` 8.0.17 ? 10.0.1
 - Risk: Low ? Medium (Polly alignment)
 - Actions (for executor):
   1. Update `<TargetFramework>` to `net10.0`
   2. Update `PackageReference` entries for packages listed above to target versions
   3. Restore and build; fix compile issues and adjust Polly/HttpClient registration usage if necessary
   4. Validate unit tests referencing this project pass
 
 ---
 ### Project: `tests\Invensys.ExternalApi.IntegrationTests`
 - Current: `net8.0` ? `net10.0`
 - Packages to update:
   - `Invensys.ExternalApi.PaySpace` 8.0.1 ? 10.0.1
   - `Invensys.ExternalApi.Odata` 8.0.1 ? 10.0.1
   - `Invensys.ExternalApi.Sage300` 8.0.1 ? 10.0.1
   - `Invensys.ExternalApi.Common` 8.0.1 ? 10.0.1
 - Risk: Medium (multiple dependencies)
 - Actions (for executor):
   1. Update `<TargetFramework>` to `net10.0`
   2. Update all `PackageReference` entries for updated packages
   3. Restore and build; fix any compilation issues
   4. Run integration tests and address any failures
 
 ---
 ### Project: `tests\Invensys.ExternalApi.UnitTests`
 - Current: `net8.0` ? `net10.0`
 - Packages to update:
   - `Microsoft.Extensions.Http` 8.0.1 ? 10.0.1
 - Risk: Low
 - Actions (for executor):
   1. Update `<TargetFramework>` to `net10.0`
   2. Update `PackageReference` for `Microsoft.Extensions.Http`
   3. Restore, build and run unit tests; fix test compatibility issues
 

## Package Update Reference
Group updates by scope with exact versions from assessment.
 
 ### Common package updates (affecting multiple projects)
 - `Microsoft.Extensions.Http` 8.0.1 ? 10.0.1 — Projects affected: `Invensys.ExternalApi.Common`, `Invensys.ExternalApi.IntegraFlow`, `Invensys.ExternalApi.UnitTests`
 - `Microsoft.Extensions.Http.Polly` 8.0.17 ? 10.0.1 — Projects affected: `Invensys.ExternalApi.IntegraFlow`, `Invensys.ExternalApi.PaySpace`, `Invensys.ExternalApi.Sage300`
 
 ### Test-related updates
 - `Microsoft.AspNetCore.Mvc.Testing` 8.0.7 ? 10.0.1 — Projects affected: `Invensys.ExternalApi.IntegrationTests`
 - `Microsoft.Extensions.DependencyInjection` 8.0.1 ? 10.0.1 — Projects affected: `Invensys.ExternalApi.PaySpace`
 
 ### All package updates to apply (consolidated)
 - `Microsoft.Extensions.Http`: 8.0.1 ? 10.0.1
 - `Microsoft.Extensions.Http.Polly`: 8.0.17 ? 10.0.1
 - `Microsoft.AspNetCore.Mvc.Testing`: 8.0.7 ? 10.0.1
 - `Microsoft.Extensions.DependencyInjection`: 8.0.1 ? 10.0.1
 
 > Note: Leave packages marked as "Compatible" unchanged unless security policy requires latest patch.


## Breaking Changes Catalog
Assessment reports no explicit binary or source incompatible API issues. Expected breaking-change categories to validate during build and test:
 - ASP.NET Core hosting/test changes affecting `Microsoft.AspNetCore.Mvc.Testing` usage and test server lifecycles
 - DependencyInjection package updates may alter service resolution behavior in edge cases — validate service registrations
 - Polly + HttpClientFactory: extension method signatures or registration patterns may shift; validate resilience policies
 - Test SDK / runner changes: confirm test discovery/adapters continue to work with updated SDK
 
For each compilation error found after upgrade, map the error to one of these categories and record fix steps in execution logs.


## Testing Strategy
 - Per-project validation: each project must build without errors after upgrade
 - Test execution (Phase 2): run all unit and integration tests
   - Unit tests: `Invensys.ExternalApi.UnitTests`
   - Integration tests: `Invensys.ExternalApi.IntegrationTests` (depends on `Testing.Emulator`)
 - Validation checklist for tests:
   - All unit tests pass
   - Integration tests pass or failures are triaged and attributed to framework/package changes
 - Automated tests owned by CI should be executed once PR is created
 
Test failure triage guidance:
 - If tests fail due to API changes in packages, prioritize remediation in code or adjust package bindings where safe
 - If test failures are environment/timing related (test host lifecycle), adjust test host setup in test project


## Risk Management
 - Overall solution risk: Low ? Medium due to multiple package bumps with potential minor API changes
 - High-attention items:
   - `Invensys.ExternalApi.PaySpace` (DI + Polly updates) — mitigation: review service registrations and HttpClient/Polly policies
   - `Invensys.ExternalApi.IntegrationTests` (test host updates) — mitigation: review test host bootstrap and test server usage
 
Contingency:
 - If critical blocking break occurs: revert upgrade branch and open investigation branch; keep `develop` unchanged
 - Keep a single atomic commit for the upgrade to simplify reverts if necessary


## Complexity & Effort Assessment
 - Project complexity ratings (relative):
   - Low: `Common`, `Odata`, `PaySpace.Entities`, `Sage300`, `Testing.Emulator`
   - Medium: `PaySpace`, `IntegraFlow`, `IntegrationTests`, `UnitTests`
 
Notes: No LOC time estimates provided. Complexity is used only for planning and resource allocation.


## Source Control Strategy
 - Create branch `upgrade-to-NET10` from `develop` and switch to it before any changes.
 - Single atomic upgrade commit is preferred: apply all project file TargetFramework changes and package version updates in one commit.
 - Create PR from `upgrade-to-NET10` to `develop` with detailed description linking to `.github/upgrades/plan.md` and `assessment.md`.
 - PR requirements: build must succeed and tests must pass (or clearly documented failing tests with remediation plan).


## Success Criteria
The migration is complete when:
1. All projects target `net10.0` (TargetFramework updated in project files)
2. All package updates from §Package Update Reference have been applied
3. Solution builds with 0 errors
4. All unit and integration tests pass (or failing tests are documented with remediation plan)
5. No outstanding known security vulnerabilities remain in the updated dependency set


## Appendices
 - Assessment reference: `.github/upgrades/assessment.md`
 - Branches: source `develop`, upgrade branch `upgrade-to-NET10`
 - Recommended executor checklist (for implementer):
   1. Ensure .NET 10 SDK is installed and `global.json` updated if present
   2. Create and switch to `upgrade-to-NET10`
   3. Update `<TargetFramework>` to `net10.0` in all project files
   4. Update PackageReference versions per §Package Update Reference
   5. `dotnet restore` and `dotnet build` the solution
   6. Fix compilation errors caused by new framework/package versions
   7. Run unit and integration tests
   8. Commit changes and open PR to `develop`
