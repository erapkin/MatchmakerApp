﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ChatServiceAssembly
{
    [ServiceContract(CallbackContract = typeof(IChatCallback), SessionMode = SessionMode.Required)]
    public interface IChat
    {
        [OperationContract(IsInitiating = true)]
        bool Connect(Client client);

        [OperationContract(IsOneWay = true)]
        void SendChat(Message msg);

        [OperationContract(IsOneWay = true)]
        void Whisper(Message msg, Client receiver);

        [OperationContract(IsOneWay = true)]
        void IsWriting(Client client);

        [OperationContract(IsOneWay = true, IsTerminating = true)]
        void Disconnect(Client client);
    }
}
