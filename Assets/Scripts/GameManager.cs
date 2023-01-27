using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  [SerializeField] GameObject gameOverText;
  [SerializeField] GameObject gameClearText;

  public void GameOver()
  {
    gameOverText.SetActive(true);
    RestartScene();
  }

  public void GameClear()
  {
    gameClearText.SetActive(true);
    RestartScene();
  }

  void RestartScene()
  {
    Scene thisScene = SceneManager.GetActiveScene();
    SceneManager.LoadScene(thisScene.name);
  }
}
