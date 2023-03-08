namespace Main.Service.RabbitMQ
{
    public interface IRabitMQProducer
    {
        public bool SendWeatherMessage<T>(T message);
    }
}
