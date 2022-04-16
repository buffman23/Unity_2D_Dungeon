using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BombHole : Trap
{

    public float direction;
    public float bombSpeed;

    public List<GameObject> ignore;

    private GameObject _bomb;


    protected override void Start()
    {
        base.Start();

        _bomb = Resources.Load<GameObject>("Prefabs/Bomb");
        
        if(ignore == null)
        {
            ignore = new List<GameObject>();
        }

        if (!ignore.Contains(this.gameObject))
        {
            ignore.Add(this.gameObject);
        }
    }
    public override void Trigger()
    {
        GameObject bombGO = Instantiate<GameObject>(_bomb);
        bombGO.transform.position = transform.position;
        bombGO.GetComponent<Bomb>().ignore = this.ignore;
        //bombGO.transform.Rotate(Vector3.forward * direction);

        Rigidbody2D rb = bombGO.GetComponent<Rigidbody2D>();
        Debug.Log(new Vector2(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad)) * bombSpeed * rb.mass);
        rb.AddForce(new Vector2(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad)) * bombSpeed * rb.mass, ForceMode2D.Impulse);
    }
}
