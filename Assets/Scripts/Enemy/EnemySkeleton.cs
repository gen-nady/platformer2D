using System.Collections;
using UnityEngine;

public class EnemySkeleton : MonoBehaviour
{
    [SerializeField]
    Animator AnimCharacterMove;
    [SerializeField]
    Rigidbody2D RbSkeleton;
    private bool IsFlip = true;
    [SerializeField]
    float Speed;
    [SerializeField]
    float Range;
    private void FixedUpdate()
    {
        AnimCharacterMove.SetFloat("Speed", Mathf.Abs(Speed));
        MoveSkeleton();
    }
    void MoveSkeleton()
    {
        if (IsFlip)
        {
            StartCoroutine(Move());
        }
        RbSkeleton.velocity = new Vector2(Speed, RbSkeleton.velocity.y);
    }
    IEnumerator Move()
    {
        IsFlip = false;
        yield return new WaitForSeconds(Range);
        Speed *= -1;
        Flip();
        IsFlip = true;
    }
    private void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
