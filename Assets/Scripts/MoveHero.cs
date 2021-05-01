using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class MoveHero : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D rbHero;
    [SerializeField]
    Animator animCharacterMove;
    [Header("Движение")]
    public float speed;
    public Transform groundCheck;
    static readonly float groundRadius = 0.2f;
    public LayerMask whatIsGround;
    int move;
    public bool isHeroRight = true;
    bool isGrounded;
    [Header("Атака")]
    bool isReadyAttackHero = true;
    public GameObject attackPosition;
    [Header("Монеты")]
    public Text coinText;
    int coin;
    [Header("Жизни")]
    int lifePoints = 3;
    [SerializeField]
    public int manaPoints = 3;
    [SerializeField]
    Image[] lifePoint, lifePointSprite, manaPoint, manaPointSprite;
    [Header("Лестницы")]
    bool isStairs = false;
    bool isStairsGo = false;
    void Start()
    {
        coin = PlayerPrefs.GetInt("Coin");
        coinText.text = coin.ToString();
    }
    [System.Obsolete]
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        animCharacterMove.SetBool("Ground", isGrounded);
        animCharacterMove.SetFloat("vSpeed", rbHero.velocity.y);
        animCharacterMove.SetFloat("Speed", Mathf.Abs(move));
        if (isStairs && isStairsGo)
        {
            rbHero.velocity = new Vector2(0, 175 * Time.fixedDeltaTime);
        }
        rbHero.velocity = new Vector2(move * speed, rbHero.velocity.y);
        if (move > 0 && !isHeroRight)
        {
            Flip();
        }
        //обратная ситуация. отражаем персонажа влево
        else if (move < 0 && isHeroRight)
        {
            Flip();
        }
        if (lifePoints == 0)
        {
            Dead();
        }
        ChangeLife();
    }
    public void ChangeLife()
    {
        if (lifePoints > 3)
            lifePoints = 3;
        if (manaPoints > 3)
            manaPoints = 3;
        for (int i = 0; i < lifePoint.Length; i++)
        {
            if (i < lifePoints)
            {
                lifePoint[i].sprite = lifePointSprite[0].sprite;
            }
            else
            {
                lifePoint[i].sprite = lifePointSprite[1].sprite;
            }
        }
        for (int i = 0; i < manaPoint.Length; i++)
        {
            if (i < manaPoints)
            {
                manaPoint[i].sprite = manaPointSprite[0].sprite;
            }
            else
            {
                manaPoint[i].sprite = manaPointSprite[1].sprite;
            }
        }
    }
    private void Flip()
    {
        isHeroRight = !isHeroRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    public void MoveRightHero()
    {
        move = 1;
    }
    public void MoveLeftHero()
    {
        move = -1;
    }
    public void MoveHeroUp()
    {
        move = 0;
    }
    public void MoveJumpHero()
    {
        isStairsGo = true;
        if (isGrounded && !isStairs)
        {
            //устанавливаем в аниматоре переменную в false
            animCharacterMove.SetBool("Ground", false);
            //прикладываем силу вверх, чтобы персонаж подпрыгнул
            rbHero.AddForce(new Vector2(0, 475));
        }
    }
    public void IsStairsV()
    {
        isStairsGo = false;
    }
    public void HeroAttack()
    {
        if (isReadyAttackHero && isGrounded)
        {
            animCharacterMove.SetBool("Attack", true);
            isReadyAttackHero = false;
            StartCoroutine(DamageWait());
        }
    }
    public void Damage(int dmg)
    {
        lifePoints -= dmg;
        ChangeLife();
        if (lifePoints < 0)
        {
            lifePoints = 0;
        }
    }
    IEnumerator DamageWait()
    {
        attackPosition.SetActive(true);
        yield return new WaitForSeconds(0.35f);
        attackPosition.SetActive(false);
        animCharacterMove.SetBool("Attack", false);
        isReadyAttackHero = true;
    }
    //проверка для прыжка
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Coin"))
        {
            coin++;
            coinText.text = coin.ToString();
            Destroy(coll.gameObject);
        }
    }
    [System.Obsolete]
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("EnemyAttack"))
        {
            Damage(1);
            Destroy(coll.gameObject);
        }
        if (coll.gameObject.CompareTag("EnemyAttackSword"))
        {
            Damage(1);
        }
        if (coll.gameObject.CompareTag("Potions"))
        {
            lifePoints += 2;
            ChangeLife();
            Destroy(coll.gameObject);
        }
        if (coll.gameObject.CompareTag("Mana"))
        {
            manaPoints++;
            ChangeLife();
            Destroy(coll.gameObject);
        }
        if (coll.gameObject.CompareTag("Exit"))
        {
            SaveGame();
            Application.LoadLevel(0);
        }
    }
    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Ladder"))
        {
            isStairs = true;
            rbHero.gravityScale = 0.4f;
        }
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Ladder"))
        {
            isStairs = false;
            rbHero.gravityScale = 1f;
        }
    }
    [System.Obsolete]
    void Dead()
    {
        SaveGame();
        Application.LoadLevel(0);
        Destroy(gameObject);
    }
    //сохранение игры
    public void SaveGame()
    {
        PlayerPrefs.SetInt("Coin", coin);
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
