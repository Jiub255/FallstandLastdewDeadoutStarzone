/// <summary>
/// Will this work as long as T and S are serializable? 
/// </summary>
[System.Serializable]
public class SerializableDouble<T, S>
{
    public T Item1;
    public S Item2;

    public SerializableDouble(T item1, S item2)
    {
        Item1 = item1;
        Item2 = item2;
    }
}