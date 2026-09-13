#!/usr/bin/env bash
# Idempotent bootstrap for the DocumentsWeb dev environment.
# Installs the toolchains that are missing from the base image (only when
# absent), restores dependencies, builds the React SPA and seeds sample data.
set -euo pipefail

REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
DOTNET_ROOT="$HOME/.dotnet"
NVM_DIR="$HOME/.nvm"
NODE_MAJOR=16

# --- System libraries required by the .NET Core 3.1 runtime on Ubuntu 24.04 ---
# libicu provides globalization data; libssl1.1 provides the OpenSSL 1.1 ABI
# that 3.1 links against and which Ubuntu 24.04 no longer ships (it has 3.x).
if ! dpkg -s libicu74 >/dev/null 2>&1; then
  sudo apt-get update -qq
  sudo apt-get install -y -qq libicu74
fi
if ! dpkg -s libssl1.1 >/dev/null 2>&1; then
  deb="$(mktemp --suffix=.deb)"
  curl -fsSL -o "$deb" \
    "http://security.ubuntu.com/ubuntu/pool/main/o/openssl/libssl1.1_1.1.1f-1ubuntu2.24_amd64.deb"
  sudo dpkg -i "$deb"
  rm -f "$deb"
fi

# --- .NET Core 3.1 SDK (project targets netcoreapp3.1) ---
if [ ! -x "$DOTNET_ROOT/dotnet" ] || ! "$DOTNET_ROOT/dotnet" --list-sdks 2>/dev/null | grep -q '^3\.1\.'; then
  curl -fsSL https://dot.net/v1/dotnet-install.sh -o /tmp/dotnet-install.sh
  bash /tmp/dotnet-install.sh --channel 3.1 --install-dir "$DOTNET_ROOT"
fi

# --- Node 16 via nvm (react-scripts 3.x predates the Node 17+ OpenSSL change) ---
if [ ! -s "$NVM_DIR/nvm.sh" ]; then
  curl -fsSL https://raw.githubusercontent.com/nvm-sh/nvm/v0.40.1/install.sh | bash
fi
# shellcheck disable=SC1091
. "$NVM_DIR/nvm.sh"
nvm install "$NODE_MAJOR" >/dev/null
nvm alias default "$NODE_MAJOR" >/dev/null
NODE_BIN="$(dirname "$(nvm which "$NODE_MAJOR")")"

export PATH="$DOTNET_ROOT:$NODE_BIN:$PATH"
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
export DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1

# --- Restore backend, install SPA deps, build the SPA ---
cd "$REPO_ROOT/DocumentsWeb"
dotnet restore
(
  cd ClientApp
  npm install
  CI=false npm run build
)

# --- Seed sample documents the app parses (idempotent) ---
SAMPLE_DIR="$HOME/documents-web-sample"
mkdir -p "$SAMPLE_DIR"
cat > "$SAMPLE_DIR/Case-2021-001.txt" <<'EOF'
$$1 Header metadata
Court of Appeals
$$9 Decision
The court finds in favor of the plaintiff.
Damages are awarded in the amount of 5000.
$$0 End
EOF
cat > "$SAMPLE_DIR/Case-2021-002.txt" <<'EOF'
$$1 Header metadata
District Court
$$9 Decision
The appeal is dismissed for lack of standing.
Costs are assigned to the appellant.
$$0 End
EOF
cat > "$SAMPLE_DIR/Case-2022-014.txt" <<'EOF'
$$1 Header metadata
Supreme Court
$$9 Decision
Judgment reversed and remanded for further proceedings.
$$0 End
EOF
cat > "$SAMPLE_DIR/Notes-no-decision.txt" <<'EOF'
$$1 Just some notes
No decision section here.
EOF

echo "DocumentsWeb install complete."
