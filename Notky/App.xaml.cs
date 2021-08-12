using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using Notky.Model;
using System.Windows.Media;

namespace Notky
{
    public partial class App : Application
    {

        private TaskbarIcon tb;
        private List<Note> notes;
        private Stream stream;
        private string appdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        private void Application_Startup(object sender, StartupEventArgs e)
        {

            if (File.Exists(appdata + "/Notky/data.bin"))
            {
                List<NoteModel> noteModels;

                using (stream = File.Open(Path.Combine(appdata + "/Notky", "data.bin"), FileMode.Open))
                    noteModels = (List<NoteModel>)new BinaryFormatter().Deserialize(stream);

                notes = new List<Note>();

                foreach (NoteModel noteModel in noteModels)
                {
                    Note note = new Note(
                        noteModel.left,
                        noteModel.top,
                        noteModel.height,
                        noteModel.width,
                        noteModel.fontSize,
                        noteModel.text,
                        noteModel.bgColor,
                        noteModel.frColor);

                    notes.Add(note);
                    note.Show();
                }
            }
            else
            {
                Directory.CreateDirectory(appdata + "/Notky");
                notes = new List<Note>();
            }

            tb = new TaskbarIcon();

            //Setting the tray icon from the embedded resource file stream
            //files need to have build action in properties set to "Embedded Resource"
            tb.Icon = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("Notky.Resources.tray.ico"));
            tb.ToolTipText = "Left click for a new note\nRight click on tray icon to close the app";

            tb.TrayLeftMouseDown += Tb_TrayLeftMouseDown;
            tb.TrayRightMouseDown += Tb_TrayRightMouseDown;
        }

        private void Tb_TrayRightMouseDown(object sender, RoutedEventArgs e)
        {
            Current.Shutdown();
        }

        private void Tb_TrayLeftMouseDown(object sender, RoutedEventArgs e)
        {
            Note note = new Note();
            notes.Add(note);
            note.Show();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            List<NoteModel> noteModels = new List<NoteModel>();

            foreach (Note note in notes)
            {
                if (!note.IsLoaded)
                    continue;
               
                noteModels.Add(new NoteModel(
                        note.Left,
                        note.Top,
                        note.Height,
                        note.Width,
                        note.Text.FontSize,
                        note.Text.Text,
                        ((SolidColorBrush)note.Background).Color.ToString(),
                        ((SolidColorBrush)note.Text.Foreground).Color.ToString()));
            }

            using (stream = File.Open(Path.Combine(appdata + "/Notky", "data.bin"), FileMode.OpenOrCreate))
                new BinaryFormatter().Serialize(stream, noteModels);
        }
    }
}