using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyOneAttack : MonoBehaviour
{   
    public Transform ShootPoint;
    public GameObject Bullet, Coin;
    public int LifePoints;
    Animator AnimCharacterMove;
    void Start()
    {
        AnimCharacterMove = GetComponent<Animator>();
        InvokeRepeating("Shoot", 0.8f, 1); //0.8 поскольку в анимации на 0.8 секунду начинает срабатывать атака
    }
    private void FixedUpdate()
    {
        if (LifePoints == 0)
        {
            Dead();
        }
    }
    //выстрел врага
    void Shoot()
    {
        AnimCharacterMove.SetBool("EnemyOneIsDamage", false);
        GameObject bulletInstantiate = Instantiate(Bullet, ShootPoint.position, Quaternion.identity) as GameObject;
        Destroy(bulletInstantiate, 2);
    }
    //нанесесние урона
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
        StartCoroutine(DeadFX());
    }
    IEnumerator DeadFX()
    {

        AnimCharacterMove.SetBool("EnemyOneIsDead", true);
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
        GameObject coinInstantiate = Instantiate(Coin, transform.position, Quaternion.identity) as GameObject;
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
            AnimCharacterMove.SetBool("EnemyOneIsDamage", true);
            Damage(1);
        }
    }
}
