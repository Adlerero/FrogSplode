using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Room camera
    [SerializeField] private float speed;
    private float currentPosX;
    private Vector3 velocity = Vector3.zero;

    //Follow player
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float verticalOffset = 0f; // Offset vertical para ajustar altura
    [SerializeField] private float verticalSmoothTime = 0.3f; // Suavizado vertical

    [SerializeField] private float correction;
   
    private float lookAhead;
    private float currentVelocityY;

    private void Update()
    {
        //Room camera
        //transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPosX, transform.position.y, transform.position.z), ref velocity, speed);

        //Follow player en eje x
        //transform.position = new Vector3(player.position.x + lookAhead, transform.position.y, transform.position.z);
        //lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed);

        float targetY = player.position.y + verticalOffset;
        float smoothY = Mathf.SmoothDamp(transform.position.y, targetY, ref currentVelocityY, verticalSmoothTime);
        
        transform.position = new Vector3(player.position.x + lookAhead, smoothY - correction, transform.position.z);
        lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed);

    }

    public void MoveToNewRoom(Transform _newRoom)
    {
        currentPosX = _newRoom.position.x;
        currentVelocityY = 0f;
    }
}