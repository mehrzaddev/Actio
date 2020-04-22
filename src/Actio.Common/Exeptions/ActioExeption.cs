using System;
using System.Collections.Generic;
using System.Text;

namespace Actio.Common.Exeptions
{
    public class ActioExeption : Exception
    {
        public string Code { get; set; }

        public ActioExeption()
        {

        }

        public ActioExeption(string code)
        {
            this.Code = code;
        }

        public ActioExeption(string message, params object[] args) : this(string.Empty, message, args)
        {

        }
        public ActioExeption(string code, string message, params object[] args) : this(null, code, message, args)
        {

        }
        public ActioExeption(Exception innerException, string message, params object[] args)
            : this(innerException, string.Empty, message, args)
        {

        }
        public ActioExeption(Exception innerException, string code, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
            Code = code;
        }

    }
}
