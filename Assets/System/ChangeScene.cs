using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    private enum SceneChangeType { OnCollide, OnKeyPress };
    
    [SerializeField] private string sceneToChangeTo;
    [SerializeField] private SceneChangeType sceneChangeType;


    private void ChangeToScene (string sceneToChangeTo)
    {
        SceneManager.LoadScene(sceneToChangeTo);
    }

    private void Update() {
        if (sceneChangeType == SceneChangeType.OnKeyPress) {
            if (Input.GetAxis("Interact") > 0) {
                ChangeToScene(sceneToChangeTo);
            }
        }
    }

    private void OnCollisionEnter(Collision other) {
        Debug.Log("Collided with " + other.gameObject.tag);
        if (sceneChangeType == SceneChangeType.OnCollide && other.gameObject.tag == "Player") {
            ChangeToScene(sceneToChangeTo);
        }
    }
}
