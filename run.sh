#!/bin/bash

set -e

echo "================================="
echo " MinecraftServerBot"
echo "================================="

echo "[1/2] Restaurando dependencias..."
dotnet restore

echo "[2/2] Iniciando bot..."
dotnet run --configuration Release
