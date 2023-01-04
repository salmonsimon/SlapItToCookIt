using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountersUI : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private SlapManager slapManager;
    [SerializeField] private Text slapsText;
    [SerializeField] private Text timePlayedText;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        slapManager.OnCountersChange += UpdateCounters;
    }

    private void OnDisable()
    {
        slapManager.OnCountersChange -= UpdateCounters;
    }

    private void Start()
    {
        UpdateCounters();
    }

    private void Update()
    {
        timePlayedText.text = slapManager.ShowCurrentTimePlayed();
    }

    public void UpdateCounters()
    {
        int currentSlapsCount = slapManager.GetCurrentSlapCount();

        slapsText.text = currentSlapsCount.ToString("#,##0");
        timePlayedText.text = slapManager.ShowCurrentTimePlayed();
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
