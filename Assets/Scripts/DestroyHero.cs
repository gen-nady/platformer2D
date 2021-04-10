using UnityEngine;
//временный скрипт, для того, чтобы герой попадая вне зоны карты, не летел бесконечно
public class DestroyHero : MonoBehaviour
{
    MoveHero MH;
    private void Start()
    {
        MH = GameObject.Find("Hero").GetComponent<MoveHero>();
    }

    [System.Obsolete]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        MH.SaveGame();
        Destroy(collision.gameObject);  
        Application.LoadLevel(0);
    }
}
