namespace MyAttd.Helpers
{
    public class RequestHandler
    {
        public void HandleIndexRequest()
        {
            // do something for the request  

            var message = "This is a much cleaner approach to access Session!";
            AppContext.Current.Session.Set("message", message);
        }

        public int GetSessionUserIDRequest()
        {
            return AppContext.Current.Session.Get<int>("_JsonUserID_ATTD");
        }

        public int GetSessionRoleIDRequest()
        {
            return AppContext.Current.Session.Get<int>("_JsonRoleID_ATTD");
        }

        public string GetSessionConStrRequest()
        {
            return AppContext.Current.Session.Get<string>("_JsonConStr_ATTD");
        }

        public string GetSessionStaffIDRequest()
        {
            return AppContext.Current.Session.Get<string>("_JsonStaffID_ATTD");
        }

        public string GetSessionAACPConStrRequest()
        {
            return AppContext.Current.Session.Get<string>("_JsonAACPConStr");
        }
    }
}
