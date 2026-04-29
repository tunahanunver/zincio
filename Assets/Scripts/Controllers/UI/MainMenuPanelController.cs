using Enums;
using Signals;

namespace Controllers.UI
{
    public class MainMenuPanelController : UIPanelController
    {
        public void OnPlayButtonClicked()
        {
            CoreUISignals.Instance.onCloseAllPanels.Invoke();
            CoreGameSignals.Instance.onLevelInitialize.Invoke();
        }
        public void OnLevelSelectButtonClicked()
        {
            CoreUISignals.Instance.onClosePanel.Invoke(UIPanelType.MainMenu);
            CoreUISignals.Instance.onOpenPanel.Invoke(UIPanelType.LevelSelect);
        }
    }
}