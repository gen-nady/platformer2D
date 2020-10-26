using UnityEngine;

public class DestroyHero : MonoBehaviour
{
    MoveHero MH;
    private void Start()
    {
        MH = GameObject.Find("Hero").GetComponent<MoveHero>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        MH.SaveGame();
        Destroy(collision.gameObject);
        Application.LoadLevel(0);
    }
}
