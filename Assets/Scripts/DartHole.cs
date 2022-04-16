using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DartHole : Trap
{

    public float direction;
    public float dartSpeed;

    private GameObject _dart;
    

    protected override void Start()
    {
        base.Start();

        _dart = Resources.Load<GameObject>("Prefabs/Dart");
    }
    public override void Trigger()
    {
        GameObject dartGO = Instantiate<GameObject>(_dart);
        dartGO.transform.position = transform.position;
        dartGO.transform.Rotate(Vector3.forward * direction);

        Rigidbody2D rb = dartGO.GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad)) * dartSpeed * rb.mass, ForceMode2D.Impulse);
    }


}
