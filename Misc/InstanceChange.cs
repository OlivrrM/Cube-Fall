using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstanceChange : MonoBehaviour
{
    public Animator transitionAnimator;
    public static bool changingScene;
    public static string targetInstanceName;
    public static float targetSpeed;
    bool startedChangingScene;

    public static bool exitingGame;
    bool startedExitingGame;

    bool setInstanceStartSpeed;
    public static void LoadInstance(string instanceName, float speed)
    {
        changingScene = true;
        targetInstanceName = instanceName;
        targetSpeed = speed;
    }
    public static void ExitApplication(float speed)
    {
        exitingGame = true;
        targetSpeed = speed;
    }
    void Update()
    {
        if (changingScene)
        {
            if (!startedChangingScene)
            {
                transitionAnimator.SetBool("Exiting", true);
                transitionAnimator.speed = targetSpeed;
                StartCoroutine(ChangeInstance(targetInstanceName));
                startedChangingScene = true;
            }
        }
        else if (exitingGame)
        {
            if (!startedExitingGame)
            {
                transitionAnimator.SetBool("Exiting", true);
                transitionAnimator.speed = targetSpeed;
                StartCoroutine(Exit());
            }
        }
        //print(StartTime.instanceFrame);
        if (!setInstanceStartSpeed)
        {
            if (StartTime.instanceFrame > 2) { transitionAnimator.speed = 1; setInstanceStartSpeed = true; }
        }
    }
    IEnumerator Exit()
    {
        yield return new WaitForSeconds(1 / targetSpeed);
        ResetVars();
        Application.Quit();
    }
    IEnumerator ChangeInstance(string instanceName)
    {
        yield return new WaitForSeconds(1 / targetSpeed);
        ResetVars();
        SceneManager.LoadScene(instanceName);
    }
    void ResetVars()
    {
        changingScene = false;
        targetInstanceName = "";
        targetSpeed = 1;
        exitingGame = false;
    }
}
