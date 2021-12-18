using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
public class Dialogue
{

    public DialogueContainer dialogueContainer;
    //TextAsset pagesFile;
    string pagesFile;


    public List<DialoguePage> dialoguePages = new List<DialoguePage>(); // stores dialogue texts
    public int currentPageNumber = 0;

 
    public string currentNodeGUID = null;
    public string nextNodeGUID = null;

    public enum nodeType
    {
        unknown,

        entry,
        dialogue,
        redirections,
        events,
        end
    }

    public nodeType currentNodeType = nodeType.unknown;
    //public nodeType nextNodeType = nodeType.unknown;
    public Dialogue(DialogueContainer _dialogueContainer)
    {
        dialogueContainer = _dialogueContainer;

        string path = $"/Assets/DialogueSystem/Resources/Localization/{ DialogueManager.selectedLang }/Dialogues/{dialogueContainer.filename}.txt";

        //pagesFile = File.ReadAllText(path);
        pagesFile = (Resources.Load<TextAsset>($"Localization/{DialogueManager.selectedLang}/Dialogues/{dialogueContainer.filename}")).text;
        ReadTextFile();
    }


    void ReadTextFile()
    {
        string txt = pagesFile;

        string[] separatingStrings = { "[PAGE_END]" + '\n' }; // ORDER IS IMPORTANT! first br with line break, next br w/out lb just for extra caution
        //string[] pages = txt.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
        string[] pages = txt.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);

        //dialogue = new List<Page>(pages.Length);

        List<string> tagWords = new List<string>();
        // SET UP TAG WORDS HERE
        tagWords.Add("FILENAME");
        tagWords.Add("PAGE_START");
        tagWords.Add("GUID");
        tagWords.Add("NAME");
        tagWords.Add("TEXT");
        tagWords.Add("CHOICE");
        tagWords.Add("PAGE_END");
        tagWords.Add("END");

        int pagenumber = 0;
        foreach (string page in pages)
        {
            DialoguePage thisPage = new DialoguePage();

            string tagWord = "";

            bool readingTheWord = false;
            foreach (char ch in page)
            {
                if (ch == '[')
                {
                    readingTheWord = true;
                }
                if (readingTheWord)
                {
                    List<char> technicalSymbols = new List<char> { '[', ']' };

                    if (technicalSymbols.Contains(ch) == false)
                    {
                        tagWord += ch;
                    }
                }
                if (ch == ']')
                {
                    readingTheWord = false;

                    foreach (string tag in tagWords)
                    {
                        if (tagWord.Contains(tag))
                        {
                            string value = "";// = tagWord.Replace(tag, ""); // delete tag word from the value
                            switch (tag)
                            {
                                default:
                                    break;

                                case "FILENAME":
                                    value = tagWord.Replace(tag + '=', "");

                                    //filename = value;
                                    //Debug.Log("filename=" + value);
                                    break;

                                case "GUID":
                                    value = tagWord.Replace(tag + '=', "");

                                    thisPage.pageGUID = value;
                                    //Debug.Log(value);
                                    break;

                                case "NAME":
                                    value = tagWord.Replace(tag + '=', "");

                                    thisPage.speakerName = value;
                                    //Debug.Log(value);
                                    break;

                                case "TEXT":
                                    value = tagWord.Replace(tag + '=', "");

                                    thisPage.pageText = value;
                                    //Debug.Log(value);
                                    break;

                                case "CHOICE":
                                    value = tagWord.Replace(tag, "");

                                    foreach (char chch in value)
                                    {
                                        value = value.Remove(0, 1);
                                        if (chch.ToString() == "=")
                                        {
                                            break;
                                        }
                                    }
                                    thisPage.choices.Add(value);
                                    //Debug.Log(value);
                                    break;

                                case "END":
                                    ///////
                                    break;

                            }
                        }

                    }
                    tagWord = "";
                }
            }
            dialoguePages.Add(thisPage);
            ++pagenumber;

        }

    }

    public Dialogue.nodeType FindNodeTypeByGUID(string _guid)
    {
        if (dialogueContainer.entryNodeData.guid == _guid)
        {
            return Dialogue.nodeType.entry;
        }

        foreach (var entry in dialogueContainer.dialogueNodesData)
        {
            if (entry.guid == _guid)
            {
                return Dialogue.nodeType.dialogue;
            }
        }
        foreach (var entry in dialogueContainer.redirectionsNodesData)
        {
            if (entry.guid == _guid)
            {
                return Dialogue.nodeType.redirections;
            }
        }
        foreach (var entry in dialogueContainer.eventNodesData)
        {
            if (entry.guid == _guid)
            {
                return Dialogue.nodeType.events;
            }
        }
        foreach (var entry in dialogueContainer.endNodesData)
        {
            if (entry.guid == _guid)
            {
                return Dialogue.nodeType.end;
            }
        }


        return Dialogue.nodeType.unknown;
    }
}
