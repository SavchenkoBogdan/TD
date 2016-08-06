/*
 * @author = Shustrik 
 * 06.08.2016
*/
public class EventAggregator : EventAggregatorBase
{
    public class EventsHolder
    {
        public CustomEventDelegate<IBaseEnemyController> UnitReachedFinishEvent = delegate { };
        public CustomEventDelegate<IBaseEnemyController> UnitDiedEvent = delegate { };
        public CustomEventDelegate<ITowerController> TowerShootEvent = delegate { };
    }

    private readonly static EventsHolder events = new EventsHolder();
    public  static EventsHolder Events
    {
        get
        {
            return events;
        }
    }
}

//public class xExampleUsage
//{
//    public void Test()
//    {
//        EventAggregator.SubscribeForEvent(EventAggregator.Events.SomeTestEvent, MyFunctionWithSameParameters);
//        EventAggregator.UnsubscribeFromEvent(EventAggregator.Events.SomeTestEvent, MyFunctionWithSameParameters);
//        EventAggregator.Publish(EventAggregator.Events.SomeTestEvent);
//        EventAggregator.PublishWithDelay(someDelay, EventAggregator.Events.SomeTestEvent);
//    }
//    public void MyFunctionWithSameParameters(<SOME_TYPE> value) { }
//}



