using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWalking : MonoBehaviour
{
    public float speed;
    public int disappearAfterSeconds;

    private Vector2 position;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DisappearAfterAwhile());
    }

    // Update is called once per frame
    void Update()
    {
        position = transform.position;
        position.x += speed * Time.deltaTime;
        transform.position = position;
    }

    IEnumerator DisappearAfterAwhile()
    {
        yield return new WaitForSeconds(disappearAfterSeconds);
        Destroy(gameObject);
    }
}
