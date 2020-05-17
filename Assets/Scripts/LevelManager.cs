using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    private static LevelManager _instance = null;

    private static int currentLevel = 0;

    public static LevelManager instance
    {
        get
        {
            return _instance;
        }
    }


    void Awake()
    {
        if(null == instance)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            if(instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ClearLevel()
    {
        Debug.Log("Cleared the Level!");
        SceneManager.LoadScene(++currentLevel);
    }
}
