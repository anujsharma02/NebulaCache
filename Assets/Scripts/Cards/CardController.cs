public class CardController
{
    public CardModel model;
    public CardView view;

    public CardController(CardModel m, CardView v)
    {
        model = m;
        view = v;
    }
     public void Select()
    {
        GameManager.Instance.SelectCard(this);
    }

    public void Flip()
    {
        view.Flip();
    }
}