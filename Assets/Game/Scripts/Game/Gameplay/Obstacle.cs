using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : CustomMonobehavior
{
    #region variables
    private float gapValue;
    private float movementSpeed;
    private float maximumPositionToDestroy;
    private float spawnXPosition;
    private float marginObstacle = 0.25f;
    private bool isPassed;

    public float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }
    public float MaximumPositionToDestroy { get => maximumPositionToDestroy; set => maximumPositionToDestroy = value; }
    public float GapValue { get => gapValue; set => gapValue = value; }
    public bool IsPassed { get => isPassed; set => isPassed = value; }
    #endregion

    #region events
    public event EventHandler EVENT_DESTROY_OBSTACLE;
    #endregion

    #region serializefield objects
    [SerializeField] GameObject obstacleTop;
    [SerializeField] GameObject obstacleBottom;
    #endregion

    public void Init(float _gapValue, float _movementSpeed, float _topPosition, float _bottomPosition)
    {

        Debug.Log("gap " + _gapValue);
        spawnXPosition = 5;
        GapValue = _gapValue;
        MovementSpeed = _movementSpeed;
        IsPassed = false;
        AdjustGapPosition();
        maximumPositionToDestroy = -5 - spawnXPosition;
        float randomYPosition = UnityEngine.Random.Range(_topPosition, _bottomPosition);
        transform.position = new Vector3(spawnXPosition, randomYPosition, -5);
    }

    private void AdjustGapPosition()
    {
        obstacleTop.transform.position = new Vector3(transform.position.x, gapValue / 2, -5);
        obstacleBottom.transform.position = new Vector3(transform.position.x, -gapValue / 2, -5);
    }

    // Update is called once per frame
    public void UpdateMethod()
    {
        transform.position += ((Vector3.left * MovementSpeed) * Time.deltaTime);
        if(transform.position.x < maximumPositionToDestroy)
        {
            DestroyObstacle();
        }
    }

    private void DestroyObstacle()
    {
        DispatchEvent(EVENT_DESTROY_OBSTACLE, this.gameObject, EventArgs.Empty);
    }
}
