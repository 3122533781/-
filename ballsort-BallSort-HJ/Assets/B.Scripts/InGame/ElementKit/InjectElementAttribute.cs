using System;

namespace Fangtang
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class InjectElementAttribute : Attribute
    {
        public InjectElementAttribute() { }

        public InjectElementAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}
