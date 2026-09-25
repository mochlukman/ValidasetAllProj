using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Generic;
using Ext.Net;
using Ext.Net.Utilities;
using CoreNET.Common.Base;
using CoreNET.Common.BO;

namespace CoreNET.Common.BO
{
  #region ConstantTablesSS.
  public class ConstantTablesSys
  {
    public const string XMLDMBULAN = "Dmbulan";
    public const string XMLDMDOK = "Dmdok";
    public const string XMLDMLEVEL = "Dmlevel";
    public const string XMLDMPEMDA = "Dmpemda";
    public const string XMLDMROLE = "Dmrole";
    public const string XMLDMSTATUS = "Dmstatus";
    public const string XMLDMTAHUN = "Dmtahun";
    public const string XMLRPT00DATAVOL = "Rpt00datavol";
    public const string XMLRPT00REVALIDATE = "Rpt00revalidate";
    public const string XMLSS00APP = "Ss00app";
    public const string XMLSS00APPCONFIG = "Ss00appconfig";
    public const string XMLSS00APPCONFIGPAR = "Ss00appconfigpar";
    public const string XMLSS00APPISSUE = "Ss00appissue";
    public const string XMLSS00APPKKP = "Ss00appkkp";
    public const string XMLSS00APPMENU = "Ss00appmenu";
    public const string XMLSS00APPMENUCONFIG = "Ss00appmenuconfig";
    public const string XMLSS00APPREG = "Ss00appreg";
    public const string XMLSS00APPREGCONFIG = "Ss00appregconfig";
    public const string XMLSS00APPVERSI = "Ss00appversi";
    public const string XMLSS00APPVERSICONFIG = "Ss00appversiconfig";


    public const string XMLAPPMENU = "Appmenu";
    public const string XMLSS01APPMENU = "Ss01appmenu";
    public const string XMLSS01APPMENUCONFIG = "Ss01appmenuconfig";
    public const string XMLSS01APPMENUKKP = "Ss01appmenukkp";
    public const string XMLSS10USER = "Ss10user";
    public const string XMLSS10USERAPP = "Ss10userapp";
    public const string XMLSS10USERMENU = "Ss10usermenu";
    public const string XMLSS10USERTYPES = "Ss10usertypes";
    public const string XMLSS11USEROL = "Ss11userol";
    public const string XMLSS20GROUP = "Ss20group";
    public const string XMLSS20GROUPCONFIG = "Ss20groupconfig";
    public const string XMLSS20GROUPMENU = "Ss20groupmenu";
    public const string XMLSS20GROUPUSER = "Ss20groupuser";
    public const string XMLSS90DICTIONARY = "Ss90dictionary";
    public const string XMLSSOPERATION = "Ssoperation";
    public const string XMLSYSDIAGRAMS = "Sysdiagrams";
    public const string XMLSYSGETCOLS = "Sysgetcols";
    public const string XMLSYSGETFILTERS = "Sysgetfilters";
    public const string XMLSYSGETKEYS = "Sysgetkeys";
    public const string XMLSYSGETROWS = "Sysgetrows";
    public const string XMLSYSGETTREECOLS = "Sysgettreecols";
    public const string XMLTTDAFTDOK = "Ttdaftdok";
    public const string XMLTTSURATTGS = "Ttsurattgs";

    public const string XMLDMBEND = "Dmbend";
    public const string XMLDMDAFTBANK = "Dmdaftbank";
    public const string XMLDMDAFTKEG = "Dmdaftkeg";
    public const string XMLDMDAFTPEMDA = "Dmdaftpemda";
    public const string XMLDMDAFTPEMDAPAR = "Dmdaftpemdapar";
    public const string XMLDMDAFTUNIT = "Dmdaftunit";
    public const string XMLDMDAFTUNITPAR = "Dmdaftunitpar";
    public const string XMLDMJENISP3 = "Dmjenisp3";
    public const string XMLDMJENISPAR = "Dmjenispar";
    public const string XMLDMJENISTRANS = "Dmjenistrans";
    public const string XMLDMMATANG = "Dmmatang";
    public const string XMLDMPEGAWAI = "Dmpegawai";
    public const string XMLDMPIHAKKE3 = "Dmpihakke3";
    public const string XMLDMTYPETRDETIL = "Dmtypetrdetil";
    public const string XMLMAKEGUNIT = "Makegunit";
    public const string XMLMAKEGUNITPAR = "Makegunitpar";
    public const string XMLMARKA = "Marka";
    public const string XMLMARKADANA = "Markadana";
    public const string XMLMARKADET = "Markadet";
    public const string XMLMARKAKAS = "Markakas";
    public const string XMLRPTDAFTDOK = "Rptdaftdok";
    public const string XMLRPTDAFTUNIT = "Rptdaftunit";
    public const string XMLRPTDAFTUNITAKUN = "Rptdaftunitakun";
    public const string XMLRPTDAFTUNITURUS = "Rptdaftuniturus";
    public const string XMLRPTDAFTURUSKEG = "Rptdafturuskeg";
    public const string XMLRPTDAFTURUSUNIT = "Rptdafturusunit";
    public const string XMLRPTLRA = "Rptlra";
    public const string XMLRPTMATANG = "Rptmatang";
    public const string XMLRPTPROFILDATAPEMDA = "Rptprofildatapemda";

    public const string XMLSIKDPEMDA = "Sikdpemda";
    
    public const string XMLSSAPP = "Ssapp";
    public const string XMLSSAPPMENU = "Ssappmenu";
    public const string XMLSSAPPUSERTYPES = "Ssappusertypes";

    public const string XMLSSCONFIGGROUP = "Ssconfiggroup";
    public const string XMLSSCONFIGPARAMS = "Ssconfigparams";
    public const string XMLSSCONFIGUSER = "Ssconfiguser";
    public const string XMLSSDICTIONARY = "Ss90dictionary";
    public const string XMLSSMAPCLASS = "Ssmapclass";
    public const string XMLSSMAPLOOKUPCLASS = "Ssmaplookupclass";
    public const string XMLSSWEBGROUP = "Sswebgroup";
    public const string XMLSSWEBGROUPAPP = "Sswebgroupapp";
    public const string XMLSSWEBGROUPUNIT = "Sswebgroupunit";
    public const string XMLSSWEBMENU = "Sswebmenu";
    public const string XMLSSWEBOTOR = "Sswebotor";
    public const string XMLSSWEBUSER = "Sswebuser";


    public const string XMLTTDAFTDOKBEND = "Ttdaftdokbend";
    public const string XMLTTDAFTDOKPHK3 = "Ttdaftdokphk3";
    public const string XMLTTDAFTDOKREF = "Ttdaftdokref";
    public const string XMLTTDAFTDOKTTD = "Ttdaftdokttd";
    public const string XMLTTTRANS = "Tttrans";
    public const string XMLTTTRANSDETIL = "Tttransdetil";
    public const string XMLTTTRANSJURNAL = "Tttransjurnal";

    public const string XMLVIEWJURNALPPKD = "Viewjurnalppkd";
    public const string XMLVIEWNERACASALDOPPKD = "Viewneracasaldoppkd";
    public const string XMLVIEWMAPMENU = "Viewmapmenu";
    public const string XMLVIEWWEBUSER = "Viewwebuser";
  }
  #endregion ConstantTablesSS.
}