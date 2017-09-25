using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace wsserver
{
    public class User
    {
        public User(Guid id, WebSocket socket) 
        {
            this.id = id;
                this.socket = socket;
               
        }
                public Guid id {get; set;}
        public WebSocket socket {get; set;}

        public int x = 0;
        public int y = 0;

        public User(WebSocket websocket){
            id = Guid.NewGuid();
            socket = websocket;
        }

        public async void Send(String mensaje){
            var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(mensaje));
            await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async void Send(Object mensaje){
            var json = JsonConvert.SerializeObject(mensaje);
            var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(json));
            await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }
        
    }
}