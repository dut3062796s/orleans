﻿

namespace Orleans.ServiceBus.Providers
{
    public interface IEventHubSettings
    {
        string ConnectionString { get; }
        string ConsumerGroup { get; }
        string Path { get; }
        int? PrefetchCount { get; }

        /// <summary>
        /// Indicates if stream provider should read all new data in partition, or from start of partition.
        /// True - read all new data added to partition.
        /// False - start reading from beginning of partition.
        /// Note: If checkpoints are used, stream provider will always begin reading from most recent checkpoint.
        /// </summary>
        bool StartFromNow { get; }
    }
}
