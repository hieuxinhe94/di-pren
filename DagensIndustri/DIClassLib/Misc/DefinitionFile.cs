using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace DIClassLib.Misc
{

    /// <summary>
    /// Note that all comparisons are CASE SENSITIVE. Just because I want to, the finnish way.
    /// </summary>
    public class DefinitionFile
    {
        public DefinitionFile()
        {
            LoadFile();
        }

        /// <summary>
        /// Makes a copy of existing file and then saves it.
        /// </summary>
        public void SaveFile()
        {
            StreamWriter writer = null;

            try
            {
                //Copy existing file
                File.Copy(FilePath, FilePath.Replace(".txt", "_" + DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss") + ".txt"));
                //Creat file
                writer = new StreamWriter(FilePath, false, FileEncoding);
                writer.Write(this.ToString());
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }


        /// <summary>
        /// If value differs, line with provided name will be updated with new value.
        /// Be careful, if name of line ain't unique all lines with that name will be updated.
        /// </summary>
        /// <param name="name">Name of line to update</param>
        /// <param name="value">Value of line</param>
        /// <returns>True if updated.</returns>
        public bool UpdateLine(string name, string value)
        {
            bool isUpdated = false;
            Block[] blocksToParse = new Block[this.Blocks.Count];
            this.Blocks.CopyTo(blocksToParse);

            //Parse blocks
            foreach (Block block in blocksToParse)
            {
                //Get block that has a line with name that match with criteria
                Line line = block.Lines.Find(delegate(Line l) { return l.Name == name; });

                if (line != null)
                {
                    if (!line.Value.Equals(value))
                    {
                        line.CompleteLine = line.CompleteLine.Replace(line.Value, value);
                        //replace line in block
                        block.Lines[block.Lines.IndexOf(line)] = line;
                        //replace block
                        this.Blocks[this.Blocks.IndexOf(block)] = block;
                        isUpdated = true;
                    }
                }
            }

            return isUpdated;
        }

        /// <summary>
        /// Campno will be inserted if not already exists.
        /// </summary>
        /// <param name="campNo">The campNo to be inserted</param>
        /// <returns>True if updated</returns>
        public bool InsertCampNo(int campNo)
        {
            bool isUpdated = false;
            Block[] blocksToParse = new Block[this.Blocks.Count];
            this.Blocks.CopyTo(blocksToParse);

            //Parse blocks
            foreach (Block block in blocksToParse)
            {
                //Get block that has a line with camptype that match with criteria
                Line line = block.Lines.Find(delegate(Line l) { return l.Name == "CampType" && (l.Value == "VT" || l.Value == "V"); });

                if (line != null)
                {
                    if (!LineMatch(block, "CampaignNo", campNo.ToString()))
                    {
                        Line campNoLine = block.Lines.Find(delegate(Line l) { return l.Name == "CampaignNo"; });
                        campNoLine.CompleteLine = campNoLine.CompleteLine.Replace(campNoLine.Value, campNoLine.Value + "," + campNo);
                        //replace line in block
                        block.Lines[block.Lines.IndexOf(campNoLine)] = campNoLine;
                        //replace block
                        this.Blocks[this.Blocks.IndexOf(block)] = block;
                        isUpdated = true;
                    }
                }
            }

            return isUpdated;
        }

        public bool IsEpiBounce(string receiveType, int campNo)
        {
            //Parse blocks
            foreach (Block block in this.Blocks)
            {
                if (block != null)
                {
                    //Get block that has a line with camptype that match with criteria
                    Line line = block.Lines.Find(delegate(Line l) { return l.Name == "CampType" && l.Value == "VT"; });

                    if (line != null)
                    {
                        if (!LineMatch(block, "ReceiveType", receiveType))
                            continue;

                        if (LineMatch(block, "CampaignNo", campNo.ToString()))
                            return true;
                    }
                }
            }

            return false;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (Block block in this.Blocks)
            {
                foreach (Line line in block.Lines)
                {
                    sb.Append(line.CompleteLine + Environment.NewLine);
                }
            }

            return sb.ToString();
        }

        private bool LineMatch(Block block, string name, string value)
        {
            Line line = block.Lines.Find(delegate(Line l) { return l.Name == name; });
            if (line != null)
            {
                //Split line
                string[] array = line.Value.Split(',');
                //Check if value is in array
                if (Array.IndexOf(array, value) > -1)
                    return true;
            }

            return false;
        }

        private void LoadFile()
        {
            if (File.Exists(FilePath))
            {
                StreamReader file = null;
                Blocks = new List<Block>();

                try
                {
                    file = new StreamReader(FilePath, FileEncoding);
                    string line;
                    Block block = null;

                    //Start reading file, line by line
                    while ((line = file.ReadLine()) != null)
                    {
                        //check if block
                        if (line.StartsWith("/*") || line.StartsWith("//"))
                        {
                            if (block != null)
                                this.Blocks.Add(block);

                            string blockname = line.StartsWith("/*") ? "Header" : "Rule";
                            block = new Block(blockname);
                            block.Lines.Add(new Line("LineComment", string.Empty, line));
                        }
                        else
                        {
                            if (this.Blocks.Count == 0)
                            {
                                block.Lines.Add(new Line("HeaderLine", string.Empty, line));
                            }
                            else
                            {
                                //Split line on whitespace
                                string[] arraySplitWhitespace = line.Split(' ');
                                string name = string.Empty;
                                string value = string.Empty;

                                //If > 1, keep doing your thing
                                if (arraySplitWhitespace.Length > 1)
                                {
                                    name = arraySplitWhitespace[1];
                                    string[] arraySplitQuote = line.Split("'".ToCharArray());
                                    //value = array.Length > 1 ? array[1] : string.Empty;
                                    value = arraySplitQuote.Length > 1 ? arraySplitQuote[1] : arraySplitWhitespace[3];

                                    string[] arrayName = name.Split('$');

                                    if (arrayName.Length > 1)
                                        name = arrayName[0];
                                }
                                else
                                {
                                    name = "blankline";
                                }

                                block.Lines.Add(new Line(name, value, line));
                            }
                        }
                    }

                    this.Blocks.Add(block);
                }
                finally
                {
                    if (file != null)
                        file.Close();
                }
            }
        }

        private string FilePath
        {
            get
            {
                return MiscFunctions.GetAppsettingsValue("DefinitionFilePath");
            }

        }

        private Encoding FileEncoding
        {
            get
            {
                // *** Use Default of Encoding.Default (Ansi CodePage)
                Encoding enc = Encoding.Default;

                // *** Detect byte order mark if any - otherwise assume default
                byte[] buffer = new byte[5];
                FileStream file = new FileStream(FilePath, FileMode.Open);
                file.Read(buffer, 0, 5);
                file.Close();

                if (buffer[0] == 0xef && buffer[1] == 0xbb && buffer[2] == 0xbf)
                    enc = Encoding.UTF8;
                else if (buffer[0] == 0xfe && buffer[1] == 0xff)
                    enc = Encoding.Unicode;
                else if (buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0xfe && buffer[3] == 0xff)
                    enc = Encoding.UTF32;
                else if (buffer[0] == 0x2b && buffer[1] == 0x2f && buffer[2] == 0x76)
                    enc = Encoding.UTF7;

                return enc;
            }
        }

        private List<Block> Blocks { get; set; }
    }

    public class Block
    {
        public Block(string name)
        {
            Name = name;
            Lines = new List<Line>();
        }

        public string Name { get; set; }

        public List<Line> Lines { get; set; }
    }

    public class Line
    {
        public Line(string name, string value, string completeLine)
        {
            Name = name;
            Value = value;
            CompleteLine = completeLine;
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public string CompleteLine { get; set; }

    }

}
