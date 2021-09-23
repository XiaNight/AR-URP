using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelection
{
    void SetSelectedIndex(int index);
    GameObject GetSelected();
}
