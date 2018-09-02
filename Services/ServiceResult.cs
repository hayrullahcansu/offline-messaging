using System.Collections.Generic;

namespace OfflineMessaging.Services
{
    public class ServiceResult
    {
        public bool Succeed => !HasError();

        public string SuccessMessage
        {
            get
            {
                if (Succeed) return "SUCCESS";
                else return "NOT_SUCCESS";
            }
        }

        public Dictionary<string, string> Errors { get; set; }

        public ServiceResult()
        {
            Errors = new Dictionary<string, string>();
        }

        public bool HasError()
        {
            return Errors.Count > 0;
        }

        public ServiceResult AddError(string key, string desc)
        {
            Errors.Add(key, desc);
            return this;
        }

        public ServiceResult SetFailed()
        {
            return AddError("ERROR", "AN_ERROR_OCCURED");
        }

        public ServiceResult SetFailed(int errorCode)
        {
            return SetFailed($"{errorCode}");
        }

        public ServiceResult SetFailed(string errorCode)
        {
            return AddError("ERROR", errorCode);
        }

        public ServiceResult NeedLogin()
        {
            return SetFailed("NEED_LOGIN");
        }
    }
}