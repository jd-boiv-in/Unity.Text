using TMPro;
using UnityEngine;

namespace JD.Text
{
    public class TextMeshUI : TextMeshProUGUI
    {
        private bool _hasDefaultValues;
        private string _defaultText;
        private Color _defaultColor;
        
        public Color DefaultColor
        {
            get
            {
                if (!_hasDefaultValues) GetDefaultValues();
                return _defaultColor;
            }
        }

        private void GetDefaultValues()
        {
            if (_hasDefaultValues) return;
            _hasDefaultValues = true;

            _defaultText = text;
            _defaultColor = color;
        }

        public void OverwriteDefaultColor(Color colorFloat)
        {
            if (!_hasDefaultValues)
            {
                _hasDefaultValues = true;
                _defaultText = text;
            }
            
            _defaultColor = colorFloat;
        }
        
        public void SetColor(Color colorFloat)
        {
            var color32 = (Color32) colorFloat;
            if (m_fontColor == colorFloat) return;
            
            var info = textInfo;
            if (info == null) return;

            for (var i = 0; i < info.characterCount; i++)
            {
                var c = info.characterInfo[i];
                var colors = info.meshInfo[c.materialReferenceIndex].colors32;

                var n = Mathf.Min(c.vertexIndex + 4, colors.Length);
                for (var j = c.vertexIndex; j < n; j++)
                    colors[j] = color32;
            }

            UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            m_fontColor = colorFloat;
        }
        
        public void SetColor(Color32 color32)
        {
            var colorFloat = (Color) color32;
            if (m_fontColor == colorFloat) return;
            
            var info = textInfo;
            if (info == null) return;

            for (var i = 0; i < info.characterCount; i++)
            {
                var c = info.characterInfo[i];
                var colors = info.meshInfo[c.materialReferenceIndex].colors32;

                var n = Mathf.Min(c.vertexIndex + 4, colors.Length);
                for (var j = c.vertexIndex; j < n; j++)
                    colors[j] = color32;
            }

            UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            m_fontColor = colorFloat;
        }
        
        public void SetAlpha(float value)
        {
            if (Mathf.Approximately(m_fontColor.a, value)) return;
            
            var info = textInfo;
            if (info == null) return;

            var a = (byte) (value * 0xFF);
            for (var i = 0; i < info.characterCount; i++)
            {
                var c = info.characterInfo[i];
                var colors = info.meshInfo[c.materialReferenceIndex].colors32;

                var n = Mathf.Min(c.vertexIndex + 4, colors.Length);
                for (var j = c.vertexIndex; j < n; j++)
                {
                    var color32 = colors[j];
                    colors[j] = new Color32(color32.r, color32.g, color32.b, a);
                }
            }
            
            UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            m_fontColor = new Color(color.r, color.g, color.b, value);
        }

        // Use the default behavior instead of doing it on our own, there might be some good reason
        // why it was done this way...
        public void UpdateVertex()
        {
            var info = textInfo;
            var color32 = (Color32) color;
            for (var i = 0; i < info.characterCount; i++)
            {
                for (var j = 0; j < 4; j++)
                    info.meshInfo[info.characterInfo[i].materialReferenceIndex].colors32[info.characterInfo[i].vertexIndex + j] = color32;
            }
            
            UpdateVertexData();
        }
        
        public void ForceVerticesDirty()
        {
            base.SetVerticesDirty();
        }
    }
}