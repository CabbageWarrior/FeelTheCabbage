using UnityEngine;
using System.Collections;

public class Cabbage : MonoBehaviour
{

	public float rotationsPerMinute;
	public float deltaMove;
	public float movingSpeed;

	// Use this for initialization
	/*void Start()
	{

	}*/

	// Update is called once per frame
	void Update()
	{
		float y0 = transform.position.y;
		float movingAmount = deltaMove * Mathf.Sin(movingSpeed * Time.time) / 100;
		
		transform.Rotate(Vector3.up * rotationsPerMinute * Time.deltaTime);
		transform.position = new Vector3(transform.position.x, y0 + movingAmount, transform.position.z);
	}
}
