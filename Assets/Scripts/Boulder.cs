using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    public float baseDamage = 20f;
    public float speedDamageThreshold = 2f;
    private Rigidbody2D _RB;

    // Start is called before the first frame update
    void Start()
    {
        _RB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D otherRB;
        if ((otherRB = collision.gameObject.GetComponent<Rigidbody2D>()) == null)
            return;

        Vector2 vec1 = -(collision.relativeVelocity - otherRB.velocity);
        Vector2 vec2 = (otherRB.transform.position + (Vector3)otherRB.centerOfMass - transform.position);

        float contribution = Vector2.Dot(vec1.normalized, vec2.normalized);
        //Debug.Log("Collision angle:" + (Mathf.Acos(contribution) * Mathf.Rad2Deg));

        Vector2 boulderImpact = vec1 * contribution;
        float mag = boulderImpact.magnitude;
        //Debug.Log("Mag: " + mag);
        if (mag >= speedDamageThreshold && (Mathf.Acos(contribution) * Mathf.Rad2Deg < 85))
        {
            Debug.Log("Mag: " + mag);
            CharacterController cc;
            if((cc = collision.gameObject.GetComponent<CharacterController>()) != null)
            {
                //float damage = baseDamage + (int)((mag - speedDamageThreshold) * 5);
                // do more damage if boulder if falling down onto entity
                float damage = (int)(Mathf.Abs(boulderImpact.x * .05f) + Mathf.Abs(boulderImpact.y) * (boulderImpact.y < 0 ? 2 : .05)) * 5;
                cc.Damage(damage);
                return;
            }

            Skeleton skele;
            if ((skele = collision.gameObject.GetComponent<Skeleton>()) != null)
            {
                //float damage = baseDamage + (int)((mag - speedDamageThreshold) * 5);
                // do more damage if boulder if falling down onto entity
                float damage = (int)(Mathf.Abs(boulderImpact.x * .05f) + Mathf.Abs(boulderImpact.y) * (boulderImpact.y < 0 ? 2 : .05)) * 5;
                skele.Damage(damage);
                return;
            }
        }

    }
}
