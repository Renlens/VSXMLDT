namespace Renlen.TranslateFile
{
    /// <summary>
    /// 文件特殊大小的枚举。支持 <see langword="long"/> 类型的大小。
    /// </summary>
    public enum FileSize : long
    {
        /// <summary>
        /// 该文件没有大小或获取大小失败
        /// </summary>
        NoSize = -1,
        /// <summary>
        /// 大小正在初始化
        /// </summary>
        Init = -2,
        /// <summary>
        /// 大小未初始化
        /// </summary>
        Uninit = -3
    }
}
