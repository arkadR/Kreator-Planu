using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;

namespace KreatorPlanu
{
	public class DataBase
	{
		public List<Block> blocks = new List<Block>();
		public SortedDictionary<string, Subject> subjects = new SortedDictionary<string, Subject>();

		public void LoadFromCSV(string dir)
		{
			using (StreamReader reader = new StreamReader(dir))
			{
				reader.ReadLine();
				while (!reader.EndOfStream)
				{
					string line = reader.ReadLine();
					string[] values = line.Split(';');					
					blocks.Add(new Block(values, this));
				}
			}
			GenerateSubjects();
		}

		public bool LoadFromSheets(string url)
		{
			WebClient client = new WebClient();
			try
			{
				client.DownloadFile(url, "temp.json");
			}
			catch 
			{
				return false;
			}
			finally
			{
				client.Dispose();
			}

			StreamReader reader = new StreamReader("temp.json");
			string json = reader.ReadToEnd();
			reader.Close();
			File.Delete("temp.json");
			json = json.Remove(0, json.IndexOf("{"));
			json = json.Remove(json.Length - 2);
			JObject parsedJson = JObject.Parse(json);
			foreach (JProperty prop in parsedJson.Children<JProperty>())
			{
				if (prop.Name.Equals("table"))
				{
					JArray rows = (JArray)prop.Last.Last.Last;
					foreach (JObject row in rows)
					{
						string[] s = new string[12];
						JArray sub = (JArray)row.Last.Last;

						for (int i = 0; i < s.Length; i++)
						{
							s[i] = "";
							if (sub[i].HasValues)
							{
								s[i] = (string)sub[i].Last;
							}
						}
						blocks.Add(new Block(s, this));
					}
				}
			}
			GenerateSubjects();
			return true;
		}

		public void Clear()
		{
			blocks.Clear();
			subjects.Clear();
		}

		public void GenerateSubjects()
		{
			int height = 5;
			for (int i = 0; i < blocks.Count; i++)
			{
				if (!subjects.ContainsKey(blocks[i].BlockID))
				{
					Subject s = new Subject(blocks[i])
					{
						Font = new Font("Arial", 8, FontStyle.Bold),
						Location = new Point(16, height),
						TextAlign = ContentAlignment.MiddleLeft
					};
					subjects.Add(blocks[i].BlockID, s);
				}
				subjects[blocks[i].BlockID].blocks.Add(blocks[i]);
				subjects[blocks[i].BlockID].UpdateCount();
			}
			foreach (Subject s in subjects.Values)
			{
				s.Top = height;
				s.checkBox.Top = height-5;
				height += 25;
			}
		}
	}
}
