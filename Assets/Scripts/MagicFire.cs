using UnityEngine;

public class MagicFire : MonoBehaviour
{
    public Vector2 MoveDir;
    public float Speed;
    private void FixedUpdate()
    {
        transform.Translate(MoveDir * Speed * Time.fixedDeltaTime); //движение объекта
    }
}