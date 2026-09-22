using System.Reflection;

Console.WriteLine("=================================");
Console.WriteLine(" McProtoNet - DIAGNÓSTICO API");
Console.WriteLine("=================================");
Console.WriteLine();

string baseDir = AppContext.BaseDirectory;

Console.WriteLine($"Carpeta: {baseDir}");
Console.WriteLine();

string[] dlls = Directory.GetFiles(baseDir, "McProtoNet*.dll");

if (dlls.Length == 0)
{
    Console.WriteLine("NO SE ENCONTRARON DLL DE McProtoNet.");
    return;
}

Console.WriteLine($"DLL encontradas: {dlls.Length}");
Console.WriteLine();

foreach (string dll in dlls)
{
    Console.WriteLine("=================================");
    Console.WriteLine($"DLL: {Path.GetFileName(dll)}");
    Console.WriteLine("=================================");

    try
    {
        Assembly assembly = Assembly.LoadFrom(dll);

        foreach (Type type in assembly.GetExportedTypes())
        {
            string name = type.FullName ?? type.Name;

            if (name.Contains("MinecraftClient", StringComparison.OrdinalIgnoreCase) ||
                name.Contains("SetProtocolPacket", StringComparison.OrdinalIgnoreCase) ||
                name.Contains("LoginStartPacket", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine();
                Console.WriteLine($"TIPO: {name}");

                Console.WriteLine("CONSTRUCTORES:");

                foreach (ConstructorInfo constructor in type.GetConstructors())
                {
                    ParameterInfo[] parameters = constructor.GetParameters();

                    if (parameters.Length == 0)
                    {
                        Console.WriteLine("  ()");
                        continue;
                    }

                    string parametros = string.Join(
                        ", ",
                        parameters.Select(p =>
                            $"{p.ParameterType.Name} {p.Name}"
                        )
                    );

                    Console.WriteLine($"  ({parametros})");
                }

                Console.WriteLine("MÉTODOS PÚBLICOS:");

                foreach (MethodInfo method in type.GetMethods(
                    BindingFlags.Public |
                    BindingFlags.Instance |
                    BindingFlags.Static |
                    BindingFlags.DeclaredOnly))
                {
                    Console.WriteLine(
                        $"  {method.ReturnType.Name} {method.Name}()"
                    );
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERROR LEYENDO DLL: {ex.Message}");
    }
}

Console.WriteLine();
Console.WriteLine("=================================");
Console.WriteLine(" DIAGNÓSTICO TERMINADO");
Console.WriteLine("=================================");
