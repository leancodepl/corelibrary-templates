#!/usr/bin/env bash

export Domains__Public='local.lncd.pl'
export Domains__ApiInternal='api.local.lncd.pl:8080'

export Logging__EnableDetailedInternalLogs=true
export Logging__MinimumLevel=Verbose
export Logging__SeqEndpoint=http://seq.local.lncd.pl:5341

export Telemetry__ZipkinEndpoint='http://zipkin:9411/api/v2/spans'

export SqlServer__ConnectionString='Server=db.local.lncd.pl,1433;Database=App;User Id=sa;Password=Passw12#'
export BlobStorage__ConnectionString='DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://storage.local.lncd.pl:10000/devstoreaccount1;'

DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
if [[ -f "$DIR/secrets.sh" ]]; then
    source "$DIR/secrets.sh"
fi
