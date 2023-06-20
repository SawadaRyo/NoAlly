using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectObjectArray
{
    /// <summary>
    /// コンポーネント展開時に呼ぶ関数
    /// </summary>
    public void Extended();

    /// <summary>
    /// コンポーネント縮小時に呼ぶ関数
    /// </summary>
    public void Closed();
}
