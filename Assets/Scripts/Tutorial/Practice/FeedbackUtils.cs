using System;
using System.Collections.Generic;

namespace Tutorial.Practice
{
    public static class FeedbackUtils
    {
        private static readonly Random Rng = new();
        
        private static List<string> _positiveFeedbackList = new()
        {
            "Correct!",
            "Good job!",
            "That's correct!",
            "That's it!",
            "That's right!",
            "Very good!",
            "Well done!",
            "Awesome!",
            "Brilliant!",
            "Excellent!",
            "Fantastic!",
        };

        private static List<string> _negativeFeedbackList = new()
        {
            "Oops... Try again!",
            "Not exactly...",
            "Good try but...",
            "That is almost it...",
            "Have another try...",
            "Have another go...",
            "Give it another shot...",
            "Give it another go...",
            "Unfortunately not :("
        };

        public static string GetRandomPositiveFeedback()
        {
            int k = Rng.Next(_positiveFeedbackList.Count-1);
            return _positiveFeedbackList[k];
        }

        public static string GetRandomNegativeFeedback()
        {
            int k = Rng.Next(_negativeFeedbackList.Count-1);
            return _negativeFeedbackList[k];
        }
    }
}