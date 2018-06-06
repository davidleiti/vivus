namespace Vivus.Core.Helpers
{
    using System.Threading.Tasks;
    using Twilio;
    using Twilio.Rest.Api.V2010.Account;
    using Twilio.Types;

    /// <summary>
    /// Represents a collection of methods to simplify the work with the Twilio API.
    /// </summary>
    public class TwilioApiHelpers
    {
        #region Private Constants

        /// <summary>
        /// The account SId.
        /// </summary>
        private const string accountSid = "ACdd21b0f68ff88e5ba341b7e465faf5d9";

        /// <summary>
        /// The authentification token.
        /// </summary>
        private const string authToken = "325aabef78b54645b1c193d7b51d7d3f";

        /// <summary>
        /// The sender phone number.
        /// </summary>
        private const string phoneNumber = "+18126693442";

        #endregion

        #region Public Methods

        /// <summary>
        /// Sends asynchronously an SMS and waits for the response.
        /// </summary>
        /// <param name="phoneNo">The phone number to send the message to.</param>
        /// <param name="message">The message of the SMS.</param>
        /// <returns></returns>
        public static async Task<MessageResource> SendSmsAsync(string phoneNo, string message)
        {
            TwilioClient.Init(accountSid, authToken);

            return await MessageResource.CreateAsync(
                to: new PhoneNumber(phoneNo),
                from: new PhoneNumber(phoneNumber),
                body: message);
        }

        #endregion
    }
}
