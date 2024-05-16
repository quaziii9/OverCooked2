using UnityEngine.Events;
using System.Collections.Generic;
using System;

namespace EventLibrary
{
    // 제네릭 UnityEvent 클래스 정의
    [Serializable]
    public class GenericEvent<T> : UnityEvent<T> { }

    public class EventManager<E> where E : Enum
    {
        // 이벤트 이름과 해당 UnityEvent를 저장하는 딕셔너리
        private static readonly Dictionary<E, UnityEventBase> eventDictionary = new Dictionary<E, UnityEventBase>();
        // 스레드 안전성을 위한 객체
        private static readonly object lockObj = new object();

        // 매개변수가 없는 UnityAction 리스너를 추가하는 메서드
        public static void StartListening(E eventName, UnityAction listener)
        {
            lock (lockObj)
            {
                // eventDictionary에서 해당 이벤트를 찾고, 리스너를 추가
                if (eventDictionary.TryGetValue(eventName, out var thisEvent))
                {
                    if (thisEvent is UnityEvent unityEvent)
                    {
                        unityEvent.AddListener(listener);
                    }
                }
                else
                {
                    // 이벤트가 존재하지 않으면 새로 생성하고 추가
                    var unityEvent = new UnityEvent();
                    unityEvent.AddListener(listener);
                    eventDictionary.Add(eventName, unityEvent);
                }
            }
        }

        // 제네릭 매개변수를 갖는 UnityAction 리스너를 추가하는 메서드
        public static void StartListening<T>(E eventName, UnityAction<T> listener)
        {
            lock (lockObj)
            {
                // eventDictionary에서 해당 이벤트를 찾고, 리스너를 추가
                if (eventDictionary.TryGetValue(eventName, out var thisEvent))
                {
                    if (thisEvent is GenericEvent<T> genericEvent)
                    {
                        genericEvent.AddListener(listener);
                    }
                }
                else
                {
                    // 이벤트가 존재하지 않으면 새로 생성하고 추가
                    var genericEvent = new GenericEvent<T>();
                    genericEvent.AddListener(listener);
                    eventDictionary.Add(eventName, genericEvent);
                }
            }
        }

        // 매개변수가 없는 UnityAction 리스너를 제거하는 메서드
        public static void StopListening(E eventName, UnityAction listener)
        {
            lock (lockObj)
            {
                // eventDictionary에서 해당 이벤트를 찾아 리스너를 제거
                if (eventDictionary.TryGetValue(eventName, out var thisEvent) && thisEvent is UnityEvent unityEvent)
                {
                    unityEvent.RemoveListener(listener);
                    // 리스너가 모두 제거된 경우 딕셔너리에서 이벤트 삭제
                    if (unityEvent.GetPersistentEventCount() == 0)
                    {
                        eventDictionary.Remove(eventName);
                    }
                }
            }
        }

        // 제네릭 매개변수를 갖는 UnityAction 리스너를 제거하는 메서드
        public static void StopListening<T>(E eventName, UnityAction<T> listener)
        {
            lock (lockObj)
            {
                // eventDictionary에서 해당 이벤트를 찾아 리스너를 제거
                if (eventDictionary.TryGetValue(eventName, out var thisEvent) && thisEvent is GenericEvent<T> genericEvent)
                {
                    genericEvent.RemoveListener(listener);
                    // 리스너가 모두 제거된 경우 딕셔너리에서 이벤트 삭제
                    if (genericEvent.GetPersistentEventCount() == 0)
                    {
                        eventDictionary.Remove(eventName);
                    }
                }
            }
        }

        // 매개변수가 없는 이벤트를 트리거하는 메서드
        public static void TriggerEvent(E eventName)
        {
            lock (lockObj)
            {
                // eventDictionary에서 해당 이벤트를 찾아 트리거
                if (eventDictionary.TryGetValue(eventName, out var thisEvent) && thisEvent is UnityEvent unityEvent)
                {
                    unityEvent.Invoke();
                }
            }
        }

        // 제네릭 매개변수를 갖는 이벤트를 트리거하는 메서드
        public static void TriggerEvent<T>(E eventName, T parameter)
        {
            lock (lockObj)
            {
                // eventDictionary에서 해당 이벤트를 찾아 트리거
                if (eventDictionary.TryGetValue(eventName, out var thisEvent) && thisEvent is GenericEvent<T> genericEvent)
                {
                    genericEvent.Invoke(parameter);
                }
            }
        }
    }
}
