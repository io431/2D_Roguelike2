using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using System.Collections.Generic;

public class MessageController : MonoBehaviour
{
    public TMP_Text messageText; // UI.Text ���g�p����ꍇ�� `public Text messageText;` �Ə����Ă��������B
    private Queue<string> messageQueue = new Queue<string>(); // FIFO�̓����𗘗p���ă��b�Z�[�W���Ǘ�
    public int maxLines = 6; // �\������e�L�X�g���C���̍ő吔

    public void ShowMessage(string message)
    {
        if (messageQueue.Count >= maxLines) // �ő�s���ȏ�̃��b�Z�[�W������ꍇ�́A�Â����b�Z�[�W���폜����B
        {
            messageQueue.Dequeue();
        }

        messageQueue.Enqueue(message); // �V�������b�Z�[�W��ǉ�����B

        messageText.text = string.Join("\n", messageQueue.ToArray()); // Queue���̂��ׂẴ��b�Z�[�W���e�L�X�g�Ƃ��ĕ\������B
    }
}