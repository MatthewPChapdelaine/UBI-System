#!/usr/bin/env bash

set -euo pipefail

APP_ID="com.matthew.UBISystem"
OUT_DIR="${1:-$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)/screenshots}"

mkdir -p "$OUT_DIR"

capture_screen() {
    local route="$1"
    local name="$2"
    local log_file="$OUT_DIR/${name}.log"
    local output_file="$OUT_DIR/${name}.png"

    flatpak kill "$APP_ID" 2>/dev/null || true
    sleep 1

    flatpak run --env=UBI_APP_START_PATH="$route" "$APP_ID" >"$log_file" 2>&1 &
    local app_pid=$!

    local window_id=""
    for _ in $(seq 1 60); do
        window_id="$(xdotool search --onlyvisible --name 'UBI System' 2>/dev/null | tail -n1 || true)"
        if [[ -n "$window_id" ]]; then
            break
        fi
        sleep 0.5
    done

    if [[ -z "$window_id" ]]; then
        echo "Could not find UBI System window for route $route" >&2
        kill "$app_pid" 2>/dev/null || true
        wait "$app_pid" 2>/dev/null || true
        return 1
    fi

    xdotool windowactivate "$window_id" 2>/dev/null || true
    sleep 3
    import -window "$window_id" "$output_file"
    xdotool windowclose "$window_id" 2>/dev/null || true
    sleep 1
    flatpak kill "$APP_ID" 2>/dev/null || true
    kill "$app_pid" 2>/dev/null || true
    wait "$app_pid" 2>/dev/null || true
}

capture_screen "/" "overview"
capture_screen "/estate" "estate"
capture_screen "/presence" "presence"
capture_screen "/leadership" "leadership"
