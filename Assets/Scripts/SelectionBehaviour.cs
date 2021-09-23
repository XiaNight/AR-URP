using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SelectionBehaviour : MonoBehaviour, ISelection
{
    public virtual GameObject GetSelected()
    {
        throw new System.NotImplementedException();
    }

    public virtual void SetSelectedIndex(int index)
    {
        throw new System.NotImplementedException();
    }
}
