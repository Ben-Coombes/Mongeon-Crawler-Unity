using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float speed = 4.0f;
    // Update is called once per frame
    void Start()
    {
        Vector3 position = this.transform.position;
        position.x = player.position.x;
        position.y = player.position.y;
        this.transform.position = position;
    }
    void Update()
    {
        float interpolation = speed * Time.deltaTime;
        Vector3 position = this.transform.position;

        position.y = Mathf.Lerp(this.transform.position.y, player.transform.position.y, interpolation);
        position.x = Mathf.Lerp(this.transform.position.x, player.transform.position.x, interpolation);

        this.transform.position = position;


    }
}
