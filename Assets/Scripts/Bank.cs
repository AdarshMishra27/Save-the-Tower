using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Bank : MonoBehaviour
{
    [SerializeField] int startingBalance = 150;
    [SerializeField] int currentBalance;
    [SerializeField] TextMeshProUGUI displayBalance;
    public int CurrentBalance
    {
        get
        {
            return currentBalance;
        }
    }

    private void Awake()
    {
        currentBalance = startingBalance;
        updateDisplay();
    }

    public void Deposit(int amount)
    {
        currentBalance += Mathf.Abs(amount);
        updateDisplay();
    }

    public void Withdrawl(int amount)
    {
        currentBalance -= Mathf.Abs(amount);
        updateDisplay();

        if (currentBalance < 0)
        {
            //Lose the game!!!
            reloadManager();
        }
    }

    void reloadManager()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    void updateDisplay()
    {
        displayBalance.text = "Gold : " + currentBalance;
    }
}
