using UnityEngine;

public class PlayerManager : MonoBehaviour
{
  [SerializeField] GameManager gameManager;
  [SerializeField] LayerMask blockLayer;
  enum DIRECTION_TYPE
  {
    STOP,
    RIGHT,
    LEFT,
  }

  DIRECTION_TYPE direction = DIRECTION_TYPE.STOP;

  Rigidbody2D rigidbody2D;

  float speed;

  bool isDead = false;

  Animator animator;
  // SE
  [SerializeField] AudioClip getItemSE;
  [SerializeField] AudioClip jumpSE;
  [SerializeField] AudioClip stampSE;
  AudioSource audioSource;

  float jumpPower = 575;

  float deadPower = 400;

  void Start()
  {
    rigidbody2D = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
    audioSource = GetComponent<AudioSource>();
  }

  void Update()
  {
    if (isDead)
    {
      animator.SetBool("isJumping", false);
      animator.Play("PlayerDeathAnimation");
      return;
    }

    float x = Input.GetAxis("Horizontal"); // 方向キーの取得
    animator.SetFloat("speed", Mathf.Abs(x));

    if (x == 0)
    {
      // 止まっている
      direction = DIRECTION_TYPE.STOP;
    }
    else if (x > 0)
    {
      // 右へ
      direction = DIRECTION_TYPE.RIGHT;
    }
    else
    {
      // 左へ
      direction = DIRECTION_TYPE.LEFT;
    }
    // スペースが押されたらJumpさせる
    if (IsGround())
    {
      animator.SetBool("isJumping", false);
      if (Input.GetKeyDown("space"))
      {
        Jump();
      }
    }
    else
    {
      animator.SetBool("isJumping", true);
    }
  }

  void FixedUpdate()
  {
    if (isDead)
    {
      return;
    }

    switch (direction)
    {
      case DIRECTION_TYPE.STOP:
        speed = 0;
        break;
      case DIRECTION_TYPE.RIGHT:
        speed = 3;
        transform.localScale = new Vector3(1, 1, 1);
        break;
      case DIRECTION_TYPE.LEFT:
        speed = -3;
        transform.localScale = new Vector3(-1, 1, 1);
        break;
    }
    rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
  }

  void Jump()
  {
    // 上に力を加える
    rigidbody2D.AddForce(Vector2.up * jumpPower);
    audioSource.PlayOneShot(jumpSE);
  }

  bool IsGround()
  {
    // 始点と終点を作成
    Vector3 leftStartPoint = transform.position - Vector3.right * 0.2f + Vector3.up * 0.01f;
    Vector3 rightStartPoint = transform.position + Vector3.right * 0.2f + Vector3.up * 0.01f;
    Vector3 endStartPoint = transform.position - Vector3.up * 0.1f;
    Debug.DrawLine(leftStartPoint, endStartPoint);
    Debug.DrawLine(rightStartPoint, endStartPoint);
    return Physics2D.Linecast(leftStartPoint, endStartPoint, blockLayer) || Physics2D.Linecast(rightStartPoint, endStartPoint, blockLayer);
  }

  void OnTriggerEnter2D(Collider2D collision)
  {
    if (isDead)
    {
      return;
    }

    switch (collision.gameObject.tag)
    {
      case "Trap":
        PlayerDeath();
        break;
      case "Finish":
        gameManager.GameClear();
        break;
      case "Item":
        // アイテム取得
        audioSource.PlayOneShot(getItemSE);
        collision.gameObject.GetComponent<ItemManager>().GetItem();
        break;
      case "Enemy":
        EnemyManager enemy = collision.gameObject.GetComponent<EnemyManager>();
        if (this.transform.position.y + 0.2f > enemy.transform.position.y)
        {
          // 上から踏んだら敵を削除
          rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
          Jump();
          audioSource.PlayOneShot(stampSE);
          enemy.DestroyEnemy();
        }
        else
        {
          // 横からぶつかったらプレイヤー破壊
          PlayerDeath();
        }
        break;
    }
  }

  void PlayerDeath()
  {
    isDead = true;
    rigidbody2D.velocity = new Vector2(0, 0);
    rigidbody2D.AddForce(Vector2.up * deadPower);
    BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
    Destroy(boxCollider2D);
    gameManager.GameOver();
  }
}
