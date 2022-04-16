using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    public static float baseDamage = 20f;
    private float speedDamageThreshold = 2f;
    private Rigidbody2D _RB;
    private Vector2 previousVelocity;

    // Start is called before the first frame update
    void Start()
    {
        _RB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        previousVelocity = _RB.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D otherRB;
        if ((otherRB = collision.gameObject.GetComponent<Rigidbody2D>()) == null)
            return;

        if (previousVelocity.magnitude < otherRB.velocity.magnitude)
            return;

        float mag = (previousVelocity - otherRB.velocity).magnitude;
        Debug.Log("Mag: " + mag);
        if (mag >= speedDamageThreshold)
        {
            CharacterController cc;
            if((cc = collision.gameObject.GetComponent<CharacterController>()) != null)
            {
                float damage = baseDamage + (int)((mag - speedDamageThreshold) * 10);
                cc.Damage(damage);
                return;
            }

            Skeleton skele;
            if ((skele = collision.gameObject.GetComponent<Skeleton>()) != null)
            {
                float damage = baseDamage + (int)((mag - speedDamageThreshold) * 10);
                skele.Damage(damage);
                return;
            }
        }

    }
}
