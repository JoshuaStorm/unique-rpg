using UnityEngine;

public class PlayerHealthBar : MonoBehaviour
{
    public Texture2D healthBarImage;
    public PlayerBehavior playerBehavior;
    public int healthBarWidthPixels = 84;
    public int healthBarHeightPixels = 210;

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
        this.DrawHealthBar();
    }

    private void DrawHealthBar()
    {
        var healthBarFrameX = 0f;
        var healthBarHeight = this.GetHealthBarHeight();
        var healthBarFrameY = Screen.height - healthBarHeight;
        var healthBarPosition = new Rect(x: healthBarFrameX, y: healthBarFrameY, width: this.healthBarWidthPixels, height: healthBarHeight);
        GUI.DrawTexture(position: healthBarPosition, image: this.healthBarImage);
    }

    private float GetHealthBarHeight()
    {
        return this.healthBarHeightPixels * this.playerBehavior.GetCurrentHealth() / this.playerBehavior.GetMaxHealth();
    }
}
