using Model;
using System.ServiceModel;

namespace WcfService
{
    [ServiceContract]
    public interface IChatCallback
    {
        [OperationContract(IsOneWay = true)]
        void ReceiveMessage(Message message);

        [OperationContract(IsOneWay = true)]
        void UserJoined(string username);
    }
}