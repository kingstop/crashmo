using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntity
{
    void Init();
    void Release();
    void Update(float deltaTime);
}
