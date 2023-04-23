using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraScript : MonoBehaviour
{
    public float rotationSpeed = 120;
    public float moveSpeed = 10;
    public float maxX = 30;
    public float minimumDistance = -3;
    public float maximumDistance = -10;

    new Camera camera;

    void Start()
    {
        camera = Camera.main;
    }

    void LateUpdate()
    {
        transform.parent.eulerAngles = new Vector3(
            Mathf.Min(
                Mathf.Max(
                    (
                        transform.parent.eulerAngles.x > 180
                            ? transform.parent.eulerAngles.x - 360
                            : transform.parent.eulerAngles.x
                    )
                        + (Input.GetKey(KeyCode.W) ? Time.deltaTime * rotationSpeed : 0)
                        - (Input.GetKey(KeyCode.S) ? Time.deltaTime * rotationSpeed : 0),
                    -maxX
                ),
                maxX
            ),
            transform.parent.eulerAngles.y
                + (Input.GetKey(KeyCode.A) ? Time.deltaTime * rotationSpeed : 0)
                - (Input.GetKey(KeyCode.D) ? Time.deltaTime * rotationSpeed : 0),
            transform.parent.eulerAngles.z
        );

        transform.localPosition = new Vector3(
            transform.localPosition.x,
            transform.localPosition.y,
            Mathf.Min(
                Mathf.Max(
                    transform.localPosition.z
                        + (Input.GetKey(KeyCode.Q) ? Time.deltaTime * moveSpeed : 0)
                        - (Input.GetKey(KeyCode.E) ? Time.deltaTime * moveSpeed : 0),
                    maximumDistance
                ),
                minimumDistance
            )
        );

        if (Input.GetButtonDown("Fire1"))
        {
            var hit = Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out var info);

            if (hit)
            {
                var cell = info.collider.GetComponent<Cell>();

                if (cell != null && cell.OnClick != null)
                {
                    cell.OnClick();
                }
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            var hit = Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out var info);

            if (hit)
            {
                var cell = info.collider.GetComponent<Cell>();

                if (cell != null && cell.OnRightClick != null)
                {
                    cell.OnRightClick();
                }
            }
        }
    }
}
