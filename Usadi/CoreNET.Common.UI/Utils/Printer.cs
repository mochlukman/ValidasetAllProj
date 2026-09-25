using CoreNET.Common.BO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.CustomProperties;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.VariantTypes;
using DocumentFormat.OpenXml.Wordprocessing;
using NotesFor.HtmlToOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace CoreNET.Common.Base
{
  /// <summary>
  /// Summary description for Printer
  /// </summary>
  public class Printer
  {
    public const string HtmlToPdfExePath = "wkhtmltopdf.exe";

    public static StringBuilder ExportHtml(ICollection cdata, string csvfname, string[] coldefs)
    {
      return ExportHtml(cdata, coldefs, new string[] { }, null);
    }
    public static StringBuilder ExportHtml(ICollection cdata, string[] coldefs)
    {
      return ExportHtml(cdata, coldefs, new string[] { }, null);
    }
    public static StringBuilder ExportHtml(ICollection cdata, string csvfname, string[] coldefs, string[] htmlcol, Hashtable strhtmlcols)
    {
      return ExportHtml(cdata, coldefs, htmlcol, strhtmlcols);
    }
    public static StringBuilder ExportHtml(ICollection cdata, string[] coldefs, string[] htmlcol, Hashtable strhtmlcols)
    {
      #region Export ke Html
      string[] cols = new string[coldefs.Length];
      for (int j = 0; j < coldefs.Length; j++)
      {
        string[] strs = coldefs[j].Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
        cols[j] = strs[0].Trim();
      }


      StringBuilder strText = new StringBuilder();

      int count = cdata.Count;
      string colname = string.Empty;
      int counter = 0;
      IDataControl dc = null;
      try
      {
        using (StringWriter sw = new StringWriter(strText))
        //using (StreamWriter sw = new StreamWriter(csvfname))
        {
          sw.WriteLine("<table border=1>");
          foreach (IDataControl o in cdata)
          {
            dc = o;
            counter++;
            string colline = "<tr>";//judul
            #region looping kolom
            string line = "<tr>";
            for (int i = 0; i < cols.Length; i++)
            {
              #region Header Row Tabel
              if (counter == 1)
              {
                string[] strs = coldefs[i].Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                if (strs.Length > 1)
                {
                  colline += "<td>" + strs[1].Trim() + "</td>";
                }
                else
                {
                  colline += "<td>" + cols[i].Trim() + "</td>";
                }
              }
              #endregion
              PropertyInfo Prop = dc.GetType().GetProperty(cols[i]);
              if (Prop == null)
              {
                throw new Exception("Nama kolom tidak terdefinisi");
              }
              object obj = Prop.GetValue(dc, null);
              string str = (obj == null) ? "-" : obj.ToString().Trim();
              if (Prop.PropertyType == typeof(System.Nullable<decimal>))
              {
                str = ((decimal)obj).ToString();
                str = str.Replace(",0000", "");
                str = str.Replace(",", ".");
              }
              else if (Prop.PropertyType == typeof(bool))
              {
                str = ((bool)obj) ? "1" : "0";
              }
              else if (Prop.PropertyType == typeof(DateTime))
              {
                DateTime dt = (DateTime)obj;
                if (dt <= new DateTime(1, 1, 1))
                {
                  str = "";
                }
              }
              str = string.IsNullOrEmpty(str) ? "-" : str;
              if (new ArrayList(htmlcol).IndexOf(cols[i]) != -1)
              {
                string id = cols[i] + counter;
                line += "<td>" + id + "</td>"; //"<td><h1>" + id + "</h1></td>";
                Hashtable h = (Hashtable)strhtmlcols[cols[i]];
                //string content = CoreNET.Common.UI.Printer.CheckBase64Encoding(str.Trim());
                string content = str.Trim();
                h[id] = content;
              }
              else
              {
                line += "<td>" + str.Trim() + "</td>";
              }
            }
            #region RowHeader
            if (counter == 1)
            {
              colline += "</tr>";
              sw.WriteLine(colline);
            }
            #endregion

            line += "</tr>";
            sw.WriteLine(line);
            #endregion
          }
          sw.WriteLine("</table>");
        }
      }
      catch (Exception ex)
      {
        UtilityBO.Log(dc, ex);
      }

      return strText;

      #endregion
    }

    public static bool IsBase64String(string s)
    {
      if (!string.IsNullOrEmpty(s) && s.StartsWith("Base64"))
      {
        return true;
      }
      else
      {
        return false;
      }
    }

    public static string EncodeBase64(string s)
    {
      byte[] byt = UTF8Encoding.UTF8.GetBytes(s);
      return "Base64" + Convert.ToBase64String(byt);

    }

    public static string DecodeBase64(string s)
    {
      s = s.Substring("Base64".Length);
      byte[] byt = Convert.FromBase64String(s);
      return Encoding.UTF8.GetString(byt);
    }

    public static string CheckBase64Encoding(string content)
    {
      if (IsBase64String(content))
      {
        return CheckHtmlEncoding(DecodeBase64(content));
      }
      else
      {
        return CheckHtmlEncoding(content);
        //return HttpUtility.HtmlDecode(content);
      }
    }

    public static bool IsHtmlFragment(string content)
    {
      if (content.Contains("<") && content.Contains("/>"))
      {
        return true;
      }
      return false;
    }

    private static string CheckHtmlEncoding(string content)
    {
      if (!string.IsNullOrEmpty(content) && content.Contains("&gt"))
      {
        return HttpUtility.HtmlDecode(content);
      }
      return content;
    }

    public static bool GeneratePdf(string commandLocation, StreamReader html, Stream pdf, Size pageSize)
    {
      Process p;
      StreamWriter stdin;
      ProcessStartInfo psi = new ProcessStartInfo
      {
        FileName = Path.Combine(commandLocation, HtmlToPdfExePath)
      };
      psi.WorkingDirectory = Path.GetDirectoryName(psi.FileName);

      // run the conversion utility
      psi.UseShellExecute = false;
      psi.CreateNoWindow = true;
      psi.RedirectStandardInput = true;
      psi.RedirectStandardOutput = true;
      psi.RedirectStandardError = true;

      // note: that we tell wkhtmltopdf to be quiet and not run scripts
      psi.Arguments = "-q -n --disable-smart-shrinking " + (pageSize.IsEmpty ? "" : "--page-width " + pageSize.Width + "mm --page-height " + pageSize.Height + "mm") + " - -";

      p = Process.Start(psi);

      try
      {
        stdin = p.StandardInput;
        stdin.AutoFlush = true;
        stdin.Write(html.ReadToEnd());
        stdin.Dispose();

        CopyStream(p.StandardOutput.BaseStream, pdf);
        p.StandardOutput.Close();
        pdf.Position = 0;

        p.WaitForExit(10000);

        return true;
      }
      catch
      {
        return false;

      }
      finally
      {
        p.Dispose();
      }
    }

    public static bool GenerateWord(string htmlStr, Stream outputStream)
    {
      try
      {
        using (MemoryStream generatedDocument = new MemoryStream())
        {
          using (WordprocessingDocument package = WordprocessingDocument.Create(generatedDocument, WordprocessingDocumentType.Document))
          {
            MainDocumentPart mainPart = package.MainDocumentPart;
            if (mainPart == null)
            {
              mainPart = package.AddMainDocumentPart();
              new Document(new Body()).Save(mainPart);
            }

            HtmlConverter converter = new HtmlConverter(mainPart);

            //http://html2openxml.codeplex.com/wikipage?title=ImageProcessing&referringTitle=Documentation
            //to process an image you must provide a base url
            //converter.BaseImageUrl = new Uri(Request.Url.Scheme + "://" + Request.Url.Authority);

            Body body = mainPart.Document.Body;

            IList<OpenXmlCompositeElement> paragraphs = converter.Parse(htmlStr);
            for (int i = 0; i < paragraphs.Count; i++)
            {
              body.Append(paragraphs[i]);
            }

            mainPart.Document.Save();
          }

          byte[] bytesInStream = generatedDocument.ToArray(); // simpler way of converting to array
          generatedDocument.Close();
          outputStream.Write(bytesInStream, 0, bytesInStream.Length);
          //Response.BinaryWrite(bytesInStream);

        }
        return true;
      }
      catch
      {
        return false;

      }
    }


    public static void CopyStream(Stream input, Stream output)
    {
      byte[] buffer = new byte[32768];
      int read;
      while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
      {
        output.Write(buffer, 0, read);
      }
    }

    public static void Print(IPrintable dc, Hashtable Htmls, string template, string filename)
    {
      Print(dc, true, 1, Htmls, template, filename);
    }
    public static void Print(IPrintable dc, bool usetemplate, int mode, Hashtable Htmls, string template, string filename)//1 paragraf,2 table
    {
      //string AllHTML = string.Empty;

      if (usetemplate)
      {
        if (File.Exists(filename))
        {
          File.Delete(filename);
        }

        try
        {
          File.Copy(template, filename, true);
        }
        catch (Exception ex)
        {
          throw ex;
          //X.Msg.Alert("Informasi", ex.Message).Show();
          //return;
        }
      }

      try
      {
        using (WordprocessingDocument package = WordprocessingDocument.Open(filename, true))
        {
          MainDocumentPart mainPart = package.MainDocumentPart;
          if (mainPart == null)
          {
            mainPart = package.AddMainDocumentPart();
            new Document(new Body()).Save(mainPart);
          }

          HtmlConverter converter = new HtmlConverter(mainPart);
          Body body = mainPart.Document.Body;

          SectionProperties SecPro = new SectionProperties();
          PageSize PSize = new PageSize
          {
            Width = 11906,
            Height = 16838
          };
          SecPro.Append(PSize);
          body.Append(SecPro);

          if (mode == 1)// Insert Paragraf menggunakan HTMLEditor
          {
            InsertHeading(body, converter, Htmls);
            mainPart.Document.Save();
          }
          else if (mode == 2) //Insert HTML String dari tabel data
          {
            InsertTables(body, converter, Htmls);
            mainPart.Document.Save();
            //DocCleansing(body, converter, Htmls);
            //mainPart.Document.Save();
          }
          else
          {
            InsertHeading(body, converter, Htmls);
            mainPart.Document.Save();
          }
          //AllHTML = mainPart.Document.ToString();
        }
      }
      catch (Exception ex)
      {
        throw ex;
        //X.MessageBox.Alert("Informasi", "Gagal cetak report karena " + ex.Message).Show();
        //return;
      }

      Hashtable DocProps = dc.GetDocProperties();
      foreach (string key in DocProps.Keys)
      {
        string html = (string)DocProps[key];
        try
        {
          SetCustomProperty(filename, key, html, PropertyTypes.Text);
        }
        catch (Exception ex)
        {
          UtilityBO.Log((IDataControl)dc, ex);
        }
      }
    }

    public static void PrintNewWord(IPrintable dc, Hashtable Htmls, string filename)
    {
      //string filename = "";
      //try
      //{
      //  filename = dc.GetFilePath(1); //Path.Combine(fpath, fname);
      //}
      //catch (Exception ex) 
      //{ 
      //  UtilityBO.Log(dc, ex);
      //  //label1.Text = "Gagal mendapatkan: Htmls=" + Htmls.Count + ";Template=" + template + ";FileName=" + filename + ";classname=" + dc.ToString();
      //  return;
      //}

      string html = string.Empty;
      foreach (string key in Htmls.Keys)
      {
        html += Htmls[key];
      }
      try
      {

        if (File.Exists(filename))
        {
          File.Delete(filename);
        }

        using (MemoryStream generatedDocument = new MemoryStream())
        {
          using (WordprocessingDocument package = WordprocessingDocument.Create(generatedDocument, WordprocessingDocumentType.Document))
          {
            MainDocumentPart mainPart = package.MainDocumentPart;
            if (mainPart == null)
            {
              mainPart = package.AddMainDocumentPart();
              new Document(new Body()).Save(mainPart);
            }

            HtmlConverter converter = new HtmlConverter(mainPart)
            {
              ConsiderDivAsParagraph = false
            };

            Body body = mainPart.Document.Body;

            IList<OpenXmlCompositeElement> paragraphs = converter.Parse(html);
            for (int i = 0; i < paragraphs.Count; i++)
            {
              body.Append(paragraphs[i]);
            }
            mainPart.Document.Save();
          }

          File.WriteAllBytes(filename, generatedDocument.ToArray());
        }
      }
      catch (Exception ex)
      {
        UtilityBO.Log(ex);
      }
    }
    private static void InsertTables(Body body, HtmlConverter converter, Hashtable Htmls)
    {
      #region Insert Heading
      int idIdx = 0;

      IEnumerable<DocumentFormat.OpenXml.Wordprocessing.Table> resview = body.OfType<DocumentFormat.OpenXml.Wordprocessing.Table>();
      foreach (DocumentFormat.OpenXml.Wordprocessing.Table t in resview)
      {
        IEnumerable<DocumentFormat.OpenXml.Wordprocessing.TableRow> rows = t.OfType<DocumentFormat.OpenXml.Wordprocessing.TableRow>();

        foreach (DocumentFormat.OpenXml.Wordprocessing.TableRow r in rows)
        {
          IEnumerable<DocumentFormat.OpenXml.Wordprocessing.TableCell> cells = r.OfType<DocumentFormat.OpenXml.Wordprocessing.TableCell>();
          foreach (DocumentFormat.OpenXml.Wordprocessing.TableCell c in cells)
          {
            try
            {
              string str = c.InnerText;
              string html = (string)Htmls[str];
              if (!string.IsNullOrEmpty(html))
              {
                html = Printer.CheckBase64Encoding(html);
                html = "<html><head></head><body>" + html + "</body></html>";
                //IList<OpenXmlCompositeElement> paragraphs = converter.Parse(html);

                ParagraphProperties ParaProperties = new ParagraphProperties(); // { ParagraphStyleId = new ParagraphStyleId() { Val = Tbl.GetHashCode().ToString() } };
                ParaProperties.AppendChild<Justification>(new Justification() { Val = JustificationValues.Left });

                //remove old text
                Paragraph p = c.Elements<Paragraph>().First();
                Run run = p.Elements<Run>().First();
                Text oldText = run.Elements<Text>().First();
                oldText.Text = "";

                string altChunkId = "myId" + (idIdx++).ToString();
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(html));
                MainDocumentPart mainDocPart = ((Document)body.Parent).MainDocumentPart;

                // Create alternative format import part.
                AlternativeFormatImportPart formatImportPart = ((Document)body.Parent).MainDocumentPart.AddAlternativeFormatImportPart(
                      AlternativeFormatImportPartType.Html, altChunkId);
                //ms.Seek(0, SeekOrigin.Begin);

                // Feed HTML data into format import part (chunk).
                formatImportPart.FeedData(ms);
                AltChunk altChunk = new AltChunk
                {
                  Id = altChunkId
                };
                c.InsertAt<AltChunk>(altChunk, 0);
              }
            }
            catch (Exception ex)
            {
              UtilityBO.Log(ex);
            }
          }
        }
      }
      #endregion
    }
    private static void DocCleansing(Body body, HtmlConverter converter, Hashtable Htmls)
    {
      #region Insert Heading
      IEnumerable<DocumentFormat.OpenXml.Wordprocessing.Table> resview = body.OfType<DocumentFormat.OpenXml.Wordprocessing.Table>();
      foreach (DocumentFormat.OpenXml.Wordprocessing.Table t in resview)
      {
        IEnumerable<DocumentFormat.OpenXml.Wordprocessing.TableRow> rows = t.OfType<DocumentFormat.OpenXml.Wordprocessing.TableRow>();

        foreach (DocumentFormat.OpenXml.Wordprocessing.TableRow r in rows)
        {
          IEnumerable<DocumentFormat.OpenXml.Wordprocessing.TableCell> cells = r.OfType<DocumentFormat.OpenXml.Wordprocessing.TableCell>();
          foreach (DocumentFormat.OpenXml.Wordprocessing.TableCell c in cells)
          {
            try
            {
              string str = c.InnerText;
              string html = (string)Htmls[str];
              if (!string.IsNullOrEmpty(html))
              {
                IList<OpenXmlCompositeElement> paragraphs = converter.Parse(html);
                for (int i = paragraphs.Count - 1; i >= 0; i--)
                {
                  r.ReplaceChild(paragraphs[i], c);
                }
              }
            }
            catch (Exception ex)
            {
              UtilityBO.Log(ex);
            }
          }
        }
      }
      #endregion
    }
    //private static void InsertTables(Body body, HtmlConverter converter, Hashtable Htmls)
    //{
    //  #region Insert Heading
    //  var resview = body.OfType<DocumentFormat.OpenXml.Wordprocessing.Table>();
    //  foreach (DocumentFormat.OpenXml.Wordprocessing.Table t in resview)
    //  {
    //    var rows = t.OfType<DocumentFormat.OpenXml.Wordprocessing.TableRow>();

    //    foreach (DocumentFormat.OpenXml.Wordprocessing.TableRow r in rows)
    //    {
    //      var cells = r.OfType<DocumentFormat.OpenXml.Wordprocessing.TableCell>();
    //      foreach (DocumentFormat.OpenXml.Wordprocessing.TableCell c in cells)
    //      {
    //        try
    //        {
    //          string str = c.InnerText;
    //          string html = (string)Htmls[str];
    //          if (!string.IsNullOrEmpty(html))
    //          {
    //            IList<OpenXmlCompositeElement> paragraphs = converter.Parse(html);
    //            for (int i = paragraphs.Count - 1; i >= 0; i--)
    //            {
    //              //c.InsertAfter<OpenXmlCompositeElement>(paragraphs[i], c);
    //              c.InsertAfterSelf<OpenXmlCompositeElement>(paragraphs[i]);
    //            }
    //            r.RemoveChild<OpenXmlCompositeElement>(c);
    //          }
    //        }
    //        catch (Exception ex) 
    //        {
    //          UtilityBO.Log(dc, ex);
    //        }
    //      }
    //    }
    //  }
    //  #endregion
    //}

    private static void InsertHeading(Body body, HtmlConverter converter, Hashtable Htmls)
    {
      #region Insert Heading
      List<Paragraph> headings = new List<Paragraph>();
      headings = body
          .OfType<Paragraph>()
          .Where(p => p.ParagraphProperties != null &&
                      p.ParagraphProperties.ParagraphStyleId != null &&
                      p.ParagraphProperties.ParagraphStyleId.Val.Value.Contains("Heading1")).ToList();

      int idIdx = 0;
      foreach (Paragraph p in headings)
      {
        try
        {
          string key = CoreNET.Common.Base.UtilityBO.FindNearestMatch(Htmls.Keys, p.InnerText);
          string html = "";
          if (key != null)
          {
            html = (string)Htmls[key];
            if (Printer.IsBase64String(html))
            {
              html = Printer.CheckBase64Encoding(html);
            }

            if (Printer.IsHtmlFragment(html))
            {
              //html cleaning
              html = Regex.Replace(html, @"[^\u0000-\u007F]", string.Empty);
              if (!html.StartsWith("<html>"))
              {
                html = string.Format(@"<html><head></head><body>{0}</body></html>", html);
              }
              string altChunkId = "mainDoc" + (idIdx++).ToString();
              MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(html));
              MainDocumentPart mainDocPart = ((Document)body.Parent).MainDocumentPart;

              AlternativeFormatImportPart formatImportPart = ((Document)body.Parent).MainDocumentPart.AddAlternativeFormatImportPart(
                    AlternativeFormatImportPartType.Html, altChunkId);

              formatImportPart.FeedData(ms);
              AltChunk altChunk = new AltChunk
              {
                Id = altChunkId
              };
              body.InsertAfter<OpenXmlCompositeElement>(altChunk, p);
            }
            else
            {
              IList<OpenXmlCompositeElement> paragraphs = converter.Parse(html);
              for (int i = paragraphs.Count - 1; i >= 0; i--)
              {
                body.InsertAfter<OpenXmlCompositeElement>(paragraphs[i], p);
              }
            }
          }
        }
        catch (Exception ex)
        {
          UtilityBO.Log(ex);
        }
      }

      #endregion
    }

    public enum PropertyTypes : int
    {
      YesNo,
      Text,
      DateTime,
      NumberInteger,
      NumberDouble
    }


    public static string SetCustomProperty(
      string fileName,
      string propertyName,
      object propertyValue,
      PropertyTypes propertyType)
    {
      // Given a document name, a property name/value, and the property type, 
      // add a custom property to a document. The method returns the original
      // value, if it existed.

      string returnValue = null;

      CustomDocumentProperty newProp = new CustomDocumentProperty();
      bool propSet = false;

      // Calculate the correct type.
      switch (propertyType)
      {
        case PropertyTypes.DateTime:

          // Be sure you were passed a real date, 
          // and if so, format in the correct way. 
          // The date/time value passed in should 
          // represent a UTC date/time.
          if ((propertyValue) is DateTime)
          {
            newProp.VTFileTime =
                new VTFileTime(string.Format("{0:s}Z",
                    Convert.ToDateTime(propertyValue)));
            propSet = true;
          }

          break;

        case PropertyTypes.NumberInteger:
          if ((propertyValue) is int)
          {
            newProp.VTInt32 = new VTInt32(propertyValue.ToString());
            propSet = true;
          }

          break;

        case PropertyTypes.NumberDouble:
          if (propertyValue is double)
          {
            newProp.VTFloat = new VTFloat(propertyValue.ToString());
            propSet = true;
          }

          break;

        case PropertyTypes.Text:
          newProp.VTLPWSTR = new VTLPWSTR(propertyValue.ToString());
          propSet = true;

          break;

        case PropertyTypes.YesNo:
          if (propertyValue is bool)
          {
            // Must be lowercase.
            newProp.VTBool = new VTBool(
              Convert.ToBoolean(propertyValue).ToString().ToLower());
            propSet = true;
          }
          break;
      }

      if (!propSet)
      {
        // If the code was not able to convert the 
        // property to a valid value, throw an exception.
        throw new InvalidDataException("propertyValue");
      }

      // Now that you have handled the parameters, start
      // working on the document.
      newProp.FormatId = "{D5CDD505-2E9C-101B-9397-08002B2CF9AE}";
      newProp.Name = propertyName;

      using (WordprocessingDocument document = WordprocessingDocument.Open(fileName, true))
      {
        CustomFilePropertiesPart customProps = document.CustomFilePropertiesPart;
        if (customProps == null)
        {
          // No custom properties? Add the part, and the
          // collection of properties now.
          customProps = document.AddCustomFilePropertiesPart();
          customProps.Properties =
              new DocumentFormat.OpenXml.CustomProperties.Properties();
        }

        Properties props = customProps.Properties;
        if (props != null)
        {
          // This will trigger an exception if the property's Name 
          // property is null, but if that happens, the property is damaged, 
          // and probably should raise an exception.
          OpenXmlElement prop =
              props.Where(
              p => ((CustomDocumentProperty)p).Name.Value
                  == propertyName).FirstOrDefault();

          // Does the property exist? If so, get the return value, 
          // and then delete the property.
          if (prop != null)
          {
            returnValue = prop.InnerText;
            prop.Remove();
          }

          // Append the new property, and 
          // fix up all the property ID values. 
          // The PropertyId value must start at 2.
          props.AppendChild(newProp);
          int pid = 2;
          foreach (CustomDocumentProperty item in props)
          {
            item.PropertyId = pid++;
          }
          props.Save();
        }
      }
      return returnValue;
    }

  }


  public class Size
  {
    public Size(bool p, int p_2, int p_3)
    {
      // TODO: Complete member initialization
      IsEmpty = p;
      Height = p_2;
      Width = p_3;
    }

    public bool IsEmpty { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }
  }
}

