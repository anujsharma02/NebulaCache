[System.Serializable]
public class CardModel
{
    public int id;
    public bool matched;

    public CardModel(int id)
    {
        this.id = id;
        matched = false;
    }
}