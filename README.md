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
- `packaging/flatpak/` - Flatpak manifest, desktop metadata, host shell, and screenshots
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

Build the publish payload and Flatpak bundle inputs:

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

## Release assets

Screenshots captured from the native Flatpak shell are stored in `packaging/flatpak/screenshots/`.
