using System;

namespace Renlen.FileTranslator
{
    [AttributeUsage(
        AttributeTargets.Property,
        AllowMultiple = false,
        Inherited = false)]
    public sealed class LanguageCaptionAttribute : Attribute
    {
        public string Caption { get; }
        public LanguageCaptionAttribute(string caption)
        {
            Caption = caption ?? "";
        }
    }
}
