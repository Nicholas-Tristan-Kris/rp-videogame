using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    [SerializeField] private Fear fear;
    [SerializeField] private Health health;
    [SerializeField] private Stamina stamina;
    [SerializeField] private GameObject inventory;
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
            inventory.SetActive(!inventory.activeSelf);
        }
    }
}
