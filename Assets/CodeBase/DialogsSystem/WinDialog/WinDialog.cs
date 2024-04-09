using UnityEngine.SceneManagement;

namespace CodeBase.DialogsSystem.WinDialog
{
    public class WinDialog : DialogBase
    {
        protected override void Confirm()
        {
            base.Confirm();
            SceneManager.LoadScene(1);
        }
    }
}