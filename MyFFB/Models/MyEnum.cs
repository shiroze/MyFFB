namespace MyAttd.Models
{
    public class MyEnum
    {
        public string ID { get; set; }
        public int _ID { get; set; }
        public string Value { get; set; }

        public decimal myValue { get; set; }
        public string myDesc { get; set; }
    }

    public enum product
    {
        CPO,
        PK,
    }

    public enum ListSendEmail
    {
        blind_copy_recipients,
        copy_recipients,
        recipients,
    }

}
