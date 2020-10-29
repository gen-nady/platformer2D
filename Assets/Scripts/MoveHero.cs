using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MoveHero : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D RbHero;
    [SerializeField]
    Animator AnimCharacterMove;
    [Header("Движение")]
    public float Speed;
    public Transform GroundCheck;
    float GroundRadius = 0.2f;
    public LayerMask WhatIsGround;
    int Move;
    bool IsHeroRight = true;
    bool IsGrounded;
    [Header("Атака")]
    bool IsReadyAttackHero = true;
    public GameObject AttackPositionRight;
    public GameObject AttackPositionLeft;
    [Header("Монеты")]
    public Text CoinT;
    int Coin;
    int LifePoints = 100;
    [Header("Лестницы")]
    bool IsStairs = false;
    bool IsStairsGo = false;
    void Start()
    {
        Coin = PlayerPrefs.GetInt("Coin");
        CoinT.text = Coin.ToString();
    }
    void FixedUpdate()
    {
        IsGrounded = Physics2D.OverlapCircle(GroundCheck.position, GroundRadius, WhatIsGround);
        AnimCharacterMove.SetBool("Ground", IsGrounded);
        AnimCharacterMove.SetFloat("vSpeed", RbHero.velocity.y);
        AnimCharacterMove.SetFloat("Speed", Mathf.Abs(Move));
        if (IsStairs && IsStairsGo)
        {
            RbHero.velocity = new Vector2(0, Speed * Time.fixedDeltaTime);
        }
        RbHero.velocity = new Vector2(Move * Speed, RbHero.velocity.y);
        if (Move > 0 && !IsHeroRight)
        {
            Flip();
        }
        //обратная ситуация. отражаем персонажа влево
        else if (Move < 0 && IsHeroRight)
        {
            Flip();
        }
        if (LifePoints == 0)
        {
            Dead();
        }
    }
    private void Flip()
    {
        IsHeroRight = !IsHeroRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    public void MoveRightHero()
    {
        Move = 1;
    }
    public void MoveLeftHero()
    {
        Move = -1;
    }
    public void MoveHeroUp()
    {
        Move = 0;
    }
    public void MoveJumpHero()
    {
        IsStairsGo = true;
        if (IsGrounded)
        {
            //устанавливаем в аниматоре переменную в false
            AnimCharacterMove.SetBool("Ground", false);
            //прикладываем силу вверх, чтобы персонаж подпрыгнул
            RbHero.AddForce(new Vector2(0, 600));
        }
        IsStairsGo = false;
    }
    public void IsStairsV()
    {
        IsStairsGo = false;
    }
    public void HeroAttack()
    {
        if (IsReadyAttackHero)
        {
            AnimCharacterMove.SetBool("Attack", true);
            IsReadyAttackHero = false;
            StartCoroutine(DamageWait());
        }
    }
    public void Damage(int dmg)
    {
        LifePoints -= dmg;
        if (LifePoints < 0)
        {
            LifePoints = 0;
        }
    }
    IEnumerator DamageWait()
    {
        if (!IsHeroRight)
        {
            AttackPositionLeft.SetActive(true);
        }
        else if (IsHeroRight)
        {
            AttackPositionRight.SetActive(true);
        }
        yield return new WaitForSeconds(0.35f);
        AttackPositionRight.SetActive(false);
        AttackPositionLeft.SetActive(false);
        AnimCharacterMove.SetBool("Attack", false);
        IsReadyAttackHero = true;
    }
    //проверка для прыжка
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Coin")
        {
            Coin++;
            CoinT.text = Coin.ToString();
            Destroy(coll.gameObject);
        }
    }
    //сбор монеток
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("EnemyAttack"))
        {
            Damage(1);
            Destroy(coll.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Ladder")
        {
            IsStairs = true;
            RbHero.gravityScale = 0.1f;
        }
    }
    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Ladder")
        {
            IsStairs = false;
            RbHero.gravityScale = 1f;
        }
    }
    void Dead()
    {
        SaveGame();
        Destroy(gameObject);
    }
    //сохранение игры
    public void SaveGame()
    {
        PlayerPrefs.SetInt("Coin", Coin);
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
