#!/bin/bash
set -e
set -o pipefail

dotnet build-server shutdown

dotnet build /gossip/app/src/Gossip.sln
dotnet build-server shutdown

dotnet test /gossip/app/tests/Integration/Gossip.IntegrationTests.csproj \
    --no-build -nodereuse:false \
    --logger:"trx;LogFileName=testresult.xml" \
    /p:UseSharedCompilation=false \
    /p:UseRazorBuildServer=false \
    /p:CollectCoverage=true \
    /p:CoverloetOutputFormat=opencover \
    /p:CoverletOutput="TestResults/opencover.xml" \
    /p:Exclude="[Gossip.TestSupport]*"

dotnet build-server shutdown
