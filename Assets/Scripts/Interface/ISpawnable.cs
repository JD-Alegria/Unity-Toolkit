using UnityEngine;

public interface ISpawnable<in TData> where TData : ScriptableObject
{
        void Init(TData data);
}