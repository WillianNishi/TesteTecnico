using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[HelpURL("https://doc.clickup.com/9017157017/p/h/8cqdtct-30337/d473739efa49988")]
public class GroundButton : MonoBehaviour
{
    [Tooltip("The events will be played in order")]
    public EventWithDelay[] Events;

    [Header("Settings")]
    [SerializeField] private Image buttonFill;

    [Header("Animator Settings")]
    [Tooltip("The GameObject that contains the Animator to be activated")]
    [SerializeField] private GameObject targetAnimatorObject;
    private Animator targetAnimator;
    [SerializeField] private string activateTriggerName = "Activate";

    private void Awake()
    {
        Cancell();
    }

    private void Start()
    {
        if (targetAnimatorObject != null)
        {
            targetAnimator = targetAnimatorObject.GetComponent<Animator>();
            if (targetAnimator != null)
            {
                targetAnimatorObject.SetActive(false); // Ensure the target object starts deactivated
            }
        }
    }

    public void Cancell()
    {
        buttonFill.fillAmount = 0;
        StopAllCoroutines();
    }

    public IEnumerator Fill(Transform player)
    {
        buttonFill.fillAmount = 0;

        while (buttonFill.fillAmount < 1)
        {
            buttonFill.fillAmount += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        player.transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        player.transform.forward = transform.forward;
        buttonFill.fillAmount = 0;

        // Ativa o Animator do objeto alvo
        if (targetAnimatorObject != null && targetAnimator != null)
        {
            targetAnimatorObject.SetActive(true);
            targetAnimator.SetTrigger(activateTriggerName);
        }

        StartCoroutine(PlayEvents());
    }

    public IEnumerator PlayEvents()
    {
        foreach (EventWithDelay Event in Events)
        {
            yield return new WaitForSeconds(Event.Delay);
            Event.Events.Invoke();
        }
    }
}

[Serializable]
public class EventWithDelay
{
    public string Name = "New Event";
    [Tooltip("Event delay in seconds")]
    public float Delay;
    public UnityEvent Events;
}