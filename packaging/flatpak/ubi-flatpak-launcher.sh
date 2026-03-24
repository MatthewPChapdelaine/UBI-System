#!/bin/sh

set -eu

APP_DIR="/app/share/ubi-app"
APP_BIN="$APP_DIR/UBI.App"
PORT="${UBI_APP_PORT:-18085}"
URL="http://127.0.0.1:${PORT}"

if [ ! -x "$APP_BIN" ]; then
    echo "UBI application binary not found: $APP_BIN" >&2
    exit 1
fi

cd "$APP_DIR"
"$APP_BIN" --urls "$URL" --contentRoot "$APP_DIR" --webroot "$APP_DIR/wwwroot" &
server_pid=$!

cleanup() {
    kill "$server_pid" 2>/dev/null || true
}

trap cleanup EXIT INT TERM

sleep 2
xdg-open "$URL" >/dev/null 2>&1 || true

wait "$server_pid"
