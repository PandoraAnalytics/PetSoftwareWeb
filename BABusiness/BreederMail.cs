using System;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;

namespace BABusiness
{
    public class BreederMail
    {
        public static string PageURL = string.Empty;
        public static string ServerPath = string.Empty;

        public enum MessageType
        {
            FORGOTPASSWORD = 1,
            CHANGEPASSWORD = 2,
            SENDUSERJOINREQUEST = 3,
            SENDMEMBERSHIPREQUEST = 4,
            NEWUSERWELCOMEEMAIL = 5,
            USERUNREGISTEREVENT = 6,
            OWNERUNREGISTEREVENT = 7,
            USERCANCELEVENTEMAIL = 8,
            OWNERCANCELEVENTEMAIL = 9,
            TICKETCHANGESTATUS = 13,
            TICKETADDNEW = 14,
            TICKETADDNEWCOMMENT = 79,
            TICKETADDNEWDOCUMENT = 80,

            BUSINESSUSERREQUEST = 81,
            BUSINESSUSERREQUESTREJECT = 82,
            BUSINESSUSERREQUESTACCEPT = 83,
            BUSINESSUSERREQUESTACCEPTADMIN = 84,

            USERCHANGEPASSWORD=85,
            NEWSTAFFWELCOMEEMAIL = 86,
        }

        private static object SyncObject1 = new object();
        private static object SyncObject2 = new object();
        private static object SyncObject3 = new object();
        private static object SyncObject4 = new object();
        private static object SyncObject5 = new object();
        private static object SyncObject6 = new object(); // USER UNREGISTEREVENT
        private static object SyncObject7 = new object(); // OWNER UNREGISTEREVENT
        private static object SyncObject8 = new object(); //USER CANCEL EVENT EMAIL
        private static object SyncObject9 = new object(); // OWNER CANCEL EVENT EMAIL
        private static object SyncObject13 = new object();
        private static object SyncObject14 = new object();
        private static object SyncObject78 = new object();
        private static object SyncObject79 = new object();

        private static object SyncObject81 = new object();//Business User Enquiry Request
        private static object SyncObject82 = new object();//Business User Enquiry Request Rejected
        private static object SyncObject83 = new object();//Business User Enquiry Request Accepted
        private static object SyncObject84 = new object();//Business User Enquiry Request Accepted Message for Admin

        private static object SyncObject85 = new object();// Old User Change Password
        private static object SyncObject86 = new object();// new staff added

        #region constant
        public const string CHANGEPASSWORDSUBJECT = "Pets.Software Password Changed";
        public const string CHANGEPASSWORDBODY = @"<html><body><div style='font-family: arial,sans-serif;background-color:#eff2f7;font-size: 14px;text-align:left;padding: 20px 15px;border-radius: 5px;'>
        <img style='max-height: 75px;' src='{0}images/logo.png' alt='logo' /><br><br>
        Hello <h3>Your password changed</h3>
        If this was you, then you can safely ignore this email.<br><br>
        If this wasn't you, your account has been compromised. Please reset your password:<br><br>
        <a href='{0}forgotpassword.aspx' target='_blank'>Reset your password</a><br><br>
        Thank you.<br><br>Regards<br>Pets.Software Team</p>
        </div></body></html>";

        public const string FORGOTPASSSUBJECT = "Pets.Software Password Assistance";
        public const string FORGOTPASSADDBODY = @"<html><body><div style='font-family: arial,sans-serif;background-color:#eff2f7;font-size: 14px;text-align:left;padding: 20px 15px;border-radius: 5px;'>
        <img style='max-height: 75px;' src='{0}images/logo.png' alt='logo' /><br><br>
        <h3>Hello,</h3>
        We received a request to reset the password associated with this e-mail address (#email). If you made this request, please follow the instructions below.<br>
        Click on the link below to reset your password using our secure server:<br><br>
        <a href='{0}resetpassword.aspx?#token' target='_blank'>Reset your password</a><br><br>
        If you did not request to have your password reset you can safely ignore this email. Rest assured your account is safe.We will never e-mail you and ask you to disclose your Pets.Software password. If you receive a suspicious e-mail, report the e-mail to us for investigation.<br><br>
        Thank you.<br><br>Regards<br>Pets.Software Team</p>
        </div></body></html>";


        public const string SENDUSERJOINREQUESTSUBJECT = "Welcome to Pets.Software";
        public const string SENDUSERJOINREQUESTBODY = @"<html><body><div style='font-family: arial,sans-serif;background-color:#eff2f7;font-size: 14px;text-align:left;padding: 20px 15px;border-radius: 5px;'>
        <img style='max-height: 75px;' src='{0}images/logo.png' alt='logo' /><br><br>
        <h3>Hello,</h3>
        We'd like to welcome you to Pets.Software, and thank you for joining us as an association member. <br><br>
To learn more about Pets.Software and see how it can work for you please <a href='https://pets.software/app/signin.aspx' target='_blank'>Click Here</a>.<br><br>
        If you have questions contact our customer service team anytime at 'info@pets.software'.<br><br>
        We’re so happy to have you!<br>
        Thank you.<br><br>Regards<br>Pets.Software Team</p>
        </div></body></html>";

        // Incomplete Code
        public const string SENDMEMBERSHIPREQUESTSUBJECT = "New Membership Request";
        public const string SENDMEMBERSHIPREQUESTBODY = @"<html><body><div style='font-family: arial,sans-serif;background-color:#eff2f7;font-size: 14px;text-align:left;padding: 20px 15px;border-radius: 5px;'>
        <img style='max-height: 75px;' src='{0}images/logo.png' alt='logo' /><br><br>
        <h3>Hello,</h3>
        <h5>New member join request.</h5> <br>Please check the following member details:<br><br> Name - #name<br>Email Address - #email<br><br>      
        <a href='https://pets.software/app/signin.aspx' target='_blank'>Click Here</a> for more details.<br><br>
        If you have questions contact our customer service team anytime at 'info@pets.software'.<br><br>       
        Thank you.<br><br>Regards<br>Pets.Software Team</p>
        </div></body></html>";


        public const string NEWUSERWELCOMEEMAILSUBJECT = "Welcome to pets.software";
        public const string NEWUSERWELCOMEEMAILBODY = @"<html><body><div style='font-family: arial,sans-serif;background-color:#eff2f7;font-size: 14px;text-align:left;padding: 20px 15px;border-radius: 5px;'>
        <img style='max-height: 75px;' src='{0}images/logo.png' alt='logo' /><br><br>
        <h3>Hello #name,</h3>
        We'd like to welcome you to pets.software, and thank you for joining us.<br><br>
Here is your login credentials:<br><br>
Username: #email<br>
Password: #pwd<br><br>
To learn more about pets.software and see how it can work for you please <a href='https://pets.software/app/signin.aspx' target='_blank'>Click Here</a>.<br><br>
        If you have questions contact our customer service team anytime at 'info@pets.software'.<br><br>
        We’re so happy to have you!<br>
        Thank you.<br><br>Regards<br>Pets.Software Team</p>
        </div></body></html>";

        //USER UNREGISTER EVENT EMAIL
        public const string USERUNREGISTEREVENTSUBJECT = "Event DeRegistration";
        public const string USERUNREGISTEREVENTBODY = @"<html><body><div style='font-family: arial,sans-serif;background-color:#eff2f7;font-size: 14px;text-align:left;padding: 20px 15px;border-radius: 5px;'>
        <img style='max-height: 75px;' src='{0}images/logo.png' alt='logo' /><br><br>
        <h4>Hello #name,</h4>
        You have successfully deregistered from the #title event.<br><br>
        Event Name:#title <br>
        Event Time and Date : #datetime <br>
        Event Venue : #venue <br>
        To learn more about pets.software and see how it can work for you please <a href='https://pets.software/app/signin.aspx' target='_blank'>Click Here</a>.<br><br>
        If you have questions contact our customer service team anytime at 'info@pets.software'.<br><br>        
        Thank you.<br><br>Regards<br>Pets.Software Team</p>
        </div></body></html>";

        //OWNER UNREGISTER EVENT EMAIL
        public const string OWNERUNREGISTEREVENTSUBJECT = "Event DeRegistration";
        public const string OWNERUNREGISTEREVENTBODY = @"<html><body><div style='font-family: arial,sans-serif;background-color:#eff2f7;font-size: 14px;text-align:left;padding: 20px 15px;border-radius: 5px;'>
        <img style='max-height: 75px;' src='{0}images/logo.png' alt='logo' /><br><br>
        <h4>Hello #ownername,</h4>
        #username has deregister for the below event. Please check the below details.<br><br>
        Event Name:#title <br>
        Event Time and Date : #datetime <br>
        Event Venue : #venue <br>
        To learn more about pets.software and see how it can work for you please <a href='https://pets.software/app/signin.aspx' target='_blank'>Click Here</a>.<br><br>
        If you have questions contact our customer service team anytime at 'info@pets.software'.<br><br>        
        Thank you.<br><br>Regards<br>Pets.Software Team</p>
        </div></body></html>";

        // OWNER CANCEL EVENT EMIAL
        public const string OWNERCANCELEVENTEMAILSUBJECT = "Event Cancel";
        public const string OWNERCANCELEVENTEMAILBODY = @"<html><body><div style='font-family: arial,sans-serif;background-color:#eff2f7;font-size: 14px;text-align:left;padding: 20px 15px;border-radius: 5px;'>
        <img style='max-height: 75px;' src='{0}images/logo.png' alt='logo' /><br><br>
        <h4>Hello #name,</h4>
        You have cancel the event because of the following reason. Please check the below details
        Event Name:#title <br>
        Event Venue : #venue <br>
        Cancel Reason : #cancelreason <br> 
        To learn more about pets.software and see how it can work for you please <a href='https://pets.software/app/signin.aspx' target='_blank'>Click Here</a>.<br><br>
        If you have questions contact our customer service team anytime at 'info@pets.software'.<br><br>        
        Thank you.<br><br>Regards<br>Pets.Software Team</p>
        </div></body></html>";

        // USER CANCEL EVENT EMIAL
        public const string USERCANCELEVENTEMAILSUBJECT = "Event Cancel";
        public const string USERCANCELEVENTEMAILBODY = @"<html><body><div style='font-family: arial,sans-serif;background-color:#eff2f7;font-size: 14px;text-align:left;padding: 20px 15px;border-radius: 5px;'>
        <img style='max-height: 75px;' src='{0}images/logo.png' alt='logo' /><br><br>
        <h4>Hello #name,</h4>
        Event owner has cancel the event, Please check the below details <br><br>
        Event Name:#title <br>
        Event Venue : #venue <br>
        Cancel Reason : #cancelreason <br> 
        To learn more about pets.software and see how it can work for you please <a href='https://pets.software/app/signin.aspx' target='_blank'>Click Here</a>.<br><br>
        If you have questions contact our customer service team anytime at 'info@pets.software'.<br><br>        
        Thank you.<br><br>Regards<br>Pets.Software Team</p>
        </div></body></html>";

        // need to change below functionality for tickets

        public const string TICKETSTATUSCHAGESUBJECT = "[pets.software Support #ticketno] status changed";
        public const string TICKETSTATUSCHAGEBODY = @"<html><body>
<div style='font-family: arial,sans-serif;background-color:#eff2f7;font-size: 14px;text-align:left;padding: 20px 15px;border-radius: 5px;'>
<table width='100%' cellspacing='10' cellpadding='10' align='center'>
<tr><td colspan='2' valign='top'><img style='max-height: 75px;' src='{0}images/logo.png' alt='logo' /></td></tr>
<tr><td colspan='2' valign='top'>Following ticket status has been changed:</td></tr>
<tr><td width='20%' valign='top'>Ticket No:</td><td width='80%' valign='top'>#ticketno</td></tr>
<tr><td width='20%' valign='top'>New Status:</td><td width='80%' valign='top'>#status</td></tr>
<tr><td width='20%' valign='top'>Flags:</td><td width='80%' valign='top'>#flags</td></tr>
<tr><td width='20%' valign='top'>EDT:</td><td width='80%' valign='top'>#edthours (hours)</td></tr>
<tr><td width='20%' valign='top'>Header:</td><td width='80%' valign='top'>#heading</td></tr>
<tr><td colspan='2' valign='top'>#notes</td></tr>
<tr><td colspan='2' valign='top'>Please <a href='https://pets.software/app/signin.aspx' target='_blank'>log in to the pets.software</a> to view the details.</td></tr>
<tr><td colspan='2' valign='top'>Regards<br>pets.software Team</td></tr>
<tr><td colspan='2' valign='top'>&nbsp;</td></tr>
<tr><td colspan='2' valign='top'>Please do not reply to this auto generated email. For any query, contact info@pets.software</td></tr>
</table></div></body></html>";

        // need to change below functionality for tickets

        public const string TICKETADDNEWSUBJECT = "[pets.software Support #ticketno] new ticket added";
        public const string TICKETADDNEWBODY = @"<html><body>
<div style='font-family: arial,sans-serif;background-color:#eff2f7;font-size: 14px;text-align:left;padding: 20px 15px;border-radius: 5px;'>
<table width='100%' cellspacing='10' cellpadding='10' align='center'>
<tr><td colspan='2' valign='top'><img style='max-height: 75px;' src='{0}images/logo.png' alt='logo' /></td></tr>
<tr><td colspan='2' valign='top'>Following ticket status has added:</td></tr>
<tr><td width='20%' valign='top'>Ticket No:</td><td width='80%' valign='top'>#ticketno</td></tr>
<tr><td width='20%' valign='top'>Ticket:</td><td width='80%' valign='top'>#heading</td></tr>
<tr><td width='20%' valign='top'>Author:</td><td width='80%' valign='top'>#author</td></tr>
<tr><td colspan='2' valign='top'>Please <a href='https://pets.software/app/signin.aspx' target='_blank'>log in to the pets.software</a> to view the details.</td></tr>
<tr><td colspan='2' valign='top'>Regards<br>pets.software Team</td></tr>
<tr><td colspan='2' valign='top'>&nbsp;</td></tr>
<tr><td colspan='2' valign='top'>Please do not reply to this auto generated email. For any query, contact info@pets.software</td></tr>
</table></div></body></html>";

        // need to change below functionality for tickets

        public const string TICKETNEWCOMMENTSUBJECT = "[pets.software Support #ticketno] new comment posted";
        public const string TICKETNEWCOMMENTBODY = @"<html><body>
<div style='font-family: arial,sans-serif;background-color:#eff2f7;font-size: 14px;text-align:left;padding: 20px 15px;border-radius: 5px;'>
<table width='100%' cellspacing='10' cellpadding='10' align='center'>
<tr><td colspan='2' valign='top'><img style='max-height: 75px;' src='{0}images/logo.png' alt='logo' /></td></tr>
<tr><td width='20%' valign='top'>Ticket No:</td><td width='80%' valign='top'>#ticketno</td></tr>
<tr><td width='20%' valign='top'>Header:</td><td width='80%' valign='top'>#heading</td></tr>
<tr><td width='20%' valign='top'>Posted By:</td><td width='80%' valign='top'>#username</td></tr>
<tr><td colspan='2' valign='top'>#comments</td></tr>
<tr><td colspan='2' valign='top'>Please <a href='https://pets.software/app/signin.aspx' target='_blank'>log in to the pets.software</a> to view the details.</td></tr>
<tr><td colspan='2' valign='top'>Regards<br>Pets.Software Team</td></tr>
</table></div></body></html>";

        // need to change below functionality for tickets

        public const string TICKETNEWDOCUMENTSUBJECT = "[pets.software Support #ticketno] document is uploaded";
        public const string TICKETNEWDOCUMENTBODY = @"<html><body>
<div style='font-family: arial,sans-serif;background-color:#eff2f7;font-size: 14px;text-align:left;padding: 20px 15px;border-radius: 5px;'>
<table width='100%' cellspacing='10' cellpadding='10' align='center'>
<tr><td colspan='2' valign='top'><img style='max-height: 75px;' src='{0}images/logo.png' alt='logo' /></td></tr>
<tr><td colspan='2' valign='top'></td></tr>
<tr><td width='20%' valign='top'>Ticket No:</td><td width='80%' valign='top'>#ticketno</td></tr>
<tr><td width='20%' valign='top'>Header:</td><td width='80%' valign='top'>#heading</td></tr>
<tr><td width='20%' valign='top'>Uploaded By:</td><td width='80%' valign='top'>#username</td></tr>
<tr><td colspan='2' valign='top'>Please <a href='https://pets.software/app/signin.aspx' target='_blank'>log in to the pets.software</a> to view the details.</td></tr>
<tr><td colspan='2' valign='top'>Regards<br>Pets.Software Team</td></tr>
</table></div></body></html>";

        // Business User Enquiry request
        public const string BUSINESSUSERREQUESTSUBJECT = "New Business User Enquiry Request";
        public const string BUSINESSUSERREQUESTBODY = @"<html><body><div style='font-family: arial,sans-serif;background-color:#eff2f7;font-size: 14px;text-align:left;padding: 20px 15px;border-radius: 5px;'>
        <img style='max-height: 75px;' src='{0}images/logo.png' alt='logo' /><br><br>
        <h3>Hello,</h3>
        <h5>New business user enquiry request.</h5> <br>Please check the following request details:<br><br> Name - #name<br>Email Address - #email<br><br>      
        <a href='https://pets.software/app/signin.aspx' target='_blank'>Click Here</a> for more details.<br><br>
        If you have questions contact our customer service team anytime at 'info@pets.software'.<br><br>       
        Thank you.<br><br>Regards<br>Pets.Software Team</p>
        </div></body></html>";

        // Business User Enquiry request rejected
        public const string BUSINESSUSERREQUESTREJECTSUBJECT = "Business User Enquiry Reject";
        public const string BUSINESSUSERREQUESTREJECTBODY = @"<html><body><div style='font-family: arial,sans-serif;background-color:#eff2f7;font-size: 14px;text-align:left;padding: 20px 15px;border-radius: 5px;'>
        <img style='max-height: 75px;' src='{0}images/logo.png' alt='logo' /><br><br>
        <h3>Hello,</h3>
        <h5>Your business user enquiry request has been rejected.</h5> <br>Please check the following details:<br><br> Name - #name<br>Email Address - #email<br>Reason - #reason<br><br>     
        <a href='https://pets.software/app/signin.aspx' target='_blank'>Click Here</a> for more details.<br><br>
        If you have questions contact our customer service team anytime at 'info@pets.software'.<br><br>       
        Thank you.<br><br>Regards<br>Pets.Software Team</p>
        </div></body></html>";

        // Business User Enquiry request accepted
        public const string BUSINESSUSERREQUESTACCEPTSUBJECT = "Business User Enquiry Approved";
        public const string BUSINESSUSERREQUESTACCEPTBODY = @"<html><body><div style='font-family: arial,sans-serif;background-color:#eff2f7;font-size: 14px;text-align:left;padding: 20px 15px;border-radius: 5px;'>
        <img style='max-height: 75px;' src='{0}images/logo.png' alt='logo' /><br><br>
        <h3>Hello,#name</h3>
        <h5>Your business user enquiry request has been approved.</h5> <br>Please check the following details:<br><br> Name - #name<br>Email Address - #email<br>Company - #company<br><br>    
        <a href='https://pets.software/app/signin.aspx' target='_blank'>Click Here</a> for more details.<br><br>
        If you have questions contact our customer service team anytime at 'info@pets.software'.<br><br>       
        Thank you.<br><br>Regards<br>Pets.Software Team</p>
        </div></body></html>";

        // Business User Enquiry request accepted message for Admin
        public const string BUSINESSUSERREQUESTACCEPTADMINSUBJECT = "Business User Enquiry Approved";
        public const string BUSINESSUSERREQUESTACCEPTADMINBODY = @"<html><body><div style='font-family: arial,sans-serif;background-color:#eff2f7;font-size: 14px;text-align:left;padding: 20px 15px;border-radius: 5px;'>
        <img style='max-height: 75px;' src='{0}images/logo.png' alt='logo' /><br><br>
        <h3>Hello,</h3>
        <h5>The enquiry request for a new business user has been approved, and the business has been successfully registered</h5> <br>Please check the following details:<br><br> Name - #name<br>Email Address - #email<br>Company - #company<br><br>    
        <a href='https://pets.software/app/signin.aspx' target='_blank'>Click Here</a> for more details.<br><br>
        If you have questions contact our customer service team anytime at 'info@pets.software'.<br><br>       
        Thank you.<br><br>Regards<br>Pets.Software Team</p>
        </div></body></html>";


        public const string USERCHANGEPASSWORDSUBJECT = "Pets.Software Password Assistance";
        public const string USERCHANGEPASSWORDBODY = @"<html><body><div style='font-family: arial,sans-serif;background-color:#eff2f7;font-size: 14px;text-align:left;padding: 20px 15px;border-radius: 5px;'>
        <img style='max-height: 75px;' src='{0}images/logo.png' alt='logo' /><br><br>
        <h3>Hello #name,</h3><h3>Your password changed</h3><br><br>
Here is your login credentials:<br><br>
Username: #email<br>
Password: #pwd<br><br>
To learn more about pets.software and see how it can work for you please <a href='https://pets.software/app/signin.aspx' target='_blank'>Click Here</a>.<br><br>
        If you have questions contact our customer service team anytime at 'info@pets.software'.<br><br>
        We’re so happy to have you!<br>
        Thank you.<br><br>Regards<br>Pets.Software Team</p>
        </div></body></html>";

        //new staff added email to staff
        public const string NEWSTAFFWELCOMEEMAILSUBJECT = "Welcome to #companyname Team";
        public const string NEWSTAFFWELCOMEEMAILBODY = @"<html><body><div style='font-family: arial,sans-serif;background-color:#eff2f7;font-size: 14px;text-align:left;padding: 20px 15px;border-radius: 5px;'>
    <img style='max-height: 75px;' src='{0}images/logo.png' alt='logo' /><br><br>
    <h3>Hello #name,</h3>
    We’re excited to welcome you as our new <strong>#jobtitle</strong> at #companyname!<br><br>
    Here are your login credentials to get started:<br><br>
    Username: #email<br>
    Password: #pwd<br><br>
    You can log in to your staff portal by clicking 
    <a href='https://pets.software/app/signin.aspx' target='_blank'>here</a>.<br><br>
    If you have any questions, feel free to reach out to your manager or email us at <a href='mailto:info@pets.software'>info@pets.software</a>.<br><br>
    We’re glad to have you on board and look forward to achieving great things together!<br><br>
    Regards,<br>
    Pets.Software Team
</div>
</body>
</html>";


        #endregion

        public static void SendEmail(MessageType xiType, object xiObject)
        {
            ParameterizedThreadStart objParameterizedThreadStart = null;
            Thread objSendMailThread = null;

            switch (xiType)
            {
                case MessageType.FORGOTPASSWORD:
                    objParameterizedThreadStart = new ParameterizedThreadStart(SendForgotPasswordMail);
                    break;

                case MessageType.CHANGEPASSWORD:
                    objParameterizedThreadStart = new ParameterizedThreadStart(SendChangePasswordMail);
                    break;

                case MessageType.SENDUSERJOINREQUEST:
                    objParameterizedThreadStart = new ParameterizedThreadStart(SendNewUserJoinMail);
                    break;

                case MessageType.SENDMEMBERSHIPREQUEST:
                    objParameterizedThreadStart = new ParameterizedThreadStart(SendMembershipRequest);
                    break;

                case MessageType.NEWUSERWELCOMEEMAIL:
                    objParameterizedThreadStart = new ParameterizedThreadStart(SendNewUserWelcomeEmail);
                    break;

                case MessageType.USERUNREGISTEREVENT:
                    objParameterizedThreadStart = new ParameterizedThreadStart(SendUserUnregisterEventMail);
                    break;

                case MessageType.OWNERUNREGISTEREVENT:
                    objParameterizedThreadStart = new ParameterizedThreadStart(SendOwnerUnregisterEventMail);
                    break;

                case MessageType.USERCANCELEVENTEMAIL:
                    objParameterizedThreadStart = new ParameterizedThreadStart(SendUserCancelEvent);
                    break;

                case MessageType.OWNERCANCELEVENTEMAIL:
                    objParameterizedThreadStart = new ParameterizedThreadStart(SendOwnerCancelEvent);
                    break;

                case MessageType.TICKETCHANGESTATUS:
                    objParameterizedThreadStart = new ParameterizedThreadStart(SendTicketStatusChangeMail);
                    break;

                case MessageType.TICKETADDNEW:
                    objParameterizedThreadStart = new ParameterizedThreadStart(SendTicketAddNewMail);
                    break;

                case MessageType.TICKETADDNEWCOMMENT:
                    objParameterizedThreadStart = new ParameterizedThreadStart(SendTicketNewCommentMail);
                    break;

                case MessageType.TICKETADDNEWDOCUMENT:
                    objParameterizedThreadStart = new ParameterizedThreadStart(SendTicketNewDocumentMail);
                    break;

                case MessageType.BUSINESSUSERREQUEST:
                    objParameterizedThreadStart = new ParameterizedThreadStart(SendBusinessUserRequestMail);
                    break;

                case MessageType.BUSINESSUSERREQUESTREJECT:
                    objParameterizedThreadStart = new ParameterizedThreadStart(SendBusinessUserRequestRejectMail);
                    break;

                case MessageType.BUSINESSUSERREQUESTACCEPT:
                    objParameterizedThreadStart = new ParameterizedThreadStart(SendBusinessUserRequestAcceptMail);
                    break;

                case MessageType.BUSINESSUSERREQUESTACCEPTADMIN:
                    objParameterizedThreadStart = new ParameterizedThreadStart(SendBusinessUserRequestAcceptMailToAdmin);
                    break;

                case MessageType.USERCHANGEPASSWORD:
                    objParameterizedThreadStart = new ParameterizedThreadStart(SendOldUserChangePasswordEmail);
                    break;

                case MessageType.NEWSTAFFWELCOMEEMAIL:
                    objParameterizedThreadStart = new ParameterizedThreadStart(SendNewStaffWelcomeEmail);
                    break;
            }

            try
            {
                objSendMailThread = new Thread(objParameterizedThreadStart);
                objSendMailThread.IsBackground = true;
                objSendMailThread.Start(xiObject);


                //objSendMailThread = new Thread(objParameterizedThreadStart);
                //objSendMailThread.IsBackground = true;

                //string[] threadparams = new string[3];
                //threadparams[0] = BusinessBase.ClientDBName;
                //threadparams[1] = BusinessBase.Timezone;
                //threadparams[2] = BusinessBase.DateFormat;
                //objSendMailThread.Name = string.Join("|", threadparams);

                //objSendMailThread.Start(xiObject);
            }
            catch { }
        }

        private static void SendForgotPasswordMail(object xiUser)
        {
            lock (SyncObject1)
            {
                User objUser = xiUser as User;
                if (objUser == null || string.IsNullOrEmpty(objUser.MainUserId)) return;

                string body = FORGOTPASSADDBODY;
                body = body.Replace("#email", objUser.Emailaddress);
                body = body.Replace("#token", objUser.UserToken);

                MailMessage mailMessage = new MailMessage();
                mailMessage.Subject = FORGOTPASSSUBJECT;
                mailMessage.Body = body;
                mailMessage.To.Add(objUser.Emailaddress);

                BreederMail.Mail(mailMessage);
            }
        }

        private static void SendChangePasswordMail(object xiUserEmail)
        {
            lock (SyncObject2)
            {
                if (xiUserEmail == null || xiUserEmail == DBNull.Value) return;

                string userEmail = string.Empty;
                try
                {
                    userEmail = Convert.ToString(xiUserEmail);
                }
                catch { }

                if (BusinessBase.IsEmail(userEmail) == false) return;

                BABusiness.User.AddUserSessionLog(userEmail, "password_updated");

                MailMessage mailMessage = new MailMessage();
                mailMessage.Subject = CHANGEPASSWORDSUBJECT;
                mailMessage.Body = CHANGEPASSWORDBODY;
                mailMessage.To.Add(userEmail);

                BreederMail.Mail(mailMessage);

                //string notification = "Your password has been changed. If this wasn't you, your account has been compromised. Please reset your password";
                //string[] userIds = User.GetUserIds(mailMessage.To);
                //foreach (string userId in userIds)
                //{
                //    //int unreadcount = Common.GetUnreadNotificationCount(userId, 1);
                //    //objNotify.SendMessage(userId, notification, unreadcount);
                //}
            }
        }

        private static void SendNewUserJoinMail(object xiUserEmail)
        {
            lock (SyncObject3)
            {
                if (xiUserEmail == null || xiUserEmail == DBNull.Value) return;

                string userEmail = string.Empty;
                try
                {
                    userEmail = Convert.ToString(xiUserEmail);
                }
                catch { }

                if (BusinessBase.IsEmail(userEmail) == false) return;

                BABusiness.User.AddUserSessionLog(userEmail, "Registration Request");

                MailMessage mailMessage = new MailMessage();
                mailMessage.Subject = SENDUSERJOINREQUESTSUBJECT;
                mailMessage.Body = SENDUSERJOINREQUESTBODY;
                mailMessage.To.Add(userEmail);

                BreederMail.Mail(mailMessage);
            }
        }

        private static void SendMembershipRequest(object xiCollection)
        {
            NameValueCollection collection = (NameValueCollection)xiCollection;

            lock (SyncObject4)
            {
                if (collection == null) return;

                string body = SENDMEMBERSHIPREQUESTBODY;
                body = body.Replace("#email", collection["memberemail"]);
                body = body.Replace("#name", collection["membername"]);

                MailMessage mailMessage = new MailMessage();
                mailMessage.Subject = SENDMEMBERSHIPREQUESTSUBJECT;
                mailMessage.Body = body;
                mailMessage.To.Add(collection["toemail"]);

                BreederMail.Mail(mailMessage);
            }
        }

        private static void SendNewUserWelcomeEmail(object xiCollection)
        {
            NameValueCollection collection = (NameValueCollection)xiCollection;

            lock (SyncObject5)
            {
                if (collection == null) return;

                string body = NEWUSERWELCOMEEMAILBODY;
                body = body.Replace("#name", collection["fname"] + " " + collection["lname"]);
                body = body.Replace("#email", collection["email"]);
                body = body.Replace("#pwd", BASecurity.Decrypt(collection["password"], BusinessBase.FixedSaltKey));

                MailMessage mailMessage = new MailMessage();
                mailMessage.Subject = NEWUSERWELCOMEEMAILSUBJECT;
                mailMessage.Body = body;
                mailMessage.To.Add(collection["email"]);

                BreederMail.Mail(mailMessage);
            }
        }

        private static void SendUserUnregisterEventMail(object xiCollection)
        {
            NameValueCollection collection = xiCollection as NameValueCollection;
            lock (SyncObject6)
            {
                if (collection == null) return;
                NameValueCollection ecollection = EventBA.GetEventDetail(collection["eventid"]);
                NameValueCollection usercollection = UserBA.GetUserDetail(collection["userid"]);

                string body = USERUNREGISTEREVENTBODY;
                body = body.Replace("#name", usercollection["fname"] + " " + usercollection["lname"]);
                body = body.Replace("#title", ecollection["title"]);
                body = body.Replace("#venue", ecollection["venue"]);
                body = body.Replace("#datetime", collection["eventdatetime"]);

                MailMessage mailMessage = new MailMessage();
                mailMessage.Subject = USERUNREGISTEREVENTSUBJECT;
                mailMessage.Body = body;
                mailMessage.To.Add(usercollection["email"]);

                BreederMail.Mail(mailMessage);
            }
        }

        private static void SendOwnerUnregisterEventMail(object xiCollection)
        {
            NameValueCollection collection = xiCollection as NameValueCollection;
            lock (SyncObject7)
            {
                if (collection == null) return;

                NameValueCollection ecollection = EventBA.GetEventDetail(collection["eventid"]);
                NameValueCollection usercollection = UserBA.GetUserDetail(collection["userid"]);

                string body = OWNERUNREGISTEREVENTBODY;
                body = body.Replace("#ownername", collection["eventownername"]);
                body = body.Replace("#username", usercollection["fname"] + " " + usercollection["lname"]);
                body = body.Replace("#title", ecollection["title"]);
                body = body.Replace("#venue", ecollection["venue"]);
                body = body.Replace("#datetime", collection["eventdatetime"]);

                MailMessage mailMessage = new MailMessage();
                mailMessage.Subject = OWNERUNREGISTEREVENTSUBJECT;
                mailMessage.Body = body;
                mailMessage.To.Add(collection["eventowneremail"]);
                //mailMessage.CC.Add(collection["eventowneremail"]);

                BreederMail.Mail(mailMessage);
            }
        }

        private static void SendUserCancelEvent(object xiCollection)
        {
            NameValueCollection collection = xiCollection as NameValueCollection;
            lock (SyncObject8)
            {
                if (collection == null) return;

                string body = USERCANCELEVENTEMAILBODY;
                body = body.Replace("#name", collection["fullname"]);
                body = body.Replace("#title", collection["title"]);
                body = body.Replace("#venue", collection["venue"]);
                body = body.Replace("#cancelreason", collection["delete_reason"]);

                MailMessage mailMessage = new MailMessage();
                mailMessage.Subject = USERCANCELEVENTEMAILSUBJECT;
                mailMessage.Body = body;
                mailMessage.To.Add(collection["email"]);

                BreederMail.Mail(mailMessage);
            }
        }

        private static void SendOwnerCancelEvent(object xiCollection)
        {
            NameValueCollection collection = xiCollection as NameValueCollection;
            lock (SyncObject9)
            {
                if (collection == null) return;

                string body = OWNERCANCELEVENTEMAILBODY;
                body = body.Replace("#name", collection["eventownername"]);
                body = body.Replace("#title", collection["title"]);
                body = body.Replace("#venue", collection["venue"]);
                body = body.Replace("#cancelreason", collection["delete_reason"]);

                MailMessage mailMessage = new MailMessage();
                mailMessage.Subject = OWNERCANCELEVENTEMAILSUBJECT;
                mailMessage.Body = body;
                mailMessage.To.Add(collection["eventowneremail"]);
                //mailMessage.CC.Add(collection["eventowneremail"]);

                BreederMail.Mail(mailMessage);
            }
        }

        private static void SendTicketStatusChangeMail(object xiNameValueCollection)
        {
            lock (SyncObject13)
            {
                //string[] threadparams = Thread.CurrentThread.Name.Split('|');
                //Thread.SetData(Thread.GetNamedDataSlot("_constring_"), threadparams[0]);
                //Thread.SetData(Thread.GetNamedDataSlot("timezone"), threadparams[1]);
                //Thread.SetData(Thread.GetNamedDataSlot("dtformat"), threadparams[2]);
                //BADBUtils.Utils.Timezone = threadparams[1];

                NameValueCollection collection = xiNameValueCollection as NameValueCollection;
                if (collection == null || string.IsNullOrEmpty(collection["ticketid"])) return;

                if (string.IsNullOrEmpty(collection["userid"])) collection["userid"] = "1";

                NameValueCollection tcollection = Ticket.GetTicket(collection["ticketid"], collection["userid"]);
                if (tcollection == null) return;

                int status = BusinessBase.ConvertToInteger(tcollection["status"]);
                if (tcollection["voneeded"] == "1" && tcollection["isbug"] != "1")
                {
                    switch (status)
                    {
                        case (int)Ticket.Status.WAITINGFORDEVAPPROVAL:
                            tcollection["statusname"] = "Waiting for customer approval";
                            break;

                        case (int)Ticket.Status.APPROVEDFORDEVELOPMENT:
                            tcollection["statusname"] += " ,VO";
                            break;
                    }
                }

                if (collection.AllKeys.Contains("platform") && !string.IsNullOrEmpty(collection["platform"]))
                {
                    tcollection["statusname"] += " [" + collection["platform"] + " platform]";
                }

                string flags = string.Empty;
                if (tcollection["isbug"] == "1") flags = "Bug";
                else
                {
                    flags = "General Feature";
                    if (tcollection["voneeded"] == "1") flags += ", VO";
                }

                string body = TICKETSTATUSCHAGEBODY;
                body = body.Replace("#ticketno", tcollection["ticketno"].PadLeft(5, '0'));
                body = body.Replace("#status", tcollection["statusname"]);
                body = body.Replace("#heading", tcollection["header"]);
                body = body.Replace("#edthours", tcollection["etd"]);
                body = body.Replace("#flags", flags);

                string notes = string.Empty; //need to change for vo needed in below if condition
                                             //if (tcollection["isbug"] != "1" && status == (int)Ticket.Status.APPROVEDFORDEVELOPMENT) notes = "<div style='color: #a94442;background-color: #f2dede;border-color: #ebccd1;padding: 15px;margin-bottom: 20px;border: 1px solid transparent;border-radius: 4px;display: block;'>As you have approved the ticket for development, Pets.Software will charge you as per the specified EDT hours in the ticket with the previously agreed hourly rate.</div>";
                                             //if (tcollection["isbug"] != "1" && status == (int)Ticket.Status.CLOSED) notes = "<div style='color: #a94442;background-color: #f2dede;border-color: #ebccd1;padding: 15px;margin-bottom: 20px;border: 1px solid transparent;border-radius: 4px;display: block;'>The agreed payment of this ticket will be handled in separate communication.</div>";
                if (tcollection["voneeded"] == "1" && tcollection["isbug"] != "1" && status == (int)Ticket.Status.APPROVEDFORDEVELOPMENT) notes = "<div style='color: #a94442;background-color: #f2dede;border-color: #ebccd1;padding: 15px;margin-bottom: 20px;border: 1px solid transparent;border-radius: 4px;display: block;'>As you have approved the ticket for development, Pets.Software will charge you as per the specified EDT hours in the ticket with the previously agreed hourly rate.</div>";
                if (tcollection["voneeded"] == "1" && tcollection["isbug"] != "1" && status == (int)Ticket.Status.CLOSED) notes = "<div style='color: #a94442;background-color: #f2dede;border-color: #ebccd1;padding: 15px;margin-bottom: 20px;border: 1px solid transparent;border-radius: 4px;display: block;'>The agreed payment of this ticket will be handled in separate communication.</div>";
                body = body.Replace("#notes", notes);

                //string nexturl = "ticket#" + collection["ticketid"] + "#" + collection["clientid"];
                //string nexturl = "ticket#" + collection["ticketid"];
                //nexturl = BASecurity.Encrypt(nexturl, BusinessBase.FixedSaltKey);
                //body = body.Replace("#urlstring", nexturl);

                if (collection.AllKeys.Contains("platform") && collection["platform"] == "Dev")
                {
                    body = string.Format(body, "https://pets.software/");//need to change
                }

                string subject = TICKETSTATUSCHAGESUBJECT.Replace("#ticketno", tcollection["ticketno"]);

                MailMessage mailMessage = new MailMessage();
                mailMessage.Subject = subject;
                mailMessage.Body = body;

                bool sendToInternalPeople = false;

                if (tcollection["voneeded"] == "1" && tcollection["isbug"] != "1")
                //if (tcollection["isbug"] != "1")
                {
                    if (status == (int)Ticket.Status.ESTIMATEEDT || status == (int)Ticket.Status.APPROVEROLLEDOUT)
                    {
                        sendToInternalPeople = true;
                    }
                }
                else
                {
                    if (status == (int)Ticket.Status.ESTIMATEEDT || status == (int)Ticket.Status.WAITINGFORDEVAPPROVAL || status == (int)Ticket.Status.APPROVEROLLEDOUT)
                    {
                        sendToInternalPeople = true;
                    }
                }

                if (sendToInternalPeople)
                {
                    mailMessage.To.Add("amitmanekar19@gmail.com");
                  
                }
                else
                {
                    mailMessage.To.Add(new MailAddress("info@pets.software", "No-Reply"));
                    mailMessage.Bcc.Add("amitmanekar19@gmail.com");
                   

                    mailMessage.Bcc.Add(tcollection["authoremail"]);

                    if (tcollection["voneeded"] == "1" && tcollection["isbug"] != "1" && (status == (int)Ticket.Status.WAITINGFORDEVAPPROVAL || status == (int)Ticket.Status.APPROVEDFORDEVELOPMENT))
                    {
                        DataTable alertDistList = Ticket.GetAllVODistListEmails();
                        if (alertDistList != null)
                        {
                            foreach (DataRow row in alertDistList.Rows)
                            {
                                string emailaddress = BusinessBase.ConvertToString(row["emailaddress"]);
                                if (BusinessBase.IsEmail(emailaddress)) mailMessage.Bcc.Add(emailaddress);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(tcollection["optionalemails"]))
                    {
                        foreach (string email in tcollection["optionalemails"].Split(','))
                        {
                            string em = email.Trim();
                            if (em.Length == 0 || !BusinessBase.IsEmail(em)) continue;

                            mailMessage.Bcc.Add(em);
                        }
                    }

                }

                BreederMail.Mail(mailMessage);
            }
        }

        private static void SendTicketAddNewMail(object xiNameValueCollection)
        {
            lock (SyncObject14)
            {
                //string[] threadparams = Thread.CurrentThread.Name.Split('|');
                //Thread.SetData(Thread.GetNamedDataSlot("_constring_"), threadparams[0]);
                //Thread.SetData(Thread.GetNamedDataSlot("timezone"), threadparams[1]);
                //Thread.SetData(Thread.GetNamedDataSlot("dtformat"), threadparams[2]);
                //BADBUtils.Utils.Timezone = threadparams[1];

                NameValueCollection collection = xiNameValueCollection as NameValueCollection;
                if (collection == null || string.IsNullOrEmpty(collection["ticketid"])) return;

                if (string.IsNullOrEmpty(collection["userid"])) collection["userid"] = "1";

                NameValueCollection tcollection = Ticket.GetTicket(collection["ticketid"], collection["userid"]);
                if (tcollection == null) return;

                string ticketNo = tcollection["ticketno"].PadLeft(5, '0');

                if (tcollection["isbug"] == "1") tcollection["header"] += " [Bug]";

                string body = TICKETADDNEWBODY;
                body = body.Replace("#ticketno", ticketNo);
                body = body.Replace("#heading", tcollection["header"]);
                body = body.Replace("#author", tcollection["author"]);

                //string nexturl = "ticket#" + collection["ticketid"] + "#" + collection["clientid"];
                //string nexturl = "ticket#" + collection["ticketid"];
                //nexturl = BASecurity.Encrypt(nexturl, BusinessBase.FixedSaltKey);
                //body = body.Replace("#urlstring", nexturl);

                string subject = TICKETADDNEWSUBJECT.Replace("#ticketno", ticketNo);

                MailMessage mailMessage = new MailMessage();
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.To.Add(tcollection["authoremail"]);
                mailMessage.To.Add("amitmanekar19@gmail.com");

                BreederMail.Mail(mailMessage);
            }
        }

        private static void SendTicketNewCommentMail(object xiNameValueCollection)
        {
            lock (SyncObject78)
            {
                //string[] threadparams = Thread.CurrentThread.Name.Split('|');
                //Thread.SetData(Thread.GetNamedDataSlot("_constring_"), threadparams[0]);
                //Thread.SetData(Thread.GetNamedDataSlot("timezone"), threadparams[1]);
                //Thread.SetData(Thread.GetNamedDataSlot("dtformat"), threadparams[2]);
                //BADBUtils.Utils.Timezone = threadparams[1];

                NameValueCollection collection = xiNameValueCollection as NameValueCollection;
                if (collection == null) return;

                NameValueCollection ticketcollection = Ticket.GetTicket(collection["ticketid"]);
                if (ticketcollection == null) return;

                string ticketNo = ticketcollection["ticketno"].PadLeft(5, '0');

                string body = TICKETNEWCOMMENTBODY;
                body = body.Replace("#ticketno", collection["ticketno"]);
                body = body.Replace("#heading", collection["header"]);
                body = body.Replace("#username", collection["username"]);
                body = body.Replace("#comments", collection["comment"]);

                //string nexturl = "ticket#" + collection["ticketid"] + "#" + collection["clientid"];
                //string nexturl = "ticket#" + collection["ticketid"];
                //nexturl = BASecurity.Encrypt(nexturl, BusinessBase.FixedSaltKey);
                //body = body.Replace("#urlstring", nexturl);

                string subject = TICKETNEWCOMMENTSUBJECT.Replace("#ticketno", ticketNo);

                MailMessage mailMessage = new MailMessage();
                mailMessage.Subject = subject;
                mailMessage.Body = body;

                mailMessage.To.Add(new MailAddress("info@pets.software", "No-Reply"));
                mailMessage.Bcc.Add("amitmanekar19@gmail.com");

                mailMessage.Bcc.Add(collection["authoremail"]);

                BreederMail.Mail(mailMessage);
            }
        }

        private static void SendTicketNewDocumentMail(object xiNameValueCollection)
        {
            lock (SyncObject79)
            {
                //string[] threadparams = Thread.CurrentThread.Name.Split('|');
                //Thread.SetData(Thread.GetNamedDataSlot("_constring_"), threadparams[0]);
                //Thread.SetData(Thread.GetNamedDataSlot("timezone"), threadparams[1]);
                //Thread.SetData(Thread.GetNamedDataSlot("dtformat"), threadparams[2]);
                //BADBUtils.Utils.Timezone = threadparams[1];

                NameValueCollection collection = xiNameValueCollection as NameValueCollection;
                if (collection == null) return;

                NameValueCollection ticketcollection = Ticket.GetTicket(collection["ticketid"]);
                if (ticketcollection == null) return;

                string ticketNo = ticketcollection["ticketno"].PadLeft(5, '0');

                string body = TICKETNEWDOCUMENTBODY;
                body = body.Replace("#ticketno", collection["ticketno"]);
                body = body.Replace("#heading", collection["header"]);
                body = body.Replace("#username", collection["username"]);

                //string nexturl = "ticket#" + collection["ticketid"] + "#" + collection["clientid"];
                //string nexturl = "ticket#" + collection["ticketid"];
                //nexturl = BASecurity.Encrypt(nexturl, BusinessBase.FixedSaltKey);
                //body = body.Replace("#urlstring", nexturl);

                string subject = TICKETNEWDOCUMENTSUBJECT.Replace("#ticketno", ticketNo);

                MailMessage mailMessage = new MailMessage();
                mailMessage.Subject = subject;
                mailMessage.Body = body;

                mailMessage.To.Add(new MailAddress("info@pets.software", "No-Reply"));
                mailMessage.Bcc.Add("amitmanekar19@gmail.com");

                mailMessage.Bcc.Add(collection["authoremail"]);

                BreederMail.Mail(mailMessage);
            }
        }

        private static void SendBusinessUserRequestMail(object xiCollection)
        {
            NameValueCollection collection = xiCollection as NameValueCollection;
            lock (SyncObject81)
            {
                if (collection == null) return;
                //NameValueCollection usercollection = UserBA.GetBusinessEnquiryDetail(collection["enquiryid"]);

                string body = BUSINESSUSERREQUESTBODY;
                body = body.Replace("#name", collection["name"]);
                body = body.Replace("#email", collection["email"]);

                MailMessage mailMessage = new MailMessage();
                mailMessage.Subject = BUSINESSUSERREQUESTSUBJECT;
                mailMessage.Body = body;
                mailMessage.To.Add("sumit.deshpande10@gmail.com");

                BreederMail.Mail(mailMessage);
            }
        }

        private static void SendBusinessUserRequestRejectMail(object xiCollection)
        {
            NameValueCollection collection = xiCollection as NameValueCollection;
            lock (SyncObject82)
            {
                if (collection == null) return;
                // NameValueCollection usercollection = UserBA.GetBusinessEnquiryDetail(collection["enquiryid"]);

                string body = BUSINESSUSERREQUESTREJECTBODY;
                body = body.Replace("#name", collection["name"]);
                body = body.Replace("#email", collection["email"]);
                body = body.Replace("#reason", collection["reason"]);

                MailMessage mailMessage = new MailMessage();
                mailMessage.Subject = BUSINESSUSERREQUESTREJECTSUBJECT;
                mailMessage.Body = body;
                mailMessage.To.Add(collection["email"]);

                BreederMail.Mail(mailMessage);
            }
        }

        private static void SendBusinessUserRequestAcceptMail(object xiCollection)
        {
            NameValueCollection collection = xiCollection as NameValueCollection;
            lock (SyncObject83)
            {
                if (collection == null) return;
                // NameValueCollection usercollection = UserBA.GetBusinessEnquiryDetail(collection["enquiryid"]);

                string body = BUSINESSUSERREQUESTACCEPTBODY;
                body = body.Replace("#name", collection["name"]);
                body = body.Replace("#email", collection["email"]);
                body = body.Replace("#company", collection["companyname"]);

                MailMessage mailMessage = new MailMessage();
                mailMessage.Subject = BUSINESSUSERREQUESTACCEPTSUBJECT;
                mailMessage.Body = body;
                mailMessage.To.Add(collection["email"]);

                BreederMail.Mail(mailMessage);
            }
        }

        private static void SendBusinessUserRequestAcceptMailToAdmin(object xiCollection)
        {
            NameValueCollection collection = xiCollection as NameValueCollection;
            lock (SyncObject84)
            {
                if (collection == null) return;
                // NameValueCollection usercollection = UserBA.GetBusinessEnquiryDetail(collection["enquiryid"]);

                string body = BUSINESSUSERREQUESTACCEPTADMINBODY;
                body = body.Replace("#name", collection["name"]);
                body = body.Replace("#email", collection["email"]);
                body = body.Replace("#company", collection["companyname"]);

                MailMessage mailMessage = new MailMessage();
                mailMessage.Subject = BUSINESSUSERREQUESTACCEPTADMINSUBJECT;
                mailMessage.Body = body;
                // mailMessage.To.Add(collection["email"]);
                mailMessage.To.Add("amitmanekar19@gmail.com");// ADMIN

                BreederMail.Mail(mailMessage);
            }
        }

        private static void SendOldUserChangePasswordEmail(object xiCollection)
        {
            NameValueCollection collection = (NameValueCollection)xiCollection;

            lock (SyncObject85)
            {
                if (collection == null) return;

                string body = USERCHANGEPASSWORDBODY;
                body = body.Replace("#name", collection["fname"] + " " + collection["lname"]);
                body = body.Replace("#email", collection["email"]);
                body = body.Replace("#pwd", BASecurity.Decrypt(collection["password"], BusinessBase.FixedSaltKey));

                MailMessage mailMessage = new MailMessage();
                mailMessage.Subject = USERCHANGEPASSWORDSUBJECT;
                mailMessage.Body = body;
                mailMessage.To.Add(collection["email"]);

                BreederMail.Mail(mailMessage);
            }
        }

        private static void SendNewStaffWelcomeEmail(object xiCollection)
        {
            NameValueCollection collection = (NameValueCollection)xiCollection;

            lock (SyncObject86)
            {
                if (collection == null) return;                

                NameValueCollection staffcollection = BUStaff.GetStaffDetail(collection["staffid"], collection["bu_id"]);
                if (staffcollection == null) return;

                NameValueCollection usercollection = UserBA.GetUserDetail(staffcollection["userid"]);
                NameValueCollection bucollection = UserBA.GetBusinessUserDetail(collection["bu_id"]);

                string body = NEWSTAFFWELCOMEEMAILBODY;
                body = body.Replace("#name", staffcollection["fname"] + " " + staffcollection["lname"]);
                body = body.Replace("#email", staffcollection["email"]);
                body = body.Replace("#pwd", BASecurity.Decrypt(usercollection["password"], BusinessBase.FixedSaltKey));
                body = body.Replace("#jobtitle", staffcollection["jobtitle"]);
                body = body.Replace("#companyname", bucollection["companyname"]);

                string subject = NEWSTAFFWELCOMEEMAILSUBJECT.Replace("#companyname", bucollection["companyname"]);

                MailMessage mailMessage = new MailMessage();
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.To.Add(staffcollection["email"]);

                BreederMail.Mail(mailMessage);
            }
        }

        public static Exception Mail(MailMessage xiMessageObject)
        {
            Exception x = null;
            try
            {
                if (xiMessageObject.To.Count == 0) return null;               
                // new code
                string subject = System.Text.RegularExpressions.Regex.Replace(xiMessageObject.Subject, "<.*?>", string.Empty).Replace("&nbsp;", string.Empty);
                xiMessageObject.Subject = subject;
                xiMessageObject.Body = string.Format(xiMessageObject.Body, PageURL);
                xiMessageObject.IsBodyHtml = true;
                xiMessageObject.From = new MailAddress("mail@pets.software", "pets.software");
             
                try
                {
                    SmtpClient smtpClient = new SmtpClient();
                    smtpClient.Host = "192.168.31.10";
                    smtpClient.Port = 25;
                    smtpClient.EnableSsl = false;                    
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.Send(xiMessageObject);
                }
                catch (Exception ex1) { x = ex1; }

            }
            catch (Exception ex) { x = ex; }

            return x;
        }
    }

}
