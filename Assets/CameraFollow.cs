using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 offset;

    private void LateUpdate()
    {
        if (playerTransform != null)
        {
            // プレイヤーの位置にオフセットを加えてカメラの位置を計算
            Vector3 targetPosition = playerTransform.position + offset;

            // カメラの位置を変更
            transform.position = targetPosition;
        }
    }
}