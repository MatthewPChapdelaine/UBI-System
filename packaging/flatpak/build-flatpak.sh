#!/usr/bin/env bash

set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
APP_DIR="$ROOT_DIR/src/UBI.App"
PUBLISH_DIR="$ROOT_DIR/packaging/flatpak/publish"

rm -rf "$PUBLISH_DIR"
dotnet publish "$APP_DIR/UBI.App.csproj" -c Release -o "$PUBLISH_DIR"

echo "Published UBI app to $PUBLISH_DIR"
