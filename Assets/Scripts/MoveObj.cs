using UnityEngine;

public class MoveObj : MonoBehaviour
{
    public float speed;
    public Vector2 moveDir;
    void FixedUpdate()
    {
            transform.Translate(moveDir * speed * Time.fixedDeltaTime); //движение объекта
    }   
}
