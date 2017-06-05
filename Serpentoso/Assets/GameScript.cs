using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour {
	
	private Vector3 currentPosition;
	private Vector3 nextPosition;
	private Vector3 currentDirection;
	private Vector3 direction;
	private int score = 0;
	private bool scored = false;
	private bool endgame = true;
	private List<Vector3> availablePoints = new List<Vector3>();

	public int gameSpeed;
	public int maxRow;
	public int maxColumn;

	public UIScript uiScript;
	public Transform snakeTransform;
	public GameObject point;
	public GameObject prefabBody;
	public GameObject myHead;
	public GameObject myTail;
	public List<GameObject> myBody;

	public void Retry()
	{
		// Destroy possible previous bodyparts
		foreach (GameObject bodypart in myBody)
		{
			Destroy (bodypart);
		}
		// clean lists
		myBody = new List<GameObject>();
		availablePoints = new List<Vector3>();

		Init ();
		// game starts when everything is initialized
		endgame = false;
	}

	public void Init ()
	{
		// snake initialization and direction
		initSnake();
		// place a point far from the center
		availablePointsGenerator();
		placePoint();

		score = 0;
		scored = false;
	}

	void initSnake()
	{
		currentPosition = new Vector3(-5, 0, 0);
		nextPosition = currentPosition;
		currentDirection = new Vector3 (1, 0, 0);
		direction = currentDirection;
		myHead.transform.localPosition = currentPosition;
		myTail.transform.localPosition = currentPosition - (direction * 2);

		GameObject newBodyPart = Instantiate<GameObject> (prefabBody);
		newBodyPart.transform.SetParent (snakeTransform);
		newBodyPart.transform.localPosition = currentPosition - direction;
		myBody.Insert (0, newBodyPart);
	}

	void availablePointsGenerator()
	{

		for (int row = maxRow * -1; row <= maxRow; row++) {
			for (int column = maxColumn * -1; column <= maxColumn; column++) {
				availablePoints.Add (new Vector3(row, column, 0));
			}
		}

		availablePoints.Remove(myTail.transform.localPosition);
		availablePoints.Remove(myBody [0].transform.localPosition);
		availablePoints.Remove(myHead.transform.localPosition);
	}

	void placePoint()
	{
		Vector3 pointPosition = availablePoints [Random.Range (0, availablePoints.Count - 1)];
		point.transform.localPosition = pointPosition;
		availablePoints.Remove (pointPosition);
	}

	// Loop functions
	void Update ()
	{
		// direction move
		changeDirection();
		// wait for the game speed or if game has stopped
		if (Time.frameCount % gameSpeed != 0 || endgame) 
			return;
		// calculate nextposition
		currentDirection = direction;
		nextPosition += currentDirection;
		// if next position is edges, lose
		checkNextEdges();
		// if next position is snake, lose
		checkNextSelfSnake();
		// if next position is point, score and next point
		checkNextPoint();
		// move snake
		if (!scored)
			moveSnake ();
		else
			enlargeSnake ();
	}

	void changeDirection()
	{
		// check input and direction is not the opossite
		if (Input.GetKey (KeyCode.UpArrow) && currentDirection.y != -1)
		{
			direction = new Vector3 (0, 1, 0);
		}
		else if (Input.GetKey (KeyCode.DownArrow) && currentDirection.y != 1)
		{
			direction = new Vector3 (0, -1, 0);
		}
		else if (Input.GetKey (KeyCode.RightArrow) && currentDirection.x != -1)
		{
			direction = new Vector3 (1, 0, 0);
		}
		else if(Input.GetKey (KeyCode.LeftArrow) && currentDirection.x != 1)
		{
			direction = new Vector3 (-1, 0, 0);
		}
	}

	void checkNextEdges()
	{
		if (nextPosition.x > maxRow ||
		    nextPosition.x < (maxRow * -1) ||
		    nextPosition.y > maxColumn ||
		    nextPosition.y < (maxColumn * -1))
		{
			gameOver (false);
		}
	}

	void checkNextSelfSnake()
	{
		if (!availablePoints.Contains(nextPosition) &&
			nextPosition != point.transform.localPosition)
		{
			gameOver (false);
		}
	}

	void checkNextPoint()
	{
		if (nextPosition == point.transform.localPosition)
		{
			score += 1;
			scored = true;
			randomizePoint ();
		}
	}

	void randomizePoint()
	{
		if (availablePoints.Count > 0) {
			placePoint ();
		} else {
			// won the game
			gameOver(true);
		}
	}

	void moveSnake()
	{
		Vector3 savedPosition = myHead.transform.localPosition;
		Vector3 myPosition = nextPosition;
		myHead.transform.localPosition = myPosition;
		availablePoints.Remove (myHead.transform.localPosition);
		// save the position for next part and so on

		foreach (GameObject bodypart in myBody)
		{
			myPosition = savedPosition;
			savedPosition = bodypart.transform.localPosition;
			bodypart.transform.localPosition = myPosition;
		}

		availablePoints.Add (myTail.transform.localPosition);
		myTail.transform.localPosition = savedPosition;
	}

	void enlargeSnake()
	{
		scored = false;

		Vector3 savedPosition = myHead.transform.localPosition;
		myHead.transform.localPosition = nextPosition;

		GameObject newBodyPart = Instantiate<GameObject> (prefabBody);
		newBodyPart.transform.SetParent (snakeTransform);
		newBodyPart.transform.localPosition = savedPosition;
		myBody.Insert (0, newBodyPart);
	}

	void gameOver(bool won)
	{
		endgame = true;
		if (won)
			uiScript.GameWon (score);
		else
			uiScript.GameOver (score);
	}
}
