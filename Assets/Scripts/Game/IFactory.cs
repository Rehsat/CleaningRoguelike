using UnityEngine;

namespace Game
{
    public interface IFactory<T>
    {
        public T Get();
    }
}
