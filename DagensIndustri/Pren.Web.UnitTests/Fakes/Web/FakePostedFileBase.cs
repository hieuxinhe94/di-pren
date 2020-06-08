using System;
using System.IO;
using System.Web;

namespace Pren.Web.UnitTests.Fakes.Web
{
    class FakePostedFileBase : HttpPostedFileBase
    {
        readonly Stream _stream;
        readonly string _contentType;
        readonly string _fileName;

        public FakePostedFileBase(string fileName, Stream stream = null, string contentType = "")
        {
            this._stream = stream;
            this._contentType = contentType;
            this._fileName = fileName;
        }

        public override int ContentLength
        {
            get { return (int)_stream.Length; }
        }

        public override string ContentType
        {
            get { return _contentType; }
        }

        public override string FileName
        {
            get { return _fileName; }
        }

        public override Stream InputStream
        {
            get { return _stream; }
        }

        public override void SaveAs(string filename)
        {
            throw new NotImplementedException();
        }
    }
}



