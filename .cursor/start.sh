#!/usr/bin/env bash
# Runs the DocumentsWeb ASP.NET Core app, which also serves the built React SPA.
# Served in Production mode: ASP.NET serves ClientApp/build as static files with
# the API on the same origin. (Development mode auto-launches the CRA dev server,
# but the .NET Core 3.1 SpaServices proxy does not reliably return the SPA shell
# in this environment, so the pre-built SPA is served instead.)
set -euo pipefail

REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
DOTNET_ROOT="$HOME/.dotnet"
NVM_DIR="$HOME/.nvm"

# shellcheck disable=SC1091
[ -s "$NVM_DIR/nvm.sh" ] && . "$NVM_DIR/nvm.sh" >/dev/null 2>&1 || true
NODE_BIN="$(dirname "$(nvm which 16 2>/dev/null || echo /usr/bin/node)")"

export PATH="$DOTNET_ROOT:$NODE_BIN:$PATH"
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
export DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
export ASPNETCORE_ENVIRONMENT=Production
export ASPNETCORE_URLS="http://localhost:5000;https://localhost:5001"
export FileDirectory="$HOME/documents-web-sample"

cd "$REPO_ROOT/DocumentsWeb"
exec dotnet run --no-launch-profile
