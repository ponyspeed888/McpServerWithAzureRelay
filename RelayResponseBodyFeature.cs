namespace McpServer1
{

    using Microsoft.AspNetCore.Http.Features;
    using System.IO;
    using System.IO.Pipelines;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class RelayResponseBodyFeature
        : IHttpResponseBodyFeature
    {
        private readonly IHttpResponseBodyFeature _inner;

        public RelayResponseBodyFeature(
            IHttpResponseBodyFeature inner)
        {
            _inner = inner;
        }

        public Stream Stream => _inner.Stream;

        public PipeWriter Writer => _inner.Writer;

        // Azure Relay throws NotImplementedException here.
        // This workaround deliberately does nothing.
        public void DisableBuffering()
        {
        }

        public Task StartAsync(
            CancellationToken cancellationToken = default)
            => _inner.StartAsync(cancellationToken);

        public Task SendFileAsync(
            string path,
            long offset,
            long? count,
            CancellationToken cancellationToken = default)
            => _inner.SendFileAsync(
                path, offset, count, cancellationToken);

        public Task CompleteAsync()
            => _inner.CompleteAsync();
    }


}
