using Svg;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace svgtools
{
    public static class SvgConverter
    {

        public static void SavePng(string tSvg,string tFileName)
        {
            MemoryStream tData = new MemoryStream(Encoding.UTF8.GetBytes(tSvg));
            Svg.SvgDocument tSvgObj = SvgDocument.Open(tData);
            MemoryStream tStream = new MemoryStream();
            tSvgObj.Draw().Save(tStream, ImageFormat.Png);

            using (Stream localFile = new FileStream(tFileName,
             FileMode.OpenOrCreate))

            {
                localFile.Write(tStream.ToArray(), 0, (int)tStream.Length);

            }
            
        }
/*
        public ActionResult Export(FormCollection fc)
        {
            string tType = fc["type"];
            string tSvg = fc["svg"];
            string tFileName = fc["filename"];
            if (string.IsNullOrEmpty(tFileName))
                tFileName = "chart";
            MemoryStream tData = new MemoryStream(Encoding.UTF8.GetBytes(tSvg));
            MemoryStream tStream = new MemoryStream();
            string tTmp = new Random().Next().ToString();
            string tExt = "";
            string tTypeString = "";

            switch (tType)
            {
                case "image/png":
                    tTypeString = "-m image/png";
                    tExt = "png";
                    break;
                case "image/jpeg":
                    tTypeString = "-m image/jpeg";
                    tExt = "jpg";
                    break;
                case "application/pdf":
                    tTypeString = "-m application/pdf";
                    tExt = "pdf";
                    break;
                case "image/svg+xml":
                    tTypeString = "-m image/svg+xml";
                    tExt = "svg";
                    break;
            }

            if (tTypeString != "")
            {
                string tWidth = fc["width"];
                Svg.SvgDocument tSvgObj = SvgDocument.Open(tData);

                switch (tExt)
                {
                    case "jpg":
                        tSvgObj.Draw().Save(tStream, ImageFormat.Jpeg);
                        break;
                    case "png":
                        tSvgObj.Draw().Save(tStream, ImageFormat.Png);
                        break;
                    case "pdf":
                        PdfWriter tWriter = null;
                        Document tDocumentPdf = null;
                        try
                        {
                            tSvgObj.Draw().Save(tStream, ImageFormat.Png);
                            tDocumentPdf = new Document(new iTextSharp.text.Rectangle((float)tSvgObj.Width, (float)tSvgObj.Height));
                            tDocumentPdf.SetMargins(0.0f, 0.0f, 0.0f, 0.0f);
                            iTextSharp.text.Image tGraph = iTextSharp.text.Image.GetInstance(tStream.ToArray());
                            tGraph.ScaleToFit((float)tSvgObj.Width, (float)tSvgObj.Height);

                            tStream = new MemoryStream();
                            tWriter = PdfWriter.GetInstance(tDocumentPdf, tStream);
                            tDocumentPdf.Open();
                            tDocumentPdf.NewPage();
                            tDocumentPdf.Add(tGraph);
                            tDocumentPdf.Close();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        finally
                        {
                            tDocumentPdf.Close();
                            tWriter.Close();
                            tData.Dispose();
                            tData.Close();

                        }
                        break;
                    case "svg":
                        tStream = tData;
                        break;
                }
            }
            return File(tStream.ToArray(), tType, tFileName);
        }

    }*/
}
}
