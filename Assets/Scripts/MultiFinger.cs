using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MyTouch : MonoBehaviour
{
    class MyFinger
    {
        public int id = -1;
        public Touch touch;

        static private List<MyFinger> fingers = new List<MyFinger>();
        static public List<MyFinger> Fingers
        {
            get
            {
                if (fingers.Count == 0)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        MyFinger mf = new MyFinger();
                        mf.id = -1;
                        fingers.Add(mf);
                    }
                }
                return fingers;
            }
        }
    }

    void Update()
    {
        Touch[] touches = Input.touches;

        foreach (MyFinger mf in MyFinger.Fingers)
        {

            if (mf.id == -1)
            {
                continue;
            }

            bool stillExit = false;

            foreach (Touch t in touches)
            {
                if (mf.id == t.fingerId)
                {
                    stillExit = true;
                    break;
                }
            }

            if (stillExit == false)
            {
                mf.id = -1;
            }

        }

        foreach (Touch t in touches)
        {
            bool stillExit = false;

            foreach (MyFinger mf in MyFinger.Fingers)
            {
                if (t.fingerId == mf.id)
                {
                    stillExit = true;
                    mf.touch = t;
                    break;
                }
            }

            if (!stillExit)
            {
                foreach (MyFinger mf in MyFinger.Fingers)
                {
                    if (mf.id == -1)
                    {
                        mf.id = t.fingerId;
                        mf.touch = t;
                        break;
                    }
                }
            }
        }

        for (int i = 0; i < MyFinger.Fingers.Count; i++)
        {
            MyFinger mf = MyFinger.Fingers[i];
            if (mf.id != -1)
            {

                switch (mf.touch.phase)
                {
                    case TouchPhase.Began:
                        //To-DO

                        break;
                    case TouchPhase.Moved:
                        //To-DO

                        break;
                    case TouchPhase.Ended:
                        //To-DO

                        break;
                    case TouchPhase.Stationary:
                        //To-DO

                        break;
                    default:
                        //To-DO

                        break;
                }

            }

        }
    }
}