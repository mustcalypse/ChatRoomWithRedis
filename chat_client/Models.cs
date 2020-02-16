using redo_mq;

namespace chat_app
{
    public class User
    {
        public string Nickname { get; set; }
    }
    public struct Message
    {
        public User User_Sent { get; set; }
        public string Payload { get; set; }
    }

    public class OnlineUsers : RedisHelper<User>
    {
        public OnlineUsers (RedisContext st, string room_name) : base (st, $"online_users-{room_name}") { }
    }
    public class MessageDeliverer : RedisHelper<Message>
    {
        public MessageDeliverer (RedisContext st, string room_name) : base (st, $"messages-{room_name}") { }
    }
}
