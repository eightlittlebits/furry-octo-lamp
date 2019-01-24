using System;

namespace generative
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    class OptionAttribute : Attribute
    {
        public string Name { get; set; }

        public OptionAttribute(string name)
        {
            Name = name;
        }
    }
}
