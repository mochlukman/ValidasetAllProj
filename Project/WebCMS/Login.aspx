<%@ Page Language="C#" AutoEventWireup="True" ValidateRequest="false" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="res/css/imageslide.css" rel="stylesheet" type="text/css" media="all" />
    <link href="res/css/login.css" rel="stylesheet" type="text/css" media="all" />
    <link href="Res/img/icon.ico" rel="shortcut icon" />
    <link href='https://fonts.googleapis.com/css?family=Source Sans Pro' rel='stylesheet' />
    <link href='https://fonts.googleapis.com/css?family=Inter' rel='stylesheet' />
    <link href='https://fonts.googleapis.com/css?family=Poppins' rel='stylesheet' />
    <style>
        .ExtButton {
            width: 100% !important;
            padding: 12px !important;
            font-size: 16px !important;
            background-color: #1a73e8 !important;
            color: white !important;
            border: none !important;
            border-radius: 4px !important;
            cursor: pointer !important;
            margin-top: 10px !important;
        }
        .ExtButton:hover {
            background-color: #1558b0 !important;
        }
        .lblUser, .lblPwd {
            display: block;
            font-weight: 600;
            margin: 10px 0 5px 0;
            color: #333;
        }
        .extranous {
            text-align: right;
            margin: 10px 0;
        }
        .extranous a {
            margin-left: 10px;
            color: #1a73e8;
            text-decoration: none;
            font-size: 13px;
        }
        .info {
            margin-top: 20px;
            text-align: center;
            font-size: 13px;
            color: #555;
        }
        .title-container {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 20px;
        }
        .title {
            font-size: 24px;
            font-weight: bold;
            color: #333;
        }
        .title-image-container img {
            height: 40px;
        }
        #mainDiv {
            max-width: 400px;
            margin: 0 auto;
            padding: 30px;
            background: #fff;
            border-radius: 8px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.1);
        }
        .row {
            display: flex;
            justify-content: center;
            align-items: center;
            min-height: 100vh;
            background: #f5f7fa;
        }
        .column {
            flex: 1;
            max-width: 500px;
        }
        #infoDiv { display: none; }
        @media (min-width: 768px) {
            #infoDiv { display: block; flex: 1; max-width: 50%; padding: 20px; }
            .row { flex-wrap: nowrap; }
        }
        .navbar {
            background: #fff;
            padding: 10px 20px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.05);
            display: flex;
            justify-content: space-between;
            align-items: center;
        }
        .navbar ul { list-style: none; margin: 0; padding: 0; display: flex; align-items: center; }
        .navbar ul li { margin: 0 10px; }
        .navbar ul li.big { font-size: 18px; font-weight: 600; }
        .navbar ul li a.image-link { display: flex; align-items: center; }
        .navbar ul li a.image-link img { height: 30px; margin-left: 5px; }
        .small { height: 20px; }
        .medium { height: 30px; }
        marquee { color: #1a73e8; }

        /* --- STYLES FOR LOADING OVERLAY & SPINNER --- */
        #loadingOverlay {
            display: none;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(255, 255, 255, 0.7);
            z-index: 9999;
            justify-content: center;
            align-items: center;
            flex-direction: column;
        }
        .spinner {
            width: 45px;
            height: 45px;
            border: 4px solid #f3f3f3;
            border-top: 4px solid #1a73e8;
            border-radius: 50%;
            animation: spin 1s linear infinite;
        }
        .loading-text {
            margin-top: 12px;
            font-family: 'Poppins', sans-serif;
            font-size: 14px;
            color: #333;
            font-weight: 500;
        }
        @keyframes spin {
            0% { transform: rotate(0deg); }
            100% { transform: rotate(360deg); }
        }
    </style>

    <!-- Load CryptoJS -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/crypto-js/3.1.9-1/crypto-js.min.js"></script>
    <script type="text/javascript">
        // HARCODE PUBLICKEY – SAMAKAN DENGAN GlobalLib.PublicKey
        window._publicKey = "D02DFB6883790059";

        function showLoading() {
            var overlay = document.getElementById('loadingOverlay');
            if (overlay) {
                overlay.style.display = 'flex';
            }
        }

        function validateLogin() {
            var txtUser = document.getElementById('txtUser');
            var txtPwd = document.getElementById('txtPwd');
            var utxtCode = document.getElementById('utxt_Code');

            if (!txtUser || !txtPwd || !utxtCode) {
                alert('Komponen form tidak ditemukan.');
                return false;
            }

            var user = txtUser.value.trim();
            var pwd = txtPwd.value.trim();

            if (user === '') {
                alert('User name harus diisi');
                return false;
            }
            if (pwd === '') {
                alert('Password harus diisi');
                return false;
            }

            try {
                var publicKey = window._publicKey || '8RFKiHT00ahQRUMA';
                var key = CryptoJS.enc.Utf8.parse(publicKey);
                var iv = CryptoJS.enc.Utf8.parse(publicKey);

                var encrypted = CryptoJS.AES.encrypt(
                    CryptoJS.enc.Utf8.parse(user + '|' + pwd),
                    key,
                    { keySize: 128 / 8, iv: iv, mode: CryptoJS.mode.CBC, padding: CryptoJS.pad.Pkcs7 }
                );

                utxtCode.value = encrypted;
                txtPwd.value = '';

                // TAMPILKAN LOADING CIRCLING JIKA DEKRIPSI & VALIDASI SUKSES
                showLoading();
                return true;
            } catch (e) {
                alert('Error: ' + e.message);
                return false;
            }
        }
    </script>
</head>
<body>
    <!-- ELEMENT LOADING OVERLAY -->
    <div id="loadingOverlay">
        <div class="spinner"></div>
        <div class="loading-text">Memproses login...</div>
    </div>

    <nav class="navbar">
        <ul><li class="big"><marquee behavior="scroll" direction="left">Selamat Datang !</marquee></li></ul>
        <ul>
            <li>
                <a href="https://api.whatsapp.com/send?phone=6281234567&text=Hallo%2C%20Boleh%20Saya%20Minta%20Informasi%20Detil%20Tentang%20SIPKD%20BMD%3F" target="_blank" class="image-link">
                    <img class="medium" src="Res/img/whatsapp.png" alt="phone_icon" />
                    <img class="small" src="Res/img/butuhbantuan.png" alt="butuh bantuan" />
                </a>
            </li>
        </ul>
    </nav>

    <div class="row">
        <div id="infoDiv" class="column">
            <div class="caption">
                <img id="rafiki" src="Res/img/bg_sipkdbmd.png" alt="SIPKDBMD" />
                <br />
                <p style="display:inline-block; margin-left:10px; vertical-align:middle;">V.260821 <asp:Literal ID="litVersi" runat="server" /></p>
            </div>
        </div>

        <div id="container" class="column">
            <div id="mainDiv" runat="server">
                <div class="title-container">
                    <div class="title">Log in</div>
                    <div class="title-image-container"><img src="Res/img/logopemda.png" alt="logo" /></div>
                </div>

                <form id="Form1" runat="server">
                    <ext:ResourceManager ID="ResourceManager1" runat="server" ShowWarningOnAjaxFailure="false" />
                    <input name="utxt_Code" runat="server" type="hidden" id="utxt_Code" />

                    <Items Cls="container">
                        <label class="lblUser">User name</label>
                        <ext:TextField ID="txtUser" runat="server" AllowBlank="false" BlankText="Your user name is required." EmptyText="User name" Cls="ExtTextField" />
                        <label class="lblPwd">Password</label>
                        <ext:TextField ID="txtPwd" runat="server" InputType="Password" AllowBlank="false" BlankText="Your password is required." EmptyText="" Cls="ExtTextField" />
                    </Items>

                    <div class="extranous">
                        <a href="#">Lupa Password</a>
                        <a href="#">Hubungi Admin</a>
                    </div>

                    <!-- TOMBOL ASP.NET (postback biasa) -->
                    <asp:Button ID="btnLogin" runat="server" Text="Log in" CssClass="ExtButton" OnClick="btnLogin_Click" OnClientClick="return validateLogin();" />

                </form>

                <div class="info">
                    <p>Untuk permintaan User name dan Password</p>
                    <p>Hubungi Admin SIPKD BMD</p>
                </div>
            </div>
        </div>
    </div>
</body>
</html>