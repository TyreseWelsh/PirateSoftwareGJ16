using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMobile
{
    public void Move();
    public float GetMoveSpeed(bool modified);
    public void SetMoveSpeed(float _speed);
}
