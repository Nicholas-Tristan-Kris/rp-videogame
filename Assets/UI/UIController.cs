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
    private PlayerManager playerManager;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
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
