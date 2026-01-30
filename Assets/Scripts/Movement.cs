using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 5.5f;

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, 0f, moveZ);
        transform.Translate(move * speed * Time.deltaTime, Space.World);
    }
}
