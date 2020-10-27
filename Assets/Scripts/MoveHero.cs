using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MoveHero : MonoBehaviour
{
    static SpriteRenderer SrHero;
    static Rigidbody2D RbHero;
    [Header("Движение")]
    public float Speed;
    public float JumpImpulse;
    float SpeedGame;   
    bool IsGrounded;
    Animator AnimCharacterMove;
    [Header("Монеты")]
    public Text CoinT;
    int Coin;
    int LifePoints=100;
    public GameObject AttackPositionRight;
    public GameObject AttackPositionLeft;
    bool IsStairs = false;
    Vector2 MoveDir = new Vector2(0,1);
    void Start()
    {
        SrHero = GetComponent<SpriteRenderer>();
        RbHero = GetComponent<Rigidbody2D>();
        AnimCharacterMove = GetComponent<Animator>();
        Coin = PlayerPrefs.GetInt("Coin");
        CoinT.text = Coin.ToString();
    }
    void FixedUpdate()
    {
        if (!IsStairs)
        {
            transform.Translate(SpeedGame * Time.fixedDeltaTime, 0, 0);
        }
        if (LifePoints == 0)
        {
            Dead();
        }
    }
    public void MoveRightHero()
    {
        SpeedGame = Speed;
        AnimationFalse();
        AnimCharacterMove.SetBool("MoveRight", true); //анимация 
    }
    public void MoveLeftHero()
    {
        SpeedGame = -Speed;
        AnimationFalse();
        AnimCharacterMove.SetBool("MoveLeft", true);
    }
    public void MoveStopHero()
    {
        if (SrHero.flipX == true)
        {
            AnimationFalse();
            AnimCharacterMove.SetBool("StayLeft", true);//анимация 
        }
        else if (SrHero.flipX == false)
        {
            AnimationFalse();
            AnimCharacterMove.SetBool("StayRight", true);//анимация 
        }     
        SpeedGame = 0;
    }
    public void MoveJumpHero()
    {
        if (IsGrounded && !IsStairs)
        {
            AnimationFalse();
            //проверка какую анимацию проигрывать
            if (SrHero.flipX == true)
            {
                AnimCharacterMove.SetBool("JumpLeft", true);
            }
            else if (SrHero.flipX == false)
            {
                AnimCharacterMove.SetBool("JumpRight", true);
            }
            RbHero.AddForce(new Vector2(0, JumpImpulse), ForceMode2D.Impulse);
            MoveStopHero();
        }
    }
    void AnimationFalse()
    {
        AnimCharacterMove.SetBool("JumpLeft", false);
        AnimCharacterMove.SetBool("JumpRight", false);
        AnimCharacterMove.SetBool("MoveLeft", false); //анимация 
        AnimCharacterMove.SetBool("MoveRight", false);
        AnimCharacterMove.SetBool("StayLeft", false);
        AnimCharacterMove.SetBool("StayRight", false);
        AnimCharacterMove.SetBool("AttackLeft", false);
        AnimCharacterMove.SetBool("AttackRight", false);
    }
    public void HeroAttack()
    {
        StartCoroutine(DamageWait());       
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
        AnimationFalse();
        if (SrHero.flipX == true)
        {
            AttackPositionLeft.SetActive(true);
            AnimCharacterMove.SetBool("AttackLeft", true);
        }
        else if (SrHero.flipX == false)
        {
            AttackPositionRight.SetActive(true);
            AnimCharacterMove.SetBool("AttackRight", true);
        }
        yield return new WaitForSeconds(0.8f);
        AttackPositionRight.SetActive(false);
        AttackPositionLeft.SetActive(false);
        MoveStopHero();   
    }
    //проверка для прыжка
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ground")
        {
            IsGrounded = true;
        }
        if (coll.gameObject.tag == "Coin")
        {
            Coin++;
            CoinT.text = Coin.ToString();
            Destroy(coll.gameObject);
        }
    }
    private void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ground")
        {
            IsGrounded = false;
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
        if (coll.gameObject.tag == "Stairs")
        {
            Debug.Log("1");
            IsStairs = true;
        }
    }
    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Stairs")
        {
            IsStairs = false;
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
