using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{
    public float height = 0.1f;
    public float speed = 3f;
    public float sinkAmount = 0.05f;
    public float returnSpeed = 3f;

    private Vector3 startPosition;
    private float currentSink = 0f;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float bobbing = Mathf.Sin(Time.time * speed) * height;

        currentSink = Mathf.Lerp(currentSink, 0, Time.deltaTime * returnSpeed);

        transform.position = new Vector3(startPosition.x, startPosition.y + bobbing - currentSink, startPosition.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            currentSink = sinkAmount;
        }
    }
}
