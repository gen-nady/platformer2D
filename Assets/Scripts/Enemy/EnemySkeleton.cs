using System.Collections;
using UnityEngine;

public class EnemySkeleton : MonoBehaviour
{
    [SerializeField]
    Animator AnimCharacterMove;
    [SerializeField]
    Rigidbody2D RbSkeleton;
    [SerializeField]
    float Speed;
    [SerializeField]
    float Range;
    public bool HeroTrigger;
    public GameObject shootPoint;
    bool isReadyAttack = true;
    public int LifePoints;
    public GameObject coin;
    private void FixedUpdate()
    {
        if (LifePoints == 0)
        {
            Dead();
        }
        RaycastHit2D hitShootLeft = Physics2D.Raycast(transform.position, Vector3.left, 1f, LayerMask.GetMask("Player"));
        RaycastHit2D hitShootRight = Physics2D.Raycast(transform.position, Vector3.right, 1f, LayerMask.GetMask("Player"));
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector3.left, 6f, LayerMask.GetMask("Player"));
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector3.right, 6f, LayerMask.GetMask("Player"));
        if (hitShootLeft.transform != null || hitShootRight.transform != null)
        {
            Speed = 0f;
            RbSkeleton.velocity = new Vector2(0f, RbSkeleton.velocity.y);
            if (isReadyAttack)
            {
                
                isReadyAttack = false;
                AnimCharacterMove.SetBool("Attack", true);
                StartCoroutine(DamageWait());
            }

        }
        else if (hitLeft.transform != null)
        {
            Speed = 2f;
            RbSkeleton.velocity = new Vector2(Vector3.left.x *Speed, RbSkeleton.velocity.y);
            Vector3 theScale = transform.localScale;
            AnimCharacterMove.SetFloat("Speed", Mathf.Abs(Speed));
            if (theScale.x > 0)
            {
                theScale.x *= -1;
                transform.localScale = theScale;
            }
            HeroTrigger = true;
        }
        else if (hitRight.transform != null)
        {
            Speed = 2f;
            RbSkeleton.velocity = new Vector2(Vector3.right.x * Speed, RbSkeleton.velocity.y);
            Vector3 theScale = transform.localScale;
            AnimCharacterMove.SetFloat("Speed", Mathf.Abs(Speed));
            if (theScale.x < 0)
            {
                theScale.x *= -1;
                transform.localScale = theScale;
            }
            HeroTrigger = true;
        }
        else
        {
            Speed = 0f;
            HeroTrigger = false;
        }
        AnimCharacterMove.SetFloat("Speed", Mathf.Abs(Speed));

        if (!HeroTrigger)
            RbSkeleton.velocity = new Vector2(0f, RbSkeleton.velocity.y);
    }
    IEnumerator DamageWait()
    {
        yield return new WaitForSeconds(0.55f);
        shootPoint.SetActive(true);
        yield return new WaitForSeconds(1.45f);
        shootPoint.SetActive(false);
        AnimCharacterMove.SetBool("Attack", false);
        isReadyAttack = true;
    }
    public void Damage(int dmg)
    {
        LifePoints -= dmg;
        if (LifePoints < 0)
        {
            LifePoints = 0;
        }
    }
    void Dead()
    {
        Destroy(gameObject);
        GameObject coinInstantiate = Instantiate(coin, transform.position, Quaternion.identity) as GameObject;
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("HeroMagicAttack")) //при соприкосновении с фаерболом, получает урон равный 5
        {
            Damage(5);
            Destroy(coll.gameObject);
        }

    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("HeroSwordAttack")) //при соприкосновении с мечом, получает урон равный 1
        {
            Damage(1);
        }
    }
}
