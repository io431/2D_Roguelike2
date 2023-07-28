using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 offset;

    private void LateUpdate()
    {
        if (playerTransform != null)
        {
            // �v���C���[�̈ʒu�ɃI�t�Z�b�g�������ăJ�����̈ʒu���v�Z
            Vector3 targetPosition = playerTransform.position + offset;

            // �J�����̈ʒu��ύX
            transform.position = targetPosition;
        }
    }
}