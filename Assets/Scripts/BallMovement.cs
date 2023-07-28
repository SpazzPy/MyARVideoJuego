using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class BallMovement : MonoBehaviour
{

    public float moveRadius = 5.0f;
    public float movementSpeed = 10.0f;

    public float minDistanceFromPlayer = 2.0f;
    public float maxDistanceFromPlayer = 20.0f;

    public TMP_Text scoreText;
    private int score = 0;

    private bool isLookingAtBall = false;
    private Vector3 playerPosition;

    private Vector3 targetPosition;

    void Start()
    {
        GenerateRandomTargetPosition();
        scoreText.text = "Puntos: 0";
        playerPosition = Camera.main.transform.position;

    }

    void Update()
    {
        UpdateGazeDetection();

        Debug.Log(isLookingAtBall);
        if (isLookingAtBall)
        {
            score++;
            scoreText.text = "Puntos: " + score;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            GenerateRandomTargetPosition();
        }
 
    }

    void GenerateRandomTargetPosition()
    {

        float distanceFromPlayer = Vector3.Distance(transform.position, playerPosition);

        if (distanceFromPlayer > maxDistanceFromPlayer)
        {
            Vector3 directionToPlayer = playerPosition - transform.position;
            targetPosition = transform.position + directionToPlayer.normalized * (distanceFromPlayer - maxDistanceFromPlayer);
        }
        else
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized * moveRadius;
            targetPosition = new Vector3(transform.position.x + randomDirection.x, transform.position.y, transform.position.z + randomDirection.y);
        }
    }

    private void UpdateGazeDetection()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform == this.transform)
            {
                isLookingAtBall = true;
            }
            else
            {
                isLookingAtBall = false;
            }
        } else
        {
            isLookingAtBall = false;
        }

        
    }


}
