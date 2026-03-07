using System;
using System.Linq;
using System.Reflection;
using System.IO;

class Program
{
    static void Main()
    {
        try 
        {
            var assembly = Assembly.LoadFile(@"D:\plugin\kayarsenal\Dumper\bin\Debug\net10.0-windows\FFXIVClientStructs.dll");
            var types = assembly.GetTypes()
                .Where(t => t.Name.Contains("GcArmy") || t.Name.Contains("Squadron"))
                .ToList();
                
            Console.WriteLine($"Found {types.Count} types related to GCArmy/Squadron");
            
            foreach (var type in types)
            {
                Console.WriteLine($"\n--- {type.FullName} ---");
                Console.WriteLine($"Size: {MarshalSizeOf(type)}");
                
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                foreach (var field in fields.OrderBy(f => GetFieldOffset(f)))
                {
                    int offset = GetFieldOffset(field);
                    string offsetStr = offset >= 0 ? $"0x{offset:X4}" : "Static/Prop";
                    Console.WriteLine($"  [{offsetStr}] {field.FieldType.Name} {field.Name}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
    
    static int GetFieldOffset(FieldInfo fi)
    {
        var attr = fi.GetCustomAttributesData().FirstOrDefault(a => a.AttributeType.Name == "FieldOffsetAttribute");
        if (attr != null && attr.ConstructorArguments.Count > 0)
        {
            return (int)attr.ConstructorArguments[0].Value;
        }
        return -1;
    }
    
    static int MarshalSizeOf(Type t)
    {
        var attr = t.GetCustomAttributesData().FirstOrDefault(a => a.AttributeType.Name == "StructLayoutAttribute");
        if (attr != null)
        {
            var sizeArg = attr.NamedArguments.FirstOrDefault(a => a.MemberName == "Size");
            if (sizeArg != null && sizeArg.TypedValue.Value != null)
            {
                return (int)sizeArg.TypedValue.Value;
            }
        }
        return -1;
    }
}
