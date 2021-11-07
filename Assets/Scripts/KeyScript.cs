using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public GameObject infoPanel;
    public GameObject player;
    public bool isDisplayed;
    private float time;
    public bool keyFound;
    // Start is called before the first frame update
    void Start()
    {
        keyFound = false;
        infoPanel.SetActive(false);
        isDisplayed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        time += Time.deltaTime;
        if (isDisplayed && (time >= 1))
        {
            infoPanel.SetActive(false);
            Destroy(gameObject);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            infoPanel.SetActive(true);
            keyFound = true;
            time = 0;
            isDisplayed = true;
        }
    }

}
