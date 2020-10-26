using UnityEngine;

public class ShootHero : MonoBehaviour
{
    public Transform ShootPoint;
    public GameObject BulletLeft, BulletRight;
    static SpriteRenderer SrHero;
    private void Start()
    {
        SrHero = GetComponent<SpriteRenderer>();    
    }
    public void Shoot()
    {
        if (SrHero.flipX == true)
        {
            GameObject bulletInstantiate = Instantiate(BulletLeft, ShootPoint.position, Quaternion.identity) as GameObject;
            Destroy(bulletInstantiate, 4);
        }
        else if (SrHero.flipX == false)
        {
            GameObject bulletInstantiate = Instantiate(BulletRight, ShootPoint.position, Quaternion.identity) as GameObject;
            Destroy(bulletInstantiate, 4);
        }      
    }
}
