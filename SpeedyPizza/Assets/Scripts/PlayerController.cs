using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections.Specialized;


public class PlayerController : MonoBehaviour
{
    public float speed;
    private Rigidbody rig;

    private float startTime = 15f;
    private float timeTaken;
    private float countdown;
    private float timer;

    private int collectabledPicked = 40;
    public int maxCollectables = 10;
    private int TotalPoints;

    public GameObject playButton;
    public TextMeshProUGUI curTimeText;
    public TextMeshProUGUI AmountText;
    public GameObject item;

    private bool isPlaying;
    

    void Awake ()
    {
        rig = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        if(!isPlaying)
            return;
        if (timer < 0f)
        {
            timer = 0;
            End();
        }
        float x = Input.GetAxis("Horizontal") * speed;
        float z = Input.GetAxis("Vertical") * speed;

        rig.velocity = new Vector3(x, rig.velocity.y, z);

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        curTimeText.text = (timer).ToString("F2");
        AmountText.text = (TotalPoints).ToString();
    }

    private void AddScore(int score)
    {
        TotalPoints += score;
        collectabledPicked--;
    }

    public void Begin ()
    {
        TotalPoints = 0;
        timer = startTime - Time.deltaTime;
        isPlaying = true;
        playButton.SetActive(false);
    }

    void End ()
    {
        timeTaken = Time.time - startTime;
        isPlaying = false;
        Debug.Log("Current Entity:" + TotalPoints);
        LeaderBoard.instance.SetLeaderboardEntry(TotalPoints);
        //LeaderBoard.instance.SetLeaderboardEntry(-Mathf.RoundToInt(timeTaken * 1000.0f));
        playButton.SetActive(true);
        
    }

    void OnTriggerEnter (Collider other)
    {
        collectabledPicked--;
        if (other.gameObject.CompareTag("Collectables"))
        {
            
            Debug.Log(collectabledPicked);
            AddScore(Random.Range(1, 50));
        }
            

        collectabledPicked++;
        Destroy(other.gameObject);

        //if (collectabledPicked == maxCollectables)
        //End();
        Quaternion rotation = Quaternion.Euler(0, 0, 0);
        Vector3 randomizePosition = new Vector3(Random.Range(0, 0), 0, Random.Range(0, 0));
        if (collectabledPicked <= 0)
        {
            collectabledPicked = 40;
            Instantiate(item, randomizePosition, rotation);
        }
    }
}
