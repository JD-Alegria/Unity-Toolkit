using UnityEngine;

namespace Jaleg.Toolkit;

public interface ISpawnable<in TData> where TData : ScriptableObject
{
        void Init(TData data);
}