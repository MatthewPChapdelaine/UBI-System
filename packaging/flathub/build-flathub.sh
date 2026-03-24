#!/usr/bin/env bash

set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
MANIFEST="$ROOT_DIR/packaging/flathub/io.github.matthewpchapdelaine.ubi-system.yaml"
BUILD_DIR="$ROOT_DIR/packaging/flathub/build-dir"
REPO_DIR="$ROOT_DIR/packaging/flathub/repo"

if ! flatpak remotes --user --columns=name | grep -qx 'flathub'; then
  flatpak remote-add --if-not-exists --user flathub https://dl.flathub.org/repo/flathub.flatpakrepo
fi

flatpak-builder \
  --user \
  --install-deps-from=flathub \
  --force-clean \
  --repo="$REPO_DIR" \
  "$BUILD_DIR" \
  "$MANIFEST"
