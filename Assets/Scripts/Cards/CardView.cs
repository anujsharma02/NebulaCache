using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    public Image front;
    public Image back;

    Animator anim;

    public CardController controller;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Init(CardController c, Sprite sprite)
    {
        controller = c;
        front.sprite = sprite;
    }

    public void OnClick()
    {
        Debug.Log("On Click Working");
    }

    public void Flip()
    {
        anim.SetTrigger("Flip");
    }
}
