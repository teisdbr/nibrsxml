namespace NibrsXml
{
    public class HTTPDataObjectTransport<T1, T2>
    {
        public HTTPDataObjectTransport(T1 dataObject1, T2 dataObject2)
        {
            DataObject1 = dataObject1;
            DataObject2 = dataObject2;
        }
        public T1 DataObject1;
        public T2 DataObject2;
    }
}
