using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoginChangeInput : MonoBehaviour
{
    EventSystem eventSystem;

    [SerializeField] private Selectable firstInput;
    [SerializeField] private Button submitButton;

    private void OnEnable()
    {
        eventSystem = EventSystem.current;

        firstInput.Select();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) 
        {
            Selectable next = eventSystem.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

            if (next != null)
            {
                next.Select();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            submitButton.onClick.Invoke();
        }
    }
}
