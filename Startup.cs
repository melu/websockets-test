using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace wsserver
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseWebSockets();
            var _websockets = new ConcurrentBag<WebSocket>();

            var world = new World();
            
            Task.Run(() => world.loop());

            app.Use(async (context, next) =>
                {
                    if (context.Request.Path == "/ws")
                    {
                        if (context.WebSockets.IsWebSocketRequest)
                        {
                            WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

                            // _websockets.Add(webSocket);
                            var user = new User(webSocket);
                            world.AddUser(user);

                            while (webSocket.State == WebSocketState.Open)
                            { 
                                var token = CancellationToken.None; 
                                var buffer = new ArraySegment<Byte>(new byte[1024 * 4]);
                                // var buffer = new ArraySegment<Byte>(new Byte[4096]);
                                var received = await webSocket.ReceiveAsync(buffer, token);

                                switch (received.MessageType)
                                {
                                    case WebSocketMessageType.Text:
                                        var request = Encoding.UTF8.GetString(buffer.Array, 
                                            buffer.Offset, 
                                            buffer.Count);

                                        world.Broadcast(request);
                                        break;
                                }
                            }

                            world.RemoveUser(user);
                        }
                        else
                        {
                            context.Response.StatusCode = 400;
                        }
                    }
                    else
                    {
                        await next();
                    }

                });
            app.UseFileServer();
        }

        private async Task Echo(HttpContext context, WebSocket webSocket)
        {
            // WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (webSocket.State == WebSocketState.Open)
            { 
                var token = CancellationToken.None; 
                var buffer = new ArraySegment<Byte>(new byte[1024 * 4]);
                // var buffer = new ArraySegment<Byte>(new Byte[4096]);
                var received = await webSocket.ReceiveAsync(buffer, token);

                switch (received.MessageType)
                {
                    case WebSocketMessageType.Text:
                        var request = Encoding.UTF8.GetString(buffer.Array, 
                            buffer.Offset, 
                            buffer.Count);
                        break;
                }
            }


            // Console.WriteLine("result: closestatus: "+result.CloseStatus);
            // Console.WriteLine("result: closestatusdescription: "+result.CloseStatusDescription);
            // Console.WriteLine("count: "+result.Count);
            // Console.WriteLine("messageType: "+result.MessageType);
            // Console.WriteLine("context.count: "+context.Items.Count);
            // var item = context.Items.FirstOrDefault();
            // Console.WriteLine("{0} -> {1}", item.Key, item.Value);
            // Console.WriteLine("buffer: "+Encoding.UTF8.GetString(buffer).Trim());


            // while (!result.CloseStatus.HasValue)
            // {
            //     await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

            //     result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            //     Console.WriteLine("result2: closestatus: "+result.CloseStatus.Value);
            //     Console.WriteLine("result2: closestatusdescription: "+result.CloseStatusDescription);
            // }
            // await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}
