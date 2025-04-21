using CMF;
using Sourse.Common.Code.Rewired.Model;
using Sourse.Common.Code.Rewired.Router;
using UnityEngine;

using DeviceType = Source.YandexGames.DeviceType;

namespace Source.Common.Code.Installer
{
    public class RewiredInstaller : MonoBehaviour
    {
        [SerializeField] private TouchController _touchController;
        [SerializeField] private AdvancedWalkerController _advancedWalkerController;
        [SerializeField] private GameObject[] _keys;
      //  [SerializeField] private ThirdPersonCamera _thirdPersonCamera;

        private GeneratorCameraVelocity _generatorCameraVelocity;
        private TouchControllerActivity _touchControllerActivity;

        private bool _isMobile = false;
        
        private void Start()
        {
            _advancedWalkerController = FindObjectOfType<AdvancedWalkerController>();
            InstallBindings();
        }

        private void InstallBindings()
        {

            bool isMobile = DeviceData.IsMobile;

            DeviceType type;
            
            if (isMobile == false)
            {
                type = DeviceType.Computer_Windows;

                foreach (var item in _keys)
                {
                    item.SetActive(true);
                }

                _isMobile = false;
            }
            else
            {
                type = DeviceType.Mobile_Android;
                
                foreach (var item in _keys)
                {
                    item.SetActive(false);
                }
                
                _isMobile = true;
            }
            
            _generatorCameraVelocity = new GeneratorCameraVelocity(type);
            _touchControllerActivity = new TouchControllerActivity(_touchController.gameObject, type);
            
            new RewiredRouter(FindObjectOfType<FollowPlayerCamera>(), _advancedWalkerController, _generatorCameraVelocity.Create, _touchControllerActivity.Update);
        }
    }
}