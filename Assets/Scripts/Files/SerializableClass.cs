using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SerializableClass<T> 
{
    public void InitClassValues(T c);
    public T ExtractClassData();
}
