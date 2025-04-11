using UniRx;
namespace Game.UI.Interactables
{
    public class ProgressPresenter
    {
        private readonly IProgressModelContainer _progressModel;
        private readonly ProgressBarView _progressBarView;

        public ProgressPresenter(IProgressModelContainer progressModel, ProgressBarView progressBarView)
        {
            _progressModel = progressModel;
            _progressBarView = progressBarView;
            progressModel.CurrentProgress.Subscribe(value =>
            {
                UpdateView();
            });
        }

        private void UpdateView()
        {
            _progressBarView.SetProgress(_progressModel.CurrentProgress.Value, _progressModel.GoalProgress.Value);
        }
    }
}
