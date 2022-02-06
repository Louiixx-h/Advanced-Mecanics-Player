using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public LayerMask mPlayer;
    public Rigidbody mRigidbody;
    public SphereCollider mSphereCollider;
    public float mSpeed = 3f;

    void Update()
    {
        Move();
    }

    void Move()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            transform.position, 
            mSphereCollider.radius, 
            mPlayer
        );

        if (colliders.Length > 0)
        {
            print("entrou");
            Vector3 pos = colliders[0].gameObject.transform.position;

            mRigidbody.MovePosition(
                Vector2.MoveTowards(
                    transform.position,
                    pos,
                    mSpeed * Time.deltaTime
                )
            );
        }
    }
}
