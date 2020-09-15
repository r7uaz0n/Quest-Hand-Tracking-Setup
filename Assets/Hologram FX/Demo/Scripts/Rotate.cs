using UnityEngine;

public class Rotate : MonoBehaviour
{
	void Update ()
	{
		transform.Rotate(Vector3.one * Time.deltaTime * 40f);
	}
}
