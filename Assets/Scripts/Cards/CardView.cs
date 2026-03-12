using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CardView : MonoBehaviour
{
    [SerializeField] private Image front;
    [SerializeField] private Image back;
    [SerializeField] private Animator animator;


    private bool canClick = false;

    private CardController controller;
    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Init(CardController c, Sprite sprite)
    {
        controller = c;

        front.sprite = sprite;

        // Disable animator so it doesn't override preview
        animator.enabled = false;
        // show front at start
        front.gameObject.SetActive(true);
        back.gameObject.SetActive(false);

        canClick = false;
        button.interactable = false;
    }

    public void OnClick()
    {
        if (!canClick)
            return;

        controller.Select();
    }

    public void Flip()
    {
        Debug.Log("Flip");
        bool showFront = !front.gameObject.activeSelf;
        front.gameObject.SetActive(showFront);
        back.gameObject.SetActive(!showFront);
    }
    public void ShowFrontInstant()
    {
        front.gameObject.SetActive(true);
        back.gameObject.SetActive(false);
    }
    public void ShowBackInstant()
    {
        front.gameObject.SetActive(false);
        back.gameObject.SetActive(true);
    }
    public void DisableInteraction()
    {
        canClick = false;
        button.interactable = false;
        StartCoroutine(DisableMatchCard());
    }
    IEnumerator DisableMatchCard()
    {
        yield return new WaitForSeconds(1f);
        front.enabled = false;
        back.enabled = false;
    }

    public void EnableInteraction()
    {
        canClick = true;
        button.interactable = true;
        Debug.Log("EnableInteraction");
    }
    public void EnableAnimator()
    {
        animator.enabled = true;
        Debug.Log("EnableAnimator");
    }
}