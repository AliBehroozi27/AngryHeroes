using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

public class Menu : MonoBehaviour
{
    public Button StartGameButton;
    public Button NewGame;
    public Button Levels;
    public Button SoundToggle;
    public Button Statistics;
    public Button Level1;
    public Button Level2;
    public Button Level3;
    public Button MainMenu;
    public Button MainMenu2;
    public Image mainMenuPanel;
    public Image levelPanel;
    public Image statisticsPanel;
    public TextMeshProUGUI level1Points;
    public TextMeshProUGUI level2Points;
    public TextMeshProUGUI level3Points;
    public TextMeshProUGUI totalPoints;
    public TextMeshProUGUI BestPoints;
    public AudioSource audio1;
    public AudioSource audio2;
    public AudioSource audioClick;
    public AudioSource audioBackClick;
    [SerializeField] Material dissolveMaterial;


    int currentLevel = 1;
    void Start()
    {
        StartCoroutine(ChangeDissolveMaterialScale());
        currentLevel = PlayerPrefs.GetInt(Prefs.CURRENT_LEVEL);
        if (currentLevel <= 1)
        {
            NewGame.interactable = false;
        }

        StartGameButton.onClick.AddListener(() => StartGame());
        NewGame.onClick.AddListener(() => StartNewGame());
        Levels.onClick.AddListener(() => GoToLevelMenu());
        Statistics.onClick.AddListener(() => GoToStatisMenu());
        SoundToggle.onClick.AddListener(() => ToggleSound());
        Level1.onClick.AddListener(() => GoToLevel(SceneLoader.Scene.LevelOne));
        Level2.onClick.AddListener(() => GoToLevel(SceneLoader.Scene.LevelTwo));
        Level3.onClick.AddListener(() => GoToLevel(SceneLoader.Scene.LevelThree));
        MainMenu.onClick.AddListener(() => GoToMainMenu());
        MainMenu2.onClick.AddListener(() => GoToMainMenu());
    }

    private void ToggleSound()
    {
        bool isSoundOn = PlayerPrefs.GetInt(Prefs.SOUND) == 0;
        if (isSoundOn)
        {
            PlayerPrefs.SetInt(Prefs.SOUND, 1);
            SoundToggle.GetComponentInChildren<TextMeshProUGUI>().text = "Sound Off";
        }
        else
        {
            PlayerPrefs.SetInt(Prefs.SOUND, 0);
            SoundToggle.GetComponentInChildren<TextMeshProUGUI>().text = "Sound On";
        }
    }

    private IEnumerator ChangeDissolveMaterialScale()
    {
        float scale = 5f, scaleAddition = 0.1f;
        for (int i = 0; i < 400; i++)
        {
            yield return new WaitForSeconds(0.1f);

            scale += scaleAddition;

            dissolveMaterial.SetFloat("_DissolveScale", scale);
        }

        for (int i = 0; i < 400; i++)
        {
            yield return new WaitForSeconds(0.1f);

            scale -= scaleAddition;

            dissolveMaterial.SetFloat("_DissolveScale", scale);
        }

        StartCoroutine(ChangeDissolveMaterialScale());
    }

    private void GoToStatisMenu()
    {
        audioClick.Play();

        mainMenuPanel.gameObject.SetActive(false);
        statisticsPanel.gameObject.SetActive(true);

        float levelPoints1 = PlayerPrefs.GetFloat(Prefs.LEVEL_POINTS_1);
        float levelPoints2 = PlayerPrefs.GetFloat(Prefs.LEVEL_POINTS_2);
        float levelPoints3 = PlayerPrefs.GetFloat(Prefs.LEVEL_POINTS_3);
        float bestPoints = PlayerPrefs.GetFloat(Prefs.BEST_POINTS);
        float total = levelPoints3 + levelPoints2 + levelPoints1;

        if (bestPoints < total)
        {
            PlayerPrefs.SetFloat(Prefs.BEST_POINTS, total);
            bestPoints = total;
        }

        level1Points.text = "Level1: " + levelPoints1;
        level2Points.text = "Level2: " + levelPoints2;
        level3Points.text = "Level3: " + levelPoints3;
        BestPoints.text = "Best: " + bestPoints;
        totalPoints.text = "Total: " + total;
    }

    private void GoToMainMenu()
    {
        audioBackClick.Play();

        mainMenuPanel.gameObject.SetActive(true);
        levelPanel.gameObject.SetActive(false);
        statisticsPanel.gameObject.SetActive(false);
    }

    private void GoToLevel(SceneLoader.Scene level)
    {
        audioClick.Play();

        SceneLoader.load(level);
    }

    private void GoToLevelMenu()
    {
        audioClick.Play();
        mainMenuPanel.gameObject.SetActive(false);
        levelPanel.gameObject.SetActive(true);

        Level2.interactable = currentLevel > 1;
        Level3.interactable = currentLevel > 2;
    }
    
    private void StartGame()
    {
        audioClick.Play();
        if (currentLevel == 3)
        {
            SceneLoader.load(SceneLoader.Scene.LevelThree);
        }
        else if(currentLevel == 2)
        {
            SceneLoader.load(SceneLoader.Scene.LevelTwo);
        }
        else
        {
            SceneLoader.load(SceneLoader.Scene.LevelOne);
        }
    }

    private void StartNewGame()
    {
        audioClick.Play();
        PlayerPrefs.SetInt(Prefs.CURRENT_LEVEL, 1);
        PlayerPrefs.SetFloat(Prefs.LEVEL_POINTS_1, 0);
        PlayerPrefs.SetFloat(Prefs.LEVEL_POINTS_2, 0);
        PlayerPrefs.SetFloat(Prefs.LEVEL_POINTS_3, 0);
        SceneLoader.load(SceneLoader.Scene.LevelOne);
    }
}
