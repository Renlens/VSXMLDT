using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Renlen.FileTranslator
{
    public class TypeList : IEnumerable<TypeRef>
    {
        private List<TypeRef> types = new List<TypeRef>();

        public int Count => types.Count;
        public TypeRef this[int index]
        {
            get => types[index];
        }
        public TypeRef this[string fullName]
        {
            get => types.FirstOrDefault(t => t.FullName == fullName);
        }

        public void Add(TypeRef type)
        {
            if (!Contains(type))
            {
                types.Add(type);
            }
        }

        public void Remove(TypeRef type)
        {
            types.Remove(type);
        }

        public bool Contains(TypeRef type)
        {
            return types.Contains(type);
        }

        public IEnumerator<TypeRef> GetEnumerator()
        {
            return types.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
