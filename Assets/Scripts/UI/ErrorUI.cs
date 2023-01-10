using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorUI : MonoBehaviour
{
    [SerializeField] private Text errorText;

    private float messageDuration = Config.ERROR_MESSAGE_DURATION;

    public IEnumerator ShowErrorMessage(string message)
    {
        GameManager.instance.GetSFXManager().PlaySound(Config.WRONG_SFX);

        errorText.text = message;
        gameObject.SetActive(true);

        yield return new WaitForSeconds(messageDuration);

        gameObject.SetActive(false);
        ClearMessageText();
    }

    private void ClearMessageText()
    {
        errorText.text = string.Empty;
    }
}
