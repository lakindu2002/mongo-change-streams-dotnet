using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using TodoApp.Configurations;

namespace TodoApp.Background
{
    public class Service : IHostedService
    {
        private readonly IMongoCollection<BsonDocument> _mongoCollection;
        private IChangeStreamCursor<BsonDocument> _changeStreamCursor;
        private Task _watchTask;
        private CancellationTokenSource _cancellationTokenSource;

        public Service(IMongoClient mongoClient, IOptions<Database> options)
        {
            var database = mongoClient.GetDatabase(options.Value.DatabaseName);
            _mongoCollection = database.GetCollection<BsonDocument>(options.Value.CollectionName);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _watchTask = Task.Run(() => WatchForChangesAsync(_cancellationTokenSource.Token), _cancellationTokenSource.Token);

            return Task.CompletedTask;
        }

        private async Task WatchForChangesAsync(CancellationToken cancellationToken)
        {
            var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<BsonDocument>>()
                .Match(change =>
                    change.OperationType == ChangeStreamOperationType.Insert ||
                    change.OperationType == ChangeStreamOperationType.Update ||
                    change.OperationType == ChangeStreamOperationType.Replace
                )
                .AppendStage<ChangeStreamDocument<BsonDocument>, ChangeStreamDocument<BsonDocument>, BsonDocument>(
                    @"{ 
                    $project: { 
                        '_id': 1, 
                        'fullDocument': 1, 
                        'ns': 1, 
                        'documentKey': 1 ,
                        'operationType': 1,
                        'updateDescription': 1,
                        'clusterTime': 1,
                        'txnNumber': 1,
                        'lsid': 1
                    }
                }"
                );

            var options = new ChangeStreamOptions
            {
                FullDocument = ChangeStreamFullDocumentOption.UpdateLookup
            };

            _changeStreamCursor = _mongoCollection.Watch(pipeline, options, cancellationToken: cancellationToken);

            while (await _changeStreamCursor.MoveNextAsync(cancellationToken))
            {
                var batch = _changeStreamCursor.Current;
                foreach (var change in batch)
                {
                    Console.WriteLine(change);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource.Cancel();
            return Task.WhenAny(_watchTask, Task.Delay(Timeout.Infinite, cancellationToken));
        }
    }
}
