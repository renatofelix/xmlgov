using System;
using System.Runtime.Serialization;

namespace CertificadorXML
{
    [Serializable]
    internal class ExcecaoCertificadoDigital : Exception
    {
        public ExcecaoCertificadoDigital()
        {
        }

        public ExcecaoCertificadoDigital(string message) : base(message)
        {
        }

        public ExcecaoCertificadoDigital(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExcecaoCertificadoDigital(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}