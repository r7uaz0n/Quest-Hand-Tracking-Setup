using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> prefabs;

    public void Spawn(int index)
    {
        Instantiate(prefabs[index], transform.position, transform.rotation); 
    }

}
