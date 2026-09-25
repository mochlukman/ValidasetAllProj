using System.Collections.Generic;
using System.Text;

namespace CoreNET.Common.BO
{
  public class DataHelper
  {
    #region Constants

    public const string ROW_HEADER = "ROW_HEADER";
    public const string ROW_1 = "row1";
    public const string ROW_2 = "row2";
    public const string ROW_3 = "row3";
    public const string ROW_4 = "row4";
    public const string ROW_5 = "row5";
    public const string ROW_6 = "row6";
    #endregion
    #region Property
    public string[] Datas = new string[] { "10", "20", "30", "40" };
    public string[] Percents = new string[] { "10", "20", "30", "40" };
    public string[] URLs = new string[] {
      ""
      , ""
      , ""
      , ""
    };
    public string[] Titles = new string[] {
      "Jumlah Produk"
      , "Jumlah Bahan"
      , "Jumlah Mitra"
      , "Jumlah Horeka"
    };
    #endregion
    #region static methods
    public static string GetMD12Row(DashboardDataObject d1, DashboardDataObject d2, DashboardDataObject d3, DashboardDataObject d4)
    {
      string html = GetTemplateMD12Row();
      html = string.Format(html
        , d1.URL, d1.Value, d1.Title, d1.Percentstr, GetBar120((int)d1.Percent)
        , d2.URL, d2.Value, d2.Title, d2.Percentstr, GetBar120((int)d2.Percent)
        , d3.URL, d3.Value, d3.Title, d3.Percentstr, GetBar120((int)d3.Percent)
        , d4.URL, d4.Value, d4.Title, d4.Percentstr, GetBar120((int)d4.Percent)
      );
      return html;
    }
    public static string GetBar120(int value)
    {
      return GetBar(120, value);
    }
    public static string GetBar160(int value)
    {
      return GetBar(160, value);
    }
    public static string GetBar(int width, int value)
    {
      string html = $@"
<div class='progress progress-md' style='width:{width}px'>
  <div class='progress-bar bg-danger' role='progressbar' style='width: {value}%' aria-valuenow='{value}' aria-valuemin='0' aria-valuemax='100'>
  </div>
</div>
      ";
      return html;
    }
    public static string GetMD4Row(DashboardDataObject d1, DashboardDataObject d2, DashboardDataObject d3)
    {
      string html = GetTemplateMD4Row();
      html = string.Format(html
        , d1.URL, d1.Title, d1.Percentstr, d1.Value, GetBar160((int)d1.Percent)
        , d2.URL, d2.Title, d2.Percentstr, d2.Value, GetBar160((int)d2.Percent)
        , d3.URL, d3.Title, d3.Percentstr, d3.Value, GetBar160((int)d3.Percent)
      );
      return html;
    }
    public static string GetRowHeader(string title, Dictionary<string, string> lefts, Dictionary<string, string> rights)
    {
      string left_tab = @"";
      foreach (string key in lefts.Keys)
      {
        left_tab += string.Format(@"<li><a href='{1}'>{0}</a></li>", key, lefts[key]);
      }
      string right_tab = @"";
      foreach (string key in rights.Keys)
      {
        right_tab += string.Format(@"<li><a href='{1}'>{0}</a></li>", key, rights[key]);
      }
      string html = $@"
          <div class='col-12'>
            <div class='page-header'>
              <h4 class='page-title'>{title}</h4>
              <div class='quick-link-wrapper w-100 d-md-flex flex-md-wrap'>
                <ul class='quick-links'>
                  {left_tab}
                </ul>
                <ul class='quick-links ml-auto'>
                  {right_tab}
                </ul>
              </div>
            </div>
          </div>
      ";
      return html;
    }
    #endregion
    #region Template
    public static string GetTemplateRows1()
    {
      string html = @"";

      return html;
    }
    public static string GetTemplateMD12Row()
    {
      #region Template
      string html = @"
          <div class='col-md-12 grid-margin'>
            <div class='card'>
              <div class='card-body'>
                <div class='row'>
                  <div class='col-lg-3 col-md-6'>
                    <div class='d-flex'>
                      <div class='wrapper' onclick='{0}'>
                        <h3 class='mb-0 font-weight-semibold'>{1}</h3>
                        <h5 class='mb-0 font-weight-medium text-primary'>{2}</h5>
                        <p class='mb-0 text-muted'>{3}</p>
                        {4}
                      </div>
                      <div class='wrapper my-auto ml-auto ml-lg-4'>
                        <div class='chartjs-size-monitor'>
                          <div class='chartjs-size-monitor-expand'>
                            <div class=''></div>
                          </div>
                          <div class='chartjs-size-monitor-shrink'>
                            <div class=''></div>
                          </div>
                        </div>
                        <canvas height='50' width='100' id='stats-line-graph-1' class='chartjs-render-monitor' style='display: block;'></canvas>
                      </div>
                    </div>
                  </div>
                  <div class='col-lg-3 col-md-6 mt-md-0 mt-4'>
                    <div class='d-flex'>
                      <div class='wrapper' onclick='{5}'>
                        <h3 class='mb-0 font-weight-semibold'>{6}</h3>
                        <h5 class='mb-0 font-weight-medium text-primary'>{7}</h5>
                        <p class='mb-0 text-muted'>{8}</p>
                        {9}
                      </div>
                      <div class='wrapper my-auto ml-auto ml-lg-4'>
                        <div class='chartjs-size-monitor'>
                          <div class='chartjs-size-monitor-expand'>
                            <div class=''></div>
                          </div>
                          <div class='chartjs-size-monitor-shrink'>
                            <div class=''></div>
                          </div>
                        </div>
                        <canvas height='50' width='100' id='stats-line-graph-2' class='chartjs-render-monitor' style='display: block;'></canvas>
                      </div>
                    </div>
                  </div>
                  <div class='col-lg-3 col-md-6 mt-md-0 mt-4'>
                    <div class='d-flex'>
                      <div class='wrapper' onclick='{10}'>
                        <h3 class='mb-0 font-weight-semibold'>{11}</h3>
                        <h5 class='mb-0 font-weight-medium text-primary'>{12}</h5>
                        <p class='mb-0 text-muted'>{13}</p>
                        {14}
                      </div>
                      <div class='wrapper my-auto ml-auto ml-lg-4'>
                        <div class='chartjs-size-monitor'>
                          <div class='chartjs-size-monitor-expand'>
                            <div class=''></div>
                          </div>
                          <div class='chartjs-size-monitor-shrink'>
                            <div class=''></div>
                          </div>
                        </div>
                        <canvas height='50' width='100' id='stats-line-graph-3' class='chartjs-render-monitor' style='display: block;'></canvas>
                      </div>
                    </div>
                  </div>
                  <div class='col-lg-3 col-md-6 mt-md-0 mt-4'>
                    <div class='d-flex'>
                      <div class='wrapper' onclick='{15}'>
                        <h3 class='mb-0 font-weight-semibold'>{16}</h3>
                        <h5 class='mb-0 font-weight-medium text-primary'>{17}</h5>
                        <p class='mb-0 text-muted'>{18}</p>
                        {19}
                      </div>
                      <div class='wrapper my-auto ml-auto ml-lg-4'>
                        <div class='chartjs-size-monitor'>
                          <div class='chartjs-size-monitor-expand'>
                            <div class=''></div>
                          </div>
                          <div class='chartjs-size-monitor-shrink'>
                            <div class=''></div>
                          </div>
                        </div>
                        <canvas height='50' width='100' id='stats-line-graph-4' class='chartjs-render-monitor' style='display: block;'></canvas>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
      ";
      #endregion Template
      return html;
    }
    public static string GetTemplateMD4Row()
    {
      #region Template
      string html = @"
          <div class='col-md-4 grid-margin stretch-card'>
            <div class='card'>
              <div class='chartjs-size-monitor'>
                <div class='chartjs-size-monitor-expand'>
                  <div class=''></div>
                </div>
                <div class='chartjs-size-monitor-shrink'>
                  <div class=''></div>
                </div>
              </div>
              <div class='card-body pb-0' onclick='{0}'>
                <div class='d-flex justify-content-between'>
                  <h4 class='card-title mb-0'>{1}</h4>
                  <p class='font-weight-semibold mb-0'>{2}</p>
                </div>
                <h3 class='font-weight-medium mb-4'>{3}</h3>
                {4}
              </div>
              <canvas class='mt-n4 chartjs-render-monitor' height='63' id='total-revenue' width='227' style='display: block; width: 227px; height: 63px;'></canvas>
            </div>
          </div>
          <div class='col-md-4 grid-margin stretch-card'>
            <div class='card'>
              <div class='chartjs-size-monitor'>
                <div class='chartjs-size-monitor-expand'>
                  <div class=''></div>
                </div>
                <div class='chartjs-size-monitor-shrink'>
                  <div class=''></div>
                </div>
              </div>
              <div class='card-body pb-0' onclick='{5}'>
                <div class='d-flex justify-content-between'>
                  <h4 class='card-title mb-0'>{6}</h4>
                  <p class='font-weight-semibold mb-0'>{7}</p>
                </div>
                <h3 class='font-weight-medium mb-4'>{8}</h3>
                {9}
              </div>
              <canvas class='mt-n3 chartjs-render-monitor' height='63' id='total-transaction' width='227' style='display: block; width: 227px; height: 63px;'></canvas>
            </div>
          </div>
          <div class='col-md-4 grid-margin stretch-card'>
            <div class='card'>
              <div class='chartjs-size-monitor'>
                <div class='chartjs-size-monitor-expand'>
                  <div class=''></div>
                </div>
                <div class='chartjs-size-monitor-shrink'>
                  <div class=''></div>
                </div>
              </div>
              <div class='card-body pb-0' onclick='{10}'>
                <div class='d-flex justify-content-between'>
                  <h4 class='card-title mb-0'>{11}</h4>
                  <p class='font-weight-semibold mb-0'>{12}</p>
                </div>
                <h3 class='font-weight-medium mb-4'>{13}</h3>
                {14}
              </div>
              <canvas class='mt-n4 chartjs-render-monitor' height='63' id='total-revenue' width='227' style='display: block; width: 227px; height: 63px;'></canvas>
            </div>
          </div>
      ";
      #endregion
      return html;
    }
    #region row3
    public string GetRows3Template()
    {
      string html = @"
          <div class='col-md-4 grid-margin stretch-card'>
            <div class='card'>
              <div class='card-body'>
                <h3 class='card-title mb-0' style='font-size:20px'>5 Transaksi Terakhir</h3>
                <div class='table-responsive'>
                  <table class='table table-stretched'>
                    <thead><tr><th>Nama</th><th>Jumlah</th></tr></thead>
                    <tbody>
                      <tr>
                        <td>
                          <p class='mb-1 text-dark font-weight-medium'>
                            NFLX
                          </p>
                          <small class='font-weight-medium'>Netflix, Inc.</small>
                        </td>
                        <td class='font-weight-medium'>$250.00
                        </td>
                      </tr>
                      <tr>
                        <td>
                          <p class='mb-1 text-dark font-weight-medium'>
                            TSLA
                          </p>
                          <small class='font-weight-medium'>Tesla, Inc.</small>
                        </td>
                        <td class='font-weight-medium'>$458.00
                        </td>
                      </tr>
                      <tr>
                        <td>
                          <p class='mb-1 text-dark font-weight-medium'>
                            GOOG
                          </p>
                          <small class='font-weight-medium'>Alphabet, Inc.</small>
                        </td>
                        <td class='font-weight-medium'>$250.00
                        </td>
                      </tr>
                      <tr>
                        <td>
                          <p class='mb-1 text-dark font-weight-medium'>
                            AMZN
                          </p>
                          <small class='font-weight-medium'>Amazon.com, Inc.</small>
                        </td>
                        <td class='font-weight-medium'>$546.00
                        </td>
                      </tr>
                      <tr>
                        <td>
                          <p class='mb-1 text-dark font-weight-medium'>
                            NFLX
                          </p>
                          <small class='font-weight-medium'>Netflix, Inc.</small>
                        </td>
                        <td class='font-weight-medium'>$250.00
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
                <a class='d-block mt-3' href='#'>Show all</a>
              </div>
            </div>
          </div>
          <div class='col-md-4 grid-margin stretch-card'>
            <div class='card'>
              <div class='card-body'>
                <h3 class='card-title mb-0' style='font-size:20px'>Omset 5 Hari Terakhir</h3>
                <div class='table-responsive'>
                  <table class='table table-stretched'>
                    <thead><tr><th>Nama</th><th>Jumlah</th></tr></thead>
                    <tbody>
                      <tr>
                        <td>
                          <p class='mb-1 text-dark font-weight-medium'>
                            NFLX
                          </p>
                          <small class='font-weight-medium'>Netflix, Inc.</small>
                        </td>
                        <td class='font-weight-medium'>$250.00
                        </td>
                      </tr>
                      <tr>
                        <td>
                          <p class='mb-1 text-dark font-weight-medium'>
                            TSLA
                          </p>
                          <small class='font-weight-medium'>Tesla, Inc.</small>
                        </td>
                        <td class='font-weight-medium'>$458.00
                        </td>
                      </tr>
                      <tr>
                        <td>
                          <p class='mb-1 text-dark font-weight-medium'>
                            GOOG
                          </p>
                          <small class='font-weight-medium'>Alphabet, Inc.</small>
                        </td>
                        <td class='font-weight-medium'>$250.00
                        </td>
                      </tr>
                      <tr>
                        <td>
                          <p class='mb-1 text-dark font-weight-medium'>
                            AMZN
                          </p>
                          <small class='font-weight-medium'>Amazon.com, Inc.</small>
                        </td>
                        <td class='font-weight-medium'>$546.00
                        </td>
                      </tr>
                      <tr>
                        <td>
                          <p class='mb-1 text-dark font-weight-medium'>
                            NFLX
                          </p>
                          <small class='font-weight-medium'>Netflix, Inc.</small>
                        </td>
                        <td class='font-weight-medium'>$250.00
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
                <a class='d-block mt-3' href='#'>Show all</a>
              </div>
            </div>
          </div>
          <div class='col-md-4 grid-margin stretch-card'>
            <div class='card'>
              <div class='card-body'>
                <h3 class='card-title mb-0' style='font-size:20px'>Omset Mitra 5 Hari</h3>
                <div class='table-responsive'>
                  <table class='table table-stretched'>
                    <thead><tr><th>Nama</th><th>Jumlah</th></tr></thead>
                    <tbody>
                      <tr>
                        <td>
                          <p class='mb-1 text-dark font-weight-medium'>
                            NFLX
                          </p>
                          <small class='font-weight-medium'>Netflix, Inc.</small>
                        </td>
                        <td class='font-weight-medium'>$250.00
                        </td>
                      </tr>
                      <tr>
                        <td>
                          <p class='mb-1 text-dark font-weight-medium'>
                            TSLA
                          </p>
                          <small class='font-weight-medium'>Tesla, Inc.</small>
                        </td>
                        <td class='font-weight-medium'>$458.00
                        </td>
                      </tr>
                      <tr>
                        <td>
                          <p class='mb-1 text-dark font-weight-medium'>
                            GOOG
                          </p>
                          <small class='font-weight-medium'>Alphabet, Inc.</small>
                        </td>
                        <td class='font-weight-medium'>$250.00
                        </td>
                      </tr>
                      <tr>
                        <td>
                          <p class='mb-1 text-dark font-weight-medium'>
                            AMZN
                          </p>
                          <small class='font-weight-medium'>Amazon.com, Inc.</small>
                        </td>
                        <td class='font-weight-medium'>$546.00
                        </td>
                      </tr>
                      <tr>
                        <td>
                          <p class='mb-1 text-dark font-weight-medium'>
                            NFLX
                          </p>
                          <small class='font-weight-medium'>Netflix, Inc.</small>
                        </td>
                        <td class='font-weight-medium'>$250.00
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
                <a class='d-block mt-3' href='#'>Show all</a>
              </div>
            </div>
          </div>
      ";
      return html;
    }
    #endregion
    #region row4
    public string GetRows4Template()
    {
      string html = @"
          <div class='col-md-4 grid-margin stretch-card'>
            <div class='card'>
              <div class='card-body'>
                <h3 class='card-title mb-0' style='font-size:20px'>Produksi 5 Hari Terakhir</h3>
                <div class='table-responsive'>
                  <table class='table table-stretched'>
                    <thead><tr><th>Nama</th><th>Jumlah</th></tr></thead>
                    <tbody>
                      <tr>
                        <td>
                          <p class='mb-1 text-dark font-weight-medium'>
                            NFLX
                          </p>
                          <small class='font-weight-medium'>Netflix, Inc.</small>
                        </td>
                        <td class='font-weight-medium'>$250.00
                        </td>
                      </tr>
                      <tr>
                        <td>
                          <p class='mb-1 text-dark font-weight-medium'>
                            TSLA
                          </p>
                          <small class='font-weight-medium'>Tesla, Inc.</small>
                        </td>
                        <td class='font-weight-medium'>$458.00
                        </td>
                      </tr>
                      <tr>
                        <td>
                          <p class='mb-1 text-dark font-weight-medium'>
                            GOOG
                          </p>
                          <small class='font-weight-medium'>Alphabet, Inc.</small>
                        </td>
                        <td class='font-weight-medium'>$250.00
                        </td>
                      </tr>
                      <tr>
                        <td>
                          <p class='mb-1 text-dark font-weight-medium'>
                            AMZN
                          </p>
                          <small class='font-weight-medium'>Amazon.com, Inc.</small>
                        </td>
                        <td class='font-weight-medium'>$546.00
                        </td>
                      </tr>
                      <tr>
                        <td>
                          <p class='mb-1 text-dark font-weight-medium'>
                            NFLX
                          </p>
                          <small class='font-weight-medium'>Netflix, Inc.</small>
                        </td>
                        <td class='font-weight-medium'>$250.00
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
                <a class='d-block mt-3' href='#'>Show all</a>
              </div>
            </div>
          </div>
          <div class='col-md-4 grid-margin stretch-card'>
            <div class='card'>
              <div class='card-body'>
                <h3 class='card-title mb-0' style='font-size:20px'>Volume 5 Hari Terakhir</h3>
                <div class='table-responsive'>
                  <table class='table table-stretched'>
                    <thead><tr><th>Nama</th><th>Jumlah</th></tr></thead>
                    <tbody>
                      <tr>
                        <td>
                          <p class='mb-1 text-dark font-weight-medium'>
                            NFLX
                          </p>
                          <small class='font-weight-medium'>Netflix, Inc.</small>
                        </td>
                        <td class='font-weight-medium'>$250.00
                        </td>
                      </tr>
                      <tr>
                        <td>
                          <p class='mb-1 text-dark font-weight-medium'>
                            TSLA
                          </p>
                          <small class='font-weight-medium'>Tesla, Inc.</small>
                        </td>
                        <td class='font-weight-medium'>$458.00
                        </td>
                      </tr>
                      <tr>
                        <td>
                          <p class='mb-1 text-dark font-weight-medium'>
                            GOOG
                          </p>
                          <small class='font-weight-medium'>Alphabet, Inc.</small>
                        </td>
                        <td class='font-weight-medium'>$250.00
                        </td>
                      </tr>
                      <tr>
                        <td>
                          <p class='mb-1 text-dark font-weight-medium'>
                            AMZN
                          </p>
                          <small class='font-weight-medium'>Amazon.com, Inc.</small>
                        </td>
                        <td class='font-weight-medium'>$546.00
                        </td>
                      </tr>
                      <tr>
                        <td>
                          <p class='mb-1 text-dark font-weight-medium'>
                            NFLX
                          </p>
                          <small class='font-weight-medium'>Netflix, Inc.</small>
                        </td>
                        <td class='font-weight-medium'>$250.00
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
                <a class='d-block mt-3' href='#'>Show all</a>
              </div>
            </div>
          </div>
          <div class='col-md-4 grid-margin stretch-card'>
            <div class='card'>
              <div class='card-body'>
                <h3 class='card-title mb-0' style='font-size:20px'>5 Top Produk</h3>
                <div class='table-responsive'>
                  <table class='table table-stretched'>
                    <thead><tr><th>Nama</th><th>Jumlah</th></tr></thead>
                    <tbody>
                      <tr>
                        <td>
                          <p class='mb-1 text-dark font-weight-medium'>
                            NFLX
                          </p>
                          <small class='font-weight-medium'>Netflix, Inc.</small>
                        </td>
                        <td class='font-weight-medium'>$250.00
                        </td>
                      </tr>
                      <tr>
                        <td>
                          <p class='mb-1 text-dark font-weight-medium'>
                            TSLA
                          </p>
                          <small class='font-weight-medium'>Tesla, Inc.</small>
                        </td>
                        <td class='font-weight-medium'>$458.00
                        </td>
                      </tr>
                      <tr>
                        <td>
                          <p class='mb-1 text-dark font-weight-medium'>
                            GOOG
                          </p>
                          <small class='font-weight-medium'>Alphabet, Inc.</small>
                        </td>
                        <td class='font-weight-medium'>$250.00
                        </td>
                      </tr>
                      <tr>
                        <td>
                          <p class='mb-1 text-dark font-weight-medium'>
                            AMZN
                          </p>
                          <small class='font-weight-medium'>Amazon.com, Inc.</small>
                        </td>
                        <td class='font-weight-medium'>$546.00
                        </td>
                      </tr>
                      <tr>
                        <td>
                          <p class='mb-1 text-dark font-weight-medium'>
                            NFLX
                          </p>
                          <small class='font-weight-medium'>Netflix, Inc.</small>
                        </td>
                        <td class='font-weight-medium'>$250.00
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
                <a class='d-block mt-3' href='#'>Show all</a>
              </div>
            </div>
          </div>
      ";
      return html;
    }
    #endregion
    #region row5
    private string GetRowTableRow(DashboardDataObject d)
    {
      //          <td class='font-weight-medium'>{d.Value}</td>
      //            <small class='font-weight-medium text-right'>{d.Percentstr}</small>

      string bar = GetBar(20, (int)d.Percent);
      string html = $@"
        <tr>
          <td>
            <p class='mb-1 text-dark font-weight-medium'>{d.Title}</p>
            <small class='font-weight-medium'>{d.Subtitle}</small>
          </td>
          <td>
            <p class='mb-1 text-dark font-weight-medium text-right'>{d.Value}</p>
            <p class='mb-1 text-dark font-weight-small text-right'>{d.Percentstr}</p>
          </td>
          <td>{bar}</td>
        </tr>
      ";
      return html;
    }
    public string GetTemplateMD4Table(DashboardTableObject tableData)
    {
      StringBuilder builder = new StringBuilder();
      foreach (DashboardDataObject d in tableData.Data)
      {
        builder.AppendLine(GetRowTableRow(d));
      }
      string htmlRows = builder.ToString();

      string html = $@"
          <div class='col-md-4 grid-margin stretch-card'>
            <div class='card'>
              <div class='card-body'>
                <h3 class='card-title mb-0' style='font-size:20px'>{tableData.Title}</h3>
                <div class='table-responsive'>
                  <table class='table table-stretched'>
                    <thead>
                      <tr>
                        <th>{tableData.Columns[0]}</th>
                        <th>{tableData.Columns[1]}</th>
                        <th></th>
                      </tr>
                    </thead>
                    <tbody>
                      {htmlRows}
                    </tbody>
                  </table>
                </div>
                <a class='d-block mt-3' href='{tableData.URL}'>{tableData.URLTitle}</a>
              </div>
            </div>
          </div>
      ";
      return html;
    }
    public string GetTemplateMD4Tables(DashboardTableObject data1, DashboardTableObject data2, DashboardTableObject data3)
    {
      StringBuilder builder = new StringBuilder();
      builder.AppendLine(GetTemplateMD4Table(data1));
      builder.AppendLine(GetTemplateMD4Table(data2));
      builder.AppendLine(GetTemplateMD4Table(data3));
      return builder.ToString();
    }
    #endregion
    #endregion

  }
}
