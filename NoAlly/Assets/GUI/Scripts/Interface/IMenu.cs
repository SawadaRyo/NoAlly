using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMenu<T> 
{
    T[,] AllButton { get; }
    T[] SelectedButtons { get; set; }
    void SetButtonMap(T[] allButtons, int indexX, int indexY);
    T SelectButton(int crossH, int crossV);
}
