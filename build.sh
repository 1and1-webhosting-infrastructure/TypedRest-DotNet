#!/bin/sh
set -e
cd `dirname $0`

echo "WARNING: You need Visual Studio on Windows to perform a full Release build"

dotnet msbuild -v:Quiet -t:Restore -t:Build -p:Configuration=DebugLinux -p:Version=${1:-1.0-dev}
