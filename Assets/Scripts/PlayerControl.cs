using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float speed;
    private void OnMouseDrag()
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime * -Input.GetAxis("Mouse X"));
    }
}
