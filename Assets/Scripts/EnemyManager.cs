using UnityEngine;

public class EnemyManager : MonoBehaviour
{
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

  void Start()
  {
    rigidbody2D = GetComponent<Rigidbody2D>();
    // ‰E‚Ö
    direction = DIRECTION_TYPE.RIGHT;
  }

  void Update()
  {
    if (!IsGround())
    {
      // •ûŒü‚ð•Ï‚¦‚é
      ChangeDirection();
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

  bool IsGround()
  {
    Vector3 startVec = transform.position + transform.right * 0.5f * transform.localScale.x;
    Vector3 endVec = startVec - transform.up * 0.5f;
    Debug.DrawLine(startVec, endVec);
    return Physics2D.Linecast(startVec, endVec, blockLayer);
  }

  void ChangeDirection()
  {
    if (direction == DIRECTION_TYPE.RIGHT)
    {
      direction = DIRECTION_TYPE.LEFT;
    }
    else if (direction == DIRECTION_TYPE.LEFT)
    {
      direction = DIRECTION_TYPE.RIGHT;
    }
  }
}
