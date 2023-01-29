using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectPool 
{
    /// <summary>
    /// オブジェクトが起動中かどうか
    /// </summary>
    public bool IsActive { get;}

    /// <summary>
    /// オブジェクトが有効になった時に呼ばれる関数
    /// </summary>
    public void Create();

    /// <summary>
    /// オブジェクトが非有効になった時に呼ばれる関数
    /// </summary>
    public void Disactive();

    /// <summary>
    /// オブジェクトが生成された時に呼ばれる関数
    /// </summary>
    /// <typeparam name="TOwner">このオブジェクトを使用するオーナーのジェネリッククラス</typeparam>
    /// <param name="Owner">このオブジェクトを使用するオーナー</param>
    public void DisactiveForInstantiate<TOwner>(TOwner Owner) where TOwner : IObjectGenerator;
}
