using Model;
using System.Collections.Generic;
using System.ServiceModel;

namespace WcfService
{
    [ServiceContract] 
    public interface IMauiChatService
    {
        [OperationContract(Name = "MauiJoin")]
        void Join(string user);

        [OperationContract(Name = "MauiLeave")]
        void Leave(string user);

        [OperationContract(Name = "MauiSendMessage")]
        void SendMessage(Message msg);

        [OperationContract(Name = "MauiGetUsers")]
        List<User> GetUsers();

        [OperationContract(Name = "MauiGetMessages")]
        List<Message> GetMessages(User user1, User user2);
    }
}
