using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace MongoExport
{
    internal class JsonDestinationWriter : IDestinationWriter
    {
        private readonly static IBsonSerializer _serializer;
        private readonly bool _closeWriter;
        private readonly TextWriter _writer;

        static JsonDestinationWriter()
        {
            _serializer = BsonDocumentSerializer.Instance;
        }

        public JsonDestinationWriter(TextWriter writer, bool closeWriter)
        {
            _writer = writer;
            _closeWriter = closeWriter;
        }

        public void Dispose()
        {
            if (_closeWriter)
            {
                _writer.Dispose();
            }
        }

        public void Write(IEnumerable<BsonDocument> documents)
        {
            var settings = new JsonWriterSettings
            {
                CloseOutput = false,
                Indent = false,
                OutputMode = JsonOutputMode.Strict
            };

            var undisposableWriter = new UndisposableTextWriter(_writer);

            foreach (var document in documents)
            {
                using (var bsonWriter = JsonWriter.Create(undisposableWriter, settings))
                {
                    _serializer.Serialize(bsonWriter, typeof(BsonDocument), document, null);
                    _writer.WriteLine();
                }
            }
        }

        private class UndisposableTextWriter : TextWriter
        {
            private readonly TextWriter _inner;

            public UndisposableTextWriter(TextWriter inner)
            {
                _inner = inner;
            }

            public override Encoding Encoding
            {
                get { return _inner.Encoding; }
            }

            public override IFormatProvider FormatProvider
            {
                get { return _inner.FormatProvider; }
            }

            public override string NewLine
            {
                get { return _inner.NewLine; }
                set { _inner.NewLine = value; }
            }

            public override void Close()
            {
                _inner.Close();
            }

            public override System.Runtime.Remoting.ObjRef CreateObjRef(Type requestedType)
            {
                return _inner.CreateObjRef(requestedType);
            }

            protected override void Dispose(bool disposing)
            {
                // Do nothing...
            }

            public override void Flush()
            {
                _inner.Flush();
            }

            public override Task FlushAsync()
            {
                return _inner.FlushAsync();
            }

            public override object InitializeLifetimeService()
            {
                return _inner.InitializeLifetimeService();
            }

            public override void Write(bool value)
            {
                _inner.Write(value);
            }

            public override void Write(char value)
            {
                _inner.Write(value);
            }

            public override void Write(char[] buffer)
            {
                _inner.Write(buffer);
            }

            public override void Write(char[] buffer, int index, int count)
            {
                _inner.Write(buffer, index, count);
            }

            public override void Write(decimal value)
            {
                _inner.Write(value);
            }

            public override void Write(double value)
            {
                _inner.Write(value);
            }

            public override void Write(float value)
            {
                _inner.Write(value);
            }

            public override void Write(int value)
            {
                _inner.Write(value);
            }

            public override void Write(long value)
            {
                _inner.Write(value);
            }

            public override void Write(object value)
            {
                _inner.Write(value);
            }

            public override void Write(string format, object arg0)
            {
                _inner.Write(format, arg0);
            }

            public override void Write(string format, object arg0, object arg1)
            {
                _inner.Write(format, arg0, arg1);
            }

            public override void Write(string format, object arg0, object arg1, object arg2)
            {
                _inner.Write(format, arg0, arg1, arg2);
            }

            public override void Write(string format, params object[] arg)
            {
                _inner.Write(format, arg);
            }

            public override void Write(string value)
            {
                _inner.Write(value);
            }

            public override void Write(uint value)
            {
                _inner.Write(value);
            }

            public override void Write(ulong value)
            {
                _inner.Write(value);
            }

            public override Task WriteAsync(char value)
            {
                return _inner.WriteAsync(value);
            }

            public override Task WriteAsync(char[] buffer, int index, int count)
            {
                return _inner.WriteAsync(buffer, index, count);
            }

            public override Task WriteAsync(string value)
            {
                return _inner.WriteAsync(value);
            }

            public override void WriteLine()
            {
                _inner.WriteLine();
            }

            public override void WriteLine(bool value)
            {
                _inner.WriteLine(value);
            }

            public override void WriteLine(char value)
            {
                _inner.WriteLine(value);
            }

            public override void WriteLine(char[] buffer)
            {
                _inner.WriteLine(buffer);
            }

            public override void WriteLine(char[] buffer, int index, int count)
            {
                _inner.WriteLine(buffer, index, count);
            }

            public override void WriteLine(decimal value)
            {
                _inner.WriteLine(value);
            }

            public override void WriteLine(double value)
            {
                _inner.WriteLine(value);
            }

            public override void WriteLine(float value)
            {
                _inner.WriteLine(value);
            }

            public override void WriteLine(int value)
            {
                _inner.WriteLine(value);
            }

            public override void WriteLine(long value)
            {
                _inner.WriteLine(value);
            }

            public override void WriteLine(object value)
            {
                _inner.WriteLine(value);
            }

            public override void WriteLine(string format, object arg0)
            {
                _inner.WriteLine(format, arg0);
            }

            public override void WriteLine(string format, object arg0, object arg1)
            {
                _inner.WriteLine(format, arg0, arg1);
            }

            public override void WriteLine(string format, object arg0, object arg1, object arg2)
            {
                _inner.WriteLine(format, arg0, arg1, arg2);
            }

            public override void WriteLine(string format, params object[] arg)
            {
                _inner.WriteLine(format, arg);
            }

            public override void WriteLine(string value)
            {
                _inner.WriteLine(value);
            }

            public override void WriteLine(uint value)
            {
                _inner.WriteLine(value);
            }

            public override void WriteLine(ulong value)
            {
                _inner.WriteLine(value);
            }

            public override Task WriteLineAsync()
            {
                return _inner.WriteLineAsync();
            }

            public override Task WriteLineAsync(char value)
            {
                return _inner.WriteLineAsync(value);
            }

            public override Task WriteLineAsync(char[] buffer, int index, int count)
            {
                return _inner.WriteLineAsync(buffer, index, count);
            }

            public override Task WriteLineAsync(string value)
            {
                return _inner.WriteLineAsync(value);
            }
        }

    }
}