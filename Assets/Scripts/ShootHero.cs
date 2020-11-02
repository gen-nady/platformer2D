using UnityEngine;

public class ShootHero : MonoBehaviour
{
    public Transform ShootPoint;
    public GameObject BulletLeft, BulletRight;
    static MoveHero MvHero;
    private void Start()
    {
        MvHero = GameObject.FindWithTag("Hero").GetComponent<MoveHero>();  
    }
    public void Shoot()
    {
        if (!MvHero.IsHeroRight)
        {
            GameObject bulletInstantiate = Instantiate(BulletLeft, ShootPoint.position, Quaternion.identity) as GameObject;
            Destroy(bulletInstantiate, 4);
        }
        else if (MvHero.IsHeroRight)
        {
            GameObject bulletInstantiate = Instantiate(BulletRight, ShootPoint.position, Quaternion.identity) as GameObject;
            Destroy(bulletInstantiate, 4);
        }
    }
}
