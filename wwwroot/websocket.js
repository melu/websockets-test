

var Client = {};

var HANDLERS = {
    "OPEN":socket.onopen,
    "CLOSE":socket.onclose
};

Client = function(){
    var scheme = document.location.protocol == "https:" ? "wss" : "ws";
    var port = document.location.port ? (":" + document.location.port) : "";
    connectionUrl.value = scheme + "://" + document.location.hostname + port + "/ws" ;

    this.socket = new WebSocket(connectionUrl);
    return this;
}();


Client.Send = function(event, data){
    var request = {
        event: event,
        data: data
    };
    Client.socket.send(req);
}