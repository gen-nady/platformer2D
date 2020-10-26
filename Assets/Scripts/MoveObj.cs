using UnityEngine;

public class MoveObj : MonoBehaviour
{
    public float Speed;
    public Vector2 MoveDir;
    static SpriteRenderer SrHero;
    private void Start()
    {
        SrHero = GetComponent<SpriteRenderer>();
    }
    void FixedUpdate()
    {
            transform.Translate(MoveDir * Speed * Time.fixedDeltaTime); //движение объекта
    }   
}
