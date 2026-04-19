using Model;
using System.Collections.Generic;
using System.ServiceModel;
namespace WcfService
{
    [ServiceContract(CallbackContract = typeof(IChatCallback))]
    public interface IChatService
    {
        [OperationContract]
        void Join(string user);

        [OperationContract]
        void Leave(string user); 

        [OperationContract]
        void SendMessage(Message msg);

        [OperationContract]
        List<User> GetUsers();

        [OperationContract]
        List<Message> GetMessages(User user1, User user2);
    }
}
