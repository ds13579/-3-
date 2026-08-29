using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float heightOffset = 2f;

    [Header("Camera Zoom")]
    public float zoom = 5f;

    void LateUpdate()
    {
        if (player == null)
            return;

        // 플레이어 따라가기
        transform.position = new Vector3(
            player.position.x,
            player.position.y + heightOffset,
            transform.position.z
        );

        // 카메라 확대/축소
        Camera.main.orthographicSize = zoom;
    }
}