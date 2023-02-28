using System;
using System.Runtime.CompilerServices;

namespace MetaParser.Json.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    internal sealed class JsonPrimaryPropertyAttribute : Attribute
    {
        public readonly string propertyName;

        public JsonPrimaryPropertyAttribute([CallerMemberName] string propertyName = "")
        {
            this.propertyName = propertyName;
        }
    }
}
