
 
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
 
public class GameStart: MonoBehaviour
{

    //�@�X�^�[�g�{�^��������������s����
    public void StartGame()
    {
        SceneManager.LoadScene("Dungeon");
    }
}