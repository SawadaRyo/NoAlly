using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMenu<T> where T : ICommandButton
{
    T[,] AllButton { get; }
    void SetButtonMap(T[] allButtons);
    T TargetButton(Vector2 mapPos);
}
