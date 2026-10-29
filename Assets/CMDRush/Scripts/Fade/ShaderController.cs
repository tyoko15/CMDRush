/*  制作者 島田
    制作内容
    フェイド全般の処理の制作
*/
using UnityEngine;
namespace Shimada
{
    public class ShaderController : MonoBehaviour
    {
        public static ShaderController shaderControllerInstance { get; private set; }

        [SerializeField] Material material;
        [SerializeField] Texture texture;
        [SerializeField] Color color;
        [SerializeField, Range(0, 1)]
        public float fadeRange;
        float timer;
        void Start()
        {

        }

        void Awake()
        {
            // すでにインスタンスが存在する場合 → 自分を破棄
            if (shaderControllerInstance != null && shaderControllerInstance != this)
            {
                Destroy(gameObject);
                return;
            }

            // 自分をインスタンスとして登録
            shaderControllerInstance = this;

            // シーンを跨いでも消えないように設定
            DontDestroyOnLoad(gameObject);
        }

        void Update()
        {
            if (material != null && texture != null)
            {
                material.SetFloat("_Range", 1 - fadeRange);
                material.SetTexture("_MaskTex", texture);
                material.SetColor("_Color", color);
            }
        }
        /// <summary>
        /// フェイドイン関数
        /// フェイドにかける秒数, 終了フラグ
        /// </summary>
        /// <param name="time">フェイドにかける秒数</param><param name="flag">終了フラグ</param> 
        public void FadeIn(float time, ref bool flag)
        {
            if (flag)
            {
                if (timer > time)
                {
                    timer = 0;
                    fadeRange = 1f;
                    flag = false;
                }
                else if (timer < time)
                {
                    timer += Time.deltaTime;
                    fadeRange = Mathf.Lerp(0f, 1f, timer / time);
                }
            }
        }
        /// <summary>
        /// フェイドアウト関数
        /// フェイドにかける秒数, 終了フラグ
        /// </summary>
        /// <param name="time">フェイドにかける秒数</param><param name="flag">終了フラグ</param> 
        public void FadeOut(float time, ref bool flag)
        {
            if (flag)
            {
                if (timer > time)
                {
                    timer = 0;
                    fadeRange = 0f;
                    flag = false;
                }
                else if (timer < time)
                {
                    timer += Time.deltaTime;
                    fadeRange = Mathf.Lerp(1f, 0f, timer / time);
                }
            }
        }
    }
}
