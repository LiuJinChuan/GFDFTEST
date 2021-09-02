using Newtonsoft.Json;
using System;
using System.Reactive.Subjects;

namespace GFDF.Infrastruct.Core
{
    public class EventBus
    {
        public static ISubject<EventData> subject = new Subject<EventData>();
        public static IdWorker idWorker;
        public static void Emit(string src, dynamic data)
        {
            EventData d = new EventData { EventId = idWorker.nextId(), EventTime = DateTime.Now, EventSource = src, BussData = data };
            subject.OnNext(d);
        }

        public static void Subscribe(Action<EventData> method)
        {
            subject.Subscribe(method);
        }

        public class EventData
        {
            public long EventId { get; set; }
            /// <summary>
            /// 事件发生的时间
            /// </summary>
            public DateTime EventTime { get; set; }

            /// <summary>
            /// 触发事件的对象
            /// </summary>
            public string EventSource { get; set; }
            private string StrBussData;
            //将对象串行化，防止可访问性受限（如匿名对象）跨dll出现异常。
            public dynamic BussData
            {
                get
                {
                    return string.IsNullOrEmpty(StrBussData) ? StrBussData : JsonConvert.DeserializeObject(StrBussData);
                }
                set
                {
                    StrBussData = JsonConvert.SerializeObject(value);
                }
            }
        }
    }
}
