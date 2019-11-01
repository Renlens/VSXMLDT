using System;

namespace Renlen.FileTranslator
{
    [AttributeUsage(
        AttributeTargets.Property,
        AllowMultiple = false,
        Inherited = false)]
    public sealed class LanguageValueAttribute : Attribute
    {
        public string Value { get; }
        public LanguageValueAttribute(string value)
        {
            Value = value ?? "";
        }
    }
}
