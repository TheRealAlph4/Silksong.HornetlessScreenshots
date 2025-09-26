using UnityEngine;

namespace HornetlessScreenshots
{
    internal class NoclipHelper : MonoBehaviour
    {
        public void Update()
        {
            if(HeroController.instance == null)
            {
                return;
            }
            bool isInTransition = HeroController.instance.transitionState != GlobalEnums.HeroTransitionState.WAITING_TO_TRANSITION;
            if(isInTransition)
            {
                HornetlessScreenshotsMod.ForcedHeroX = HeroController.instance.transform.GetPositionX();
                HornetlessScreenshotsMod.ForcedHeroY = HeroController.instance.transform.GetPositionY();
                return;
            }
            if (HornetlessScreenshotsMod.IsNoclip)
            {
                HeroController.instance.transform.SetPositionX(HornetlessScreenshotsMod.ForcedHeroX);
                HeroController.instance.transform.SetPositionY(HornetlessScreenshotsMod.ForcedHeroY);
            }
        }
    }
}
