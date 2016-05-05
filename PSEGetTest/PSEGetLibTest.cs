﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PSEGetLib;
//using org.apache.pdfbox.pdmodel;
//using org.apache.pdfbox.util;
using System.Collections.Specialized;
using PSEGetLib.DocumentModel;
using PSEGetLib.Converters;
using PSEGetLib.Service;
using PSEGetLib.Interfaces;

namespace PSEGetTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ReportReaderTest
    {
        //private PDDocument doc;        
        private string pdfDocPath = @"C:\Users\arnolddiaz\Downloads\stockQuotes_05052016.pdf";

        public ReportReaderTest()
        {
            //
            // TODO: Add constructor logic here
            //            
            //doc = PDDocument.load(@"C:\Users\Arnold\Documents\Projects\PSEGet3\trunk\PSEGetTest\stockQuotes_09202010.pdf");
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestReaderFirstLine()
        {

            //PDFTextStripper stripper = new PDFTextStripper();
            var pdfSharpService = new PdfTextSharpService();

            PSEReportReader reader = new PSEReportReader(pdfSharpService.ExtractTextFromPdf(pdfDocPath));

            string expected = "The Philippine Stock Exchange, Inc";
            string actual = reader.PSEReportString[0].Trim();
            Assert.AreEqual(expected, actual);           
        }

        [TestMethod]
        public void TestReaderLastLine()
        {
            //PDFTextStripper stripper = new PDFTextStripper();
            var pdfSharpService = new PdfTextSharpService();
            PSEReportReader reader = new PSEReportReader(pdfSharpService.ExtractTextFromPdf(pdfDocPath));

            string expected = "*** Grand total includes main,oddlot and block sale transactions";
            string actual = reader.PSEReportString[reader.PSEReportString.Count - 1].Trim();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestReaderFill_TradDate()
        {
            //PDFTextStripper stripper = new PDFTextStripper();
            var pdfSharpService = new PdfTextSharpService();
            PSEReportReader reader = new PSEReportReader(pdfSharpService.ExtractTextFromPdf(pdfDocPath));

            PSEDocument pd = new PSEDocument();
            reader.Fill(pd);

            DateTime expected = DateTime.Parse("05/05/2016");
            DateTime actual = pd.TradeDate;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestStringCollection_IndexOfString()
        {
            var sc = new StringCollection();
            sc.Add("Hello World!");
            sc.Add("Kamusta na ba?");
            sc.Add("Talaga lang ha");
            sc.Add("Kamote ka ba?");

            int expected = 2;
            int actual = sc.IndexOfString("Talaga");

            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void TestParseDouble()
        {
            //FOREIGN BUYING      Php 7,979,297,874.23
            Nullable<double> expected = 7979297874.23;
            Nullable<double> actual = "FOREIGN BUYING      Php 7,979,297,874.23".ParseDouble();

            Assert.AreEqual(expected, actual, "ParseDouble first test failed");

            expected = -7979297874.23;
            actual = "FOREIGN BUYING      Php (7,979,297,874.23)".ParseDouble();

            Assert.AreEqual(expected, actual, "ParseDouble second test failed");
        }

        [TestMethod]
        public void TestParseInt()
        {
            //ODDLOT VOLUME:     :      1,043,508
            Nullable<int> expected = 1043508;
            Nullable<int> actual = "ODDLOT VOLUME:     :      1,043,508".ParseInt();

            Assert.AreEqual(expected, actual, "ParseInt first test failed");

            expected = -1043508;
            actual = "ODDLOT VOLUME:     :      (1,043,508)".ParseInt();

            Assert.AreEqual(expected, actual, "ParseInt second test failed");
        }

        [TestMethod]
        public void TestParseUlong()
        {
            //GRAND TOTAL 7,786,326,861,123 Php
            Nullable<ulong> expected = 7786326861123;
            Nullable<ulong> actual = "GRAND TOTAL 7,786,326,861,123 Php".ParseUlong();

            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void TestParseLong()
        {
            //GRAND TOTAL 7,786,326,861,123 Php

            Nullable<long> expected = 7786326861123;
            Nullable<long> actual = "GRAND TOTAL 7,786,326,861,123 Php".ParseLong();

            Assert.AreEqual(expected, actual, "ParseLong first test failed");

            expected = -7786326861123;
            actual = "GRAND TOTAL (7,786,326,861,123) Php".ParseLong();

            Assert.AreEqual(expected, actual, "ParseLong second test failed");

        }


        [TestMethod]
        public void TestNameValueCollection_ContainsValue()
        {
            NameValueCollection sectorNameMap = new NameValueCollection();

            sectorNameMap.Add(PSEDocument.FINANCIAL, "F I N A N C I A L S");
            sectorNameMap.Add(PSEDocument.INDUSTRIAL, "I N D U S T R I A L");
            sectorNameMap.Add(PSEDocument.HOLDING, "H O L D I N G   F I R M S");
            sectorNameMap.Add(PSEDocument.MINING_OIL, "M I N I N G   &   O I L");
            sectorNameMap.Add(PSEDocument.PROPERTY, "P R O P E R T Y");
            sectorNameMap.Add(PSEDocument.SERVICE, "S E R V I C E S");

            bool expected = true;
            bool actual = sectorNameMap.ContainsValue("H O L D I N G   F I R M S");

            Assert.AreEqual(expected, actual, "NameValueCollection first test failed");

            expected = false;
            actual = sectorNameMap.ContainsValue("Hello World");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestNameValueCollection_GetKey()
        {
            NameValueCollection sectorNameMap = new NameValueCollection();
            sectorNameMap.Add(PSEDocument.FINANCIAL, "F I N A N C I A L S");
            sectorNameMap.Add(PSEDocument.INDUSTRIAL, "I N D U S T R I A L");
            sectorNameMap.Add(PSEDocument.HOLDING, "H O L D I N G   F I R M S");
            sectorNameMap.Add(PSEDocument.MINING_OIL, "M I N I N G   &   O I L");
            sectorNameMap.Add(PSEDocument.PROPERTY, "P R O P E R T Y");
            sectorNameMap.Add(PSEDocument.SERVICE, "S E R V I C E S");
            sectorNameMap.Add(PSEDocument.PREFERRED, "P R E F E R R E D");
            sectorNameMap.Add(PSEDocument.SME, "SMALL AND MEDIUM ENTERPRISES");

            string expected = PSEDocument.HOLDING;
            string actual = sectorNameMap.GetKey("H O L D I N G   F I R M S");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestReader_ReportBody()
        {
            //PDFTextStripper stripper = new PDFTextStripper();
            var pdfSharpService = new PdfTextSharpService();
            PSEReportReader reader = new PSEReportReader(pdfSharpService.ExtractTextFromPdf(pdfDocPath));

            PSEDocument pd = new PSEDocument();
            reader.Fill(pd);

            //Bid Ask Open High Low Close Volume Value NFB/S

            //METROBANK MBT 78.75 78.8 78.95 79.1 78.55 78.8 2,050,170 161,601,172 7,609,520

            StockItem stock = pd.GetStock("MBT");
            double expected = 78.75;
            double actual = stock.Bid;
            Assert.AreEqual(expected, actual);

            expected = 78.95;
            actual = stock.Open;
            Assert.AreEqual(expected, actual);

            expected = 2050170;
            actual = stock.Volume;
            Assert.AreEqual(expected, actual);

            expected = 161601172;
            actual = stock.Value;
            Assert.AreEqual(expected, actual);

            expected = 7609520;
            actual = stock.NetForeignBuy;
            Assert.AreEqual(expected, actual);

            //PSBANK PSB 98.55 102.9 102.9 102.9 102.9 102.9 50 5,145 -
            stock = pd.GetStock("PSB");
            expected = 0;
            actual = stock.High;
            Assert.AreEqual(expected, actual);

            actual = stock.Volume;
            Assert.AreEqual(expected, actual);

            actual = stock.Value;
            Assert.AreEqual(expected, actual);

            actual = stock.NetForeignBuy;
            Assert.AreEqual(expected, actual);           

            //MANULIFE                 MFC     557 559 556 560 556 559 160 89,380 (16,680)
            stock = pd.GetStock("MFC");
            expected = 559;
            actual = stock.Close;
            Assert.AreEqual(expected, actual);

            expected = 160;
            actual = stock.Volume;
            Assert.AreEqual(expected, actual);

            expected = 89380;
            actual = stock.Value;
            Assert.AreEqual(expected, actual);

            expected = -16680;
            actual = stock.NetForeignBuy;
            Assert.AreEqual(expected, actual);               

            //ENERGY DEVT.             EDC     6.04 6.06 5.76 6.06 5.76 6.06 84,308,100 499,835,886 18,306,534
            stock = pd.GetStock("EDC");
            expected = 6.06;
            actual = stock.Ask;
            Assert.AreEqual(expected, actual);

            expected = 5.76;
            actual = stock.Low;
            Assert.AreEqual(expected, actual);

            expected = 84308100;
            actual = stock.Volume;
            Assert.AreEqual(expected, actual);

            expected = 499835886;
            actual = stock.Value;
            Assert.AreEqual(expected, actual);

            expected = 18306534;
            actual = stock.NetForeignBuy;
            Assert.AreEqual(expected, actual);
            
            //COSMOS                   CBC     - - - - - - - - -
            stock = pd.GetStock("CBC");
            expected = 0;
            actual = stock.High;
            Assert.AreEqual(expected, actual);

            actual = stock.Volume;
            Assert.AreEqual(expected, actual);

            actual = stock.Value;
            Assert.AreEqual(expected, actual);

            actual = stock.NetForeignBuy;
            Assert.AreEqual(expected, actual);    

            //VULCAN IND`L             VUL     0.81 0.86 0.79 0.86 0.79 0.86 140,000 118,170 -
            stock = pd.GetStock("VUL");
            expected = 0.86;
            actual = stock.Close;
            Assert.AreEqual(expected, actual);

            expected = 140000;
            actual = stock.Volume;
            Assert.AreEqual(expected, actual);

            expected = 118170;
            actual = stock.Value;
            Assert.AreEqual(expected, actual);

            expected = 0;
            actual = stock.NetForeignBuy;
            Assert.AreEqual(expected, actual);

            //ANSCOR                   ANS     3.12 3.15 3.12 3.15 3.12 3.15 227,000 710,740 62,400
            stock = pd.GetStock("ANS");
            expected = 3.12;
            actual = stock.Bid;
            Assert.AreEqual(expected, actual);

            expected = 3.12;
            actual = stock.Open;
            Assert.AreEqual(expected, actual);

            expected = 227000;
            actual = stock.Volume;
            Assert.AreEqual(expected, actual);

            expected = 710740;
            actual = stock.Value;
            Assert.AreEqual(expected, actual);

            expected = 62400;
            actual = stock.NetForeignBuy;
            Assert.AreEqual(expected, actual);

            //SEAFRONT RES.            SPM     0.95 1.2 - - - - - - -
            stock = pd.GetStock("SPM");
            expected = 1.2;
            actual = stock.Ask;
            Assert.AreEqual(expected, actual);

            expected = 0;
            actual = stock.Volume;
            Assert.AreEqual(expected, actual);

            actual = stock.Value;
            Assert.AreEqual(expected, actual);

            actual = stock.NetForeignBuy;
            Assert.AreEqual(expected, actual);

            //SINOPHIL                 SINO    0.305 0.31 0.31 0.31 0.305 0.305 1,190,000 366,450 -
            stock = pd.GetStock("SINO");
            expected = 0.31;
            actual = stock.High;
            Assert.AreEqual(expected, actual);

            expected = 0.305;
            actual = stock.Close;
            Assert.AreEqual(expected, actual);

            expected = 1190000;
            actual = stock.Volume;
            Assert.AreEqual(expected, actual);

            expected = 366450;
            actual = stock.Value;
            Assert.AreEqual(expected, actual);

            expected = 0;
            actual = stock.NetForeignBuy;
            Assert.AreEqual(expected, actual);
            

            //AYALA LAND               ALI     17.24 17.26 17.3 17.32 17.1 17.24 7,144,400 123,230,996 (79,735,452)
            stock = pd.GetStock("ALI");
            expected = 17.24;
            actual = stock.Close;
            Assert.AreEqual(expected, actual);

            expected = 7144400;
            actual = stock.Volume;
            Assert.AreEqual(expected, actual);

            expected = 123230996;
            actual = stock.Value;
            Assert.AreEqual(expected, actual);

            expected = -79735452;
            actual = stock.NetForeignBuy;
            Assert.AreEqual(expected, actual);


            //MARSTEEL B               MCB     - - - - - - - - -
            stock = pd.GetStock("MCB");
            expected = 0;
            actual = stock.Bid;
            Assert.AreEqual(expected, actual);

            actual = stock.Open;
            Assert.AreEqual(expected, actual);

            actual = stock.Close;
            Assert.AreEqual(expected, actual);

            actual = stock.Volume;
            Assert.AreEqual(expected, actual);

            actual = stock.Value;
            Assert.AreEqual(expected, actual);

            actual = stock.NetForeignBuy;
            Assert.AreEqual(expected, actual);
           

            //SM DEVT                  SMDC 9.35 9.4 8.64 9.8 8.64 9.35 3,641,200 33,750,420 633,735
            stock = pd.GetStock("SMDC");
            expected = 9.35;
            actual = stock.Bid;
            Assert.AreEqual(expected, actual);

            expected = 8.64;
            actual = stock.Low;
            Assert.AreEqual(expected, actual);

            expected = 3641200;
            actual = stock.Volume;
            Assert.AreEqual(expected, actual);

            expected = 33750420;
            actual = stock.Value;
            Assert.AreEqual(expected, actual);

            expected = 633735;
            actual = stock.NetForeignBuy;
            Assert.AreEqual(expected, actual);

            //GMA NETWORK              GMA7 7.4 7.45 7.4 7.45 7 7.4 408,500 3,010,200 -
            stock = pd.GetStock("GMA7");
            expected = 7;
            actual = stock.Low;
            Assert.AreEqual(expected, actual);

            expected = 408500;
            actual = stock.Volume;
            Assert.AreEqual(expected, actual);

            expected = 3010200;
            actual = stock.Value;
            Assert.AreEqual(expected, actual);

            expected = 0;
            actual = stock.NetForeignBuy;
            Assert.AreEqual(expected, actual);

            //DIGITEL                  DGTL 1.51 1.52 1.53 1.54 1.51 1.52 36,056,000 54,884,680 (686,960)
            stock = pd.GetStock("DGTL");
            expected = 1.53;
            actual = stock.Open;
            Assert.AreEqual(expected, actual);

            expected = 36056000;
            actual = stock.Volume;
            Assert.AreEqual(expected, actual);

            expected = 54884680;
            actual = stock.Value;
            Assert.AreEqual(expected, actual);

            expected = -686960;
            actual = stock.NetForeignBuy;
            Assert.AreEqual(expected, actual);

            //WATERFRONT               WPI     0.25 0.265 0.27 0.27 0.27 0.27 50,000 13,500 -
            stock = pd.GetStock("WPI");
            expected = 0.27;
            actual = stock.Close;
            Assert.AreEqual(expected, actual);

            expected = 50000;
            actual = stock.Volume;
            Assert.AreEqual(expected, actual);

            expected = 13500;
            actual = stock.Value;
            Assert.AreEqual(expected, actual);

            expected = 0;
            actual = stock.NetForeignBuy;
            Assert.AreEqual(expected, actual);

            //GEOGRACE                 GEO     0.72 0.73 0.7 0.8 0.7 0.73 58,761,000 43,384,240 (7,200)
            stock = pd.GetStock("GEO");
            expected = 0.8;
            actual = stock.High;
            Assert.AreEqual(expected, actual);

            expected = 58761000;
            actual = stock.Volume;
            Assert.AreEqual(expected, actual);

            expected = 43384240;
            actual = stock.Value;
            Assert.AreEqual(expected, actual);

            expected = -7200;
            actual = stock.NetForeignBuy;
            Assert.AreEqual(expected, actual);

            //SEMIRARA MINING          SCC     142.1 143 139 145 139 143 569,960 80,377,827 (41,123,492)
            stock = pd.GetStock("SCC");
            expected = 143;
            actual = stock.Ask;
            Assert.AreEqual(expected, actual);

            expected = 569960;
            actual = stock.Volume;
            Assert.AreEqual(expected, actual);

            expected = 80377827;
            actual = stock.Value;
            Assert.AreEqual(expected, actual);

            expected = -41123492;
            actual = stock.NetForeignBuy;
            Assert.AreEqual(expected, actual);

            //PHILODRILL A             OV      0.014 0.015 0.014 0.015 0.014 0.014 173,200,000 2,525,900 -
            stock = pd.GetStock("OV");
            expected = 0.014;
            actual = stock.Close;
            Assert.AreEqual(expected, actual);

            expected = 173200000;
            actual = stock.Volume;
            Assert.AreEqual(expected, actual);

            expected = 2525900;
            actual = stock.Value;
            Assert.AreEqual(expected, actual);

            expected = 0;
            actual = stock.NetForeignBuy;
            Assert.AreEqual(expected, actual);

        }


        [TestMethod]
        public void TestReader_SectorSummary()
        {
            //PDFTextStripper stripper = new PDFTextStripper();
            IPdfService pdfService = new PdfTextSharpService();
            var reader = new PSEReportReader(pdfService.ExtractTextFromPdf(pdfDocPath));

            var pd = new PSEDocument();
            reader.Fill(pd);
            
            // psei
            SectorItem psei = pd.GetSector(PSEDocument.PSEI);
                       
            ulong expected = 3150242905;
            ulong actual = psei.Volume;

            Assert.AreEqual(expected, actual, "PSEi volume not equal");

            double expected_value = 5634576802.26;
            double actual_value = pd.GetSector(PSEDocument.PSEI).Value;

            Assert.AreEqual(expected_value, actual_value, "PSEi value not equal");

            expected_value = 7007.63;
            actual_value = psei.Open;
            Assert.AreEqual(expected_value, actual_value, "PSEi open not equal");

            expected_value = 7011.28;
            actual_value = psei.High;
            Assert.AreEqual(expected_value, actual_value, "PSEi high not equal");

            expected_value = 6986.86;
            actual_value = psei.Low;
            Assert.AreEqual(expected_value, actual_value, "PSEi low not equal");

            expected_value = 6999.75;
            actual_value = psei.Close;
            Assert.AreEqual(expected_value, actual_value, "PSEi close not equal");

            
            // financial
            SectorItem financial = pd.GetSector(PSEDocument.FINANCIAL);

            expected = 8105540;
            actual = financial.Volume;

            Assert.AreEqual(expected, actual, "Financial volume not equal");

            expected_value = 755542372.19;
            actual_value = financial.Value;

            Assert.AreEqual(expected, actual, "Financial value not equal");

            //913.01 935.52 909.34 935.52 2.47 22.51 24,780,801 882,690,827.9
            expected_value = 1585.35;
            actual_value = financial.Open;

            Assert.AreEqual(expected_value, actual_value, "Financial open not equal");

            expected_value = 1585.39;
            actual_value = financial.High;

            Assert.AreEqual(expected_value, actual_value, "Financial high not equal");

            expected_value = 1577.85;
            actual_value = financial.Low;

            Assert.AreEqual(expected_value, actual_value, "Financial low not equal");

            expected_value = 1583.44;
            actual_value = financial.Close;

            Assert.AreEqual(expected_value, actual_value);


            // mining
            SectorItem mining = pd.GetSector(PSEDocument.MINING_OIL);

            expected = 2528326978;
            actual = mining.Volume;

            Assert.AreEqual(expected, actual, "Mining volume not equal");

            expected_value = 143087427.64;
            actual_value = mining.Value;

            Assert.AreEqual(expected, actual, "Mining value not equal");

            //11,644.77 12,468.64 11,644.77 12,387.7 7.97 914.68 3,832,444,034 977,394,265.25

            expected_value = 10885.19;
            actual_value = mining.Open;

            Assert.AreEqual(expected_value, actual_value);

            expected_value = 10886.63;
            actual_value = mining.High;

            Assert.AreEqual(expected_value, actual_value);

            expected_value = 10700.58;
            actual_value = mining.Low;

            Assert.AreEqual(expected_value, actual_value);

            expected_value = 10740.09;
            actual_value = mining.Close;

            Assert.AreEqual(expected_value, actual_value);

            SectorItem pse = pd.GetSector(PSEDocument.PSEI);
            expected_value = -616052104.21;
            actual_value = pse.NetForeignBuy;

            Assert.AreEqual(expected_value, actual_value);

        }

        //[TestMethod]
        public void TestDownloadHistoricalData()
        {
            const string symbol = "MEG";
            const int numYears = 10;
            string downloadStr = "http://www.pse.com.ph/servlet/PSEChartServlet?securitySymbol=%s&years=%f";
            downloadStr = downloadStr.Replace("%s", symbol).Replace("%f", numYears.ToString());

            Uri downloadUri = new Uri(downloadStr);
            HistoricalDataDownloader downloader = new HistoricalDataDownloader(downloadUri);
            downloader.Download();

            HistoricalDataReader reader = downloader.GetReader();

            CSVOutputSettings outputSettings = new CSVOutputSettings();
            outputSettings.CSVFormat = "S,D,O,H,L,C,V,F";
            outputSettings.Delimiter = ",";
            outputSettings.OutputDirectory = "C:\\Users\\yeahbah\\Documents\\projects";
            outputSettings.Filename = symbol +".csv";
            outputSettings.DateFormat = "MM/DD/YYYY";

            reader.ToCSV(outputSettings);
                 
        }

        [TestMethod]
        public void TestDownloadHistoricalDataUsingUtil()
        {
            List<string> stockList = new List<string>();
            stockList.Add("MEG");
            stockList.Add("DMC");
            stockList.Add("PX");
            stockList.Add("NIKL");
            stockList.Add("GEO");
            stockList.Add("SCC");
            stockList.Add("ALI");
            stockList.Add("AC");
            stockList.Add("EEI");
            stockList.Add("GERI");
            stockList.Add("FGEN");
            stockList.Add("FPH");
            stockList.Add("ANI");
            stockList.Add("ELI");
            stockList.Add("LPZ");
            stockList.Add("MUSX");
            stockList.Add("LC");

            CSVOutputSettings outputSettings = new CSVOutputSettings();
            outputSettings.CSVFormat = "S,D,O,H,L,C,V,F";
            outputSettings.Delimiter = ",";
            outputSettings.OutputDirectory = "C:\\Users\\yeahbah\\Documents\\projects";
            outputSettings.Filename = "HistoricalData.csv";
            outputSettings.DateFormat = "MM/DD/YYYY";


            //Util.DownloadAndConvertHistoricalData(stockList, 20, outputSettings,
            //    null);
                       
        }

    }
}
