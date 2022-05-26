using System.Collections.Generic;
using System.Linq;

namespace DefaultNamespace
{
    public class Set
    {
        HashSet<SetCard> set = new();
        private SetCardEvaluation _fillEvaluation;
        private SetCardEvaluation _colorEvaluation;
        private SetCardEvaluation _numberEvaluation;
        private SetCardEvaluation _shapeEvaluation;
        public int DiffPropsCount { get; set; }

        public HashSet<SetCard> GetSet()
        {
            return set;
        }

        public void AddToSet(SetCard card)
        {
            if (set.Count == 3)
            {
                set.Clear();
            }

            set.Add(card);
        }

        public bool IsSet()
        {
            DiffPropsCount = 0;
            bool returnValue = true;
            List<SetCard> setList = new List<SetCard>(set);
            
            var numbersAllTheSame = 
                setList[0].Number == setList[1].Number
                    && setList[1].Number == setList[2].Number;
            var numbersAllDifferent = 
                setList[0].Number != setList[1].Number
                     && setList[0].Number != setList[2].Number
                         && setList[1].Number != setList[2].Number;
            if(numbersAllTheSame)
            {
                _numberEvaluation = SetCardEvaluation.ALL_SAME;
            }
            else if(numbersAllDifferent)
            {
                _numberEvaluation = SetCardEvaluation.ALL_DIFFERENT;
                DiffPropsCount++;
            }
            else
            {
                _numberEvaluation = SetCardEvaluation.MIXED;
                returnValue = false;
            }

            var shapesAllTheSame = 
                setList[0].Shape == setList[1].Shape
                    && setList[1].Shape == setList[2].Shape;
            var shapesAllDifferent = 
                setList[0].Shape != setList[1].Shape
                     && setList[0].Shape != setList[2].Shape
                         && setList[1].Shape != setList[2].Shape;
            if (shapesAllTheSame)
            {
                _shapeEvaluation = SetCardEvaluation.ALL_SAME;
            }
            else if (shapesAllDifferent)
            {
                _shapeEvaluation = SetCardEvaluation.ALL_DIFFERENT;
                DiffPropsCount++;
            }
            else
            {
                _shapeEvaluation = SetCardEvaluation.MIXED;
                returnValue = false;
            }


            var fillsAllTheSame = setList[0].Fill == setList[1].Fill
                    && setList[1].Fill == setList[2].Fill;

            var fillsAllDifferent = setList[0].Fill != setList[1].Fill
                     && setList[0].Fill != setList[2].Fill
                         && setList[1].Fill != setList[2].Fill;
            if (fillsAllTheSame)
            {
                _fillEvaluation = SetCardEvaluation.ALL_SAME;
            }
            else if (fillsAllDifferent)
            {
                _fillEvaluation = SetCardEvaluation.ALL_DIFFERENT;
                DiffPropsCount++;
            }
            else
            {
                _fillEvaluation = SetCardEvaluation.MIXED;
                returnValue = false;
            }


            var colorsAllTheSame = setList[0].Color == setList[1].Color
                    && setList[1].Color == setList[2].Color;
            var colorsAllDifferent = setList[0].Color != setList[1].Color
                     && setList[0].Color != setList[2].Color
                         && setList[1].Color != setList[2].Color;

            if (colorsAllTheSame)
            {
                _colorEvaluation = SetCardEvaluation.ALL_SAME;
            }
            else if (colorsAllDifferent)
            {
                _colorEvaluation = SetCardEvaluation.ALL_DIFFERENT;
                DiffPropsCount++;
            }
            else
            {
                _colorEvaluation = SetCardEvaluation.MIXED;
                returnValue = false;
            }
            
            // If all of them are the same cards?
            if (colorsAllTheSame
                 && fillsAllTheSame
                     && shapesAllTheSame
                         && numbersAllTheSame)
            {
                returnValue = false;
            }

            return returnValue;
        }

        public void RemoveFromSet(SetCard cv)
        {
            set.Remove(cv);
        }

        public int GetSize()
        {
            return set.Count;
        }

        public void ClearSet()
        {
            set.Clear();
        }

        public string GetReason()
        {
            if (IsSet())
            {
                return
                    "ALL these cards have" +
                    $"{(_fillEvaluation == SetCardEvaluation.ALL_SAME ? " the same fill," : "")}" + 
                    $"{(_fillEvaluation == SetCardEvaluation.ALL_DIFFERENT ? " different fills," : "")}" + 
                    $"{(_colorEvaluation == SetCardEvaluation.ALL_SAME ? " the same color," : "")}" +
                    $"{(_colorEvaluation == SetCardEvaluation.ALL_DIFFERENT ? " different colors," : "")}" +
                    $"{(_numberEvaluation == SetCardEvaluation.ALL_SAME ? " the same number," : "")}" +
                    $"{(_numberEvaluation == SetCardEvaluation.ALL_DIFFERENT ? " different numbers," : "")}" +
                    $"{(_shapeEvaluation == SetCardEvaluation.ALL_SAME ? " and the same shape" : "")}" +
                    $"{(_shapeEvaluation == SetCardEvaluation.ALL_DIFFERENT ? " and different shapes" : "")}" +
                    ".";
            }

            var txtList = new List<string>();
            if (_fillEvaluation == SetCardEvaluation.MIXED)
            {
                txtList.Add(" 1 of the cards has a different fill");
            }
            if (_colorEvaluation == SetCardEvaluation.MIXED)
            {
                txtList.Add(" 1 of the cards has a different color");
            }
            if (_numberEvaluation == SetCardEvaluation.MIXED)
            {
                txtList.Add(" 1 of the cards has a different number");
            }
            if (_shapeEvaluation == SetCardEvaluation.MIXED)
            {
                txtList.Add(" 1 of the cards has a different shape");
            }

            var final = txtList.Count >  1 ? string.Join(", ", txtList.Take(txtList.Count - 1)) + " and " + txtList.Last() : txtList.FirstOrDefault();

            return
                final + " than the others.";
        }
    }
}