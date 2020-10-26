using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOneAttack : MonoBehaviour
{
    public Transform ShootPoint;
    public GameObject Bullet, Coin;
    public int LifePoints;
    static MagicFire MF;
    void Start()
    {
        MF = GameObject.Find("Hero").GetComponent<MagicFire>();
        InvokeRepeating("Shoot", 0.8f, 1);
    }
    private void FixedUpdate()
    {
        if (LifePoints == 0)
        {
            Dead();
        }
    }
    void Shoot()
    {
        GameObject bulletInstantiate = Instantiate(Bullet, ShootPoint.position, Quaternion.identity) as GameObject;
        Destroy(bulletInstantiate, 2);
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
        GameObject coinInstantiate = Instantiate(Coin, transform.position, Quaternion.identity) as GameObject;
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("HeroMagicAttack")) //при соприкосновении с пулей врага, получает уровн равный пуле или ракете
        {
            Damage(5);
            Destroy(coll.gameObject);
        }
       
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("HeroSwordAttack"))
        {
            Damage(1);
        }
    }
}
