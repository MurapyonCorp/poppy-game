using UnityEngine;

public class PlayerManager : MonoBehaviour
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

  float jumpPower = 400;

  void Start()
  {
    rigidbody2D = GetComponent<Rigidbody2D>();
  }

  void Update()
  {
    float x = Input.GetAxis("Horizontal");

    if (x == 0)
    {
      // �~�܂��Ă���
      direction = DIRECTION_TYPE.STOP;
    }
    else if (x > 0)
    {
      // �E��
      direction = DIRECTION_TYPE.RIGHT;
    }
    else if (x < 0)
    {
      // ����
      direction = DIRECTION_TYPE.LEFT;
    }
    // �X�y�[�X�������ꂽ��Jump������
    if (IsGround() && Input.GetKeyDown("space"))
    {
      Jump();
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
    // ��ɗ͂�������
    rigidbody2D.AddForce(Vector2.up * jumpPower);
  }

  bool IsGround()
  {
    // �n�_�ƏI�_���쐬
    Vector3 leftStartPoint = transform.position - Vector3.right * 0.2f + Vector3.up * 0.01f;
    Vector3 rightStartPoint = transform.position + Vector3.right * 0.2f + Vector3.up * 0.01f;
    Vector3 endStartPoint = transform.position - Vector3.up * 0.1f;
    Debug.DrawLine(leftStartPoint, endStartPoint);
    Debug.DrawLine(rightStartPoint, endStartPoint);
    return Physics2D.Linecast(leftStartPoint, endStartPoint, blockLayer) || Physics2D.Linecast(rightStartPoint, endStartPoint, blockLayer);
  }
}
