using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renlen.FileTranslator
{
    internal static class Global
    {
        /// <summary>
        /// 全局唯一随机函数（项目内）
        /// </summary>
        public static Random GRandom { get; } = new Random();

        public static State GState = new FlagState<FileAccess>();
    }

    public interface IState<T> where T : notnull, Enum
    {
        public bool this[T state] { get; }
        public void Add(T state);
        public void Rom(T state);
        public void Set(T state);
        public void Clear();
    }

    public class FlagState<T> : IState<T> where T : notnull, Enum
    {
        private T init;
        private T state;

        public FlagState()
        {
            init = default;
            state = default;
        }
        public FlagState(T init)
        {
            this.init = init;
            this.state = init;
        }

        public bool this[T state]
        {
            get
            {
                return state.HasFlag(state);
            }
        }

        public void Add(T state)
        {

        }

        public void Rom(T state)
        {
            throw new NotImplementedException();
        }

        public void Set(T state)
        {
            throw new NotImplementedException();
        }
        public void Clear()
        {
            throw new NotImplementedException();
        }

    }
}
