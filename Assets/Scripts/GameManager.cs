using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  [SerializeField] GameObject gameOverText;
  [SerializeField] GameObject gameClearText;

  public void GameOver()
  {
    gameOverText.SetActive(true);
    Invoke("RestartScene", 1.5f);
  }

  public void GameClear()
  {
    gameClearText.SetActive(true);
    Invoke("RestartScene", 1.5f);
  }

  void RestartScene()
  {
    Scene thisScene = SceneManager.GetActiveScene();
    SceneManager.LoadScene(thisScene.name);
  }
}
