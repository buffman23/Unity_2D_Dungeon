using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteTrap : Trap
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Trigger()
    {
        base.Trigger();
        Destroy(this.gameObject);
    }
}
