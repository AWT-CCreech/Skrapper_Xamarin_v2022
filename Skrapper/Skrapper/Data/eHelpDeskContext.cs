using MyWebService;
using System;

namespace Skrapper
{
    public class eHelpDeskContext
    {
        public static bool runningInTestMode;

        #region --: WebService DYNAMIC LOADER :--

        //public async ScannerWebService GetWebServiceRef()
        public static ScannerWebServiceSoapClient GetWebServiceRef()
        {
            /************************************************************
			 * GetWebServiceRef() returns a web service reference based
			 * on the TestMode checkbox.  If TestMode is checked then
			 * this function returns a webServiceRef to the TestServer,
			 * otherwise it returns a LIVE webservice reference.
			 * 
			 * This function use the Global gWS to keep the web service
			 * reference open and dynamically changes between the Test
			 * and LIVE modes as necessary.
			 * 
			 * TEST - ScannerWebServiceSopa12
			 * PROD - ScannerWebServiceSoap
			 * **********************************************************/
            try
            {
                Globals.gWS ??= new ScannerWebServiceSoapClient(ScannerWebServiceSoapClient.EndpointConfiguration.ScannerWebServiceSoap);

                if (Globals.gWS != null)
                {
                    if (Globals.bTestMode)
                    {
                        Globals.gWS = new ScannerWebServiceSoapClient(ScannerWebServiceSoapClient.EndpointConfiguration.ScannerWebServiceSoap12);
                    }
                    else
                    {
                        Globals.gWS = new ScannerWebServiceSoapClient(ScannerWebServiceSoapClient.EndpointConfiguration.ScannerWebServiceSoap);
                    }
                }
            }
            catch (Exception excc)
            {
                Console.WriteLine("GetWebServiceRef() ==> " + excc.Message);
            }
            return Globals.gWS;
        }

        public static bool WebServiceConnected()
        {

            return (Globals.gWS != null);
        }
        #endregion
    }
}
