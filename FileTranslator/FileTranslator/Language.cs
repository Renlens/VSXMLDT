using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Renlen.FileTranslator
{
    /// <summary>
    /// 支持翻译的语言列表。
    /// </summary>
    public sealed class Language : IComparable<Language>, IComparable
    {
        #region 枚举

        /// <summary>
        /// 应用默认
        /// </summary>
        [LanguageValue("default")]
        [LanguageCaption("应用默认")]
        public static Language Default { get; } = new Language("Default");
        /// <summary>
        /// 自动检测
        /// </summary>
        [LanguageValue("auto")]
        [LanguageCaption("自动检测")]
        public static Language Auto { get; } = new Language("Auto");
        /// <summary>
        /// 中文
        /// </summary>
        [LanguageValue("zh")]
        [LanguageCaption("中文")]
        public static Language Chinese { get; } = new Language("Chinese");
        /// <summary>
        ///	英语
        /// </summary>
        [LanguageValue("en")]
        [LanguageCaption("英语")]
        public static Language English { get; } = new Language("English");
        /// <summary>
        ///	粤语
        /// </summary>
        [LanguageValue("yue")]
        [LanguageCaption("粤语")]
        public static Language Cantonese { get; } = new Language("Cantonese");
        /// <summary>
        /// 文言文
        /// </summary>
        [LanguageValue("wyw")]
        [LanguageCaption("文言文")]
        public static Language ClassicalChinese { get; } = new Language("ClassicalChinese");
        /// <summary>
        /// 日语
        /// </summary>
        [LanguageValue("jp")]
        [LanguageCaption("日语")]
        public static Language Japanese { get; } = new Language("Japanese");
        /// <summary>
        ///	韩语
        /// </summary>
        [LanguageValue("kor")]
        [LanguageCaption("韩语")]
        public static Language Korean { get; } = new Language("Korean");
        /// <summary>
        /// 法语
        /// </summary>
        [LanguageValue("fra")]
        [LanguageCaption("法语")]
        public static Language French { get; } = new Language("French");
        /// <summary>
        /// 西班牙语
        /// </summary>
        [LanguageValue("spa")]
        [LanguageCaption("西班牙语")]
        public static Language Spanish { get; } = new Language("Spanish");
        /// <summary>
        /// 泰语
        /// </summary>
        [LanguageValue("th")]
        [LanguageCaption("泰语")]
        public static Language Thai { get; } = new Language("Thai");
        /// <summary>
        ///	阿拉伯语
        /// </summary>
        [LanguageValue("ara")]
        [LanguageCaption("阿拉伯语")]
        public static Language Arabic { get; } = new Language("Arabic");
        /// <summary>
        /// 俄语
        /// </summary>
        [LanguageValue("ru")]
        [LanguageCaption("俄语")]
        public static Language Russian { get; } = new Language("Russian");
        /// <summary>
        ///	 葡萄牙语
        /// </summary>
        [LanguageValue("pt")]
        [LanguageCaption("葡萄牙语")]
        public static Language Portuguese { get; } = new Language("Portuguese");
        /// <summary>
        ///	德语
        /// </summary>
        [LanguageValue("de")]
        [LanguageCaption("德语")]
        public static Language German { get; } = new Language("German");
        /// <summary>
        /// 意大利语
        /// </summary>
        [LanguageValue("it")]
        [LanguageCaption("意大利语")]
        public static Language Italian { get; } = new Language("Italian");
        /// <summary>
        /// 希腊语
        /// </summary>
        [LanguageValue("el")]
        [LanguageCaption("希腊语")]
        public static Language Greek { get; } = new Language("Greek");
        /// <summary>
        ///  荷兰语
        /// </summary>
        [LanguageValue("nl")]
        [LanguageCaption("荷兰语")]
        public static Language Dutch { get; } = new Language("Dutch");
        /// <summary>
        ///	 波兰语
        /// </summary>
        [LanguageValue("pl")]
        [LanguageCaption("波兰语")]
        public static Language Polish { get; } = new Language("Polish");
        /// <summary>
        ///	保加利亚语
        /// </summary>
        [LanguageValue("bul")]
        [LanguageCaption("保加利亚语")]
        public static Language Bulgarian { get; } = new Language("Bulgarian");
        /// <summary>
        ///爱沙尼亚语
        /// </summary>
        [LanguageValue("est")]
        [LanguageCaption("爱沙尼亚语")]
        public static Language Estonian { get; } = new Language("Estonian");
        /// <summary>
        ///丹麦语
        /// </summary>
        [LanguageValue("dan")]
        [LanguageCaption("丹麦语")]
        public static Language Danish { get; } = new Language("Danish");
        /// <summary>
        ///芬兰语
        /// </summary>
        [LanguageValue("fin")]
        [LanguageCaption("芬兰语")]
        public static Language Finnish { get; } = new Language("Finnish");
        /// <summary>
        ///捷克语
        /// </summary>
        [LanguageValue("cs")]
        [LanguageCaption("捷克语")]
        public static Language Czech { get; } = new Language("Czech");
        /// <summary>
        /// 罗马尼亚语
        /// </summary>
        [LanguageValue("rom")]
        [LanguageCaption("罗马尼亚语")]
        public static Language Romanian { get; } = new Language("Romanian");
        /// <summary>
        ///斯洛文尼亚语
        /// </summary>
        [LanguageValue("slo")]
        [LanguageCaption("斯洛文尼亚语")]
        public static Language Slovenian { get; } = new Language("Slovenian");
        /// <summary>
        ///瑞典语
        /// </summary>
        [LanguageValue("swe")]
        [LanguageCaption("瑞典语")]
        public static Language Swedish { get; } = new Language("Swedish");
        /// <summary>
        /// 匈牙利语
        /// </summary>
        [LanguageValue("hu")]
        [LanguageCaption("匈牙利语")]
        public static Language Hungarian { get; } = new Language("Hungarian");
        /// <summary>
        ///	繁体中文
        /// </summary>
        [LanguageValue("cht")]
        [LanguageCaption("繁体中文")]
        public static Language TraditionalChinese { get; } = new Language("TraditionalChinese");
        /// <summary>
        ///越南语
        /// </summary>
        [LanguageValue("vie")]
        [LanguageCaption("越南语")]
        public static Language Vietnamese { get; } = new Language("Vietnamese");

        #endregion

        public static Language[] GetLanguages()
        {
            Type type = typeof(Language);
            IEnumerable<Language> languages = type.GetProperties(BindingFlags.Public | BindingFlags.Static).Select(p => (Language)p.GetValue(null));
            return languages.ToArray();
        }
        public static Language[] GetLanguages(LanguageFiter fiter)
        {
            return fiter switch
            {
                LanguageFiter.None => GetLanguages(),
                LanguageFiter.RemoveAuto => GetLanguages(Auto),
                LanguageFiter.RemoveDefault => GetLanguages(Default),
                LanguageFiter.RemoveAutoAndDefault => GetLanguages(Auto, Default),
                _ => GetLanguages(),
            };
        }
        public static Language[] GetLanguages(params Language[] removeList)
        {
            Type type = typeof(Language);
            IEnumerable<Language> languages = type.GetProperties(BindingFlags.Public | BindingFlags.Static).Select(p => (Language)p.GetValue(null));
            if (removeList == null || removeList.Length == 0)
            {
                return languages.ToArray();
            }
            else
            {
                languages = languages.Except(removeList);
                return languages.ToArray();
            }
        }
        public string Name { get; }
        public string Value { get; }
        public string Caption { get; }
        public Language This => this;

        private Language(string name)
        {
            Type type = typeof(Language);
            PropertyInfo info = type.GetProperty(name, BindingFlags.Public | BindingFlags.Static);
            if (info == null)
                throw new ArgumentException("无效的语言名称。", nameof(name));
            else
            {
                Name = name;
                LanguageValueAttribute value = info.GetCustomAttribute<LanguageValueAttribute>();
                if (value == null)
                {
                    throw new Exception("未在此名称的属性上找到 LanguageValueAttribute 特性。");
                }
                Value = value.Value;
                LanguageCaptionAttribute caption = info.GetCustomAttribute<LanguageCaptionAttribute>();
                if (caption != null)
                {
                    Caption = caption.Caption;
                }
            }
        }
        //private Language(string name, string value, string caption)
        //{
        //    Name = name;
        //    Value = value;
        //    Caption = caption;
        //}

        public override string ToString()
        {
            return Name;
        }

        public int CompareTo(Language other)
        {
            return Name.CompareTo(other.Name);
        }

        int IComparable.CompareTo(object obj)
        {
            if (obj is Language o)
            {
                return CompareTo(o);
            }
            else
            {
                throw new ArgumentException("Language 类型无法与其他类型进行比较。", nameof(obj));
            }
        }

        #region 重载 (未使用)
        /// <summary>
        /// 确定两个对象的引用是否相等
        /// </summary>
        /// <param name="obj"></param>
        ///// <returns></returns>
        //public override bool Equals(object obj)
        //{
        //    if (obj == null || typeof(Language) != obj.GetType())
        //    {
        //        return false;
        //    }
        //    return this == (Language)obj;
        //}
        //public override int GetHashCode()
        //{
        //    return Value.GetHashCode() ^ Name.GetHashCode();
        //}
        ///// <summary>
        ///// 如果 <see cref="Value"/> 和 <see cref="Name"/> 均相等，则返回 <see langword="true"/> .
        ///// </summary>
        ///// <param name="obj1"></param>
        ///// <param name="obj2"></param>
        ///// <returns></returns>
        //public static bool operator ==(Language obj1, Language obj2)
        //{
        //    return obj1?.Value == obj2?.Value && obj1?.Name == obj2?.Name;
        //}
        ///// <summary>
        ///// 如果 <see cref="Value"/> 和 <see cref="Name"/> 有一个不相等，则返回 <see langword="true"/> .
        ///// </summary>
        ///// <param name="obj1"></param>
        ///// <param name="obj2"></param>
        ///// <returns></returns>
        //public static bool operator !=(Language obj1, Language obj2)
        //{
        //    return obj1?.Value != obj2?.Value && obj1?.Name != obj2?.Name;
        //}
        #endregion
    }

    [Flags]
    public enum LanguageFiter
    {
        None = 0,
        RemoveAuto = 1,
        RemoveDefault = 2,
        RemoveAutoAndDefault = RemoveAuto | RemoveDefault
    }
}
