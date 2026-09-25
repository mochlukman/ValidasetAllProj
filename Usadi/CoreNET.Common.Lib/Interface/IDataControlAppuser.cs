namespace CoreNET.Common.Base
{
  public interface IDataControlApp : IDataControl
  {
    void ResetParams(string idapp);
    string GetAppParam(string par);
  }
  public interface IDataControlAppuser : IDataControl
  {
    string GetUserID();
    string GetGroupingDataID();
    string GetUserPwd();
    string GetUserType();
    string GetUserDebugInfo();
    bool IsAdmin();
    bool IsEnableLookup();
    bool IsVisibleLookup();
    void SetUserID(string appID, string userID);
    void SetUserOL(string userID, string key);
    void UpdatePwd(string pwd);
    object GetValueProperty(string par);
    void SetValueProperty(string par, object val);
  }
}
