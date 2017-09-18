namespace wsserver
{
    public class Event
    {
        public string name { get; set; }
        public User user { get; set; }
        public object data { get; set; }
        public Event(string name, User user, object data)
        {
            this.name = name;
            this.user = user;
            this.data = data;

        }

    }
}