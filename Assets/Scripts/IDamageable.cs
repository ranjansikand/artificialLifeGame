using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public abstract bool Alive();
    public abstract void Damage(float damage);
}
