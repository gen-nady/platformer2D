using UnityEngine;
public class ShootHero : MonoBehaviour
{
    public Transform shootPoint;
    public GameObject bulletLeft, bulletRight;
    static MoveHero mvHero;
    private void Start()
    {
        mvHero = GetComponent<MoveHero>();
    }
    public void Shoot()
    {
        if (mvHero.manaPoints > 0)
        {
            mvHero.manaPoints--;
            mvHero.ChangeLife();
            if (!mvHero.isHeroRight)
            {
                GameObject bulletInstantiate = Instantiate(bulletLeft, shootPoint.position, Quaternion.identity) as GameObject;
                Destroy(bulletInstantiate, 4);
            }
            else if (mvHero.isHeroRight)
            {
                GameObject bulletInstantiate = Instantiate(bulletRight, shootPoint.position, Quaternion.identity) as GameObject;
                Destroy(bulletInstantiate, 4);
            }
        }
    }
}
