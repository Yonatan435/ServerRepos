using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Exercise
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class Server : IServer
    {
        List<KeyValuePair<string, string>> users;
        bool _sendData;
        Random _randomNumbers, _randomBytes;
        Timer _timer;
        OperationContext _context;
        
        public Server()
        {
            _randomNumbers = new Random();
            _randomBytes = new Random();
            users = new List<KeyValuePair<string, string>>();
            users.Add(new KeyValuePair<string, string>("1", "2"));
            _timer = new Timer(2000);
        }
        
        public bool Authenticate(string userName, string password)
        {
            bool userExists = users.Find(x => x.Key == userName && x.Value == password).Equals(new KeyValuePair<string, string>(userName, password));
            if (userExists)
            {
                _context = OperationContext.Current;
                StartSendingInt();
            }
            return userExists;
        }

        private void StartSendingInt()
        {
            _timer.Elapsed += OnTimerElapsed;
            _timer.Start();
        }

        private void OnTimerElapsed(Object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                IServerCallback callBack = _context.GetCallbackChannel<IServerCallback>();
                int randomNumber = _randomNumbers.Next();
                callBack.ReceiveInt(randomNumber);
                Console.WriteLine("Sending random number: {0}", randomNumber);
            }
            catch(Exception ex)
            { Console.WriteLine("Error sending number"); }
        }

        public void StartDataTransfer()
        {
            try
            {
                IServerCallback callBack = _context.GetCallbackChannel<IServerCallback>();
                
                _sendData = true;
                byte[] bytes;
                while (_sendData)
                {
                    bytes = new byte[1000];
                    _randomBytes.NextBytes(bytes);
                    callBack.ReceiveData(bytes);
                }
            }
            catch (Exception ex)
            { Console.WriteLine("Error sending data"); }
        }
        public void StopDataTransfer()
        {
            _sendData = false;
        }
        public void Logout()
        {
            _sendData = false;
            _timer.Stop();
        }
    }
}
