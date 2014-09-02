using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
namespace exiftoolHelper {
	public partial class MainForm : Form {
		public MainForm() {
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e) {
			if (folderBrowserDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) {
				return;
			}
			var exe = Path.Combine(Application.StartupPath, @"exiftool");
			exe += ".exe";
			if (!File.Exists(exe)) {
				System.IO.File.WriteAllBytes(exe, exiftoolHelper.Resources.exiftool);
			}
			var exportDir = Path.Combine(folderBrowserDialog.SelectedPath, "export");
			if (!Directory.Exists(exportDir)) {
				Directory.CreateDirectory(exportDir);
			}
			
			foreach (var file in Directory.EnumerateFiles(folderBrowserDialog.SelectedPath,"*.*")) {
				try {
					var img = new Bitmap(new System.Drawing.Bitmap(file));
					var fi = new System.IO.FileInfo(file);
					var newNameDir = Path.Combine(exportDir, fi.Name);
					img.Save(newNameDir, System.Drawing.Imaging.ImageFormat.Jpeg);
					img.Dispose();
					var info = new ProcessStartInfo(exe, " -overwrite_original --Software -tagsFromFile " + file + " -EXIF " + newNameDir);
					info.CreateNoWindow = true;
					info.WorkingDirectory = Path.GetDirectoryName(openFileDialog.FileName);
					var p = new Process();
					p.StartInfo = info;
					p.Start();
					p.WaitForExit();
					info = new ProcessStartInfo(exe, " -overwrite_original -Software= " + newNameDir);
					info.WorkingDirectory = Path.GetDirectoryName(openFileDialog.FileName);
					info.CreateNoWindow = true;
					p = System.Diagnostics.Process.Start(info);
					p.WaitForExit();
				} catch (Exception) {
					
				}
			}
			File.Delete(exe);
		}
	}
}
