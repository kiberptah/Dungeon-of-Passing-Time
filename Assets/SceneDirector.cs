using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class SceneDirector : MonoBehaviour
{
    private void OnEnable()
    {
        EventDirector.scene_Reset += ResetScene;
    }
    private void OnDisable()
    {
        EventDirector.scene_Reset -= ResetScene;
    }

    void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
