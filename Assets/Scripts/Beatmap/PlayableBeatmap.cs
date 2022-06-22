namespace Beatmap
{
    public class PlayableBeatmap : Beatmap
    {
        protected override void Start()
        {
            base.Start();
            PlayAudio();
        }

        public override void PlayAudio()
        {
            StopAudio();
            songSource.time = 0f;
            songSource.Play();
        }

        public override void StopAudio()
        {
            songSource.Stop();
        }
    }
}