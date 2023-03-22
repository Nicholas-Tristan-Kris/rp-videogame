using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    //TODO: figure out if each of these stats should have its own class or not
    //maybe create an audio visual class that can be used for all of these where it just stores the stat value and if the stat value reaches thresholds then certain audio visual elements are triggered
    [SerializeField] private AudioVisual fear;
    [SerializeField] private AudioVisual health;
    [SerializeField] private AudioVisual stamina;
    [SerializeField] private GameObject ui;
    
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Inventory") > 0)
        {
            ui.SetActive(!ui.activeSelf);
        }
    }
}
