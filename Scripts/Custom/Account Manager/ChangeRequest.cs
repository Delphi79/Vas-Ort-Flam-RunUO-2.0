using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Accounting
{
    public enum RequestType
    {
        Email,
        Password,
        ChangeEmail
    }

    public class ChangeRequest
    {
        private Mobile _Mobile;
        private RequestType _RequestType;
        private string _Key;
        private string _RequestString;

        public Mobile Mobile { get { return _Mobile; } }
        public RequestType RequestType { get { return _RequestType; } }
        public string RequestString { get { return _RequestString; } }

        public ChangeRequest( Mobile m, RequestType type, string key, string requestString )
        {
            _Mobile = m;
            _RequestType = type;
            _Key = key;
            _RequestString = requestString;
        }

        public bool Authenticate( string key )
        {
            bool match = key == _Key;
            return match;
        }        
    }
}
