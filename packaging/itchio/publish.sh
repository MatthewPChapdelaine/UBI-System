#!/usr/bin/env bash

set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
OUT_DIR="$ROOT_DIR/packaging/itchio/out/linux"
PUBLISH_DIR="$ROOT_DIR/packaging/flatpak/publish"

usage() {
    cat <<'EOF'
Usage: ./packaging/itchio/publish.sh [--dry-run]

Environment:
  ITCH_IO_USER     Required itch.io account name
  ITCH_IO_PROJECT  Required itch.io project slug
  ITCH_IO_CHANNEL  Optional channel override (defaults to linux)
  BUTLER_API_KEY   Optional butler auth token
EOF
}

require_command() {
    if ! command -v "$1" >/dev/null 2>&1; then
        echo "Required command not found: $1" >&2
        exit 1
    fi
}

DRY_RUN=0

for arg in "$@"; do
    case "$arg" in
        --dry-run)
            DRY_RUN=1
            ;;
        -h|--help)
            usage
            exit 0
            ;;
        *)
            echo "Unknown argument: $arg" >&2
            usage
            exit 1
            ;;
    esac
done

: "${ITCH_IO_USER:?Set ITCH_IO_USER to your itch.io username.}"
: "${ITCH_IO_PROJECT:?Set ITCH_IO_PROJECT to your itch.io project slug.}"

require_command dotnet
require_command git

CHANNEL="${ITCH_IO_CHANNEL:-linux}"
GIT_VERSION="$(git -C "$ROOT_DIR" describe --tags --always --dirty)"
DESTINATION="${ITCH_IO_USER}/${ITCH_IO_PROJECT}:${CHANNEL}"

"$ROOT_DIR/packaging/flatpak/build-flatpak.sh"

if [ ! -x "$PUBLISH_DIR/UBI.App" ]; then
    echo "Expected published binary not found at $PUBLISH_DIR/UBI.App" >&2
    exit 1
fi

rm -rf "$OUT_DIR"
mkdir -p "$OUT_DIR"
cp -R "$PUBLISH_DIR/." "$OUT_DIR/"

cat >"$OUT_DIR/run-ubi-system.sh" <<'EOF'
#!/usr/bin/env bash

set -euo pipefail

APP_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
APP_BIN="$APP_DIR/UBI.App"
PORT="${UBI_APP_PORT:-18085}"
URL="http://127.0.0.1:${PORT}"

cleanup() {
    kill "$server_pid" 2>/dev/null || true
}

if [ ! -x "$APP_BIN" ]; then
    echo "UBI application binary not found: $APP_BIN" >&2
    exit 1
fi

trap cleanup EXIT INT TERM
"$APP_BIN" --urls "$URL" --contentRoot "$APP_DIR" --webroot "$APP_DIR/wwwroot" &
server_pid=$!

sleep 2
if ! xdg-open "$URL" >/dev/null 2>&1; then
    echo "Open $URL in your browser."
fi

wait "$server_pid"
EOF
chmod +x "$OUT_DIR/run-ubi-system.sh"

cat >"$OUT_DIR/README.txt" <<'EOF'
UBI System itch.io package

To launch the local dashboard:

1. Extract the archive.
2. Run ./run-ubi-system.sh
3. If your browser does not open automatically, visit http://127.0.0.1:18085

Stop the app with Ctrl+C in the terminal where it is running.
EOF

cat >"$OUT_DIR/build-info.txt" <<EOF
project=UBI System
target=linux
version=$GIT_VERSION
source_repo=$(git -C "$ROOT_DIR" remote get-url origin)
EOF

echo "Staged itch.io artifact in $OUT_DIR"
echo "Destination: $DESTINATION"
echo "User version: $GIT_VERSION"

if [ "$DRY_RUN" -eq 1 ]; then
    echo "Dry run enabled; skipping butler push."
    exit 0
fi

require_command butler
butler push "$OUT_DIR" "$DESTINATION" --userversion "$GIT_VERSION"
