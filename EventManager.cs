using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace wsserver
{
    public class EventManager
    {
        private ConcurrentQueue<Event> _events;

        private Dictionary<string, Action<Event>> _handlers;


        public EventManager(){
            _events = new ConcurrentQueue<Event>();
            _handlers = new Dictionary<string, Action<Event>>();
        }

        public void ProcessEventsQueue(){
            while(_events.TryDequeue(out Event evento)){
                if(_handlers.ContainsKey(evento.name)){
                    _handlers.TryGetValue(evento.name, out var func);
                    func.DynamicInvoke(evento, evento.data);
                }
            }
        }

        public void On(String eventName, Action<Event> handler){
            _handlers.Add(eventName, handler);
        }

        public void RecieveEvent(Event ev){
            _events.Enqueue(ev);
        }
    }
}