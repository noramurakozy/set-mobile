using System.Collections;
using System.Collections.Generic;

namespace Feedback
{
    public interface IGFormManager
    {
        public IEnumerator SendGFormData(List<GFormQuestion> questionList);
    }
}