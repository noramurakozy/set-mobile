using System.Collections.Generic;

namespace DefaultNamespace
{
    public class Set
    {
        HashSet<SetCard> set = new();
        
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
            List<SetCard> setList = new List<SetCard>(set);
            if (!(((setList[0].Number == setList[1].Number)
                   && (setList[1].Number == setList[2].Number))
                  || ((setList[0].Number != setList[1].Number)
                      && ((setList[0].Number != setList[2].Number)
                          && (setList[1].Number != setList[2].Number)))))
            {
                return false;
            }

            if (!(((setList[0].Shape == setList[1].Shape)
                   && (setList[1].Shape == setList[2].Shape))
                  || ((setList[0].Shape != setList[1].Shape)
                      && ((setList[0].Shape != setList[2].Shape)
                          && (setList[1].Shape != setList[2].Shape)))))
            {
                return false;
            }

            if (!(((setList[0].Fill == setList[1].Fill)
                   && (setList[1].Fill == setList[2].Fill))
                  || ((setList[0].Fill != setList[1].Fill)
                      && ((setList[0].Fill != setList[2].Fill)
                          && (setList[1].Fill != setList[2].Fill)))))
            {
                return false;
            }

            if (!(((setList[0].Color == setList[1].Color)
                   && (setList[1].Color == setList[2].Color))
                  || ((setList[0].Color != setList[1].Color)
                      && ((setList[0].Color != setList[2].Color)
                          && (setList[1].Color != setList[2].Color)))))
            {
                return false;
            }

            if ((((setList[0].Color == setList[1].Color)
                  && (setList[1].Color == setList[2].Color))
                 && (((setList[0].Fill == setList[1].Fill)
                      && (setList[1].Fill == setList[2].Fill))
                     && (((setList[0].Shape == setList[1].Shape)
                          && (setList[1].Shape == setList[2].Shape))
                         && ((setList[0].Number == setList[1].Number)
                             && (setList[1].Number == setList[2].Number))))))
            {
                return false;
            }

            return true;
        }

        public void RemoveFromSet(SetCard cv)
        {
            set.Remove(cv);
        }

        public int GetSize()
        {
            return set.Count;
        }
    }
}