using UnityEngine;

public class TargetHealthBar : MonoBehaviour
{
    public Texture2D healthBarFrameImage;
    public Texture2D healthBarImage;
    public ClickToAttack playerCharacterClickToAttack;
    public int healthBarWidthPixels = 420;
    public int healthBarHeightPixels = 42;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI()
    {
        if (this.playerCharacterClickToAttack.HasTarget())
        {
            if (this.healthBarFrameImage != null)
            {
                this.DrawHealthBarFrame();
            }
            this.DrawHealthBar();
        }
    }

    private void DrawHealthBarFrame()
    {
        var healthBarFrameX = (Screen.width - this.healthBarWidthPixels) / 2;
        var healthBarFrameY = this.healthBarHeightPixels;
        var healthBarFramePosition = new Rect(x: healthBarFrameX, y: healthBarFrameY, width: this.healthBarWidthPixels, height: this.healthBarHeightPixels);
        GUI.DrawTexture(position: healthBarFramePosition, image: this.healthBarFrameImage);
    }

    private void DrawHealthBar()
    {
        var healthBarFrameX = (Screen.width - this.healthBarWidthPixels) / 2;
        var healthBarFrameY = this.healthBarHeightPixels;
        var healthBarWidth = this.GetHealthBarWidth();
        var healthBarPosition = new Rect(x: healthBarFrameX, y: healthBarFrameY, width: healthBarWidth, height: this.healthBarHeightPixels);
        GUI.DrawTexture(position: healthBarPosition, image: this.healthBarImage);
    }

    private float GetHealthBarWidth()
    {
        var enemy = this.playerCharacterClickToAttack.GetTarget();
        return this.healthBarWidthPixels * enemy.GetCurrentHealth() / enemy.GetMaxHealth();
    }
}
