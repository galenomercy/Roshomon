using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictimControl : ActorControl
{
    private bool isDeath;
    public bool IsDeath
    {
        get
        {
            return isDeath;
        }
    }
    private int deathTime = -1;
    public int DeathTime
    {
        get
        {
            return deathTime;
        }
    }
    public override void Restore(bool withData)
    {
        base.Restore(withData);
        if (withData)
        {
            isDeath = false;
            deathTime = -1;
        }
    }
    public void Die(bool withData, int deathTime = -1)
    {
        headImage.color = Color.red;
        if (withData)
        {
            isDeath = true;
            this.deathTime = deathTime;
        }
    }
}