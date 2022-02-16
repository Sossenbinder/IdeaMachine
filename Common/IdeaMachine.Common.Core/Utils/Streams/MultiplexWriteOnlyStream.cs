using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IdeaMachine.Common.Core.Utils.Streams
{
	public class MultiplexWriteOnlyStream : Stream
	{
		private readonly List<Stream> _streams;

		public MultiplexWriteOnlyStream(params Stream[] streams)
		{
			_streams = streams.ToList();
		}

		public MultiplexWriteOnlyStream(IEnumerable<Stream> streams)
			: this(streams.ToArray())
		{
		}

		public override void Flush()
		{
			foreach (var stream in _streams)
			{
				stream.Flush();
			}
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		public override void SetLength(long value)
		{
			foreach (var stream in _streams)
			{
				stream.SetLength(value);
			}
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			Parallel.ForEach(_streams, stream => stream.Write(buffer, offset, count));
		}

		public override bool CanRead => false;

		public override bool CanSeek => false;

		public override bool CanWrite => true;

		public override long Length => _streams.FirstOrDefault()?.Length ?? 0;

		public override long Position
		{
			get => throw new NotSupportedException();
			set
			{
				foreach (var stream in _streams)
				{
					stream.Position = value;
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}

			foreach (var stream in _streams)
			{
				stream.Position = 0;
			}
		}
	}
}