using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public PlayerController playerController;

    public GameObject locked;

    public GameObject unLocked;

    public GameObject confirmMessage;

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void RemoveStats()
    {
        PlayerPrefs.DeleteAll();
        GameManager.Instance.maxDeathTimer = 20;
        PlayerAttack.Instance.damage = 1;
        playerController.walkSpeed = 5;
        MenuUpGradeScript.Instance.doubleJumpUnLocked = 0;

        // playerController.canDoubleJump = false;
        GameManager.Instance.vanquishedAmt = 0;

        locked.SetActive(true);
        unLocked.SetActive(false);
    }

    public void HideConfirmMessage()
    {
        confirmMessage.SetActive(false);
    }

    public void ShowConfirmMessage()
    {
        confirmMessage.SetActive(true);
    }
}
