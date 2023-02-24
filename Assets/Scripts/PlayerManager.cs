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

  Animator animator;

  float jumpPower = 575;

  void Start()
  {
    rigidbody2D = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
  }

  void Update()
  {
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
    switch (IsGround())
    {
      case true:
        animator.SetBool("isJumping", false);
        if (Input.GetKeyDown("space"))
        {
          Jump();
        }
        break;
      case false:
        animator.SetBool("isJumping", true);
        break;
    }
  }

  void FixedUpdate()
  {
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
    if (collision.gameObject.tag == "Trap")
    {
      PlayerDeath();
    }
    else if (collision.gameObject.tag == "Finish")
    {
      gameManager.GameClear();
    }
    else if (collision.gameObject.tag == "Item")
    {
      // アイテム取得
      collision.gameObject.GetComponent<ItemManager>().GetItem();
    }
    else if (collision.gameObject.tag == "Enemy")
    {
      EnemyManager enemy = collision.gameObject.GetComponent<EnemyManager>();
      if (this.transform.position.y + 0.2f > enemy.transform.position.y)
      {
        // 上から踏んだら敵を削除
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
        Jump();
        enemy.DestroyEnemy();
      }
      else
      {
        // 横からぶつかったらプレイヤー破壊
        PlayerDeath();
      }
    }
  }

  void PlayerDeath()
  {
    rigidbody2D.velocity = new Vector2(0, 0);
    rigidbody2D.AddForce(Vector2.up * jumpPower);
    animator.Play("PlayerDeathAnimation");
    gameManager.GameOver();
  }
}
