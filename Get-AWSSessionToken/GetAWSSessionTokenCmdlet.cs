using System;
using System.Management.Automation;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;


namespace Get_AWSSessionToken
{
    [Cmdlet(VerbsCommon.Get, "AWSSessionToken")]
    [OutputType(typeof(Credentials))]
    public class GetAWSSessionTokenCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The Multi Factor Authentication token taken from your device.", Position = 0)]
        public string MFAToken { get; set; }

        [Parameter(HelpMessage = "The time in seconds you want the token to last. Defaults to 3600.", Position = 1)]
        public int Duration { get; set; } = 3600;

        protected override void ProcessRecord()
        {
            try
            {
                string mfaDevice = Environment.GetEnvironmentVariable("DEVOPS_AWS_MFA_DEVICE");
                string devopsAccessKey = Environment.GetEnvironmentVariable("DEVOPS_AWS_ACCESS_KEY_ID");
                string devopsSecretKey = Environment.GetEnvironmentVariable("DEVOPS_AWS_SECRET_ACCESS_KEY");
                AmazonSecurityTokenServiceClient stsClient = string.IsNullOrEmpty(devopsAccessKey)
                    ? new AmazonSecurityTokenServiceClient()
                    : new AmazonSecurityTokenServiceClient(new BasicAWSCredentials(devopsAccessKey, devopsSecretKey));

                using (stsClient)
                {
                    GetSessionTokenRequest request = new GetSessionTokenRequest
                    {
                        DurationSeconds = Duration,
                        SerialNumber = mfaDevice,
                        TokenCode = MFAToken
                    };

                    Task<GetSessionTokenResponse> response = stsClient.GetSessionTokenAsync(request);
                    response.Wait(1000);

                    WriteObject(response.Result.Credentials);
                }
            }
            catch (Exception exception)
            {
                ThrowTerminatingError(new ErrorRecord(new Exception("Error getting session token", exception), "Error getting session token", ErrorCategory.InvalidOperation, null));
            }
        }
    }
}
