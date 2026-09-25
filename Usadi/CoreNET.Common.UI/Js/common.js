var home = function () {
    parent.home();
};

var validateFilterWhenAdd = function () {
    try {
        if (!!FormFilter1) {
            if (FormFilter1.getForm().isValid()) {
                return true;
            } else {
                return false;
            }
        }
    } catch (exception) { }
    return true;
};

var refreshMasterPage = function () {
    //update 07/06/2019
    if (!!parent) {
        if (!!parent.CoreNET) {
            if (!!parent.CoreNET.RefreshMasterPage) {
                try {
                    parent.CoreNET.RefreshMasterPage();
                }
                catch (exception) {
                }
            }
        }
    }
};

var loadingmask = function () {
    try {
        document.getElementById("loading-mask").style.display = '';
    } catch (exception) { }
};

var loadingmasklink = function () {
    document.getElementById("loading-mask-link").style.display = '';
};
var reloadForm = function () {
    if (!!parent.CoreNET.Methods1) {
        parent.CoreNET.Methods1.RefreshFormEntries();
    }
};
window.onerror = function (message, url, lineNumber) {
    /*code to execute on an error*/
    return true; /*prevents browser error messages*/
}

var linkify = function (txt, cell, row) {
    return '<a href="#" onclick="CoreNET.Methods1.OpenTab(\'' + txt + '\', \'' + row.data.Versi + '\');">' + txt + '</a>';
};
var openurl = function (url) {
    return '<a href="' + url + '" target="_blank">' + url + '</a>';
};
var iconifygrid = function (status) {
    try {
        var strs = status.split('=');
        if (strs.length == 1) {
            return toIcon(strs[0], '');
        } else {
            return toIcon(strs[0], strs[1]);
        }
    } catch (exception) { }
}
var iconify = function (status, qtip) {
    var strs = status.split('=');
    if (strs.length == 1) {
        return toIcon(strs[0], qtip);
    } else {
        return toIcon(strs[0], strs[1]);
    }
}
var toIcon = function (status, qtip) {
    if (status != null) {
        try {
            //var qtip = ''
            if (qtip == '') {
                qtip = iconket(status);
                try {
                    qtip = myiconket(status);
                } catch (exception) {
                }
            }
            if (status.indexOf("axd") > -1) {
                return '<img style="width:16px;height:16px;" title="' + qtip + '" src="' + status + '"/>';
            } else {
                return '<img style="width:16px;height:16px;" title="' + qtip + '" src="../Res/Icons/' + status + '"/>';
            }
        } catch (exception) {
        }
    }//*/
};
var sysiconify = function (status) {//dipake dimana? kayanya udah dimerge di toIcon.sys ini untuk xtd???
    var qtip = iconket(status);
    try {
        if (!!myiconket) {
            qtip = myiconket(status);
        }
    } catch (exception) {
    }
    return '<img style="width:16px;height:16px;" title="' + qtip + '" src="' + status + '"/>';
};
var imagify = function (status) {
    return '<img style="width:200px;height:200px;" src="../Res/Img/' + status + '"/>';
};
//return '<iframe border="0"><p style="width:20px;word-wrap: break-word;color:blue">' + text + '</p></iframe>';
var formattext = function (text) {
    return '<iframe style="border:0;width:190px;height:60px" src="../Blank.aspx?val=' + text + '"></iframe>';
};
var checkify = function (val) {
    if ((val == "true") || (val == "1")) {
        return '<img style="width:16px;height:16px;" src="/icons/accept-png/ext.axd"/>';
    } else {
        return '<img style="width:16px;height:16px;" src="../Res/Img/blank.png"/>';
    }
};
var change = function (value) {
    return String.format(template, (value > 0) ? "green" : "red", value);
};

var iconket = function (txt) {
    if (txt == "segitigapink.png") {
        return "segitigapink.png";
    } else if (txt == "segitigakuning.png") {
        return "segitigakuning.png";
    } else if (txt == "segitigabiru.png") {
        return "segitigabiru.png";
    } else if (txt == "buletpink.png") {
        return "buletpink.png";
    } else if (txt == "buletkuning.png") {
        return "buletkuning.png";
    } else if (txt == "buletbiru.png") {
        return "buletbiru.png";
    } else if (txt == "kotakpink.png") {
        return "kotakpink.png";
    } else if (txt == "kotakkuning.png") {
        return "kotakkuning.png";
    } else if (txt == "kotakbiru.png") {
        return "kotakbiru.png";
    } else if (txt == "kupatbiru.png") {
        return "kupatbiru.png";
    } else if (txt == "buletijo.png") {
        return "buletijo.png";
    } else {
        return "Tidak ada informasi";
    }
};
function formatCurrency(amount, decimalSeparator, thousandsSeparator, nDecimalDigits) {
    var num = parseFloat(amount); //convert to float  
    //default values  
    decimalSeparator = decimalSeparator || '.';
    thousandsSeparator = thousandsSeparator || ',';
    nDecimalDigits = nDecimalDigits == null ? 2 : nDecimalDigits;

    var fixed = num.toFixed(nDecimalDigits); //limit or add decimal digits  
    //separate begin [$1], middle [$2] and decimal digits [$4]  
    var parts = new RegExp('^(-?\\d{1,3})((?:\\d{3})+)(\\.(\\d{' + nDecimalDigits + '}))?$').exec(fixed);

    if (parts) { //num >= 1000 || num < = -1000  
        return parts[1] + parts[2].replace(/\d{3}/g, thousandsSeparator + '$&') + (parts[4] ? decimalSeparator + parts[4] : '');
    } else {
        return fixed.replace('.', decimalSeparator);
    }
};

String.prototype.format = function () {
    a = this;
    for (k in arguments) {
        a = a.replace("{" + k + "}", arguments[k])
    }
    return a
}
var formatint = function (value) {
    return formatCurrency(value, ',', '.', 0);
};
var formatmoney = function (value) {
    return formatCurrency(value, ',', '.', 0);
};

var formatdec = function (value) {
    return formatCurrency(value, ',', '.', 2);
};

var formatdec1 = function (value) {
    var val = value.replace(',', '.');
    return val;
};

var filterclick = function (key) {
    CoreNET.Methods1.FilterClick(key);
};
var selectclick = function (key, target) {
    CoreNET.Methods1.SelectClick(key, target);
    //CoreNET.Methods1.SelectClick({
    //  params: {
    //    key: key
    //  },
    //  success: function (result) {
    //    Store1.reload();
    //    alert('tes1');
    //  }
    //});
    //alert('tes2');
};
var setComboValue = function (key, value) {
    if (!!CoreNET.Methods1) {
        CoreNET.Methods1.SetComboValue(key, value);
    } else {
        CoreNET.SetComboValue(key, value);
    }
};
var formatDate = function (d) {
    var date = new Date(d);
    month = '' + (date.getMonth() + 1),
        day = '' + date.getDate(),
        year = date.getFullYear();
    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;
    return [day, month, year].join('/');
};
function getQSByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
};

function htmlEncode(html) {
    return document.createElement('a').appendChild(
        document.createTextNode(html)).parentNode.innerHTML;
};

function htmlDecode(html) {
    var a = document.createElement('a'); a.innerHTML = html;
    return a.textContent;
};

var opentab = function (title, url) {
    var tabpages = parent.Pages;
    if (!tabpages) {
        tabpages = parent.parent.Pages;
    }
    var tab = tabpages.getItem(title);

    if (!tab) {
        tab = tabpages.add({
            id: title,
            title: title,
            closable: true,
            autoLoad: {
                showMask: true,
                url: url,
                mode: "iframe",
                maskMsg: "Loading ."
            },
            listeners: {
                update: {
                    fn: function (tab, cfg) {
                        cfg.iframe.setHeight(cfg.iframe.getSize().height - 20);
                    },
                    scope: this,
                    single: true
                }
            }
        });
    }
    tabpages.setActiveTab(tab);
}
var ClearDetilPage = function () {
    if (!!parent) {
        if (!!parent.MyMethods) {
            parent.MyMethods.RefreshDetilPage('');
        }
    }
}
var ExpandFrame = function () {
    try {
        if (!!parent) {
            /*parent.CoreNET.ExpandTopFrame();*/
            if (!!parent.CoreNET) {
                if (location.search.indexOf('child') != -1) {/*child=QS di URL Master*/
                    if (!!parent.CoreNET.ExpandTopFrame) {
                        parent.CoreNET.ExpandTopFrame();
                    }
                } else {
                    if (!!parent.CoreNET.ExpandBotFrame) {
                        parent.CoreNET.ExpandBotFrame();
                    }
                }
            } else
                if (!!parent.ExpandFrame) {
                    parent.ExpandFrame();
                }
        }
    } catch (exception) {
    }
}

var NormalizeFrame = function () {
    if (!!parent) {
        if (!!parent.NormalizeFrame) {
            parent.NormalizeFrame();
        } else
            if (!!parent.CoreNET) {
                if (!!parent.CoreNET.NormalizeFrame) {
                    parent.CoreNET.NormalizeFrame();
                }
            }
    }
}
var ShowTips = function (txt) {
    if (txt == "segitigapink.png") {
        return "Dokumen baru dibuat";
    } else if (txt == "segitigakuning.png") {
        return "Komentar dari ketua tim";
    } else if (txt == "segitigabiru.png") {
        return "Komentar dari anggota tim";
    } else if (txt == "buletpink.png") {
        return "Dokumen diapprove oleh Ketua Tim";
    } else if (txt == "buletkuning.png") {
        return "Komentar dari pengendali teknis";
    } else if (txt == "buletbiru.png") {
        return "Komentar dari ketua tim";
    } else if (txt == "kotakpink.png") {
        return "Dokumen diapprove oleh Pengendali Teknis";
    } else if (txt == "kotakkuning.png") {
        return "Komentar dari penanggung jawab";
    } else if (txt == "kotakbiru.png") {
        return "Komentar dari pengendali teknis";
    } else if (txt == "kupatbiru.png") {
        return "Dokumen diapprove oleh Penanggung jawab";
    }
};
//commands.push({
//  iconCls: 'icon-table',
//  command: 'Detil'
//});
//commands.push({
//  iconCls: 'icon-pencil',
//  command: 'EditForm'
//});

var GetCommandStatus = function (commands, status) {
    if (status == -1) {
        commands.push({
            iconCls: 'icon-cancel',
            command: 'Cancel'
        });
        return;
    }
    if (status > -1) {
        commands.push({
            iconCls: 'icon-folder',
            command: 'EditPath'
        });
        commands.push({
            iconCls: 'icon-note',
            command: 'EditHelp'
        });
        commands.push({
            iconCls: 'icon-comment',
            command: 'EditMsg'
        });
    }

}


