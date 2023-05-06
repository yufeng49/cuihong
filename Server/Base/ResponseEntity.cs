namespace Server.Base
{
    public class ResponseEntity<T>
    {
        public string code;

        public string msg;

        public T data;


        public string success;
        public string message;
        public object timestamp;
    }
}
