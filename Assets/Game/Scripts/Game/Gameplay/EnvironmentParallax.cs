using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnvironmentParallax : MonoBehaviour
{
    // Start is called before the first frame update
    [BoxGroup("Environment parallax attribute")]
    [SerializeField] GameObject midground;
    [BoxGroup("Environment parallax attribute")]
    [SerializeField] GameObject background4;
    [BoxGroup("Environment parallax attribute")]
    [SerializeField] GameObject background3;
    [BoxGroup("Environment parallax attribute")]
    [SerializeField] GameObject background2;

    private float movementSpeed = 0.0f;

    public float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }

    public void Init(float _movementSpeed)
    {
        MovementSpeed = _movementSpeed;
    }

    // Update is called once per frame
    public void UpdateMethod()
    {
        MoveMidGround();
        MoveBackground4();
        MoveBackground3();
        MoveBackground2();
    }

    private void MoveBackground2()
    {
        background2.transform.position += ((Vector3.left * (MovementSpeed / 6)) * Time.deltaTime);
        if (background2.transform.position.x < (-5.626 * 2))
        {
            background2.transform.position = new Vector3(0, background2.transform.position.y, background2.transform.position.z);
        }
    }

    private void MoveBackground3()
    {
        background3.transform.position += ((Vector3.left * (MovementSpeed / 5)) * Time.deltaTime);
        if (background3.transform.position.x < (-5.626 * 2))
        {
            background3.transform.position = new Vector3(0, background3.transform.position.y, background3.transform.position.z);
        }
    }

    private void MoveBackground4()
    {
        background4.transform.position += ((Vector3.left * (MovementSpeed/3)) * Time.deltaTime);
        if (background4.transform.position.x < (-5.626 * 2))
        {
            background4.transform.position = new Vector3(0, background4.transform.position.y, background4.transform.position.z);
        }
    }

    private void MoveMidGround()
    {
        midground.transform.position += ((Vector3.left * MovementSpeed) * Time.deltaTime);
        if (midground.transform.position.x < (-5.626 * 2))
        {
            midground.transform.position = new Vector3(0, midground.transform.position.y, midground.transform.position.z);
        }
    }
}
