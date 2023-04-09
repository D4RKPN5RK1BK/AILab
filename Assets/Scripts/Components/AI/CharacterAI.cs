using Components.Controllers;
using UnityEngine;

namespace Components.AI
{
    public class CharacterAI : MonoBehaviour
    {
        [Tooltip("Объект за которым персонаж будет следовать")]
        public GameObject pointOfIntrest;

        private DefaultCharacterController _defaultCharacterController;

        public void Start()
        {
            _defaultCharacterController = GetComponent<DefaultCharacterController>();
        }

        public void Update()
        {
            _defaultCharacterController.Move(pointOfIntrest.gameObject.transform.position - transform.position);
        }
    }
}
