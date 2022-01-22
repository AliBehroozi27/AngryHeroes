using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public enum Scene
    {
        LevelOne,
        LevelTwo,
        LevelThree,
        Menu
    }
    public static void load(string name)
    {
        SceneManager.LoadScene(name);
    }

    public static void load(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }
}
