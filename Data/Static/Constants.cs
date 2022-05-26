namespace someOnlineStore.Data.Static
{
    public static class Constants
    {
        public static string generateConfirmationEmail(string URL)
        {
            var message ="<html>"+
                "<h1>Confirm your email</h1>" +
                "Hello an account was was created using this Email on WStore, to confirm your email click the following URL within 2 hours.</p> </br>" +
                $"<a href=\"{URL}\">Confirm your account here.</a></br>" +
                "<p> If this was not you ignore this message.</p>"+
                "</html>";
            return message;
        }

        public static string generateEmailChangeConfirmationMail(string URL)
        {
            var message = "<html>" +
                "<h1>Your Email has been changed</h1>" +
                "Hello to confirm your new email click the following URL within 2 hours.</p> </br>" +
                $"<a href=\"{URL}\">Confirm your email here.</a></br>" +
                "<p> If this was not you ignore this message and change your password immedietely.</p>" +
                "</html>";
            return message;
        }

        public static string generatePasswordResetMail(string URL)
        {
            var message = "<html>" +
                "<h1>A Password reset was requested on your email</h1>" +
                "<p>Hello to confirm your new password click the following URL within 2 hours.</p> </br>" +
                $"<a href=\"{URL}\">Confirm your new password here.</a></br>" +
                "<p> If this was not you ignore this message and change your password immedietely.</p>" +
                "</html>";

            return message;
        }
    }
}
