# UBI System Itch.io Release Kit

This document packages the `itch.io` positioning, page copy, launch checklist, and pricing guidance for `UBI System`.

## Recommended positioning

Lead with the product as a desktop simulation and operations tool, not as a policy manifesto and not as an AI experiment.

- Core identity: desktop workflow and dashboard application
- Audience: simulation designers, civic-tech tinkerers, policy prototype builders, educators, and interface enthusiasts
- Primary value: explore managed UBI workflows through a local, interactive desktop app
- Best `itch.io` classification: `Tool`
- Best launch framing: niche simulation software or civic-tech prototype

## Suggested project metadata

- Project title: `UBI System`
- Subtitle: `A desktop dashboard for exploring managed UBI operations, organizational workflows, and intervention tracking.`
- Classification: `Tool`
- Platform:
  - `Linux`
- Tags:
  - `simulation`
  - `tool`
  - `dashboard`
  - `management`
  - `blazor`
  - `opensource`
  - `civic-tech`
  - `prototype`

If you later ship Windows builds, add them only after validating the packaging and screenshot flow there too.

## Store page copy

### Tagline

`A desktop operations prototype for exploring managed UBI workflows, dashboards, and intervention systems.`

### Short description

`UBI System is a Linux desktop application built with .NET and Blazor for testing local UBI operations, case workflows, company views, and intervention dashboards.`

### Full description

`UBI System` is a desktop-oriented application for exploring how a managed UBI platform might look and behave as real software.

Instead of presenting the idea as a design document alone, the project turns it into an interactive application with local persistence, workflow screens, operational dashboards, and native Linux packaging.

This makes it useful as a prototype environment for simulation, interface review, product concept validation, and discussion around system operations.

#### Included in the current build

- Overview, estate, presence, leadership, and operations screens
- Local JSON-backed persistence for company, employee, and intervention data
- Local API endpoints for company, workspace, employee, and intervention workflows
- Native Linux desktop packaging through Flatpak
- Existing screenshot assets suitable for a first `itch.io` release

#### Who it is for

- Interface and systems designers
- Policy-tech and civic-tech experimenters
- Educators demonstrating operational dashboard concepts
- Developers interested in Blazor-based desktop tooling

#### What makes this different

`UBI System` is not just a whitepaper or static mockup. It is a functioning local application with data flow, screens, persistence, and packaging work already in place.

That makes it a better fit for people who want to inspect an implementation direction, interact with a working interface, and evaluate the operational framing as software.

#### Scope note

This is best presented as a prototype or simulation environment, not as a real financial product or production deployment system.

That framing makes the page more credible and sets user expectations correctly.

## Recommended disclosure language

If you need to address AI concerns, keep the wording restrained:

`UBI System is a working desktop software prototype developed through direct implementation and iteration. Any AI-assisted work supported development tasks but does not replace the authored application logic, workflows, or packaging.`

## Existing visual assets to use

The repository already contains screenshots in `packaging/flatpak/screenshots/`:

- `overview.png`
- `estate.png`
- `leadership.png`
- `presence.png`

Use those immediately for the first `itch.io` page, then add an operations-focused screenshot if that view has improved since the existing capture pass.

Suggested captions:

- `Overview dashboard for system-wide visibility`
- `Estate view for organization and asset context`
- `Presence view for workforce and participation tracking`
- `Leadership dashboard for high-level operational review`

## Release checklist

### Product build

- From `src/UBI.App`, run `dotnet build`
- From `src/UBI.App`, run `dotnet run` and confirm the core screens load
- Verify local persistence still writes under `src/UBI.App/App_Data/`
- If shipping Flatpak instructions, run `./packaging/flatpak/build-flatpak.sh`
- Confirm the upload package includes clear start instructions

### Store assets

- Use the existing screenshot set as the base gallery
- Create a cover image focused on the cleanest dashboard view
- If possible, add one short clip scrolling through overview and operations screens
- Avoid policy-heavy poster art; the application UI should be the proof

### Page structure

- Lead with "desktop operations prototype" language
- Explain the app before the larger philosophy behind it
- Describe the current screens and local workflows concretely
- Add a scope note clarifying that it is a simulation/prototype environment
- Keep any AI disclosure small and secondary

### Packaging

- Prefer one clearly labeled Linux download first
- Use a filename such as `UBI-System-linux-x64.zip`
- Include a short `README` with launch steps and package contents
- If Flatpak is the main path, say so plainly on the page

### Launch follow-up

- Publish a devlog explaining the interface goals and current scope
- Ask users whether they are most interested in the dashboard UX, the prototype concept, or the technical stack
- Use early feedback to decide whether the next update should improve visuals, workflows, or cross-platform packaging

## Pricing strategy

Recommended launch model: `Free` or `Pay what you want`

Best default recommendation: `Pay what you want` with a suggested price of `$5`

Why:

- The app is more specialized than a general-purpose consumer tool
- Curious users may try it if free, while supporters can still signal interest
- A modest suggested price frames it as thoughtful prototype software rather than disposable content

If your main goal is maximum reach for feedback, launch free first and add optional donations later.

Do not position this as a premium enterprise product on `itch.io`; the platform responds better to clear, honest prototype pricing.

## Recommended launch sentence

`UBI System is a Linux desktop prototype for exploring managed UBI operations through dashboards, local workflows, and interactive system views.`

## Upload automation

The repository includes:

- `packaging/itchio/publish.sh` for repeatable `itch.io` Linux uploads
- `packaging/itchio/generate-page.py` for generating page-ready store copy and metadata files

Examples:

```bash
ITCH_IO_USER=yourname ITCH_IO_PROJECT=ubi-system \
  ./packaging/itchio/publish.sh --dry-run

ITCH_IO_USER=yourname ITCH_IO_PROJECT=ubi-system \
  ./packaging/itchio/publish.sh
```

This stages the existing publish payload, adds a local launcher script, and then pushes the result to the configured `itch.io` channel through `butler`.

Generate page files with:

```bash
./packaging/itchio/generate-page.py
```
