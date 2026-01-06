using UnityEngine;

public class SoundSettingsParticipant : SettingsParticipantBehaviour
{
    public override int Order => 1;

    public override void Capture(SettingsData data)
    {
        data.sound.Set(AudioManager.Instance.CurrentBgm, AudioManager.Instance.CurrentSfx);
        Debug.Log($"[OpenSettingPanel] CurrentBgm={AudioManager.Instance.CurrentBgm}, CurrentSfx={AudioManager.Instance.CurrentSfx}");
    }

    public override void Apply(SettingsData data)
    {
        AudioManager.Instance.ForceSetWithoutSave(data.sound.bgmVolume, data.sound.sfxVolume);
        //AudioManager.Instance.PreviewBgm(data.sound.bgmVolume);
        //AudioManager.Instance.PreviewSfx(data.sound.sfxVolume);

        Debug.Log($"[OpenSettingPanel] CurrentBgm={AudioManager.Instance.CurrentBgm}, CurrentSfx={AudioManager.Instance.CurrentSfx}");
    }
}
