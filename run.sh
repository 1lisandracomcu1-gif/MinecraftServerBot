#!/bin/bash

set -e

echo "================================="
echo " MinecraftServerBot"
echo "================================="

echo "[1/2] Restaurando dependencias..."
dotnet restore

echo "[2/2] Comprobando compilación..."
dotnet build --configuration Release

echo "================================="
echo " COMPILACIÓN TERMINADA"
echo "================================="
