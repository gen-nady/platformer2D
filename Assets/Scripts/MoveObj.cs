using UnityEngine;

public class MoveObj : MonoBehaviour
{
    public float Speed;
    public Vector2 MoveDir;
    void FixedUpdate()
    {
            transform.Translate(MoveDir * Speed * Time.fixedDeltaTime); //движение объекта
    }   
}
