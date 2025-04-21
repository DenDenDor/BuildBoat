using System.Collections.Generic;
using Goossyaa;
using UnityEngine;

namespace CMF
{
    //This script provides simple mouse cursor locking functionality;
    public class MouseCursorLock : MonoBehaviour
    {
        //Whether to lock the mouse cursor at the start of the game;
        public bool lockCursorAtGameStart = true;

        //Key used to unlock mouse cursor;
        public List<KeyCode> unlockKeyCodes;

        //Key used to lock mouse cursor;
        public List<KeyCode> lockKeyCodes;

        public bool isActive;

        [Header("Debug:")] public bool isLocked = false;

        void Start()
        {
            if (lockCursorAtGameStart)
                SetCursorLocked(true, true);
        }

        public void SetCursorLocked(bool isLock, bool isVisible = true)
        {
          //  if(DebugOn.Instance)Debug.Log("MouseCursorLock : SetCursorLocked : isLock = " + isLock);

            if (isLock)
            {
                // if (YandexGame.EnvironmentData.isMobile) 
                //     return;
                //
                // Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                // Cursor.lockState = CursorLockMode.None;
            }

            // Cursor.visible = isVisible;

            isLocked = isLock;
        }

        void Update()
        {
            foreach (var keyKode in unlockKeyCodes)
            {
                if (Input.GetKeyDown(keyKode) && isActive)
                    SetCursorLocked(false,true);
            }

            foreach (var lockKeyCode in lockKeyCodes)
            {
                if (Input.GetKeyDown(lockKeyCode) && isActive)
                    SetCursorLocked(true,false);
            }
        }
    }
}