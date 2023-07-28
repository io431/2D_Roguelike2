using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using System.Collections.Generic;

public class MessageController : MonoBehaviour
{
    public TMP_Text messageText; // UI.Text を使用する場合は `public Text messageText;` と書いてください。
    private Queue<string> messageQueue = new Queue<string>(); // FIFOの特性を利用してメッセージを管理
    public int maxLines = 6; // 表示するテキストラインの最大数

    public void ShowMessage(string message)
    {
        if (messageQueue.Count >= maxLines) // 最大行数以上のメッセージがある場合は、古いメッセージを削除する。
        {
            messageQueue.Dequeue();
        }

        messageQueue.Enqueue(message); // 新しいメッセージを追加する。

        messageText.text = string.Join("\n", messageQueue.ToArray()); // Queue内のすべてのメッセージをテキストとして表示する。
    }
}