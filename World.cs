using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace wsserver
{

    public class World : EventManager
    {
        private ConcurrentDictionary<Guid, User> _users;

        private ConcurrentBag<Object> entities;

        public World() : base(){
            _users = new ConcurrentDictionary<Guid, User>();          
        }

        public void AddUser(User user){
            _users.TryAdd(user.id, user);
            Console.WriteLine("{0} users joined", _users.Count);
        }

        public void RemoveUser(User user){
            _users.TryRemove(user.id, out var usuarioRemovido);
            Console.WriteLine("user {0} has leave", usuarioRemovido.id);
        }

        public void loop(){
            while(true){
                Broadcast("prueba");
                Thread.Sleep(1000);
            }
        }

        public async void Broadcast(string mensaje){
            foreach(var user in _users.Values){
                await Task.Run(() => user.Send(mensaje));
            }
        }
    }
}