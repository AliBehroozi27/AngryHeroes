using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] int level = 1;
    [SerializeField] Hero[] heroes;
    [SerializeField] AntiHero[] antiHeroes;
    [SerializeField] Box[] boxs;
    [SerializeField] Vector3 heroPosition = new Vector3(-21, 5, 0);
    [SerializeField] public static Vector3 luanchPosition = new Vector3(-16, 3, 0);
    int heroIndex;
    float points;

    public Button btnNextLevel;
    public Button btnRetry;
    public Button btnMenu;
    public Button btnRestart;
    public Button btnMainMenu;
    public Button btnSound;
    public Button btnGoToLevelOne;
    public Image menuPanel;
    public Image statusPanel;
    public TextMeshProUGUI txtFinalPoints;
    public TextMeshProUGUI txtFinalPointsTitle;
    public TextMeshProUGUI txtPoints;
    public AudioSource audioClick;

    bool allEnemiesAreDead;
    void Start()
    {
        setupButtons();
        arrangeHeors();
        setupActions();
    }

    private void setupButtons()
    {
        btnNextLevel.onClick.AddListener(() => goToNextLevel());
        btnRetry.onClick.AddListener(() => {
            audioClick.Play();
            SceneLoader.load(SceneManager.GetActiveScene().name);
            });
        btnRestart.onClick.AddListener(() => {
            audioClick.Play();
            SceneLoader.load(SceneManager.GetActiveScene().name);
            });
        btnGoToLevelOne.onClick.AddListener(() => {
            audioClick.Play();
            SceneLoader.load(SceneLoader.Scene.LevelOne);
            });
        btnSound.onClick.AddListener(() => toggleSound());
        btnMainMenu.onClick.AddListener(() => {
            audioClick.Play();
            SceneLoader.load(SceneLoader.Scene.Menu);
            });
        btnMenu.onClick.AddListener(() => {
            audioClick.Play();
            menuPanel.gameObject.SetActive(!menuPanel.gameObject.activeInHierarchy);
            });
    }

    private void goToNextLevel()
    {
        audioClick.Play();
        if (level == 1)
        {
            SceneLoader.load(SceneLoader.Scene.LevelTwo);
        }
        else if (level == 2)
        {
            SceneLoader.load(SceneLoader.Scene.LevelThree);
        }
    }

    private void toggleSound()
    {
        audioClick.Play();
       
        bool isSoundOn = PlayerPrefs.GetInt(Prefs.SOUND) == 0;
        if (isSoundOn)
        {
            PlayerPrefs.SetInt(Prefs.SOUND, 1);
            btnSound.GetComponentInChildren<TextMeshProUGUI>().text = "Sound Off";
        }
        else
        {
            PlayerPrefs.SetInt(Prefs.SOUND, 0);
            btnSound.GetComponentInChildren<TextMeshProUGUI>().text = "Sound On";
        }
    }

    private void setupActions()
    {
        foreach (Hero hero in heroes)
        {
            hero.onLuanchAction += () => onHeroLaunched();
        }

        foreach (Box box in boxs)
        {
            box.onDestroyAction += (point) => onBoxDestroy(point);
        }

        foreach (AntiHero antiHero in antiHeroes)
        {
            antiHero.onDestroyAction += (point) => onAntiHeroDestroy(point);
        }
    }

    void Update()
    {
        
    }

    private void arrangeHeors()
    {
        if (heroIndex >= heroes.Length)
        {
            StartCoroutine(setEndGameState());
            return;
        }

        Vector3 hp = heroPosition;

        heroes[heroIndex].transform.position = luanchPosition;
        heroes[heroIndex].isReadyToLaunch = true;

        for(int i=heroIndex+1; i < heroes.Length; i++)
        {
            heroes[i].transform.position = hp;
            hp.y -= 1.5f;
        }
    }
    private void updatePoints()
    {
        txtPoints.text = "Points: " + points.ToString();
    }
    public void onHeroLaunched()
    {
        StartCoroutine(loadNextHero());
    }

    public void onBoxDestroy(float point)
    {
        points += point;
        updatePoints();
    }

    public void onAntiHeroDestroy(float point)
    {
        points += point;
        updatePoints();

        foreach(AntiHero antiHero in antiHeroes)
        {
            if (antiHero.gameObject.activeInHierarchy)
            {
                return;
            }
        }

        allEnemiesAreDead = true;
        StartCoroutine(setEndGameState());
    }

    private IEnumerator setEndGameState()
    {
        yield return new WaitForSeconds(6f);

        if (allEnemiesAreDead)
        {
            foreach (Hero hero in heroes)
            {
                if (hero.gameObject.activeInHierarchy)
                {
                    points += hero.point;
                }
            }
            int currentLevel = PlayerPrefs.GetInt(Prefs.CURRENT_LEVEL);
            if (currentLevel < 2)
            {
                PlayerPrefs.SetInt(Prefs.CURRENT_LEVEL, 2);
            }
            else if (currentLevel == 2)
            {
                PlayerPrefs.SetInt(Prefs.CURRENT_LEVEL, 3);
            }
        }

        btnNextLevel.interactable = allEnemiesAreDead && level < 3;


        if (level == 1)
        {
            PlayerPrefs.SetFloat(Prefs.LEVEL_POINTS_1, points);
        } else if (level == 2)
        {
            PlayerPrefs.SetFloat(Prefs.LEVEL_POINTS_2, points);
        } else
        {
            PlayerPrefs.SetFloat(Prefs.LEVEL_POINTS_3, points);
        }

        txtFinalPoints.text = points.ToString();
        menuPanel.gameObject.SetActive(false);
        btnMenu.gameObject.SetActive(false);
        txtPoints.gameObject.SetActive(false);
        statusPanel.gameObject.SetActive(true);

        
    }

    public IEnumerator loadNextHero()
    {
        yield return new WaitForSeconds(0.5f);
        heroIndex++;
        arrangeHeors();
    }
}
