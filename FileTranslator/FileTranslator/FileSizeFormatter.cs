using System;
using System.Globalization;

namespace Renlen.FileTranslator
{
    public class NumberFormatter : IFormatProvider, ICustomFormatter
    {
        public static NumberFormatter Formatter { get; } = new NumberFormatter();
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
            {
                return this;
            }
            else
            {
                return null;
            }
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            string ufmt = format.ToUpper(CultureInfo.InvariantCulture);
            int length = 0;
            bool isNumber = double.TryParse(arg.ToString(), out double n);
            if (isNumber)
            {

                if (ufmt == "S" ||
                     (ufmt.StartsWith("S") &&
                      int.TryParse(ufmt.Substring(1), out length)))
                {
                    if (n < 0)
                    {
                        return "-";
                    }

                    string[] ranks = { " B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
                    int rankMax = ranks.Length - 1;

                    int rank = 0;
                    while (n >= 1000)
                    {
                        n /= 1024;
                        rank++;
                        if (rank > rankMax)
                        {
                            throw new ArgumentOutOfRangeException(nameof(arg));
                        }
                    }
                    if (length < 4)
                    {
                        length = 4;
                    }
                    string nStr;
                    if (n > 0)
                    {
                        nStr = n.ToString($"G{length - 1}");
                    }
                    else
                    {
                        nStr = n.ToString($"G{length - 2}");
                    }
                    return $"{nStr.PadLeft(length)} {ranks[rank]}";
                }
                else if (ufmt == "Z" ||
                          (ufmt.StartsWith("Z") &&
                           int.TryParse(ufmt.Substring(1), out length)))
                {
                    if (n < 0)
                    {
                        return "-";
                    }
                    string[] ranks = { "", "万", "亿", "万亿", "亿亿" };
                    int rankMax = ranks.Length - 1;

                    int rank = 0;
                    while (n >= 10000)
                    {
                        n /= 10000;
                        rank++;
                        if (rank > rankMax)
                        {
                            throw new ArgumentOutOfRangeException(nameof(arg));
                        }
                    }
                    if (length < 4)
                    {
                        length = 4;
                    }
                    string nStr = n.ToString($"G{length}");
                    return $"{nStr.PadLeft(length)} {ranks[rank]}";
                }
                else
                {
                    return Other();
                }
                //return "-";
            }
            // Provide default formatting if arg is not an number.
            else
            {
                return Other();
            }

            string Other()
            {
                try
                {
                    return HandleOtherFormats(format, arg);
                }
                catch (FormatException e)
                {
                    throw new FormatException(string.Format("The format of '{0}' is invalid.", format), e);
                }
            }
        }

        private string HandleOtherFormats(string format, object arg)
        {
            if (arg is IFormattable)
                return ((IFormattable)arg).ToString(format, CultureInfo.CurrentCulture);
            else if (arg != null)
                return arg.ToString();
            else
                return string.Empty;
        }
    }

    public static class FileSizeFormatterExtend
    {
        public static string ToShow(this object value, string format)
        {
            return string.Format(NumberFormatter.Formatter, $"{{0:{format}}}", value);
        }
    }
}
