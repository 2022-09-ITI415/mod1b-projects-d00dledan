using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayController : MonoBehaviour
{

    public static GameplayController instance;

    public GameObject fruit_PickUp;
    public GameObject bomb_PickUp;

    //set up boundaries to spawn objects within game boundaries 
    private float min_X = -10f;
    private float max_X = 10f;
    private float min_Y = -6.2f;
    private float max_Y = 6.2f;
    private float z_Pos = 5.8f;

    private Text score_Text;
    private int scoreCount;
    // Start is called before the first frame update
    void Awake()
    {
        MakeInstance();
    }

    private void Start()
    {
        score_Text = GameObject.Find("Score").GetComponent<Text>();
        Invoke("StartSpawning", 0.5f);
    }

    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void StartSpawning()
    {
        StartCoroutine(SpawnPickUps());
    }

    public void CancelSpawning()
    {
        CancelInvoke("StartSpawning");
    }

    IEnumerator SpawnPickUps()
    {
        yield return new WaitForSeconds(Random.Range(1f, 1.5f));

        //create a new fruit object on these coordinates if number is greater than 2
        if (Random.Range(0, 10) >= 2)
        {
            Instantiate(fruit_PickUp, new Vector3(Random.Range(min_X, max_X), Random.Range(min_Y, max_Y), z_Pos), Quaternion.identity);
        }
        else
        {
            Instantiate(bomb_PickUp, new Vector3(Random.Range(min_X, max_X), Random.Range(min_Y, max_Y), z_Pos), Quaternion.identity);
        }

        Invoke("StartSpawning", 0f);

    }

    public void IncreaseScore()
    {
        scoreCount++;
        score_Text.text = "Score: " + scoreCount;
    }

}
