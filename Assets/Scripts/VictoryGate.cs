using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryGate : MonoBehaviour
{
    public GameObject infoPanelLocked;
    public bool displayLockedText;
    private float time;

    public GameObject key;
    public KeyScript keyScript;
    // Start is called before the first frame update
    void Start()
    {
        infoPanelLocked.SetActive(false);
        displayLockedText = false;
    }

    // Update is called once per frame
    void Update()
    {
        key = GameObject.Find("Dungeon_Key_Set");

        keyScript = key.GetComponent<KeyScript>();

        time += Time.deltaTime;
        if (displayLockedText && (time >= 1))
        {
            infoPanelLocked.SetActive(false);
            displayLockedText = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (keyScript.keyFound == true)
            {
                SceneManager.LoadScene(3);
            } else
            {
                time = 0;
                infoPanelLocked.SetActive(true);
                displayLockedText = true;
            }
        }
    }
}
