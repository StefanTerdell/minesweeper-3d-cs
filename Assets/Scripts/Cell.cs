using System;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private Vector3Int id;
    private bool mined;
    private bool flagged;
    private int close;
    private bool revealed;

    [SerializeField]
    private Number number;

    public Action OnClick;
    public Action OnRightClick;

    public bool Revealed
    {
        get => revealed;
    }

    public int Close
    {
        get => close;
        set
        {
            number.Set(value);
            close = value;
        }
    }

    public bool Flagged
    {
        get => flagged;
        set => flagged = value;
    }
    public bool Mined
    {
        get => mined;
        set => mined = value;
    }
    public Vector3Int Id
    {
        get => id;
        set => id = value;
    }

    MeshRenderer mr;
    Collider col;
    Rigidbody rb;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        mr = GetComponent<MeshRenderer>();
        col = GetComponent<Collider>();
    }

    public void Reveal()
    {
        flagged = false;
        mr.enabled = false;
        col.enabled = false;
        revealed = true;

        if (mined)
        {
            Explosion.Explode(transform.position);
        }
        else
        {
            number.gameObject.SetActive(true);
        }
    }

    public void ApplyExplosiveForce(Vector3 origin)
    {
        number.gameObject.SetActive(false);
        rb.isKinematic = false;
        var diff =
            (transform.position - origin)
            + new Vector3(
                UnityEngine.Random.Range(-.5f, .5f),
                UnityEngine.Random.Range(-.5f, .5f),
                UnityEngine.Random.Range(-.5f, .5f)
            );
        var mag = diff.magnitude;
        rb.AddForce((diff / mag) * Mathf.Max(1, 10 - mag) * 500);
    }

    void OnGUI()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        if (mined)
        {
            GUI.color = flagged ? Color.yellow : Color.red;
            GUI.Label(new Rect(pos.x, Screen.height - pos.y, 100, 30), "X");
        }
        else if (flagged)
        {
            GUI.color = Color.magenta;
            GUI.Label(new Rect(pos.x, Screen.height - pos.y, 100, 30), "F");
        }
        else if (close != 0)
        {
            // GUI.Label(new Rect(pos.x, Screen.height - pos.y, 100, 30), Close.ToString());
        }
    }
}
