using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourplanner.ViewModels
{
    internal class ResultViewModel : BaseViewModel
    {
        private string? _resultText;

        public string? ResultText
        {
            get { return _resultText; }
            set
            {
                _resultText = value;
                OnPropertyChanged();
            }
        }

        public void DisplayResults(string resultText)
        {
            ResultText = resultText;
        }
    }
}
