using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Exercise
{
    
    [ServiceContract(/*Name ="IServer", SessionMode =SessionMode.Allowed,*/ CallbackContract = typeof(IServerCallback))]

    public interface IServer
    {
        [OperationContract]
        bool Authenticate(string userName, string password);
        [OperationContract(IsOneWay = true)]
        void StartDataTransfer();
        [OperationContract(IsOneWay = true)]
        void StopDataTransfer();
        [OperationContract(IsOneWay = true)]
        void Logout();
    }
    public interface IServerCallback
    {
        [OperationContract(IsOneWay = true)]
        void ReceiveInt(int number);
        [OperationContract(IsOneWay = true)]
        void ReceiveData(byte[] data);

    }
}
