using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementCommandButton : CommandButton, IElementCommand
{
    [SerializeField, Header("�����̎��")]
    ElementType _elementType;

    public ElementType TypeOfElement => _elementType;
}
