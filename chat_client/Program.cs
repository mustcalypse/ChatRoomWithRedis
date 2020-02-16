namespace chat_app
{
    class Program
    {
        static void Main (string[] args)
        {
            if (!(args.Length == 2 && args[0] != "" && args[1] != ""))
            {
                System.Console.WriteLine ("usage :\nnick_name room_name");
                return;
            }
            var context = new redo_mq.RedisContext ();

            var currentUser = new User { Nickname = args[0] };

            var onlineUsers = new OnlineUsers (context, args[1]);

            onlineUsers.Added += i => System.Console.WriteLine (i.Nickname + " has joined");

            onlineUsers.Removed += i => System.Console.WriteLine (i.Nickname + " has left");

            System.Action<Message> messageReceived = i => System.Console.WriteLine (
                (currentUser.Nickname == i.User_Sent.Nickname? "ME": i.User_Sent.Nickname) + " : " + i.Payload);

            onlineUsers.Add (currentUser);

            StartInteraction (currentUser, onlineUsers, new MessageDeliverer (context, args[1]), messageReceived);
        }

        static void StartInteraction (User _current_user,
            OnlineUsers _online_motherfuckers,

            MessageDeliverer _msg_delivery_service_anonim_sirketi,
            System.Action<Message> msg_delivered)
        {
            _msg_delivery_service_anonim_sirketi.Added += msg_delivered;
            bool isMuted = false;
            while (true)
            {
                var _input = System.Console.ReadLine ();
                if (_input == "!q") { _online_motherfuckers.Remove (_current_user); break; }
                else if (_input == "!o")
                {
                    System.Console.WriteLine ("online users :");
                    foreach (var item in _online_motherfuckers.Get ())
                        System.Console.WriteLine (item.Nickname);
                }
                else if (_input == "!h")
                {
                    System.Console.WriteLine ("message history:");
                    foreach (var item in _msg_delivery_service_anonim_sirketi.Get ())
                        System.Console.WriteLine ($"{item.User_Sent.Nickname} : {item.Payload}");
                }
                else if (_input == "!m" && !isMuted)
                {
                    System.Console.WriteLine ("chat's muted.");
                    _msg_delivery_service_anonim_sirketi.Added -= msg_delivered;
                    isMuted = true;
                }
                else if (_input == "!u" && isMuted)
                {
                    System.Console.WriteLine ("chat's unmuted.");
                    _msg_delivery_service_anonim_sirketi.Added += msg_delivered;
                    isMuted = false;
                }
                else
                {
                    var _msg_to_send = new Message ();
                    _msg_to_send.User_Sent = _current_user;
                    _msg_to_send.Payload = _input;
                    _msg_delivery_service_anonim_sirketi.Add (_msg_to_send);
                }
            }
        }
    }
}
