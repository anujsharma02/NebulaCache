// Controls communication between model and view
public class CardController
{
    public CardModel model;
    public CardView view;

    public CardController(CardModel m, CardView v)
    {
        model = m;
        view = v;
    }

    // Called when card is clicked
    public void Select()
    {
        GameManager.Instance.SelectCard(this);
    }

    // Called when card is clicked
    public void Flip()
    {
        view.Flip();
    }
}