using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{
    [SerializeField] private float playerSpeed;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

    }

    void Update()
    {
        Vector3 localLocation = transform.localPosition;
        Vector3 deviation = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        transform.localPosition = localLocation + deviation * playerSpeed * Time.deltaTime;

        animator.SetFloat("Horizontal", deviation.x);
        animator.SetFloat("Vertical", deviation.y);
    }
}
