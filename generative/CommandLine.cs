using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace generative
{
    class CommandLine
    {
        internal static T ParseCommandLine<T>(string[] args) where T : new()
        {
            T options = new T();

            var properties = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Select(prop => new { Property = prop, Attribute = prop.GetCustomAttribute<OptionAttribute>() })
                .Where(x => x.Attribute != null)
                .ToDictionary(x => x.Attribute.Name, x => x.Property);

            bool done = false;

            for (int i = 0; i < args.Length && !done; i++)
            {
                if (properties.TryGetValue(args[i], out var p))
                {
                    // for a bool property that is present on the command line set the option to true
                    if (p.PropertyType == typeof(bool))
                    {
                        p.SetValue(options, true);
                    }
                    else
                    {
                        // grab the next value on the command line, convert it to the property type and assign the value
                        string value = args[++i];

                        p.SetValue(options, TypeDescriptor.GetConverter(p.PropertyType).ConvertFromString(value));
                    }
                }
                else
                    done = true;
            }

            return options;
        }
    }
}
