using System;
using IE.RSB.Movement;
#if MirraGames
using DeviceType = romanlee17.MirraGames.Interfaces.DeviceType;
#else
using DeviceType = Source.YandexGames.DeviceType;
#endif
using Source.Common.Code.Scripts.Level.Player.Movement;

namespace Sourse.Common.Code.Rewired.Model
{
    public class GeneratorCameraVelocity
    {
        
        private readonly DeviceType _type;

        public GeneratorCameraVelocity(DeviceType type)
        {
            _type = type;
        }

        public (ICameraVelocity, ICharacterVelocity) Create()
        {
            switch (_type)
            {
                case DeviceType.Unknown:
                    return (new ComputerCameraBasedInput(), new ComputerCharacterVelocity());
                
                case DeviceType.Mobile_Android:
                    return (new MobileCameraBasedInput(), new MobileCharacterVelocity());
                
                case DeviceType.Mobile_IPhone:
                    return (new MobileCameraBasedInput(), new MobileCharacterVelocity());
                
                case DeviceType.Mobile_IPad:
                    return (new MobileCameraBasedInput(), new MobileCharacterVelocity());
                
                case DeviceType.Computer_Windows:
                    return (new ComputerCameraBasedInput(), new ComputerCharacterVelocity());
                
                case DeviceType.Computer_Linux:
                    return (new ComputerCameraBasedInput(), new ComputerCharacterVelocity());
                
                case DeviceType.Computer_Mac:
                    return (new ComputerCameraBasedInput(), new ComputerCharacterVelocity());
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}