using System.Linq;
using KModkit;
using UnityEngine;

public class MyFirstModule : MonoBehaviour
{
        public KMBombInfo BombInfo;
        public KMBombModule BombModule;
        public KMAudio Audio;
        public KMSelectable CounterButton;
        public KMSelectable SubmitButton;
        public TextMesh Counter;

        private int _count;
        private int count
        {
                get { return _count; }
                set { _count = value; Counter.text = "" + _count; }
        }
        private int solution;

        private const string MODULE_NAME = "My First Module";
        private static int _moduleId = 0;
        private int moduleId;

        private void Start()
        {
                moduleId = _moduleId++;

                CounterButton.OnInteract += OnPressCounter;
                SubmitButton.OnInteract += OnPressSubmit;
                
                // Create solution
                count = 0;
                solution = BombInfo.GetSerialNumberNumbers().Sum() + BombInfo.GetSerialNumberLetters().Count();
                
                Debug.LogFormat(@"[{0} {1}] Solution: {2}", MODULE_NAME, moduleId, solution);
        }

        private bool OnPressCounter()
        {
                if ((int) BombInfo.GetTime() % 2 != 0)
                {
                        Debug.LogFormat(@"[{0} {1}] Button Pressed on an odd second. Strike!", MODULE_NAME, moduleId);
                        
                        BombModule.HandleStrike();
                        
                        return false;
                }
                
                ++count;
                
                Debug.LogFormat(@"[{0} {1}] Button Pressed. Counter is now {2}", MODULE_NAME, moduleId, count);

                return false;
        }

        private bool OnPressSubmit()
        {
                if (count == solution)
                {
                        BombModule.HandlePass();
                        Debug.LogFormat(@"[{0} {1}] Module solved", MODULE_NAME, moduleId);
                } else
                {
                        BombModule.HandleStrike();
                        Debug.LogFormat(@"[{0} {1}] Button pressed when not correct count", MODULE_NAME, moduleId);
                        count = 0;
                }
                
                return false;
        }
}