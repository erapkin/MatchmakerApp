using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace ChatServiceAssembly
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode=ConcurrencyMode.Single, UseSynchronizationContext = false)]
    public class ChatService : IChat
    {
        Dictionary<Client, IChatCallback> clients = new Dictionary<Client, IChatCallback>();

        List<Client> listOfClients = new List<Client>();
        //BFT TODO: Move this to top  EAR: complete
        object padLock = new object();
        public IChatCallback CurrentCallback
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<IChatCallback>();
            }
        }

        //BFT TODO: Return client or change name  EAR: will do after chat client can send msgs - debug assembly is currently being referenced 
        public bool SearchClientsByName(string name)
        {
            foreach (Client client in clients.Keys)
            {
                if (client.Name == name)
                {
                    return true;
                }
            }
            return false;
        }
        public bool Connect (Client client)
        {
            if (!clients.ContainsValue(CurrentCallback) && !SearchClientsByName(client.Name))
            { 
                lock (padLock)
                {
                    clients.Add(client, CurrentCallback);
                    listOfClients.Add(client);

                    foreach (Client key in clients.Keys)
                    {
                        IChatCallback callback = clients[key];
                        try
                        {
                            callback.RefreshClients(listOfClients);
                            callback.UserJoin(client);
                        }
                        catch
                        {
                            clients.Remove(key);
                            return false;
                        }
                    }
                }
                return false;
            }
            return true;
        }

        //BFT TODO: Is it sending or receiving? EAR: renamed method: it will say the sent msg to the client, callback is on receive. 
        public void Say (Message msg)
        {
            lock (padLock)
            {
                foreach (IChatCallback callback in clients.Values)
                {
                    callback.Receive(msg);
                }
            }
        }

        //DG TODO:  possibly rename or have it return a boolean  (ex; check client for data) 
        public void IsWriting(Client client)
        {
            lock (padLock)
            {
                foreach (IChatCallback callback in clients.Values)
                {
                    callback.IsWritingCallback(client);
                }
            }
        }
        //DG TODO: change foreach loop and if statement into a containskey method from dictioanry
        public void Disconnect(Client client)
        {
           
            foreach (Client cl in clients.Keys)
                if (client.Name == cl.Name)
                {
                    lock (padLock)
                    {
                        this.clients.Remove(cl);
                        this.listOfClients.Remove(cl);
                        
                        foreach (IChatCallback callback in clients.Values)
                        {
                            callback.RefreshClients(this.listOfClients);
                            callback.UserLeave(client);
                        }
                    }
                    return;
                }
        }
    }
}
