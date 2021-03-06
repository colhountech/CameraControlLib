﻿using CameraControlLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraController
{
    public class Settings
    {
        /// <summary>
        /// Gets or sets the filename for the JSON settings file.
        /// </summary>
        [JsonIgnore]
        public string Filename { get; set; }

        public CameraReference DefaultCamera = null;
        public bool OverrideCameraRanges = false;
        public List<string> HiddenProperties = new List<string>();
        public List<PresetGroup> PresetGroups { get; } = new List<PresetGroup>();

        public static Settings Load(StreamReader reader)
        {
            var jsonString = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<Settings>(jsonString);
        }

        public void Save()
        {
            if (Filename == null)
                throw new InvalidOperationException("Settings filename not set");

            using (var writer = new StreamWriter(Filename, false, Encoding.UTF8))
            {
                writer.Write(JsonConvert.SerializeObject(this, Formatting.Indented));
            }

            OnSaved();
        }

        public event EventHandler<EventArgs> Saved;
        protected void OnSaved()
        {
            Saved?.Invoke(this, new EventArgs());
        }
    }

    public class CameraReference : ICameraDescriptor
    {
        public string DevicePath { get; set; }
        public string Name { get; set; }
    }
}
