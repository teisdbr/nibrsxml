namespace NibrsXml
{
    public class HTTPDataObjectTransport<T1, T2, T3>
    {
        public HTTPDataObjectTransport(T1 dataObject1, T2 dataObject2, T3 dataObject3)
        {
            DataObject1 = dataObject1;
            DataObject2 = dataObject2;
            DataObject3 = dataObject3;
        }
        public T1 DataObject1;
        public T2 DataObject2;
        public T3 DataObject3;
    }
}
