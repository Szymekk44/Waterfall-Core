﻿using System;
using System.Collections.Generic;
using System.IO;



namespace Waterfall.System.Core.Waterbash.Commands
{
    public class cd : WSHCommand
    {
        public override string HelpNote { get; set; } = "Changes the current working directory.";
        public cd()
        {
            MinimumParamsLength = 1;
            paramActions = new Dictionary<string, Action<Watershell>>
            {

            };
        }
        public override void Execute(string[] Params, Watershell myShell)
        {
            string Path = Params[0];
            if (Path.StartsWith("\\"))
            {
                Path = myShell.GetPath() + Path;
            }
            else if (Path == "..")
            {
                Path = myShell.GetPath();
                if (Path == myShell.GetDrive() + @":\")
                {
                    myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Red));
                    myShell.CWriteLine("You can't get out of this directory.");
                    myShell.CChangeColor(myShell.GetColor(Watershell.ConsoleColor.Gray));
                }
                else
                {
                    Path = Path.Remove(Path.Length - 1);
                    int lastSlashIndex = Path.LastIndexOf(@"\");
                    string result = Path.Substring(0, lastSlashIndex + 1);
                    Path = result;
                }
            }
            else if (Path[1] != ':') //disk
            {
                Path = myShell.GetPath() + Path;
            }

            if (Path[Path.Length - 1] != '\\')
                Path += "\\";

            Path = Path.Replace('/', '\\');


            if (!Directory.Exists(Path))
            {
                myShell.CWriteLine($"cd: {Path}: No such file or directory");
                return;
            }
            if (File.Exists(Path))
            {
                myShell.CWriteLine($"cd: {Path}: Not a directory");
                return;
            }

            myShell.ChangePath(Path);
        }

    }
}


