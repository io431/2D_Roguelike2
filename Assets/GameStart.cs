
 
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
 
public class GameStart: MonoBehaviour
{

    //　スタートボタンを押したら実行する
    public void StartGame()
    {
        SceneManager.LoadScene("Dungeon");
    }
}