namespace college_events_admin_API.Services
{
    public class BackgroundUpdateService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BackgroundUpdateService> _logger;

        public BackgroundUpdateService(IServiceProvider serviceProvider, ILogger<BackgroundUpdateService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"~~~ Проверка актуальности мероприятий ~~~ Время проверки: {DateTime.Now} ~~~");

                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var service = scope.ServiceProvider.GetRequiredService<EventsService>();
                        int savedCount = service.UpdateExpiredEvents();

                        if (savedCount > 0)
                        {
                            _logger.LogInformation($"~~~ Обновлено мероприятий: {savedCount} ~~~");
                        }
                        else
                        {
                            _logger.LogInformation($"~~~ Нет мероприятий для обновления ~~~");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"~~~ Ошибка обновления статусов! {ex.Message} !!!");
                }

                await Task.Delay(600000, stoppingToken);
            }
        }
    }
}
