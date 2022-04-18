using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTrap : Trap
{
    public GameObject go;
    public bool loop;

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
        if (loop)
        {
            go.SetActive(!go.activeInHierarchy);
        }
        else
        {
            go.SetActive(true);
        }
    }
}
