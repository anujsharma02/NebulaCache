using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CardView : MonoBehaviour
{
    [SerializeField] private Image front;
    [SerializeField] private Image back;
    [SerializeField] private Animator animator;


    private bool canClick;
    private CardController controller;
    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    // Initialize card with sprite
    public void Init(CardController c, Sprite sprite)
    {
        controller = c;
        front.sprite = sprite;
        animator.enabled = false;
        ShowFrontInstant();
        canClick = false;
        button.interactable = false;
    }

    // UI click event
    public void OnClick()
    {
        if (!canClick) return;

        controller.Select();
    }

    // Flip card
    public void Flip()
    {
        bool showFront = !front.gameObject.activeSelf;
        if (showFront == front.gameObject.activeSelf)
            return;
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

    // Disable matched card
    public void DisableInteraction()
    {
        canClick = false;
        button.interactable = false;
        StartCoroutine(HideMatchedCard());
    }
    IEnumerator HideMatchedCard()
    {
        yield return new WaitForSeconds(1f);
        front.enabled = false;
        back.enabled = false;
    }

    // Enable clicking
    public void EnableInteraction()
    {
        canClick = true;
        button.interactable = true;
    }
    public void EnableAnimator()
    {
        animator.enabled = true;
    }
}