using UnityEngine;

namespace Feedback
{
    public class GFormQuestion : MonoBehaviour
    {
        [SerializeField] public string entryID;
        [SerializeField] public bool required;
        public object Answer { get; set; }
    }
}