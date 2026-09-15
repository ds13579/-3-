using UnityEngine;
using UnityEngine.SceneManagement; // 씬 이동에 필요한 패키지

public class MainMenuManager : MonoBehaviour
{
    // '게임 시작' 버튼을 누르면 실행될 기능
    public void PlayGame()
    {
        // game 씬으로 화면 이동 (씬 파일 이름이 game인 경우)
        SceneManager.LoadScene("game");
    }
}