using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountersUI : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private Text slapsText;
    [SerializeField] private Text timePlayedText;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        UpdateCounters();
    }

    private void Update()
    {
        timePlayedText.text = GameManager.instance.GetCurrentProgressManager().ShowCurrentTimePlayed();
    }

    public void UpdateCounters()
    {
        int currentSlapsCount = GameManager.instance.GetCurrentProgressManager().GetCurrentSlapCount();

        slapsText.text = currentSlapsCount.ToString("#,##0");
        timePlayedText.text = GameManager.instance.GetCurrentProgressManager().ShowCurrentTimePlayed();
    }

    public void ShowCounters(bool value)
    {
        if (value)
        {
            animator.SetTrigger(Config.ANIMATOR_SHOW_COUNTERS);
        }
        else
        {
            animator.SetTrigger(Config.ANIMATOR_HIDE_COUNTERS);
        }
    }
}
