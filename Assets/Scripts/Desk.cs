namespace Realstone
{
    using UnityEngine;

    public class Desk : MonoBehaviour
    {
        private Employee owner = null;

        public Employee GetOwner()
        {
            return owner;
        }

        public void SetOwner(Employee employee)
        {
            if (owner != null)
            {
                GameManager.instance.employeeManager.MoveToUnassign(owner.gameObject);
                owner.UnassignOnDesk();
            }
            owner = employee;
        }

        public void RemoveOwner()
        {
            owner = null;
        }

        public void SetOrigin()
        {
            Utils.CopyTransform(owner.gameObject, transform);
        }

        public void SetOffWork()
        {
            Utils.CopyTransform(owner.gameObject, transform);
            owner.gameObject.transform.position += Vector3.down * 10;
        }
    }
}