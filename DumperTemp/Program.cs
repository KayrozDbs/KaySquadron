using System;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Text;

class Program
{
    static void Main()
    {
        try 
        {
            var assembly = Assembly.LoadFrom(@"D:\plugin\kayarsenal\Dumper\bin\Debug\net10.0-windows\Lumina.dll");
            var type = assembly.GetType("Lumina.Excel.Sheets.GcArmyExpedition");
            
            if (type == null) {
                // Try to find it generically
                type = assembly.GetTypes().FirstOrDefault(t => t.Name == "GcArmyExpedition");
            }
            
            if (type == null) {
                Console.WriteLine("Type 'GcArmyExpedition' not found in Lumina");
                return;
            }
                
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"--- {type.FullName} ---");
            
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                sb.AppendLine($"  {prop.PropertyType.Name} {prop.Name}");
            }
            
            Console.WriteLine(sb.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}
