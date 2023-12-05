using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DimensionalArray
{
    
    //シリアライズされた子要素クラス
    [System.Serializable]
    public class GenericArray<T>
    {
        [SerializeField]
        T[] _childArray;

        public T[] ChildArrays => _childArray;
    }
}
