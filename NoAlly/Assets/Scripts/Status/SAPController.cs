//日本語コメント可
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SAPController : MonoBehaviour
{
    [SerializeField, Tooltip("オブジェクトの必殺技ゲージの上限")]
    protected float _maxSAP = 20;
    [Tooltip("オブジェクトの現在の必殺技ゲージ")]
    protected FloatReactiveProperty _sapReactiveProperty = new();

    bool _enableIncrease = false;

    public float MaxSAP => _maxSAP;
    public IReadOnlyReactiveProperty<float> SAP => _sapReactiveProperty;

    /// <summary>
    /// プレイヤーのSAP回復
    /// </summary>
    /// <param name="sapPuls"></param>
    public void SAPPuls(float sapPuls)
    {
        _sapReactiveProperty.Value += sapPuls;
        if (_sapReactiveProperty.Value > _maxSAP)
        {
            _sapReactiveProperty.Value = _maxSAP;
        }
    }

    /// <summary>
    /// プレイヤーのSAP使用
    /// </summary>
    /// <param name="useSAP"></param>
    public void UseSAP(float useSAP)
    {
        _sapReactiveProperty.Value -= useSAP;
    }

    public void UseSAP(float useSAP, float interval)
    {
        _sapReactiveProperty.Value -= useSAP;
        if (_enableIncrease)
        {

            StartCoroutine(IncreaseSAP(interval));
        }
    }

    IEnumerator IncreaseSAP(float interval)
    {
        while (true)
        {
            if (_sapReactiveProperty.Value >= _maxSAP) break;
            SAPPuls(1);
            yield return new WaitForSeconds(interval);
        }
    }

    /// <summary>
    /// ReactivePropertyのデストラクタ
    /// </summary>
    void OnDisable()
    {
        _sapReactiveProperty.Dispose();
    }
}
