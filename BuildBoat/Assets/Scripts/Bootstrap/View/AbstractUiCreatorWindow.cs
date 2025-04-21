using UnityEngine;

public abstract class AbstractUiCreatorWindow : AbstractWindowUi
{
   protected T CreatePrefab<T>(T prefab, Transform point) where T : MonoBehaviour
   {
      T created = Instantiate(prefab, point);

      return created;
   }
}
