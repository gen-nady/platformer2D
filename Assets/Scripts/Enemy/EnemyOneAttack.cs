using System.Collections;
using UnityEngine;
public class EnemyOneAttack : MonoBehaviour
{   
    public Transform shootPoint;
    public GameObject bullet, coin;
    public int lifePoints;
    Animator animCharacterMove;
    void Start()
    {
        animCharacterMove = GetComponent<Animator>();
        InvokeRepeating("Shoot",0.8f,2.5f); //0.8 поскольку в анимации на 0.8 секунду начинает срабатывать атака
    }
    private void FixedUpdate()
    {
        if (lifePoints == 0)
        {
            Dead();
        }
    }
    //выстрел врага
    void Shoot()
    {
        animCharacterMove.SetBool("EnemyOneIsDamage", false);
        GameObject bulletInstantiate = Instantiate(bullet, shootPoint.position, Quaternion.identity) as GameObject;
        Destroy(bulletInstantiate, 2);
    }
    //нанесесние урона
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
        StartCoroutine(DeadFX());
    }
    IEnumerator DeadFX()
    {

        animCharacterMove.SetBool("EnemyOneIsDead", true);
        yield return new WaitForSeconds(0.4f);
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
            animCharacterMove.SetBool("EnemyOneIsDamage", true);
            Damage(1);
        }
    }
}
