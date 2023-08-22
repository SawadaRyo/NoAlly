using System;
using DG.Tweening;
using UnityEngine;

public class WeaponElementColor : MonoBehaviour
{
    [SerializeField, Header("")]
    WeaponElementBlade[] weaponElements = new WeaponElementBlade[4];
    [SerializeField, Header("武器の属性色")]
    Color[] _weaponElementColor = new[] { new Color(0f, 0f, 0f, 0f)
                                        , new Color(1f, 0f, 0f, 0.5f)
                                        , new Color(0f, 1f, 0f, 0.5f)
                                        , new Color(0f, 0f, 1f, 0.5f) };

    [Tooltip("現在セットしている属性刃")]
    WeaponElementBlade _currentElement;
    [Tooltip("属性刃のsequence")]
    Sequence _seq = null;

    public void SetElement(WeaponType weaponType)
    {
        foreach (var weapon in weaponElements)
        {
            if (weapon.type == weaponType)
            {
                _currentElement = weapon;
                break;
            }
        }
    }

    public void ActiveElement(ElementType elementType)
    {
        switch (elementType)
        {
            case ElementType.RIGIT:
                Array.ForEach(_currentElement.weaponRenderer, x => x.enabled = false);
                break;
            default:
                Array.ForEach(_currentElement.weaponRenderer, x => x.enabled = true);
                break;
        }
        ChangeColor(elementType);
    }

    void ChangeColor(ElementType elementType)
    {
        foreach (var weaponRenderer in _currentElement.weaponRenderer)
        {
            DOTween.To(() => weaponRenderer.material.color,
                 n => weaponRenderer.material.color = n,
                 _weaponElementColor[(int)elementType],
                 duration: _currentElement.interval);
        }
    }
}

[System.Serializable]

public class WeaponElementBlade
{
    [SerializeField]
    public WeaponType type = WeaponType.NONE;
    [SerializeField, Header("変色にかかる時間")]
    public float interval = 0.1f;
    [SerializeField]
    public Renderer[] weaponRenderer = null;
}
