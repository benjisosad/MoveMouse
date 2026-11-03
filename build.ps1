[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$compiler = Join-Path $env:WINDIR 'Microsoft.NET\Framework64\v4.0.30319\csc.exe'
if (-not (Test-Path -LiteralPath $compiler -PathType Leaf)) {
    throw 'No se encontro el compilador de .NET Framework de 64 bits. Se requiere Windows de 64 bits con .NET Framework 4.x.'
}

$outputDirectory = Join-Path $PSScriptRoot 'dist'
$executable = Join-Path $outputDirectory 'MoveMouse.exe'
New-Item -ItemType Directory -Path $outputDirectory -Force | Out-Null

$compilerArguments = @(
    '/nologo'
    '/target:winexe'
    '/platform:anycpu'
    '/optimize+'
    '/warnaserror+'
    "/out:$executable"
    '/reference:System.Windows.Forms.dll'
    '/reference:System.Drawing.dll'
    (Join-Path $PSScriptRoot 'AssemblyInfo.cs')
    (Join-Path $PSScriptRoot 'MoveMouse.cs')
)

& $compiler @compilerArguments
if ($LASTEXITCODE -ne 0) {
    throw "La compilacion fallo con codigo $LASTEXITCODE."
}

Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'LEEME.txt') -Destination $outputDirectory -Force
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'LICENSE') -Destination $outputDirectory -Force
$hash = (Get-FileHash -LiteralPath $executable -Algorithm SHA256).Hash.ToLowerInvariant()
"$hash  MoveMouse.exe" | Set-Content -LiteralPath (Join-Path $outputDirectory 'SHA256SUMS.txt') -Encoding ascii

Write-Host "Compilado SIN FIRMA: $executable"
Write-Host 'Compilar no elimina los bloqueos del Control inteligente de aplicaciones.'
