using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleControl : ActorControl
{
    public bool IsKiller
    {
        get;
        set;
    }
    public override void Restore(bool withData)
    {
        base.Restore(withData);
        if (withData)
        {
            IsKiller = false;
        }
    }
    public void Kill(bool withData)
    {
        if (withData)
        {
            posRecordsList.Add(this.transform.position);
        }
        headImage.color = Color.black;
    }
}
