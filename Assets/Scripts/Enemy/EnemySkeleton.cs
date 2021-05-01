using System.Collections;
using UnityEngine;

public class EnemySkeleton : MonoBehaviour
{
    [SerializeField]
    Animator animCharacterMove;
    [SerializeField]
    Rigidbody2D rbSkeleton;
    [SerializeField]
    float speed;
    [SerializeField]
    float range;
    public bool heroTrigger;
    public GameObject shootPoint;
    bool isReadyAttack = true;
    public int lifePoints;
    public GameObject coin;
    private void FixedUpdate()
    {
        if (lifePoints == 0)
        {
            Dead();
        }
        RaycastHit2D hitShootLeft = Physics2D.Raycast(transform.position, Vector3.left, 1f, LayerMask.GetMask("Player"));
        RaycastHit2D hitShootRight = Physics2D.Raycast(transform.position, Vector3.right, 1f, LayerMask.GetMask("Player"));
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector3.left, 6f, LayerMask.GetMask("Player"));
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector3.right, 6f, LayerMask.GetMask("Player"));
        if (hitShootLeft.transform != null || hitShootRight.transform != null)
        {
            speed = 0f;
            rbSkeleton.velocity = new Vector2(0f, rbSkeleton.velocity.y);
            if (isReadyAttack)
            {
                isReadyAttack = false;
                animCharacterMove.SetBool("Attack", true);
                StartCoroutine(DamageWait());
            }
        }
        else if (hitLeft.transform != null)
        {
            speed = 2f;
            rbSkeleton.velocity = new Vector2(Vector3.left.x * speed, rbSkeleton.velocity.y);
            Vector3 theScale = transform.localScale;
            animCharacterMove.SetFloat("Speed", Mathf.Abs(speed));
            if (theScale.x > 0)
            {
                theScale.x *= -1;
                transform.localScale = theScale;
            }
            heroTrigger = true;
        }
        else if (hitRight.transform != null)
        {
            speed = 2f;
            rbSkeleton.velocity = new Vector2(Vector3.right.x * speed, rbSkeleton.velocity.y);
            Vector3 theScale = transform.localScale;
            animCharacterMove.SetFloat("Speed", Mathf.Abs(speed));
            if (theScale.x < 0)
            {
                theScale.x *= -1;
                transform.localScale = theScale;
            }
            heroTrigger = true;
        }
        else
        {
            speed = 0f;
            heroTrigger = false;
        }
        animCharacterMove.SetFloat("Speed", Mathf.Abs(speed));

        if (!heroTrigger)
            rbSkeleton.velocity = new Vector2(0f, rbSkeleton.velocity.y);
    }
    IEnumerator DamageWait()
    {
        yield return new WaitForSeconds(0.55f);
        shootPoint.SetActive(true);
        yield return new WaitForSeconds(1.45f);
        shootPoint.SetActive(false);
        animCharacterMove.SetBool("Attack", false);
        isReadyAttack = true;
    }
    public void Damage(int dmg)
    {
        lifePoints -= dmg;
        if (lifePoints < 0)
        {
            lifePoints = 0;
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
