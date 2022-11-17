using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO.Compression;
using System.Net;
using System.Web;

namespace Light_Novel_EPUB
{
    public partial class Form1 : Form
    {
        HttpClient client;
        XmlWriterSettings sts;
        JArray novelChaptersArray;

        public Form1()
        {
            InitializeComponent();

            ServicePointManager.DefaultConnectionLimit = 100;

            client = new HttpClient();

            sts = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t"
            };

            saveFolder.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            folderBrowserDialog1.SelectedPath = saveFolder.Text;

            log.Text = "Enter full novel url (e.g. https://lightnovels.me/novel/release-that-witch.html) and press Search" + Environment.NewLine;
        }


        private async void search_Click(object sender, EventArgs e)
        {
            log.AppendText("Searching..." + Environment.NewLine);

            saveEpub.Enabled = false;
            search.Enabled = false;
            currentCount.Text = "0";
            totalCount.Text = "0";
            try
            {
                string content = await GetStringContent(url.Text);

                int novelDataStart = content.IndexOf("<script id=\"__NEXT_DATA__\" type=\"application/json\">");
                int novelDataEnd = content.IndexOf("</script>", novelDataStart);
                string novelData = content.Substring(novelDataStart + 51, novelDataEnd - novelDataStart - 51);

                JObject novelDataJson = JObject.Parse(novelData);

                JToken novelInfo = novelDataJson["props"]["pageProps"]["novelInfo"];

                if (novelInfo != null)
                {
                    string novelId = novelInfo["novel_id"].ToString();
                    string novelName = novelInfo["novel_name"].ToString();
                    string novelImage = novelInfo["novel_image"].ToString();
                    string novelDescription = novelInfo["novel_description"].ToString();

                    string novelChapters = await GetStringContent("https://lightnovels.me/api/chapters?id=" + novelId + "&index=1&limit=15000");

                    JObject novelChaptersJson = JObject.Parse(novelChapters);

                    novelChaptersArray = (JArray)novelChaptersJson["results"];

                    title.Text = novelName;
                    description.Text = novelDescription;
                    chaptersCount.Text = novelChaptersArray.Count.ToString();
                    totalCount.Text = novelChaptersArray.Count.ToString();

                    if (novelImage != "")
                        coverImage.ImageLocation = "https://lightnovels.me" + novelImage;
                    else
                        coverImage.ImageLocation = "https://lightnovels.me/uploads/thumbs/lightnovel-placeholder.jpg";

                    saveEpub.Enabled = true;

                    log.AppendText("Novel found" + Environment.NewLine);
                }
                else
                {
                    title.Text = "";
                    chaptersCount.Text = "N/A";
                    description.Text = "";
                    coverImage.Image = null;
                    log.AppendText("Novel not found" + Environment.NewLine);
                }   
            }
            catch (Exception ex)
            {
                log.AppendText("Error: " + ex.Message + Environment.NewLine);

            }

            search.Enabled = true;
        }

        private async void saveEpub_Click(object sender, EventArgs e)
        {
            currentCount.Text = "0";
            try
            {
                log.AppendText("Saving as EPUB..." + Environment.NewLine);

                string novelName = title.Text;
                string novelDescription = description.Text;
                string epubLocation = Path.Combine(saveFolder.Text, novelName + ".epub");
                string novelGuid = Guid.NewGuid().ToString();

                if (File.Exists(epubLocation))
                    File.Delete(epubLocation);

                var zipArchive = ZipFile.Open(epubLocation, ZipArchiveMode.Create);

                // Creating mimetype file
                var mimetypeFile = zipArchive.CreateEntry("mimetype");
                using (var file = mimetypeFile.Open())
                {
                    Byte[] data = new UTF8Encoding(true).GetBytes("application/epub+zip");
                    file.Write(data, 0, data.Length);
                }

                // Creating container.xml file
                var containerFile = zipArchive.CreateEntry("META-INF/container.xml");

                using (var file = containerFile.Open())
                {
                    using (XmlWriter writer = XmlWriter.Create(file, sts))
                    {
                        writer.WriteStartDocument();

                        writer.WriteStartElement("container", "urn:oasis:names:tc:opendocument:xmlns:container");
                        writer.WriteAttributeString("version", "1.0");

                        writer.WriteStartElement("rootfiles");

                        writer.WriteStartElement("rootfile");
                        writer.WriteAttributeString("full-path", "OEBPS/content.opf");
                        writer.WriteAttributeString("media-type", "application/oebps-package+xml");
                        writer.WriteEndElement(); //rootfile

                        writer.WriteEndElement(); //rootfiles
                        writer.WriteEndElement(); //container
                        writer.WriteEndDocument();
                    }
                }

                // Saving cover.jpg
                var coverFile = zipArchive.CreateEntry("OEBPS/images/cover.jpg");
                using (var file = coverFile.Open())
                {
                    coverImage.Image.Save(file, System.Drawing.Imaging.ImageFormat.Jpeg);
                }

                // Asynchronously downloading chapters
                var task = novelChaptersArray.Select(x => AsyncChapter(x, zipArchive));
                var chapterInfo = await Task.WhenAll(task);



                // Creating content.opf file
                var contentFile = zipArchive.CreateEntry("OEBPS/content.opf");

                using (var file = contentFile.Open())
                {
                    using (XmlWriter writer = XmlWriter.Create(file, sts))
                    {
                        writer.WriteStartDocument();

                        writer.WriteStartElement("package", "http://www.idpf.org/2007/opf");
                        writer.WriteAttributeString("xmlns", "http://www.idpf.org/2007/opf");
                        writer.WriteAttributeString("unique-identifier", "BookId");
                        writer.WriteAttributeString("version", "3.0");

                        writer.WriteStartElement("metadata");

                        string prefix = "dc";
                        string ns = "http://purl.org/dc/elements/1.1/";

                        writer.WriteAttributeString("xmlns", prefix, null, ns);
                        writer.WriteAttributeString("xmlns", "opf", null, "http://www.idpf.org/2007/opf");

                        writer.WriteStartElement(prefix, "title", ns);
                        writer.WriteRaw(novelName);
                        writer.WriteEndElement(); //title

                        writer.WriteStartElement(prefix, "language", ns);
                        writer.WriteValue("en");
                        writer.WriteEndElement(); //language

                        writer.WriteStartElement(prefix, "identifier", ns);
                        writer.WriteAttributeString("id", "BookId");
                        writer.WriteValue(novelGuid);
                        writer.WriteEndElement(); //identifier

                        writer.WriteStartElement(prefix, "date", ns);
                        writer.WriteValue(DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture));
                        writer.WriteEndElement(); //date

                        writer.WriteStartElement(prefix, "description", ns);
                        writer.WriteRaw(novelDescription);
                        writer.WriteEndElement(); //description

                        writer.WriteStartElement("meta");
                        writer.WriteAttributeString("property", "dcterms:modified");
                        writer.WriteValue(DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture));
                        writer.WriteEndElement(); //meta

                        writer.WriteStartElement("meta");
                        writer.WriteAttributeString("name", "cover");
                        writer.WriteAttributeString("content", "CoverImage");
                        writer.WriteEndElement(); //meta

                        writer.WriteEndElement(); //metadata


                        writer.WriteStartElement("manifest");

                        writer.WriteStartElement("item");
                        writer.WriteAttributeString("id", "ncx");
                        writer.WriteAttributeString("href", "toc.ncx");
                        writer.WriteAttributeString("media-type", "application/x-dtbncx+xml");
                        writer.WriteEndElement(); //item

                        writer.WriteStartElement("item");
                        writer.WriteAttributeString("id", "CoverImage");
                        writer.WriteAttributeString("href", "images/cover.jpg");
                        writer.WriteAttributeString("media-type", "image/jpeg");
                        writer.WriteEndElement(); //item

                        foreach (string[] elem in chapterInfo)
                        {
                            writer.WriteStartElement("item");
                            writer.WriteAttributeString("id", "chapter" + elem[0]);
                            writer.WriteAttributeString("href", elem[1]);
                            writer.WriteAttributeString("media-type", "application/xhtml+xml");
                            writer.WriteEndElement(); //item
                        }
                        writer.WriteEndElement(); //manifest

                        writer.WriteStartElement("spine");
                        writer.WriteAttributeString("toc", "ncx");

                        foreach (string[] elem in chapterInfo)
                        {
                            writer.WriteStartElement("itemref");
                            writer.WriteAttributeString("idref", "chapter" + elem[0]);
                            writer.WriteEndElement(); //itemref
                        }

                        writer.WriteEndElement(); //spine
                        writer.WriteEndElement(); //package
                        writer.WriteEndDocument();
                    }
                }

                // Creating toc.ncx file
                var tocFile = zipArchive.CreateEntry("OEBPS/toc.ncx");

                using (var file = tocFile.Open())
                {
                    using (XmlWriter writer = XmlWriter.Create(file, sts))
                    {
                        writer.WriteStartDocument();
                        writer.WriteStartElement("ncx", "http://www.daisy.org/z3986/2005/ncx/");
                        writer.WriteAttributeString("xmlns", "http://www.daisy.org/z3986/2005/ncx/");
                        writer.WriteAttributeString("version", "2005-1");

                        writer.WriteStartElement("head");

                        writer.WriteStartElement("meta");
                        writer.WriteAttributeString("name", "dtb:uid");
                        writer.WriteAttributeString("content", novelGuid);
                        writer.WriteEndElement(); //meta

                        writer.WriteStartElement("meta");
                        writer.WriteAttributeString("name", "dtb:depth");
                        writer.WriteAttributeString("content", "2");
                        writer.WriteEndElement(); //meta

                        writer.WriteStartElement("meta");
                        writer.WriteAttributeString("name", "dtb:totalPageCount");
                        writer.WriteAttributeString("content", "0");
                        writer.WriteEndElement(); //meta

                        writer.WriteStartElement("meta");
                        writer.WriteAttributeString("name", "dtb:maxPageNumber");
                        writer.WriteAttributeString("content", "0");
                        writer.WriteEndElement(); //meta

                        writer.WriteEndElement(); //head

                        writer.WriteStartElement("docTitle");

                        writer.WriteStartElement("text");
                        writer.WriteRaw(novelName);
                        writer.WriteEndElement(); //text

                        writer.WriteEndElement(); //docTitle

                        writer.WriteStartElement("docAuthor");

                        writer.WriteStartElement("text");
                        writer.WriteRaw("");
                        writer.WriteEndElement(); //text

                        writer.WriteEndElement(); //docAuthor

                        writer.WriteStartElement("navMap");

                        foreach (string[] elem in chapterInfo)
                        {
                            writer.WriteStartElement("navPoint");
                            writer.WriteAttributeString("id", "chapter" + elem[0]);
                            writer.WriteAttributeString("playOrder", elem[0]);

                            writer.WriteStartElement("navLabel");
                            writer.WriteStartElement("text");
                            writer.WriteRaw(elem[2]);
                            writer.WriteEndElement(); //text
                            writer.WriteEndElement(); //navLabel

                            writer.WriteStartElement("content");
                            writer.WriteAttributeString("src", elem[1]);
                            writer.WriteEndElement(); //content
                            writer.WriteEndElement(); //navPoint
                        }

                        writer.WriteEndElement(); //navMap
                        writer.WriteEndElement(); //ncx
                        writer.WriteEndDocument();
                    }
                }

                zipArchive.Dispose();
                log.AppendText("Saving finished" + Environment.NewLine);
            }
            catch (Exception ex)
            {
                log.AppendText("Error: " + ex.Message + Environment.NewLine);
            }
        }

        private void coverImage_Paint(object sender, PaintEventArgs e)
        {
            if (coverImage.Image == null)
            {
                using (Font myFont = new Font("Arial", 14))
                {
                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;
                    e.Graphics.DrawString("No image", myFont, Brushes.White, coverImage.ClientRectangle, sf);

                }
            }
        }

        private void saveTo_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                saveFolder.Text = folderBrowserDialog1.SelectedPath;
            }
        }


        public async Task<string[]> AsyncChapter(JToken token, ZipArchive zipArchive)
        {

            string chapterIndex = token["chapter_index"].ToString();
            string chapterName = HttpUtility.HtmlEncode(token["chapter_name"].ToString());
            string chapterId = token["id"].ToString();
            string chapterFileName = "";

            try
            {
                string novelChapterPage = await GetStringContent("https://lightnovels.me" + token["slug"]);

                int novelChapterContentStart = novelChapterPage.IndexOf("<div class=\"chapter-content\">");
                int novelChapterContentEnd = novelChapterPage.IndexOf("</div><p class", novelChapterContentStart);

                string novelChapterContent = novelChapterPage.Substring(novelChapterContentStart + 29, novelChapterContentEnd - novelChapterContentStart - 29);

                if (novelChapterContent.IndexOf("<div class") >= 0)
                {
                    int index1 = novelChapterContent.IndexOf("<div class");
                    int index2 = novelChapterContent.IndexOf(">", index1);
                    int index3 = novelChapterContent.LastIndexOf("</div>");

                    novelChapterContent = novelChapterContent.Substring(index2 + 1, index3 - index2 - 1);
                }

                chapterFileName = "chapter-" + chapterIndex + "-" + chapterId + ".xhtml";

                var chapterFile = zipArchive.CreateEntry("OEBPS/" + chapterFileName);

                using (var file = chapterFile.Open())
                {
                    using (XmlWriter writer = XmlWriter.Create(file, sts))
                    {
                        writer.WriteStartDocument();

                        writer.WriteDocType("html", "-//W3C//DTD XHTML 1.1//EN", "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd", null);

                        writer.WriteStartElement("html", "http://www.w3.org/1999/xhtml");
                        writer.WriteAttributeString("xmlns", "http://www.w3.org/1999/xhtml");
                        writer.WriteAttributeString("xmlns", "epub", null, "http://www.idpf.org/2007/ops");

                        writer.WriteStartElement("head");

                        writer.WriteStartElement("meta");
                        writer.WriteAttributeString("http-equiv", "Default-Style");
                        writer.WriteAttributeString("content", "text/html; charset=utf-8");
                        writer.WriteEndElement(); //meta

                        writer.WriteStartElement("title");
                        writer.WriteRaw(chapterName);
                        writer.WriteEndElement(); //title

                        writer.WriteEndElement(); //head

                        writer.WriteStartElement("body");
                        writer.WriteRaw(novelChapterContent);
                        writer.WriteEndElement(); //body

                        writer.WriteEndElement(); //html
                        writer.WriteEndDocument();
                    }
                }
            }
            catch (Exception ex)
            {
                log.AppendText("Error at chapter " + chapterIndex + ": " + ex.Message + Environment.NewLine);
            }

            currentCount.Text = (Int32.Parse(currentCount.Text) + 1).ToString();

            return new string[] { chapterIndex, chapterFileName, chapterName };
        }


        private async Task<string> GetStringContent(string url)
        {
            string result = "";
            do
            {
                try
                {
                    result = await client.GetStringAsync(url);
                    break;
                }
                catch(HttpRequestException ex)
                {
                    log.AppendText("Error getting content in page: " + url + Environment.NewLine + ex.Message + Environment.NewLine);
                    log.AppendText("Retrying..." + Environment.NewLine);

                    await Task.Delay(500);
                }
                
            }
            while (true);

            return result;
        }

    }
}
