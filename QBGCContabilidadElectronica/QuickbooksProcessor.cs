using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Interop.QBXMLRP2;

namespace QB_App
{
    public class QuickbooksProcessor
    {
        RequestProcessor2 qbRequestProcessor = null;
        public string lastError;
        public string ticket = null;

        public bool QueryQB(string query, out string response)
        {
            response = "";
            try
            {
                response = qbRequestProcessor.ProcessRequest(ticket, query);
            }
            catch (Exception ex)
            {
                lastError = "Error = " + ex.Message;
                return false;
            }
            return true;
        }

        public bool OpenConnection(string strAppName, string qbFileName)
        {
            qbRequestProcessor = new RequestProcessor2();
            qbRequestProcessor.OpenConnection("", strAppName);
            ticket = qbRequestProcessor.BeginSession(qbFileName, QBFileMode.qbFileOpenDoNotCare);
            return true;
        }

        public bool CloseConnection()
        {
            if (ticket != null)
            {
                qbRequestProcessor.EndSession(ticket);
            }
            if (qbRequestProcessor != null)
            {
                qbRequestProcessor.CloseConnection();
            }

            return true;
        }
    }
}
