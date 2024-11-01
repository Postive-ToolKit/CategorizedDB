using Runtime.Attributes;

namespace Postive.SimpleSoundAssetManager.Runtime.Attributes
{
    public class SoundSelectorAttribute : CategoryElementSelectorAttribute {
        public SoundSelectorAttribute() : base() {
            ElementFinder = SoundDB.Instance;
        }
    }
}