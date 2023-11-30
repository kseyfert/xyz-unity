using System;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [Serializable]
    public class DialogData
    {
        [SerializeField] private List<Phrase> phrases = new List<Phrase>();
        public List<Phrase> Phrases => phrases;

        public DialogData(params string[] messages)
        {
            foreach (var message in messages)
            {
                phrases.Add(new Phrase(message));
            }
        }

        public DialogData(params string[][] phrases)
        {
            foreach (var pair in phrases)
            {
                if (pair.Length <= 0) continue;

                var phrase = pair.Length == 1 ? new Phrase(pair[0]) : new Phrase(pair[0], pair[1]);
                this.phrases.Add(phrase);
            }
        }

        public bool IsPersonalized()
        {
            return phrases.Exists(item => item.HasAuthor());
        }

        [Serializable]
        public struct Phrase
        {
            [SerializeField] private string message;
            [SerializeField] private string author;
            [SerializeField] private Sprite avatar;

            public string Message => message;
            public string Author => author;
            public Sprite Avatar => avatar;

            public Phrase(string message)
            {
                this.message = message;
                author = null;
                avatar = null;
            }

            public Phrase(string message, string author)
            {
                this.message = message;
                this.author = author;
                avatar = null;
            }

            public Phrase(string message, string author, Sprite avatar)
            {
                this.message = message;
                this.author = author;
                this.avatar = avatar;
            }

            public bool HasAuthor()
            {
                return author != null || avatar != null;
            }
        }
    }
}