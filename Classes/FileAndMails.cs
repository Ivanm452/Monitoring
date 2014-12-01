using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monitoring.Classes
{
    class FileAndMails
    {
        public string filePath;
        public List<string> mails;
        public bool poslato;
        public string klijentNaziv;
        public bool potencijalnoNeaktivna;
        public bool maticniNePostoji;

        public FileAndMails(string filePath, List<string> mails, bool poslato, bool potencijalnoNeaktivna, bool maticniNePostoji)
        {
            this.filePath = filePath;
            this.mails = mails;
            this.poslato = poslato;
            this.potencijalnoNeaktivna = potencijalnoNeaktivna;
            this.maticniNePostoji = maticniNePostoji;
        }
    }
}
