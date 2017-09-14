using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

namespace wsserver
{
    public class User
    {
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
        
    }
}