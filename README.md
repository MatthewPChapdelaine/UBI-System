# UBI System

UBI System is a .NET 8 Blazor desktop-oriented application for managed UBI operations. It turns the design documents in this repository into a working product with local persistence, operational workflows, dashboard views, and native Linux desktop packaging through Flatpak.

## Features

- Overview, estate, presence, leadership, and operations screens
- Local JSON-backed persistence for company, employee, and intervention data
- HTTP API endpoints for company, workspace, employees, and interventions
- Native GTK and WebKit Flatpak host for Linux desktop use
- Screenshot automation for release assets

## Project layout

- `src/UBI.App/` - Blazor application and local API
- `packaging/flatpak/` - Existing local-build Flatpak manifest, desktop metadata, host shell, and screenshots
- `packaging/flathub/` - Flathub-oriented source-build Flatpak manifest and metadata
- `Managed_UBI_System_Design.md` - system design reference
- `Managed_UBI_Implementation_Roadmap.md` - roadmap reference

## Local development

Build the application:

```bash
cd src/UBI.App
dotnet build
```

Run the application locally:

```bash
cd src/UBI.App
dotnet run
```

The app serves the dashboard locally and persists workflow data under `src/UBI.App/App_Data/`.

## Flatpak packaging

The repository now contains two Flatpak packaging tracks:

- `packaging/flatpak/` keeps the original local-build workflow that installs a pre-published payload from `packaging/flatpak/publish/`.
- `packaging/flathub/` contains a Flathub-oriented source-build manifest using the Flathub-compatible app ID `io.github.matthewpchapdelaine.ubi-system`.

Build the publish payload and Flatpak bundle inputs for the local packaging flow:

```bash
./packaging/flatpak/build-flatpak.sh
```

Validate the AppStream metadata:

```bash
appstreamcli validate packaging/flatpak/com.matthew.UBISystem.metainfo.xml
```

Build the Flatpak locally:

```bash
flatpak-builder --user --force-clean flatpak-build packaging/flatpak/com.matthew.UBISystem.yaml
flatpak-builder --user --install --force-clean flatpak-build packaging/flatpak/com.matthew.UBISystem.yaml
```

Validate the Flathub-oriented metadata:

```bash
appstreamcli validate packaging/flathub/io.github.matthewpchapdelaine.ubi-system.metainfo.xml
desktop-file-validate packaging/flathub/io.github.matthewpchapdelaine.ubi-system.desktop
```

Build the Flathub-oriented manifest locally:

```bash
./packaging/flathub/build-flathub.sh
```

When preparing the actual Flathub submission, move the manifest and adjacent files from `packaging/flathub/` into the top level of the Flathub submission repository, because Flathub requires the final submission manifest to live at repository root.

## Release assets

Screenshots captured from the native Flatpak shell are stored in `packaging/flatpak/screenshots/`.

## Itch.io upload automation

Use `packaging/itchio/publish.sh` to publish the Linux upload for an existing `itch.io` project through `butler`.

Required environment variables:

- `ITCH_IO_USER` - your `itch.io` account name
- `ITCH_IO_PROJECT` - the existing `itch.io` project slug

Optional environment variables:

- `ITCH_IO_CHANNEL` - overrides the default `linux` channel
- `BUTLER_API_KEY` - used by `butler` if you prefer token-based auth

Examples:

```bash
ITCH_IO_USER=yourname ITCH_IO_PROJECT=ubi-system \
  ./packaging/itchio/publish.sh --dry-run

ITCH_IO_USER=yourname ITCH_IO_PROJECT=ubi-system \
  ./packaging/itchio/publish.sh
```

The script reuses the existing Flatpak publish payload build, stages a launcher script for local use, and then pushes the staged directory with `butler`.
