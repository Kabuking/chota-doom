using Characters.Player.Global;
using UnityEngine;

namespace Modules.Player.Scripts.Components.MovementTriggers
{
    public class OrientRotationTowardsMovement: MonoBehaviour
    {
        public bool OnZoneExit_KeepPreviousSetting = true;

        public bool limitRotation = false;
        public bool orientToRotation = false;
        public float rotationRateIfOrient = 2;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                DebugX.LogWithColorYellow("Entered zone");
                other.GetComponent<PCharacterMovement>().SetLimitOrientToRotation(limitRotation);
                other.GetComponent<PCharacterMovement>().SetOrientToRotation(orientToRotation);
                other.GetComponent<PCharacterMovement>().SetRotationRate(rotationRateIfOrient);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && OnZoneExit_KeepPreviousSetting)
            {
                DebugX.LogWithColorYellow("Exit zone");
                other.GetComponent<PCharacterMovement>().SetLimitOrientToRotation(false);
                other.GetComponent<PCharacterMovement>().SetOrientToRotation(false);
                other.GetComponent<PCharacterMovement>().ResetLookForward();
            }        
        }
    }
}