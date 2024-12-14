﻿using System;
using System.Collections.Generic;
using System.IO;
using Waterfall.System.Core.CLI;

namespace Waterfall.System.Core.Waterbash.Commands
{
    public class cat : WSHCommand
    {
        public override string HelpNote { get; set; } = "Displays the contents of files.";
        public cat()
        {
            paramActions = new Dictionary<string, Action<Watershell>>
            {

            };
        }
        public override void Execute(string[] Params, Watershell myShell)
        {
            if (Params.Length == 0)
            {
                ExecuteDir(myShell.GetPath(), myShell);
                return;
            }

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

            //if (Path[Path.Length - 1] != '\\')
            //	Path += "\\";

            Path = Path.Replace('/', '\\');

            ExecuteDir(Path, myShell);
        }
        void ExecuteDir(string dir, Watershell myShell)
        {
            CLIDrawText.DrawMassText(File.ReadAllText(dir), myShell);
        }
    }
}
