using Goossyaa;
using IE.RSB;
using UnityEngine;
#if MirraGames
using DeviceType = romanlee17.MirraGames.Interfaces.DeviceType;
#else
using DeviceType = Source.YandexGames.DeviceType;
#endif

namespace Sourse.Common.Code.Rewired.Model
{
    public class TouchControllerActivity
    {
        private readonly GameObject _gameObject;
        private readonly DeviceType _type;
        
        public TouchControllerActivity(GameObject gameObject, DeviceType type)
        {
            _gameObject = gameObject;
            _type = type;
        }

        private void SetDevice(bool isActive)
        {
            Object.FindObjectOfType<TouchControls_ManipulateCharacter>().SetMouseInput(isActive == false);
            
            _gameObject.SetActive(isActive);
        }

        public void Update()
        {
            switch (_type)
            {
                case DeviceType.Unknown:
                    SetDevice(false);
                    break;
                
                case DeviceType.Mobile_Android:
                    SetDevice(true);
                    break;
                
                case DeviceType.Mobile_IPhone:
                    SetDevice(true);
                    break;
                
                case DeviceType.Mobile_IPad:
                    SetDevice(true);
                    break;
                
                case DeviceType.Computer_Windows:
                    SetDevice(false);
                    break;
                
                case DeviceType.Computer_Linux:
                    SetDevice(false);
                    break;
                
                case DeviceType.Computer_Mac:
                    SetDevice(false);
                    break;
                
                default:
                    SetDevice(false);
                    break;
            }
        }
    }
}