using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagement : MonoBehaviour
{
    public Animator animator;
    private Transform player;
    private Transform goal;
    private PlayerGoalGenerator playerGoal;
    private bool isMainScene = false;
    private bool isGrassland;
    private ChickFollow firstChick;
    private bool ifChickFollow;
    [SerializeField] private float playerGoalDist = 15;
    [SerializeField] private float transitionTime = 2;
    [SerializeField] private GameObject loadingCanvas;
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private Slider progressSlider;


    // Start is called before the first frame update
    void Start()
    {

        if (SceneManager.GetActiveScene().name == "GrasslandScene"
            || SceneManager.GetActiveScene().name == "DesertScene")
        {
            isMainScene = true;
            playerGoal = FindObjectOfType<PlayerGoalGenerator>();
            player = GameObject.FindGameObjectWithTag("Player").transform;
            goal = GameObject.FindGameObjectWithTag("Goal").transform;
            firstChick = FindObjectOfType<ChickensGenerator>().GetFirstChick().GetComponent<ChickFollow>();

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMainScene) return;

        bool ifChickFollow = firstChick.GetFollow();

        if (Vector3.Distance(player.position, goal.position) <= playerGoalDist && ifChickFollow == true)
        {
            AudioManager.Instance.StopClip("GrasslandBGM");
            AudioManager.Instance.StopClip("BGM");

            SceneManager.LoadScene("EndCutscene");
            AudioManager.Instance.PlayClip("EndBGM");
        }
    }

    public void LoadNextScene()
    {
        var name = SceneManager.GetActiveScene().name;
        if (name == "GrasslandScene" || name == "DesertScene")
            StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
        else
        {
            AudioManager.Instance.StopClip("MenuBGM");
            if (Random.Range(0, 2) == 0)
            {
                StartCoroutine(LoadGame("GrasslandScene"));
                isGrassland = true;
                AudioManager.Instance.PlayClip("GrasslandBGM");
            }
            else
            {
                StartCoroutine(LoadGame("DesertScene"));
                isGrassland = false;
                AudioManager.Instance.PlayClip("BGM");
            }
        }

    }

    IEnumerator LoadScene(int sceneIndex)
    {
        animator.SetTrigger("Start");


        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneIndex);

    }

    public IEnumerator LoadGame(string sceneName)
    {
        progressSlider.value = 0;
        menuCanvas.SetActive(false);
        loadingCanvas.SetActive(true);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;
        float progress = 0;

        while (!asyncOperation.isDone)
        {
            progress = Mathf.MoveTowards(progress, asyncOperation.progress, Time.deltaTime/5);
            progressSlider.value = progress;
            if (progress >= 0.9f)
            {
                progressSlider.value = 1;
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;

        }

    }

    public bool GetIsGrassland()
    {
        return isGrassland;
    }
}
