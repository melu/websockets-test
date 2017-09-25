

var GameClient = {};

var HANDLERS = {
    // "OPEN":socket.onopen,
    // "CLOSE":socket.onclose
};

GameClient = function(){
    var scheme = document.location.protocol == "https:" ? "wss" : "ws";
    var port = document.location.port ? (":" + document.location.port) : "";
    var connectionUrl = scheme + "://" + document.location.hostname + port + "/ws" ;
    this.socket = new WebSocket(connectionUrl);
    return this;
}();


GameClient.Send = function(event, data){
    console.log("send");
    var request = {
        event: event,
        data: data
    };
    GameClient.socket.send(JSON.stringify(request));
}

GameClient.On = function(event, handler){
    GameClient.HANDLERS[event] = handler;
}

GameClient.socket.onmessage = function(event){

    // console.log(event);
    // console.log(event.name);
    var msg = JSON.parse(event.data);
    console.log(event.data);
    // console.log(event.data["name"]);
    console.log(msg.name);
    console.log(msg.data);
    // console.log(event.data.data);
}