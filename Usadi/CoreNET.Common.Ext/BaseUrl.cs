using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CoreNET.Common.Base
{
  public class BaseUrl : IURL
  {
    #region Properties
    #region Property Url
    private string _Url;
    public string Url
    {
      get { return _Url; }
      set { _Url = value; }
    }
    #endregion
    #region Property Local
    private string _Local;
    public string Local
    {
      get { return _Local; }
      set { _Local = value; }
    }
    #endregion
    #region Property App
    private string _App;
    public string App
    {
      get { return _App; }
      set { _App = value; }
    }
    #endregion
    #region Property Roleid
    private string _Roleid;
    public string Roleid
    {
      get { return _Roleid; }
      set { _Roleid = value; }
    }
    #endregion
    #region Property I
    private string _I;
    public string I
    {
      get { return _I; }
      set { _I = value; }
    }
    #endregion
    #region Property Id
    private string _Id;
    public string Id
    {
      get { return _Id; }
      set { _Id = value; }
    }
    #endregion
    #region Property Enable
    private string _Enable;
    public string Enable
    {
      get { return _Enable; }
      set { _Enable = value; }
    }
    #endregion
    #region Property Tb
    private string _Tb;
    public string Tb
    {
      get { return _Tb; }
      set { _Tb = value; }
    }
    #endregion
    #region Property At
    private string _At;
    public string At
    {
      get { return _At; }
      set { _At = value; }
    }
    #endregion
    #region Property Kt
    private string _Kt;
    public string Kt
    {
      get { return _Kt; }
      set { _Kt = value; }
    }
    #endregion
    #region Property Pt
    private string _Pt;
    public string Pt
    {
      get { return _Pt; }
      set { _Pt = value; }
    }
    #endregion
    #region Property Pj
    private string _Pj;
    public string Pj
    {
      get { return _Pj; }
      set { _Pj = value; }
    }
    #endregion
    #region Property PropertyName
    private string _PropertyName;
    public string PropertyName
    {
      get { return _PropertyName; }
      set { _PropertyName = value; }
    }
    #endregion
    #region Property PropertyName2
    private string _PropertyName2;
    public string PropertyName2
    {
      get { return _PropertyName2; }
      set { _PropertyName2 = value; }
    }
    #endregion
    #region Property Procid
    private string _Procid;
    public string Procid
    {
      get { return _Procid; }
      set { _Procid = value; }
    }
    #endregion
    #region Property Level
    private string _Level;
    public string Level
    {
      get { return _Level; }
      set { _Level = value; }
    }
    #endregion
    #region Property Child
    private string _Child;
    public string Child
    {
      get { return _Child; }
      set { _Child = value; }
    }
    #endregion
    #region Property Passdc
    private string _Passdc;
    public string Passdc
    {
      get { return _Passdc; }
      set { _Passdc = value; }
    }
    #endregion
    #region Property Current
    private string _Current;
    public string Current
    {
      get { return _Current; }
      set { _Current = value; }
    }
    #endregion
    #region Property Title
    private string _Title;
    public string Title
    {
      get { return _Title; }
      set { _Title = value; }
    }
    #endregion
    #region Property Titleid
    private string _Titleid;
    public string Titleid
    {
      get { return _Titleid; }
      set { _Titleid = value; }
    }
    #endregion
    #region Property Kkp
    private string _Kkp;
    public string Kkp
    {
      get { return _Kkp; }
      set { _Kkp = value; }
    }
    #endregion
    #region Property TBFolder
    private string _TBFolder;
    public string TBFolder
    {
      get { return _TBFolder; }
      set { _TBFolder = value; }
    }
    #endregion
    #region Property LCFolder
    private string _LCFolder;
    public string LCFolder
    {
      get { return _LCFolder; }
      set { _LCFolder = value; }
    }
    #endregion
    #region Property Path
    private string _Path;
    public string Path
    {
      get { return _Path; }
      set { _Path = value; }
    }
    #endregion
    #region Property Pathpname
    private string _Pathpname;
    public string Pathpname
    {
      get { return _Pathpname; }
      set { _Pathpname = value; }
    }
    #endregion
    #region Property Targetpathpname
    private string _Targetpathpname;
    public string Targetpathpname
    {
      get { return _Targetpathpname; }
      set { _Targetpathpname = value; }
    }
    #endregion
    #region Property Mode
    private string _Mode;
    public string Mode
    {
      get { return _Mode; }
      set { _Mode = value; }
    }
    #endregion
    #region Property Kolaborasi
    private string _Kolaborasi;
    public string Kolaborasi
    {
      get { return _Kolaborasi; }
      set { _Kolaborasi = value; }
    }
    #endregion
    #region Property Noversi
    private string _Noversi;
    public string Noversi
    {
      get { return _Noversi; }
      set { _Noversi = value; }
    }
    #endregion
    #region Property Hide
    private string _Hide;
    public string Hide
    {
      get { return _Hide; }
      set { _Hide = value; }
    }
    #endregion
    #region Property Versi
    private string _Versi;
    public string Versi
    {
      get { return _Versi; }
      set { _Versi = value; }
    }
    #endregion
    #region Property Role
    private string _Role;
    public string Role
    {
      get { return _Role; }
      set { _Role = value; }
    }
    #endregion
      
    #endregion
    #region Methods
    public BaseUrl()
    {
      Local = HttpContext.Current.Request["local"];
      App = GlobalAsp.GetRequestApp();
      Roleid = HttpContext.Current.Request["roleid"];
      I = GlobalAsp.GetRequestI().ToString();
      Id = GlobalAsp.GetRequestId().ToString();
      Enable = HttpContext.Current.Request["enable"];
      Path = HttpContext.Current.Request["path"];
    }
    public string ConstructURL()
    {
      return UtilityExt.ValidateURL(this, string.Format("{0}?local={1}&app={2}&roleid={3}&i={4}&id={5}"
          + "&enable={6}&pname={7}&passdc={8}&path={9}&mode={10}"
        , Url, Local, App, Roleid, I, Id
        , Enable, PropertyName, Passdc, Path, Mode));
    }
    public string GetTitle()
    {
      return HttpContext.Current.Request["title"];
    }
    public string GetTabTip()
    {
      return HttpContext.Current.Request["title"];
    }
    #endregion
  }
}
