﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using WordCloud;

namespace WordCloudTestApp
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			var lines = File.ReadLines("../../content/counts.csv");
			Words = new List<string>(100);
			Frequencies = new List<int>(100);
			foreach (var line in lines)
			{
				var textValue = line.Split(new char[] {','});
				Words.Add(textValue[0]);
				Frequencies.Add(int.Parse(textValue[1]));
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
		
			var mWc = new WordCloud.WordCloud(1000, 600);
			if(resultPictureBox.Image != null) resultPictureBox.Image.Dispose();
			Image i = mWc.Draw(Words, Frequencies);
			resultPictureBox.Image = i;
		}

		List<string> Words { get; set; }

		List<int> Frequencies { get; set; } 
	}
}