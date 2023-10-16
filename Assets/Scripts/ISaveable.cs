using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable<T> where T : struct
{
    public void RestoreStateFromModel();

    public void Save(bool writeImmediately = false);

    public T ToModel();
}
