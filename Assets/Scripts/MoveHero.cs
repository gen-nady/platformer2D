using System.Collections;
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
    static readonly float GroundRadius = 0.2f;
    public LayerMask WhatIsGround;
    int Move;
    public bool IsHeroRight = true;
    bool IsGrounded;
    [Header("Атака")]
    bool IsReadyAttackHero = true;
    public GameObject AttackPosition;
    [Header("Монеты")]
    public Text CoinT;
    int Coin;
    [Header("Жизни")]
    int LifePoints = 3;
    [SerializeField]
    Image[] Life_Point, LifePointSprite;
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
            RbHero.velocity = new Vector2(0, 175*Time.fixedDeltaTime);
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
        ChangeLife();
    }
    void ChangeLife()
    {
        for (int i = 0; i < Life_Point.Length; i++)
        {
            if (i < LifePoints)
            {
                Life_Point[i].sprite = LifePointSprite[0].sprite;
            }
            else
            {
                Life_Point[i].sprite = LifePointSprite[1].sprite;
            }
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
        if (IsGrounded &&!IsStairs)
        {
            //устанавливаем в аниматоре переменную в false
            AnimCharacterMove.SetBool("Ground", false);
            //прикладываем силу вверх, чтобы персонаж подпрыгнул
            RbHero.AddForce(new Vector2(0, 475));
        }
    }
    public void IsStairsV()
    {
        IsStairsGo = false;
    }
    public void HeroAttack()
    {
        if (IsReadyAttackHero && IsGrounded)
        {
            AnimCharacterMove.SetBool("Attack", true);
            IsReadyAttackHero = false;
            StartCoroutine(DamageWait());
        }
    }
    public void Damage(int dmg)
    {
        LifePoints -= dmg;
        ChangeLife();
        if (LifePoints < 0)
        {
            LifePoints = 0;
        }
    }
    IEnumerator DamageWait()
    {
        AttackPosition.SetActive(true);
        yield return new WaitForSeconds(0.35f);
        AttackPosition.SetActive(false);
        AnimCharacterMove.SetBool("Attack", false);
        IsReadyAttackHero = true;
    }
    //проверка для прыжка
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Coin"))
        {
            Coin++;
            CoinT.text = Coin.ToString();
            Destroy(coll.gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("EnemyAttack"))
        {
            Damage(1);
            Destroy(coll.gameObject);
        }
        if (coll.gameObject.CompareTag("Potions"))
        {
            LifePoints += 2;
            ChangeLife();
            Destroy(coll.gameObject);
        }
    }
    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Ladder"))
        {
            IsStairs = true;
            RbHero.gravityScale = 0.4f;
        }   
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Ladder"))
        {
            IsStairs = false;
            RbHero.gravityScale = 1f;      
        }
    }
    void Dead()
    {
        SaveGame();
        Application.LoadLevel(0);
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
